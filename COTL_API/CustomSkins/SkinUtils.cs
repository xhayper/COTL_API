using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using UnityEngine;

namespace COTL_API.CustomSkins;

internal static class SkinUtils
{
    public static bool SkinP1Loaded { get; internal set; }

    public static Action? SkinP1ToLoad { get; set; } = () => { };

    public static bool SkinP2Loaded { get; internal set; }

    public static Action? SkinP2ToLoad { get; set; } = () => { };

    public static event Action OnFindSkinP1 = () =>
    {
        SkinP1ToLoad.Invoke();
        SkinP1Loaded = true;
    };

    public static event Action OnFindSkinP2 = () =>
    {
        SkinP2ToLoad.Invoke();
        SkinP2Loaded = true;
    };

    public static void ApplyOverride(Skin skin, Attachment a, int slot, string ovrName, AtlasRegion atlasRegion,
        float scaleX, float scaleY, float translationX, float translationY)
    {
        switch (a)
        {
            case MeshAttachment meshAttachment:
            {
                float minX = int.MaxValue;
                float maxX = int.MinValue;
                float minY = int.MaxValue;
                float maxY = int.MinValue;

                for (var j = 0; j < meshAttachment.Vertices.Length; j++)
                    switch (j % 3)
                    {
                        case 0:
                            minY = Math.Min(minY, meshAttachment.Vertices[j]);
                            maxY = Math.Max(maxY, meshAttachment.Vertices[j]);
                            break;
                        case 1:
                            minX = Math.Min(minX, meshAttachment.Vertices[j]);
                            maxX = Math.Max(maxX, meshAttachment.Vertices[j]);
                            break;
                    }

                var diffX = maxX - minX;
                var diffY = maxY - minY;

                var centerX = minX + diffX / 2.0f;
                var centerY = minY + diffY / 2.0f;

                var regionAttachment = new RegionAttachment("Custom" + ovrName);
                regionAttachment.SetRegion(atlasRegion);

                regionAttachment.X = centerY - translationY;
                regionAttachment.Y = centerX - translationX;
                regionAttachment.rotation = -90;
                regionAttachment.ScaleX = scaleX;
                regionAttachment.ScaleY = scaleY;
                regionAttachment.Width = diffX;
                regionAttachment.Height = diffY;
                skin.SetAttachment(slot, ovrName, regionAttachment);
                break;
            }
            case RegionAttachment regionAttachment:
                regionAttachment.Name = "Custom" + ovrName;
                atlasRegion.name = "Custom" + atlasRegion.name;

                regionAttachment.SetRegion(atlasRegion);

                regionAttachment.X += translationX;
                regionAttachment.Y += translationY;
                regionAttachment.ScaleX = scaleX;
                regionAttachment.ScaleY = scaleY;
                regionAttachment.rotation = -90;

                skin.SetAttachment(slot, ovrName, regionAttachment);
                break;
            default:
                LogWarning(
                    $"Attachment {a.Name} is not a MeshAttachment or RegionAttachment, skipping...");
                break;
        }
    }

    public static Skin ApplyAllOverrides(Skin from, Skin to,
        List<Tuple<int, string, float, float, float, float>> overrides, Material material, AtlasAssetBase atlas)
    {
        var name = to.name;
        from.Attachments.ToList().ForEach(att => { to.SetAttachment(att.SlotIndex, att.Name, att.Attachment.Copy()); });
        from.Bones.ToList().ForEach(to.Bones.Add);
        from.Constraints.ToList().ForEach(to.Constraints.Add);

        foreach (var (slot, ovrName, translationX, translationY, scaleX, scaleY) in overrides)
        {
            var atlasRegion = atlas.GetAtlas().FindRegion(slot + ":" + ovrName);
            var a = from.GetAttachment(slot, ovrName);

            if (a is null)
                continue;

            a = a.Copy();

            ApplyOverride(to, a, slot, ovrName, atlasRegion, scaleX, scaleY, translationX, translationY);
        }

        var skin2 = to.GetRepackedSkin(name, material, out var runtimeMaterial, out var runtimeTexture);
        CustomSkinManager.CachedTextures.Clear();
        AtlasUtilities.ClearCache();

        return skin2;
    }

    public static List<Tuple<int, string, float, float, float, float>> CreateSkinAtlas(string name, Texture2D sheet,
        string atlasText, Func<AtlasRegion, List<Tuple<int, string>>> regionOverrideFunction, out Material skinMaterial,
        out SpineAtlasAsset atlasAsset)
    {
        sheet.name = atlasText.Replace("\r", "").Split('\n')[1].Trim();

        Material mat = new(Shader.Find("Spine/Skeleton"))
        {
            mainTexture = sheet
        };
        skinMaterial = mat;

        Material[] materials = [mat];
        var atlas = SpineAtlasAsset.CreateRuntimeInstance(new TextAsset(atlasText), materials, true);
        atlasAsset = atlas;

        List<Tuple<int, string>> overrideRegions = [];

        foreach (var ovrs in atlas.GetAtlas().regions.Select(regionOverrideFunction)) overrideRegions.AddRange(ovrs);

        List<Tuple<int, string, float, float, float, float>> overrides = [];
        var list = atlas.GetAtlas().regions;
        for (var index = 0; index < list.Count; index++)
        {
            var scale = new[] { 0f, 0f, 1f, 1f };
            var atlasRegion = list[index];
            var nameSplit = atlasRegion.name.Split('#');
            if (nameSplit.Length == 2)
            {
                var scales = nameSplit[1];
                var scaleSplit = scales.Split(',');
                switch (scaleSplit.Length)
                {
                    case 2:
                        scale[2] = float.Parse(scaleSplit[0]);
                        scale[3] = float.Parse(scaleSplit[1]);
                        break;
                    case 4:
                        scale[0] = float.Parse(scaleSplit[2]);
                        scale[1] = float.Parse(scaleSplit[3]);
                        scale[2] = float.Parse(scaleSplit[0]);
                        scale[3] = float.Parse(scaleSplit[1]);
                        break;
                    default:
                        LogWarning($"Invalid scale length, Expected 3 or 4, got {scale.Length}.");
                        break;
                }

                atlasRegion.name = nameSplit[0];
            }

            overrides.Add(Tuple.Create(overrideRegions[index].Item1, overrideRegions[index].Item2, scale[0], scale[1],
                scale[2], scale[3]));
        }

        return overrides;
    }

    internal static void InvokeOnFindSkin(PlayerType who)
    {
        if (who == PlayerType.P1)
        {
            OnFindSkinP1.Invoke();
            OnFindSkinP1 = delegate { };
        }
        else
        {
            OnFindSkinP2.Invoke();
            OnFindSkinP2 = delegate { };
        }
    }
}