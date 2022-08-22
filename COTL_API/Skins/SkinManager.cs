using BepInEx;
using HarmonyLib;
using Lamb.UI;
using LeTai.Asset.TranslucentImage;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using static Spine.Unity.AttachmentTools.AttachmentRegionExtensions;
using static WorshipperData;

namespace COTL_API.Skins
{
    internal class SkinManager
    {
        internal static Dictionary<string, SpineAtlasAsset> customAtlases = new();
        internal static Dictionary<string, Skin> customSkins = new();
        internal static Dictionary<string, Texture> skinTextures = new();

        internal static readonly List<Tuple<int, string, string>> SLOTS = new List<Tuple<int, string, string>>()
        {
            Tuple.Create(76, "HEAD_SKIN_BTM", "Head/HeadCustom_Btm"),
            Tuple.Create(76, "HEAD_SKIN_BTM_BACK", "Head/HeadCustomBack_Btm"),
            Tuple.Create(78, "HEAD_SKIN_TOP", "Head/HeadCustom_Top"),
            Tuple.Create(78, "HEAD_SKIN_TOP_BACK", "Head/HeadCustomBack_Top"),
            Tuple.Create(79, "MARKINGS", "Head/HeadCustomMarkings")
        };

        public static void AddCustomSkin(string name, Texture2D sheet, string atlasText) {

            sheet.name = atlasText.Replace("\r", "").Split('\n')[1].Trim();
            skinTextures.Add(name, sheet);

            Material mat = new Material(Shader.Find("Spine/Skeleton"));
            mat.mainTexture = sheet;
            Material[] materials = new Material[] { mat };
            SpineAtlasAsset atlas = SpineAtlasAsset.CreateRuntimeInstance(new TextAsset(atlasText), materials, true);
            customAtlases.Add(name, atlas);

            CreateNewFollowerType(name);
            CreateSkin(name);

        }

        internal static void CreateNewFollowerType(string name)
        {
            WorshipperData.Instance.Characters.Add(new WorshipperData.SkinAndData()
            {
                Title = name,
                Skin = new List<WorshipperData.CharacterSkin>()
                {
                    new WorshipperData.CharacterSkin()
                    {
                        Skin = name
                    }
                },
                SlotAndColours = WorshipperData.Instance.Characters[0].SlotAndColours,
                StartingSlotAndColours = WorshipperData.Instance.Characters[0].StartingSlotAndColours,
                TwitchPremium = false,
                _hidden = false,
                _dropLocation = WorshipperData.DropLocation.Other,
                _invariant = false
            });
        }

        internal static void CreateSkin(string name)
        {
            // Create skin
            var skin = new Skin(name);

            WorshipperData.Instance.SkeletonData.Skeleton.Data.FindSkin("Dog").Attachments.ToList().ForEach(att =>
            {
                if (!att.Name.Contains("HEAD"))
                {
                    skin.SetAttachment(att.SlotIndex, att.Name, att.Attachment.Copy());
                }
            });

            for (int i = 0; i < 4; i++)
            {
                var atlasRegion = customAtlases[name].GetAtlas().FindRegion(SLOTS[i].Item3);
                MeshAttachment customAttachment = (MeshAttachment) WorshipperData.Instance.SkeletonData.Skeleton.Data.FindSkin("Dog").GetAttachment(SLOTS[i].Item1, SLOTS[i].Item2).Copy();

                float minX = int.MaxValue;
                float maxX = int.MinValue;
                float minY = int.MaxValue;
                float maxY = int.MinValue;

                for (int j = 0; j < customAttachment.Vertices.Length; j++)
                {
                    if (j%3 == 0)
                    {
                        minY = Math.Min(minY, customAttachment.Vertices[j]);
                        maxY = Math.Max(maxY, customAttachment.Vertices[j]);
                    }
                    else if (j % 3 == 1)
                    {

                        minX = Math.Min(minX, customAttachment.Vertices[j]);
                        maxX = Math.Max(maxX, customAttachment.Vertices[j]);
                    }
                }

                customAttachment.Name = SLOTS[i].Item3;
                customAttachment.SetRegion(atlasRegion);
                customAttachment.HullLength = 4;
                customAttachment.Triangles = new int[] { 1, 2, 3, 1, 3, 0 };
                float pw = atlasRegion.page.width;
                float ph = atlasRegion.page.height;
                float x = atlasRegion.x;
                float y = atlasRegion.y;
                float w = atlasRegion.width;
                float h = atlasRegion.height;
                customAttachment.UVs = new float[] { (x+w)/pw, y/ph, (x+w)/pw, (y+h)/ph, x/pw, (y+h)/ph, x/pw, y/ph }; 
                customAttachment.Vertices = new float[] { minY, minX, 1, maxY, minX, 1, maxY, maxX, 1, minY, maxX, 1 };
                customAttachment.WorldVerticesLength = 8;

                skin.SetAttachment(SLOTS[i].Item1, SLOTS[i].Item2, customAttachment);
            }

            customSkins.Add(name, skin);
        }

        [HarmonyPatch(typeof(SkeletonData), "FindSkin", new Type[] { typeof(string) })]
        public class SkinPatch
        {
            public static void Postfix(ref Skin __result, SkeletonData __instance, string skinName)
            {
                if (__result == null)
                {
                    if (customSkins.ContainsKey(skinName))
                    {
                        DataManager.SetFollowerSkinUnlocked(skinName);
                        __result = customSkins[skinName];
                    }
                }
            }
        }

        [HarmonyPatch(typeof(IndoctrinationCharacterItem<IndoctrinationFormItem>), "Configure", new Type[] { typeof(SkinAndData) })]
        public class IndoctrinationFormPatch
        {
            public static void Postfix(IndoctrinationFormItem __instance)
            {
                if (skinTextures.ContainsKey(__instance.Skin))
                {
                    __instance._spine.overrideTexture = skinTextures[__instance.Skin];
                }
            }
        }

        [HarmonyPatch(typeof(IndoctrinationColourItem), "Configure", new Type[] { typeof(int), typeof(int), typeof(SkinAndData) })]
        public class IndoctrinationColourPatch
        {
            public static void Postfix(IndoctrinationColourItem __instance)
            {
                if (skinTextures.ContainsKey(__instance.Skin))
                {
                    __instance._spine.overrideTexture = skinTextures[__instance.Skin];
                }
            }
        }

        [HarmonyPatch(typeof(IndoctrinationVariantItem), "Configure", new Type[] { typeof(int), typeof(int), typeof(SkinAndData) })]
        public class IndoctrinationVariantPatch
        {
            public static void Postfix(IndoctrinationVariantItem __instance)
            {
                if (skinTextures.ContainsKey(__instance.Skin))
                {
                    __instance._spine.overrideTexture = skinTextures[__instance.Skin];
                }
            }
        }

        [HarmonyPatch(typeof(FollowerInformationBox), "ConfigureImpl")]
        public class FollowerInformationBoxPatch
        {
            public static void Postfix(FollowerInformationBox __instance)
            {
                if (skinTextures.ContainsKey(__instance.FollowerInfo.SkinName))
                {
                    __instance.FollowerSpine.overrideTexture = skinTextures[__instance.FollowerInfo.SkinName];
                }
            }
        }


        // TODO: Temp fix. Destroy the transparent image used when recruiting follower. It hides custom meshes due to render order.
        // Need to find out how to fix render order.
        [HarmonyPatch(typeof(UIFollowerIndoctrinationMenuController), "OnShowStarted")]
        public class TransparencyPatch
        {
            public static void Postfix(UIFollowerIndoctrinationMenuController __instance)
            {
                var image = __instance.gameObject.GetComponentsInChildren(typeof(TranslucentImage))[0].gameObject;
                UnityEngine.Object.Destroy(image);
            }   
        }
    }
}
