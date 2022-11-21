using Spine.Unity.AttachmentTools;
using Spine.Unity;
using UnityEngine;
using Spine;

namespace COTL_API.CustomSkins;

internal class SkinUtils
{
    public static bool SkinsLoaded;

    public static event Action OnFindSkin = () =>
    {
        SkinToLoad?.Invoke();
        SkinsLoaded = true;
    };

    public static Action? SkinToLoad = () => { };

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
                {
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
                }

                var diffX = maxX - minX;
                var diffY = maxY - minY;

                minX += translationX;
                minY += translationY;

                var centerX = minX + (diffX / 2.0f);
                var centerY = minY + (diffY / 2.0f);

                minX = centerX - ((diffX / 2.0f) * scaleX);
                maxX = centerX + ((diffX / 2.0f) * scaleX);
                minY = centerY - ((diffY / 2.0f) * scaleY);
                maxY = centerY + ((diffY / 2.0f) * scaleY);

                meshAttachment.Name = "Custom" + ovrName;
                meshAttachment.SetRegion(atlasRegion);
                atlasRegion.name = "Custom" + atlasRegion.name;
                meshAttachment.HullLength = 4;
                meshAttachment.Triangles = new[] { 1, 2, 3, 1, 3, 0 };
                float pw = atlasRegion.page.width;
                float ph = atlasRegion.page.height;
                float x = atlasRegion.x;
                float y = atlasRegion.y;
                float w = atlasRegion.width;
                float h = atlasRegion.height;
                meshAttachment.UVs = new[]
                    { (x + w) / pw, y / ph, (x + w) / pw, (y + h) / ph, x / pw, (y + h) / ph, x / pw, y / ph };
                meshAttachment.Vertices = new[] { minY, minX, 1, maxY, minX, 1, maxY, maxX, 1, minY, maxX, 1 };
                meshAttachment.WorldVerticesLength = 8;

                skin.SetAttachment(slot, ovrName, meshAttachment);
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

                skin.SetAttachment(slot, ovrName, regionAttachment);
                break;
            default:
                Plugin.Instance!.Logger.LogWarning(
                    $"Attachment {a.Name} is not a MeshAttachment or RegionAttachment, skipping...");
                break;
        }
    }

    public static Skin? ApplyAllOverrides(Skin from, Skin to,
        List<Tuple<int, string, float, float, float, float>> overrides, Material material, AtlasAssetBase atlas)
    {
        var name = to.name;
        from.Attachments.ToList().ForEach(att => { to.SetAttachment(att.SlotIndex, att.Name, att.Attachment.Copy()); });
        from.Bones.ToList().ForEach(bone => { to.Bones.Add(bone); });
        from.Constraints.ToList().ForEach(con => { to.Constraints.Add(con); });

        foreach (var (slot, ovrName, translationX, translationY, scaleX, scaleY) in overrides)
        {
            var atlasRegion = atlas.GetAtlas().FindRegion(slot + ":" + ovrName);
            var a = from.GetAttachment(slot, ovrName).Copy();

            ApplyOverride(to, a, slot, ovrName, atlasRegion, scaleX, scaleY, translationX, translationY);
        }

        Material runtimeMaterial;
        Texture2D runtimeTexture;

        var skin2 = to.GetRepackedSkin(name, material, out runtimeMaterial, out runtimeTexture);
        CustomSkinManager.CachedTextures.Clear();
        AtlasUtilities.ClearCache();
        RepackMeshAttachments(skin2, overrides);

        foreach (var slot in from ovr in overrides let ovrName = ovr.Item2 select ovr.Item1)
        {
        }

        return skin2;
    }

    private static void RepackMeshAttachments(Skin skin, List<Tuple<int, string, float, float, float, float>> overrides)
    {
        foreach (var (slot, ovrName, _, _, _, _) in overrides)
        {
            var a = skin.GetAttachment(slot, ovrName).Copy();
            if (a is not MeshAttachment mesh) continue;
            if (mesh.RendererObject is AtlasRegion atlasRegion)
            {
                float pw = atlasRegion.page.width;
                float ph = atlasRegion.page.height;
                float x = atlasRegion.x;
                float y = atlasRegion.y;
                float w = atlasRegion.width;
                float h = atlasRegion.height;
                mesh.Triangles = new[] { 1, 2, 3, 1, 3, 0 };
                mesh.UVs = new[]
                {
                    (x + w) / pw, 1 - ((y + h) / ph), (x + w) / pw, 1 - (y / ph), x / pw, 1 - (y / ph), x / pw,
                    1 - ((y + h) / ph)
                };
                mesh.WorldVerticesLength = 8;
            }

            skin.SetAttachment(slot, ovrName, mesh);
        }
    }

    public static List<Tuple<int, string, float, float, float, float>> CreateSkinAtlas(string name, Texture2D sheet,
        string atlasText, Func<AtlasRegion, Tuple<int, string>?> regionOverrideFunction, out Material skinMaterial,
        out SpineAtlasAsset atlasAsset)
    {
        sheet.name = atlasText.Replace("\r", "").Split('\n')[1].Trim();

        Material mat = new(Shader.Find("Spine/Skeleton"))
        {
            mainTexture = sheet
        };
        skinMaterial = mat;

        Material[] materials = { mat };
        var atlas = SpineAtlasAsset.CreateRuntimeInstance(new TextAsset(atlasText), materials, true);
        atlasAsset = atlas;

        List<Tuple<int, string>> overrideRegions = new();

        foreach (var region in atlas.GetAtlas().regions)
        {
            var ovr = regionOverrideFunction.Invoke(region);
            if (ovr != null) overrideRegions.Add(ovr);
            else Plugin.Instance!.Logger.LogError($"Failed to parse region with name: {region.name}");
        }

        List<Tuple<int, string, float, float, float, float>> overrides = new();
        List<AtlasRegion> list = atlas.GetAtlas().regions;
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
                        Plugin.Instance!.Logger.LogWarning($"Invalid scale length: {scale.Length}");
                        break;
                }

                atlasRegion.name = nameSplit[0];
            }

            overrides.Add(Tuple.Create(overrideRegions[index].Item1, overrideRegions[index].Item2, scale[0], scale[1],
                scale[2], scale[3]));
        }

        return overrides;
    }

    internal static void InvokeOnFindSkin()
    {
        OnFindSkin.Invoke();
        OnFindSkin = delegate { };
    }
}