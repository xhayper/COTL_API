using static Spine.Unity.AttachmentTools.AttachmentRegionExtensions;
using LeTai.Asset.TranslucentImage;
using System.Collections.Generic;
using Spine.Unity;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using Lamb.UI;
using System;
using Spine;
using Spine.Unity.AttachmentTools;
using System.IO;
using UnityEngine.Experimental.Rendering;

namespace COTL_API.CustomSkins;

[HarmonyPatch]
public class CustomSkinManager
{
    internal static readonly Dictionary<string, SpineAtlasAsset> CustomAtlases = new();
    internal static readonly Dictionary<string, Skin> CustomSkins = new();
    internal static readonly Dictionary<string, Texture2D> SkinTextures = new();
    internal static readonly Dictionary<string, Material> SkinMaterials = new();

    internal static readonly List<Tuple<int, string>> Slots = new() {
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
        Tuple.Create(85, "Face/EYE_FIRE7")
    };

    public static void AddCustomSkin(string name, Texture2D sheet, string atlasText)
    {
        sheet.name = atlasText.Replace("\r", "").Split('\n')[1].Trim();
        SkinTextures.Add(name, sheet);

        Material mat = new(Shader.Find("Spine/Skeleton")) {
            mainTexture = sheet
        };
        SkinMaterials.Add(name, mat);

        Material[] materials = { mat };
        SpineAtlasAsset atlas = SpineAtlasAsset.CreateRuntimeInstance(new TextAsset(atlasText), materials, true);
        CustomAtlases.Add(name, atlas);

        List<Tuple<int, string>> overrideRegions = atlas.GetAtlas().regions.Select(r => Slots.First(s =>
            (s.Item2 == r.name.Split(':')[1]) &&
            (s.Item1 == (int)(SlotsEnum)Enum.Parse(typeof(SlotsEnum), r.name.Split(':')[0])))).ToList();
        
        List<Tuple<int, string, float, float, float, float>> overrides = new();
        List<AtlasRegion> list = atlas.GetAtlas().regions;
        for (int index = 0; index < list.Count; index++)
        {
            float[] scale = new[] { 0f, 0f, 1f, 1f };
            AtlasRegion atlasRegion = list[index];
            string[] nameSplit = atlasRegion.name.Split(':');
            if (nameSplit.Length == 3)
            {
                string scales = nameSplit[2];
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
                    Plugin.Logger.LogWarning($"Invalid scale length: {scale.Length}");
                }
                atlasRegion.name = nameSplit[0] + ":" + nameSplit[1];
            }
            else if (nameSplit.Length > 3)
            {
                Plugin.Logger.LogWarning($"Invalid name length: {nameSplit.Length}");
            }
            overrides.Add(Tuple.Create(overrideRegions[index].Item1, overrideRegions[index].Item2, scale[0], scale[1], scale[2], scale[3]));
        }

        CreateNewFollowerType(name);
        CreateSkin(name, overrides);
    }

    internal static void CreateNewFollowerType(string name)
    {
        WorshipperData.Instance.Characters.Add(new WorshipperData.SkinAndData() {
            Title = name,
            Skin = new List<WorshipperData.CharacterSkin> {
                new() {
                    Skin = name
                }
            },
            SlotAndColours = new List<WorshipperData.SlotsAndColours> {
                new() {
                    SlotAndColours = new List<WorshipperData.SlotAndColor> {
                        new ("MARKINGS", new Color(1, 1, 0)),
                        new ("ARM_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("ARM_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("BODY_SKIN", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_BOWED", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_UP", new Color(1, 0.7f, 0))
                    },
                    AllColor = new Color(1, 0.7f, 0)
                },
                new() {
                    SlotAndColours = new List<WorshipperData.SlotAndColor> {
                        new ("MARKINGS", new Color(0, 1, 0)),
                        new ("ARM_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("ARM_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("BODY_SKIN", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_BOWED", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_UP", new Color(1, 0.7f, 0))
                    },
                    AllColor = new Color(1, 0.7f, 0)
                },
                new() {
                    SlotAndColours = new List<WorshipperData.SlotAndColor> {
                        new ("MARKINGS", new Color(0, 1, 1)),
                        new ("ARM_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("ARM_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("BODY_SKIN", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_BOWED", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_UP", new Color(1, 0.7f, 0))
                    },
                    AllColor = new Color(1, 0.7f, 0)
                },
                new() {
                    SlotAndColours = new List<WorshipperData.SlotAndColor> {
                        new ("MARKINGS", new Color(0, 0, 1)),
                        new ("ARM_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("ARM_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("BODY_SKIN", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_BOWED", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_UP", new Color(1, 0.7f, 0))
                    },
                    AllColor = new Color(1, 0.7f, 0)
                },
                new() {
                    SlotAndColours = new List<WorshipperData.SlotAndColor> {
                        new ("MARKINGS", new Color(1, 0, 1)),
                        new ("ARM_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("ARM_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("BODY_SKIN", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_BOWED", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_UP", new Color(1, 0.7f, 0))
                    },
                    AllColor = new Color(1, 0.7f, 0)
                },
                new() {
                    SlotAndColours = new List<WorshipperData.SlotAndColor> {
                        new ("MARKINGS", new Color(1, 0, 0)),
                        new ("ARM_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("ARM_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("BODY_SKIN", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_BOWED", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_UP", new Color(1, 0.7f, 0))
                    },
                    AllColor = new Color(1, 0.7f, 0)
                },
                new() {
                    SlotAndColours = new List<WorshipperData.SlotAndColor> {
                        new ("MARKINGS", new Color(1, 0.5f, 0)),
                        new ("ARM_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("ARM_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("BODY_SKIN", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_BOWED", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_UP", new Color(1, 0.7f, 0))
                    },
                    AllColor = new Color(1, 0.7f, 0)
                },
                new() {
                    SlotAndColours = new List<WorshipperData.SlotAndColor> {
                        new ("MARKINGS", new Color(0.5f, 0, 1f)),
                        new ("ARM_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("ARM_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("BODY_SKIN", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_BOWED", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_UP", new Color(1, 0.7f, 0))
                    },
                    AllColor = new Color(1, 0.7f, 0)
                },
                new() {
                    SlotAndColours = new List<WorshipperData.SlotAndColor> {
                        new ("MARKINGS", new Color(0.7f, 0.7f, 0.7f)),
                        new ("ARM_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("ARM_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("BODY_SKIN", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_BOWED", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_UP", new Color(1, 0.7f, 0))
                    },
                    AllColor = new Color(1, 0.7f, 0)
                },
                new() {
                    SlotAndColours = new List<WorshipperData.SlotAndColor> {
                        new ("MARKINGS", new Color(0.1f, 0.1f, 0.1f)),
                        new ("ARM_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("ARM_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("BODY_SKIN", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_BOWED", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_UP", new Color(1, 0.7f, 0))
                    },
                    AllColor = new Color(1, 0.7f, 0)
                }
            },
            StartingSlotAndColours = new List<WorshipperData.SlotsAndColours> {
                new() {
                    SlotAndColours = new List<WorshipperData.SlotAndColor> {
                        new ("MARKINGS", new Color(1, 1, 0)),
                        new ("ARM_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("ARM_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_LEFT_SKIN", new Color(1, 0.82f, 0)),
                        new ("LEG_RIGHT_SKIN", new Color(1, 0.82f, 0)),
                        new ("BODY_SKIN", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_BOWED", new Color(1, 0.7f, 0)),
                        new ("BODY_SKIN_UP", new Color(1, 0.7f, 0))
                    },
                    AllColor = new Color(1, 0.7f, 0)
                }
            },
            TwitchPremium = false,
            _hidden = false,
            _dropLocation = WorshipperData.DropLocation.Other,
            _invariant = false
        });
    }

    internal static void CreateSkin(string name, List<Tuple<int, string, float, float, float, float>> overrides)
    {
        // Create skin
        Skin skin = new(name);

        Skin dog = WorshipperData.Instance.SkeletonData.Skeleton.Data.FindSkin("Dog");
        dog.Attachments.ToList().ForEach(att =>
        {
            if (!overrides.Any(o => o.Item1 == att.SlotIndex && o.Item2 == att.Name))
            {
                skin.SetAttachment(att.SlotIndex, att.Name, att.Attachment.Copy());
            }
        });
        dog.Bones.ToList().ForEach(bone =>
        {
            skin.Bones.Add(bone);
        });
        dog.Constraints.ToList().ForEach(con =>
        {
            skin.Constraints.Add(con);
        });

        foreach (Tuple<int, string, float, float, float, float> ovr in overrides)
        {
            string ovrName = ovr.Item2;
            int slot = ovr.Item1;
            float translationX = ovr.Item3;
            float translationY = ovr.Item4;
            float scaleX = ovr.Item5;
            float scaleY = ovr.Item6;
            AtlasRegion atlasRegion = CustomAtlases[name].GetAtlas().FindRegion((SlotsEnum)slot + ":" + ovrName);
            Attachment a = WorshipperData.Instance.SkeletonData.Skeleton.Data.FindSkin("Dog")
                .GetAttachment(slot, ovrName).Copy();

            if (ovrName == "MARKINGS")
            {
                a = WorshipperData.Instance.SkeletonData.Skeleton.Data.FindSkin("Dog")
                    .GetAttachment(78, "HEAD_SKIN_TOP").Copy();
            }
            
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

                customAttachment.Name = "Custom" + ovrName;
                customAttachment.SetRegion(atlasRegion);
                atlasRegion.name = "Custom" + atlasRegion.name;
                customAttachment.HullLength = 4;
                customAttachment.Triangles = new[] { 1, 2, 3, 1, 3, 0 };
                float pw = atlasRegion.page.width;
                float ph = atlasRegion.page.height;
                float x = atlasRegion.x;
                float y = atlasRegion.y;
                float w = atlasRegion.width;
                float h = atlasRegion.height;
                customAttachment.UVs = new[]
                    { (x + w) / pw, y / ph, (x + w) / pw, (y + h) / ph, x / pw, (y + h) / ph, x / pw, y / ph };
                customAttachment.Vertices = new[] { minY, minX, 1, maxY, minX, 1, maxY, maxX, 1, minY, maxX, 1 };
                customAttachment.WorldVerticesLength = 8;

                skin.SetAttachment(slot, ovrName, customAttachment);
            }
            else
                Plugin.Logger.LogWarning(ovr + " is not a MeshAttachment. Skipping.");
        }
        
        
        Material runtimeMaterial;
        Texture2D runtimeTexture;
        
        var skin2 = skin.GetRepackedSkin(name, SkinMaterials[name], out runtimeMaterial, out runtimeTexture);
        
        foreach (Tuple<int, string, float, float, float, float> ovr in overrides)
        {
            string ovrName = ovr.Item2;
            int slot = ovr.Item1;
            Attachment a = skin2.GetAttachment(slot, ovrName).Copy();
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

                skin2.SetAttachment(slot, ovrName, mesh);
            }
        }
        
        CustomSkins.Add(name, skin2);
        
    }

    [HarmonyPatch(typeof(SkeletonData), nameof(SkeletonData.FindSkin), new[] { typeof(string) })]
    [HarmonyPostfix]
    public static void SkinPatch(ref Skin __result, SkeletonData __instance, string skinName)
    {
        if (__result != null) return;
        if (!CustomSkins.ContainsKey(skinName)) return;

        DataManager.SetFollowerSkinUnlocked(skinName);
        __result = CustomSkins[skinName];
    }
  
    [HarmonyPatch(typeof(Graphics), "CopyTexture", new[] { typeof(Texture), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(Texture), typeof(int), typeof(int), typeof(int), typeof(int) })]
    [HarmonyPrefix]
    public static bool Graphics_CopyTexture(ref Texture src, int srcElement, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight, ref Texture dst, int dstElement, int dstMip, int dstX, int dstY)
    {
        if (src is Texture2D s2d)
        {
            if (src.graphicsFormat != dst.graphicsFormat)
            {
                var orig = DuplicateTexture(s2d, dst.graphicsFormat);
                Texture2D dst2d = (Texture2D)dst;
                var fullPix = orig.GetPixels32();
                var croppedPix = new Color32[srcWidth * srcHeight];
                for (int i = 0; i < srcHeight; i++)
                {
                    for (int j = 0; j < srcWidth; j++)
                    {
                        croppedPix[(i * srcWidth) + j] = fullPix[((i + srcY) * orig.width) + j + srcX];
                    }
                }
                dst2d.SetPixels32(croppedPix);
            }
            return false;
        }

        return true;
    }
    
    private static Texture2D DuplicateTexture(Texture2D source, GraphicsFormat format)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
            source.width,
            source.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear
        );
 
        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new(source.width, source.height, format, TextureCreationFlags.None);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
    
    [HarmonyPatch(typeof(FollowerInformationBox), nameof(FollowerInformationBox.ConfigureImpl))]
    [HarmonyPostfix]
    public static void FollowerInformationBox_ConfigureImpl(FollowerInformationBox __instance) {
        if (SkinTextures.ContainsKey(__instance.FollowerInfo.SkinName))
            __instance.FollowerSpine.Skeleton.Skin = CustomSkins[__instance.FollowerInfo.SkinName];
    }

// TODO: Temp fix. Destroy the transparent image used when recruiting follower. It hides custom meshes due to render order.
// Need to find out how to fix render order.
    [HarmonyPatch(typeof(UIFollowerIndoctrinationMenuController),
        nameof(UIFollowerIndoctrinationMenuController.OnShowStarted))]
    [HarmonyPostfix]
    public static void UIFollowerIndoctrinationMenuController_OnShowStarted(
        UIFollowerIndoctrinationMenuController __instance)
    {
        GameObject image = __instance.gameObject.GetComponentsInChildren(typeof(TranslucentImage))[0].gameObject;
        UnityEngine.Object.Destroy(image);
    }
}