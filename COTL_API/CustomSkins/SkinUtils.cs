using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace COTL_API.CustomSkins;

internal class SkinUtils
{
    public static bool SkinsLoaded = false;
    
    public static event Action OnFindSkin = () =>
    {
        SkinToLoad.Invoke();
        SkinsLoaded = true;
    };
    
    public static Action SkinToLoad = () => { };

    public static void ApplyOverride(Skin skin, Attachment a, int slot, string ovrName, AtlasRegion atlasRegion, float scaleX, float scaleY, float translationX, float translationY)
    {
        if (a is MeshAttachment meshAttachment)
        {
                float minX = int.MaxValue;
                float maxX = int.MinValue;
                float minY = int.MaxValue;
                float maxY = int.MinValue;
                
                for (int j = 0; j < meshAttachment.Vertices.Length; j++)
                {
                    if (j % 3 == 0)
                    {
                        minY = Math.Min(minY, meshAttachment.Vertices[j]);
                        maxY = Math.Max(maxY, meshAttachment.Vertices[j]);
                    }
                    else if (j % 3 == 1)
                    {
                        minX = Math.Min(minX, meshAttachment.Vertices[j]);
                        maxX = Math.Max(maxX, meshAttachment.Vertices[j]);
                    }
                }
                
                float diffX = maxX - minX;
                float diffY = maxY - minY;

                minX += translationX;
                minY += translationY;
                
                float centerX = minX + (diffX / 2.0f);
                float centerY = minY + (diffY / 2.0f);
                
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
        }
        else if (a is RegionAttachment regionAttachment)
        {
            regionAttachment.Name = "Custom" + ovrName;
            atlasRegion.name = "Custom" + atlasRegion.name;
            
            regionAttachment.SetRegion(atlasRegion);
            
            regionAttachment.X += translationX;
            regionAttachment.Y += translationY;
            regionAttachment.ScaleX = scaleX;
            regionAttachment.ScaleY = scaleY;
            
            skin.SetAttachment(slot, ovrName, regionAttachment);
        }
        else
        {
            Plugin.Instance!.Logger.LogWarning($"Attachment {a.Name} is not a MeshAttachment or RegionAttachment, skipping...");
        }
    }

    public static Skin ApplyAllOverrides(Skin from, Skin to,
        List<Tuple<int, string, float, float, float, float>> overrides, Material material, AtlasAssetBase atlas)
    {
        string name = to.name;
        from.Attachments.ToList().ForEach(att =>
        {
            to.SetAttachment(att.SlotIndex, att.Name, att.Attachment.Copy());
        });
        from.Bones.ToList().ForEach(bone =>
        {
            to.Bones.Add(bone);
        });
        from.Constraints.ToList().ForEach(con =>
        {
            to.Constraints.Add(con);
        });

        foreach (Tuple<int, string, float, float, float, float> ovr in overrides)
        {
            string ovrName = ovr.Item2;
            int slot = ovr.Item1;
            float translationX = ovr.Item3;
            float translationY = ovr.Item4;
            float scaleX = ovr.Item5;
            float scaleY = ovr.Item6;
            AtlasRegion atlasRegion = atlas.GetAtlas().FindRegion(slot + ":" + ovrName);
            Attachment a = from.GetAttachment(slot, ovrName).Copy();

            SkinUtils.ApplyOverride(to, a, slot, ovrName, atlasRegion, scaleX, scaleY, translationX, translationY);
        }
        
        
        Material runtimeMaterial;
        Texture2D runtimeTexture;
        
        var skin2 = to.GetRepackedSkin(name, material, out runtimeMaterial, out runtimeTexture);
        CustomSkinManager._cachedTextures.Clear();
        AtlasUtilities.ClearCache();
        RepackMeshAttachments(skin2, overrides);
        
        foreach (Tuple<int, string, float, float, float, float> ovr in overrides)
        {
            string ovrName = ovr.Item2;
            int slot = ovr.Item1;
        }

        return skin2;
    }

    private static void RepackMeshAttachments(Skin skin, List<Tuple<int, string, float, float, float, float>> overrides)
    {
        foreach (Tuple<int, string, float, float, float, float> ovr in overrides)
        {
            string ovrName = ovr.Item2;
            int slot = ovr.Item1;
            Attachment a = skin.GetAttachment(slot, ovrName).Copy();
            if (a is MeshAttachment mesh)
            {
                AtlasRegion atlasRegion = mesh.RendererObject as AtlasRegion;
                if (atlasRegion != null)
                {
                    float pw = atlasRegion.page.width;
                    float ph = atlasRegion.page.height;
                    float x = atlasRegion.x;
                    float y = atlasRegion.y;
                    float w = atlasRegion.width;
                    float h = atlasRegion.height;
                    mesh.Triangles = new[] { 1, 2, 3, 1, 3, 0 };
                    mesh.UVs = new[]
                        { (x + w) / pw, 1-((y + h) / ph), (x + w) / pw, 1-(y / ph), x / pw, 1-(y / ph), x / pw, 1-((y + h) / ph) };
                    mesh.WorldVerticesLength = 8;
                }

                skin.SetAttachment(slot, ovrName, mesh);
            }
        }
    }

    public static List<Tuple<int, string, float, float, float, float>> CreateSkinAtlas(string name, Texture2D sheet, string atlasText, Func<AtlasRegion, Tuple<int, string>> regionOverrideFunction, out Material skinMaterial, out SpineAtlasAsset atlasAsset)
    {
        sheet.name = atlasText.Replace("\r", "").Split('\n')[1].Trim();

        Material mat = new(Shader.Find("Spine/Skeleton")) {
            mainTexture = sheet
        };
        skinMaterial = mat;

        Material[] materials = { mat };
        SpineAtlasAsset atlas = SpineAtlasAsset.CreateRuntimeInstance(new TextAsset(atlasText), materials, true);
        atlasAsset = atlas;

        List<Tuple<int, string>> overrideRegions = new();
        
        foreach (AtlasRegion region in atlas.GetAtlas().regions)
        {
            Tuple<int, string> ovr = regionOverrideFunction.Invoke(region);
            if (ovr != null) overrideRegions.Add(ovr);
            else Plugin.Instance!.Logger.LogError($"Failed to parse region with name: {region.name}");
        }
        
        List<Tuple<int, string, float, float, float, float>> overrides = new();
        List<AtlasRegion> list = atlas.GetAtlas().regions;
        for (int index = 0; index < list.Count; index++)
        {
            float[] scale = new[] { 0f, 0f, 1f, 1f };
            AtlasRegion atlasRegion = list[index];
            string[] nameSplit = atlasRegion.name.Split('#');
            if (nameSplit.Length == 2)
            {
                string scales = nameSplit[1];
                string[] scaleSplit = scales.Split(',');
                if (scaleSplit.Length == 2)
                {
                    scale[2] = float.Parse(scaleSplit[0]);
                    scale[3] = float.Parse(scaleSplit[1]);
                }
                else if (scaleSplit.Length == 4)
                {
                    scale[0] = float.Parse(scaleSplit[2]);
                    scale[1] = float.Parse(scaleSplit[3]);
                    scale[2] = float.Parse(scaleSplit[0]);
                    scale[3] = float.Parse(scaleSplit[1]);
                }
                else
                {
                    Plugin.Instance!.Logger.LogWarning($"Invalid scale length: {scale.Length}");
                }

                atlasRegion.name = nameSplit[0];
            }
            overrides.Add(Tuple.Create(overrideRegions[index].Item1, overrideRegions[index].Item2, scale[0], scale[1], scale[2], scale[3]));
        }

        return overrides;
    }

    internal static void InvokeOnFindSkin()
    {
        OnFindSkin.Invoke();
        OnFindSkin = delegate {  };
    }
}