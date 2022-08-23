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
        internal static Dictionary<string, Material> skinMaterials = new();

        internal static readonly List<Tuple<int, string>> SLOTS = new List<Tuple<int, string>>()
        {
            Tuple.Create(38, "ARM_LEFT_SKIN"),
            Tuple.Create(39, "Body/SleeveLeft"),
            Tuple.Create(49, "WEAPON_HAND_SKIN"),
            Tuple.Create(50, "LEG_LEFT_SKIN"),
            Tuple.Create(51, "LEG_RIGHT_SKIN"),
            Tuple.Create(54, "BODY_SKIN"),
            Tuple.Create(54, "BODY_SKIN_BOWED"),
            Tuple.Create(54, "BODY_SKIN_UP"),
            Tuple.Create(54, "Body/Body_Lvl3"),
            Tuple.Create(59, "BowlBtm"),
            Tuple.Create(60, "BowlFood"),
            Tuple.Create(61, "BowlFront"),
            Tuple.Create(67, "ARM_RIGHT_SKIN"),
            Tuple.Create(68, "Body/SleeveRight"),
            Tuple.Create(76, "HEAD_SKIN_BTM"),
            Tuple.Create(76, "HEAD_SKIN_BTM_BACK"),
            Tuple.Create(78, "HEAD_SKIN_TOP"),
            Tuple.Create(78, "HEAD_SKIN_TOP_BACK"),
            Tuple.Create(79, "MARKINGS"),
            Tuple.Create(81, "Angry_Colouring"),
            Tuple.Create(81, "Embarrassed_Colouring"),
            Tuple.Create(81, "Possessed_Colouring"),
            Tuple.Create(81, "Sick_Colouring"),
            Tuple.Create(82, "Face/MOUTH_BEDREST"),
            Tuple.Create(82, "Face/MOUTH_CHEEKY"),
            Tuple.Create(82, "Face/MOUTH_DEAD"),
            Tuple.Create(82, "Face/MOUTH_DERP"),
            Tuple.Create(82, "Face/MOUTH_ENLIGHTENED"),
            Tuple.Create(82, "Face/MOUTH_GRIN"),
            Tuple.Create(82, "Face/MOUTH_HAPPY"),
            Tuple.Create(82, "Face/MOUTH_HAPPY-2"),
            Tuple.Create(82, "Face/MOUTH_HUNGRY1"),
            Tuple.Create(82, "Face/MOUTH_HUNGRY2"),
            Tuple.Create(82, "Face/MOUTH_INDIFFERENT"),
            Tuple.Create(82, "Face/MOUTH_INDIFFERENT_HUNGRY"),
            Tuple.Create(82, "Face/MOUTH_INSANE"),
            Tuple.Create(82, "Face/MOUTH_KISS"),
            Tuple.Create(82, "Face/MOUTH_KISS_BIG"),
            Tuple.Create(82, "Face/MOUTH_MUMBLE"),
            Tuple.Create(82, "Face/MOUTH_MUMBLE_HUNGRY"),
            Tuple.Create(82, "Face/MOUTH_RED"),
            Tuple.Create(82, "Face/MOUTH_SACRIFICE"),
            Tuple.Create(82, "Face/MOUTH_SAD"),
            Tuple.Create(82, "Face/MOUTH_SADDER"),
            Tuple.Create(82, "Face/MOUTH_SCARED"),
            Tuple.Create(82, "Face/MOUTH_SICK"),
            Tuple.Create(82, "Face/MOUTH_SLEEP_0"),
            Tuple.Create(82, "Face/MOUTH_SLEEP_1"),
            Tuple.Create(82, "Face/MOUTH_TALK_HAPPY"),
            Tuple.Create(82, "Face/MOUTH_TALK_INDIFFERENT"),
            Tuple.Create(82, "Face/MOUTH_TONGUE_1"),
            Tuple.Create(82, "Face/MOUTH_TONGUE_2"),
            Tuple.Create(82, "Face/MOUTH_TONGUE_3"),
            Tuple.Create(82, "Face/MOUTH_TONGUE_4"),
            Tuple.Create(82, "Face/MOUTH_WORRIED"),
            Tuple.Create(82, "MOUTH_ENLIGHTENED2"),
            Tuple.Create(84, "EYE"),
            Tuple.Create(84, "EYE_ANGRY"),
            Tuple.Create(84, "EYE_ANGRY_UP"),
            Tuple.Create(84, "EYE_BLACK"),
            Tuple.Create(84, "EYE_BRAINWASHED"),
            Tuple.Create(84, "EYE_CLOSED"),
            Tuple.Create(84, "EYE_CRAZY"),
            Tuple.Create(84, "EYE_DEAD"),
            Tuple.Create(84, "EYE_DISSENTER"),
            Tuple.Create(84, "EYE_DISSENTER_ANGRY"),
            Tuple.Create(84, "EYE_ENLIGHTENED"),
            Tuple.Create(84, "EYE_HALF_CLOSED"),
            Tuple.Create(84, "EYE_HALF_CLOSED_ANGRY"),
            Tuple.Create(84, "EYE_INSANE"),
            Tuple.Create(84, "EYE_SACRIFICE_1"),
            Tuple.Create(84, "EYE_SACRIFICE_2"),
            Tuple.Create(84, "EYE_SHOCKED"),
            Tuple.Create(84, "EYE_SICK_2_RIGHT"),
            Tuple.Create(84, "EYE_SICK_RIGHT"),
            Tuple.Create(84, "EYE_SLEEPING"),
            Tuple.Create(84, "EYE_SLEEPING_SICK"),
            Tuple.Create(84, "EYE_SLEEPING_TIRED"),
            Tuple.Create(84, "EYE_SLEEPY_RIGHT"),
            Tuple.Create(84, "EYE_SMILE"),
            Tuple.Create(84, "EYE_SMILE_DOWN"),
            Tuple.Create(84, "EYE_SMILE_UP"),
            Tuple.Create(84, "EYE_SQUINT"),
            Tuple.Create(84, "EYE_STARS"),
            Tuple.Create(84, "EYE_UNCONVERTED"),
            Tuple.Create(84, "EYE_UP"),
            Tuple.Create(84, "EYE_VERY_ANGRY"),
            Tuple.Create(84, "EYE_WHITE"),
            Tuple.Create(84, "EYE_WORRIED_RIGHT"),
            Tuple.Create(84, "Face/EYE_FIRE1"),
            Tuple.Create(84, "Face/EYE_FIRE2"),
            Tuple.Create(84, "Face/EYE_FIRE3"),
            Tuple.Create(84, "Face/EYE_FIRE4"),
            Tuple.Create(84, "Face/EYE_FIRE5"),
            Tuple.Create(84, "Face/EYE_FIRE6"),
            Tuple.Create(84, "Face/EYE_FIRE7"),
            Tuple.Create(85, "EYE"),
            Tuple.Create(85, "EYE_ANGRY_LEFT"),
            Tuple.Create(85, "EYE_ANGRY_UP_LEFT"),
            Tuple.Create(85, "EYE_BLACK"),
            Tuple.Create(85, "EYE_BRAINWASHED"),
            Tuple.Create(85, "EYE_CLOSED"),
            Tuple.Create(85, "EYE_CRAZY"),
            Tuple.Create(85, "EYE_DEAD"),
            Tuple.Create(85, "EYE_DISSENTER"),
            Tuple.Create(85, "EYE_DISSENTER_ANGRY"),
            Tuple.Create(85, "EYE_ENLIGHTENED"),
            Tuple.Create(85, "EYE_HALF_CLOSED"),
            Tuple.Create(85, "EYE_HALF_CLOSED_ANGRY_LEFT"),
            Tuple.Create(85, "EYE_INSANE_LEFT"),
            Tuple.Create(85, "EYE_SACRIFICE_1"),
            Tuple.Create(85, "EYE_SACRIFICE_2"),
            Tuple.Create(85, "EYE_SHOCKED"),
            Tuple.Create(85, "EYE_SICK_2_LEFT"),
            Tuple.Create(85, "EYE_SICK_LEFT"),
            Tuple.Create(85, "EYE_SLEEPING"),
            Tuple.Create(85, "EYE_SLEEPING_SICK"),
            Tuple.Create(85, "EYE_SLEEPING_TIRED"),
            Tuple.Create(85, "EYE_SLEEPY_LEFT"),
            Tuple.Create(85, "EYE_SMILE"),
            Tuple.Create(85, "EYE_SMILE_DOWN"),
            Tuple.Create(85, "EYE_SMILE_UP"),
            Tuple.Create(85, "EYE_SQUINT"),
            Tuple.Create(85, "EYE_STARS"),
            Tuple.Create(85, "EYE_UNCONVERTED"),
            Tuple.Create(85, "EYE_UP"),
            Tuple.Create(85, "EYE_VERY_ANGRY"),
            Tuple.Create(85, "EYE_WHITE"),
            Tuple.Create(85, "EYE_WORRIED_LEFT"),
            Tuple.Create(85, "Face/EYE_FIRE1"),
            Tuple.Create(85, "Face/EYE_FIRE2"),
            Tuple.Create(85, "Face/EYE_FIRE3"),
            Tuple.Create(85, "Face/EYE_FIRE4"),
            Tuple.Create(85, "Face/EYE_FIRE5"),
            Tuple.Create(85, "Face/EYE_FIRE6"),
            Tuple.Create(85, "Face/EYE_FIRE7"),
        };

        public static void AddCustomSkin(string name, Texture2D sheet, string atlasText) {

            sheet.name = atlasText.Replace("\r", "").Split('\n')[1].Trim();
            skinTextures.Add(name, sheet);

            Material mat = new Material(Shader.Find("Spine/Skeleton"));
            mat.mainTexture = sheet;
            skinMaterials.Add(name, mat);

            Material[] materials = new Material[] { mat };
            SpineAtlasAsset atlas = SpineAtlasAsset.CreateRuntimeInstance(new TextAsset(atlasText), materials, true);
            customAtlases.Add(name, atlas);

            List<string> overrides = atlas.GetAtlas().regions.Select(r => SLOTS.First(s => s.Item2 == r.name).Item2).ToList();

            CreateNewFollowerType(name);
            CreateSkin(name, overrides);

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

        internal static void CreateSkin(string name, List<string> overrides)
        {
            // Create skin
            var skin = new Skin(name);

            WorshipperData.Instance.SkeletonData.Skeleton.Data.FindSkin("Dog").Attachments.ToList().ForEach(att =>
            {
                if (!overrides.Contains(att.Name))
                {
                    skin.SetAttachment(att.SlotIndex, att.Name, att.Attachment.Copy());
                }
            });

            for (int i = 0; i < overrides.Count; i++)
            {
                string or = overrides[i];
                var slot = SLOTS.First(s => or == s.Item2).Item1;
                var atlasRegion = customAtlases[name].GetAtlas().FindRegion(or);
                Attachment a = WorshipperData.Instance.SkeletonData.Skeleton.Data.FindSkin("Dog").GetAttachment(slot, or).Copy();
                if (a is MeshAttachment customAttachment)
                {

                    float minX = int.MaxValue;
                    float maxX = int.MinValue;
                    float minY = int.MaxValue;
                    float maxY = int.MinValue;

                    for (int j = 0; j < customAttachment.Vertices.Length; j++)
                    {
                        if (j % 3 == 0)
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

                    customAttachment.Name = "Custom" + or;
                    customAttachment.SetRegion(atlasRegion);
                    atlasRegion.name = "Custom" + atlasRegion.name;
                    customAttachment.HullLength = 4;
                    customAttachment.Triangles = new int[] { 1, 2, 3, 1, 3, 0 };
                    float pw = atlasRegion.page.width;
                    float ph = atlasRegion.page.height;
                    float x = atlasRegion.x;
                    float y = atlasRegion.y;
                    float w = atlasRegion.width;
                    float h = atlasRegion.height;
                    customAttachment.UVs = new float[] { (x + w) / pw, y / ph, (x + w) / pw, (y + h) / ph, x / pw, (y + h) / ph, x / pw, y / ph };
                    customAttachment.Vertices = new float[] { minY, minX, 1, maxY, minX, 1, maxY, maxX, 1, minY, maxX, 1 };
                    customAttachment.WorldVerticesLength = 8;

                    skin.SetAttachment(slot, or, customAttachment);
                }
                else
                {
                    Plugin.logger.LogWarning(overrides[i] + " is not a MeshAttachment. Skipping.");
                }
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

        [HarmonyPatch(typeof(MeshGenerator), "GenerateSingleSubmeshInstruction")]
        public class MeshPatch
        {
            public static void Postfix(ref SkeletonRendererInstruction instructionOutput)
            {
                if (instructionOutput.submeshInstructions.Items[0].skeleton.Skin != null && instructionOutput.attachments.Exists(att => att != null && att.Name.StartsWith("Custom")))
                {
                    instructionOutput.submeshInstructions.Items[0].material = skinMaterials[instructionOutput.submeshInstructions.Items[0].skeleton.Skin.Name];
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
                    __instance.FollowerSpine.Skeleton.Skin = (customSkins[__instance.FollowerInfo.SkinName]);
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
