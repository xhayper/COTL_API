using BepInEx;
using HarmonyLib;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using static Spine.Unity.AttachmentTools.AttachmentRegionExtensions;

namespace COTL_API.Skins
{
    internal class SkinManager
    {
        public static List<Atlas> CustomAtlases { get; set; }

        public static void RunExperimentalCode()
        {
            // Adds a character with name Test and skin Test.
            WorshipperData.Instance.Characters.Add(new WorshipperData.SkinAndData()
            {
                Title = "Test",
                Skin = new List<WorshipperData.CharacterSkin>()
                {
                    new WorshipperData.CharacterSkin()
                    {
                        Skin = "Test"
                    }
                },
                SlotAndColours = WorshipperData.Instance.Characters[0].SlotAndColours,
                StartingSlotAndColours = WorshipperData.Instance.Characters[0].StartingSlotAndColours,
                TwitchPremium = false,
                _hidden = false,
                _dropLocation = WorshipperData.DropLocation.Other,
                _invariant = false
            });

            CustomAtlases = new List<Atlas>();

            // Create atlas
            Texture2D placeholder = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            byte[] imgBytes = File.ReadAllBytes(Paths.PluginPath + "/APIAssets/placeholder.png");
            placeholder.LoadImage(imgBytes);
            placeholder.name = "placeholder";
            Material m1 = new Material(Shader.Find("Spine/Skeleton"));
            m1.mainTexture = placeholder;
            Material[] materials = new Material[] {m1};
            Atlas a = SpineAtlasAsset.CreateRuntimeInstance(new TextAsset(File.ReadAllText(Paths.PluginPath + "/APIAssets/custom_skin_atlas.txt")), materials, true ).GetAtlas();

            CustomAtlases.Add(a);

            Plugin.logger.LogInfo(a.ToString());
        }

        [HarmonyPatch(typeof(SkeletonData), "FindSkin", new Type[] { typeof(string) })]
        public class SkinPatch
        {
            public static void Postfix(ref Skin __result, SkeletonData __instance, string skinName)
            {
                if (__result == null)
                {
                    // If name is Test, create new skin (should get from list in future)
                    if (skinName == "Test")
                    {
                        DataManager.SetFollowerSkinUnlocked(skinName); // Unlocks the Test skin
                        Skin skin = new Skin(skinName);
                        __instance.FindSkin("Dog").Attachments.ToList().ForEach(att =>
                        {
                            if (!att.Name.Contains("HEAD_SKIN")) {
                                // Copy all non-head attachments from Dog skin
                                skin.SetAttachment(att.SlotIndex, att.Name, att.Attachment);
                            }
                            else
                            {
                                // Create head
                                var atlasRegion = CustomAtlases[0].FindRegion("Head/HeadCustomBack_Btm");
                                MeshAttachment customAttachment = (MeshAttachment)att.Attachment.Copy();
                                customAttachment.SetRegion(atlasRegion);

                                customAttachment.HullLength = 4;
                                customAttachment.Triangles = new int[] { 0, 1, 2, 1, 2, 3 };
                                customAttachment.UVs = new float[] { 1, 1, 0, 1, 0, 0, 1, 0 };
                                customAttachment.RegionUVs = new float[] { 1, 1, 0, 1, 0, 0, 1, 0 };
                                customAttachment.Vertices = new float[] { -1, -1, 1, 1, -1, 1, 1, 1, 1, -1, 1,1 };
                                customAttachment.WorldVerticesLength = 4;

                                var ma = (MeshAttachment)customAttachment;
                                skin.SetAttachment(att.SlotIndex, att.Name, customAttachment);
                            }
                        });
                        __result = skin;
                    }
                }
            }
        }
    }
}
