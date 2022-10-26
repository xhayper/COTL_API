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

    internal static readonly Dictionary<string, Tuple<int, string>> SimplifiedSkinNames = new() {
        { "LEFT_ARM_SKIN", Tuple.Create(38, "ARM_LEFT_SKIN") },
        { "LEFT_SLEEVE", Tuple.Create(39, "Body/SleeveLeft") },
        { "WEAPON_HAND_SKIN", Tuple.Create(49, "WEAPON_HAND_SKIN") },
        { "LEFT_LEG_SKIN", Tuple.Create(50, "LEG_LEFT_SKIN") },
        { "RIGHT_LEG_SKIN", Tuple.Create(51, "LEG_RIGHT_SKIN") },
        { "BODY_SKIN", Tuple.Create(54, "BODY_SKIN") },
        { "BODY_SKIN_BOWED", Tuple.Create(54, "BODY_SKIN_BOWED") },
        { "BODY_SKIN_UP", Tuple.Create(54, "BODY_SKIN_UP") },
        { "Body_Lvl3", Tuple.Create(54, "Body/Body_Lvl3") },
        { "BowlBtm", Tuple.Create(59, "BowlBtm") },
        { "BowlFood", Tuple.Create(60, "BowlFood") },
        { "BowlFront", Tuple.Create(61, "BowlFront") },
        { "RIGHT_ARM_SKIN", Tuple.Create(67, "ARM_RIGHT_SKIN") },
        { "RIGHT_SLEEVE", Tuple.Create(68, "Body/SleeveRight") },
        { "HEAD_SKIN_BTM", Tuple.Create(76, "HEAD_SKIN_BTM") },
        { "HEAD_SKIN_BTM_BACK", Tuple.Create(76, "HEAD_SKIN_BTM_BACK") },
        { "HEAD_SKIN_TOP", Tuple.Create(78, "HEAD_SKIN_TOP") },
        { "HEAD_SKIN_TOP_BACK", Tuple.Create(78, "HEAD_SKIN_TOP_BACK") },
        { "MARKINGS", Tuple.Create(79, "MARKINGS") },
        { "Angry_Colouring", Tuple.Create(81, "Angry_Colouring") },
        { "Embarrassed_Colouring", Tuple.Create(81, "Embarrassed_Colouring") },
        { "Possessed_Colouring", Tuple.Create(81, "Possessed_Colouring") },
        { "Sick_Colouring", Tuple.Create(81, "Sick_Colouring") },
        { "MOUTH_BEDREST", Tuple.Create(82, "Face/MOUTH_BEDREST") },
        { "MOUTH_CHEEKY", Tuple.Create(82, "Face/MOUTH_CHEEKY") },
        { "MOUTH_DEAD", Tuple.Create(82, "Face/MOUTH_DEAD") },
        { "MOUTH_DERP", Tuple.Create(82, "Face/MOUTH_DERP") },
        { "MOUTH_ENLIGHTENED", Tuple.Create(82, "Face/MOUTH_ENLIGHTENED") },
        { "MOUTH_GRIN", Tuple.Create(82, "Face/MOUTH_GRIN") },
        { "MOUTH_HAPPY", Tuple.Create(82, "Face/MOUTH_HAPPY") },
        { "MOUTH_HAPPY_2", Tuple.Create(82, "Face/MOUTH_HAPPY-2") },
        { "MOUTH_HUNGRY_1", Tuple.Create(82, "Face/MOUTH_HUNGRY1") },
        { "MOUTH_HUNGRY_2", Tuple.Create(82, "Face/MOUTH_HUNGRY2") },
        { "MOUTH_INDIFFERENT", Tuple.Create(82, "Face/MOUTH_INDIFFERENT") },
        { "MOUTH_INDIFFERENT_HUNGRY", Tuple.Create(82, "Face/MOUTH_INDIFFERENT_HUNGRY") },
        { "MOUTH_INSANE", Tuple.Create(82, "Face/MOUTH_INSANE") },
        { "MOUTH_KISS", Tuple.Create(82, "Face/MOUTH_KISS") },
        { "MOUTH_KISS_BIG", Tuple.Create(82, "Face/MOUTH_KISS_BIG") },
        { "MOUTH_MUMBLE", Tuple.Create(82, "Face/MOUTH_MUMBLE") },
        { "MOUTH_MUMBLE_HUNGRY", Tuple.Create(82, "Face/MOUTH_MUMBLE_HUNGRY") },
        { "MOUTH_RED", Tuple.Create(82, "Face/MOUTH_RED") },
        { "MOUTH_SACRIFICE", Tuple.Create(82, "Face/MOUTH_SACRIFICE") },
        { "MOUTH_SAD", Tuple.Create(82, "Face/MOUTH_SAD") },
        { "MOUTH_SADDER", Tuple.Create(82, "Face/MOUTH_SADDER") },
        { "MOUTH_SCARED", Tuple.Create(82, "Face/MOUTH_SCARED") },
        { "MOUTH_SICK", Tuple.Create(82, "Face/MOUTH_SICK") },
        { "MOUTH_SLEEP_0", Tuple.Create(82, "Face/MOUTH_SLEEP_0") },
        { "MOUTH_SLEEP_1", Tuple.Create(82, "Face/MOUTH_SLEEP_1") },
        { "MOUTH_TALK_HAPPY", Tuple.Create(82, "Face/MOUTH_TALK_HAPPY") },
        { "MOUTH_TALK_INDIFFERENT", Tuple.Create(82, "Face/MOUTH_TALK_INDIFFERENT") },
        { "MOUTH_TONGUE_1", Tuple.Create(82, "Face/MOUTH_TONGUE_1") },
        { "MOUTH_TONGUE_2", Tuple.Create(82, "Face/MOUTH_TONGUE_2") },
        { "MOUTH_TONGUE_3", Tuple.Create(82, "Face/MOUTH_TONGUE_3") },
        { "MOUTH_TONGUE_4", Tuple.Create(82, "Face/MOUTH_TONGUE_4") },
        { "MOUTH_WORRIED", Tuple.Create(82, "Face/MOUTH_WORRIED") },
        { "MOUTH_ENLIGHTENED_2", Tuple.Create(82, "MOUTH_ENLIGHTENED2") },
        { "RIGHT_EYE", Tuple.Create(84, "EYE") },
        { "RIGHT_EYE_ANGRY", Tuple.Create(84, "EYE_ANGRY") },
        { "RIGHT_EYE_ANGRY_UP", Tuple.Create(84, "EYE_ANGRY_UP") },
        { "RIGHT_EYE_BLACK", Tuple.Create(84, "EYE_BLACK") },
        { "RIGHT_EYE_BRAINWASHED", Tuple.Create(84, "EYE_BRAINWASHED") },
        { "RIGHT_EYE_CLOSED", Tuple.Create(84, "EYE_CLOSED") },
        { "RIGHT_EYE_CRAZY", Tuple.Create(84, "EYE_CRAZY") },
        { "RIGHT_EYE_DEAD", Tuple.Create(84, "EYE_DEAD") },
        { "RIGHT_EYE_DISSENTER", Tuple.Create(84, "EYE_DISSENTER") },
        { "RIGHT_EYE_DISSENTER_ANGRY", Tuple.Create(84, "EYE_DISSENTER_ANGRY") },
        { "RIGHT_EYE_ENLIGHTENED", Tuple.Create(84, "EYE_ENLIGHTENED") },
        { "RIGHT_EYE_HALF_CLOSED", Tuple.Create(84, "EYE_HALF_CLOSED") },
        { "RIGHT_EYE_HALF_CLOSED_ANGRY", Tuple.Create(84, "EYE_HALF_CLOSED_ANGRY") },
        { "RIGHT_EYE_INSANE", Tuple.Create(84, "EYE_INSANE") },
        { "RIGHT_EYE_SACRIFICE_1", Tuple.Create(84, "EYE_SACRIFICE_1") },
        { "RIGHT_EYE_SACRIFICE_2", Tuple.Create(84, "EYE_SACRIFICE_2") },
        { "RIGHT_EYE_SHOCKED", Tuple.Create(84, "EYE_SHOCKED") },
        { "RIGHT_EYE_SICK_2", Tuple.Create(84, "EYE_SICK_2_RIGHT") },
        { "RIGHT_EYE_SICK", Tuple.Create(84, "EYE_SICK_RIGHT") },
        { "RIGHT_EYE_SLEEPING", Tuple.Create(84, "EYE_SLEEPING") },
        { "RIGHT_EYE_SLEEPING_SICK", Tuple.Create(84, "EYE_SLEEPING_SICK") },
        { "RIGHT_EYE_SLEEPING_TIRED", Tuple.Create(84, "EYE_SLEEPING_TIRED") },
        { "RIGHT_EYE_SLEEPY", Tuple.Create(84, "EYE_SLEEPY_RIGHT") },
        { "RIGHT_EYE_SMILE", Tuple.Create(84, "EYE_SMILE") },
        { "RIGHT_EYE_SMILE_DOWN", Tuple.Create(84, "EYE_SMILE_DOWN") },
        { "RIGHT_EYE_SMILE_UP", Tuple.Create(84, "EYE_SMILE_UP") },
        { "RIGHT_EYE_SQUINT", Tuple.Create(84, "EYE_SQUINT") },
        { "RIGHT_EYE_STARS", Tuple.Create(84, "EYE_STARS") },
        { "RIGHT_EYE_UNCONVERTED", Tuple.Create(84, "EYE_UNCONVERTED") },
        { "RIGHT_EYE_UP", Tuple.Create(84, "EYE_UP") },
        { "RIGHT_EYE_VERY_ANGRY", Tuple.Create(84, "EYE_VERY_ANGRY") },
        { "RIGHT_EYE_WHITE", Tuple.Create(84, "EYE_WHITE") },
        { "RIGHT_EYE_WORRIED", Tuple.Create(84, "EYE_WORRIED_RIGHT") },
        { "RIGHT_EYE_FIRE_1", Tuple.Create(84, "Face/EYE_FIRE1") },
        { "RIGHT_EYE_FIRE_2", Tuple.Create(84, "Face/EYE_FIRE2") },
        { "RIGHT_EYE_FIRE_3", Tuple.Create(84, "Face/EYE_FIRE3") },
        { "RIGHT_EYE_FIRE_4", Tuple.Create(84, "Face/EYE_FIRE4") },
        { "RIGHT_EYE_FIRE_5", Tuple.Create(84, "Face/EYE_FIRE5") },
        { "RIGHT_EYE_FIRE_6", Tuple.Create(84, "Face/EYE_FIRE6") },
        { "RIGHT_EYE_FIRE_7", Tuple.Create(84, "Face/EYE_FIRE7") },
        { "LEFT_EYE", Tuple.Create(85, "EYE") },
        { "LEFT_EYE_ANGRY", Tuple.Create(85, "EYE_ANGRY_LEFT") },
        { "LEFT_EYE_ANGRY_UP", Tuple.Create(85, "EYE_ANGRY_UP_LEFT") },
        { "LEFT_EYE_BLACK", Tuple.Create(85, "EYE_BLACK") },
        { "LEFT_EYE_BRAINWASHED", Tuple.Create(85, "EYE_BRAINWASHED") },
        { "LEFT_EYE_CLOSED", Tuple.Create(85, "EYE_CLOSED") },
        { "LEFT_EYE_CRAZY", Tuple.Create(85, "EYE_CRAZY") },
        { "LEFT_EYE_DEAD", Tuple.Create(85, "EYE_DEAD") },
        { "LEFT_EYE_DISSENTER", Tuple.Create(85, "EYE_DISSENTER") },
        { "LEFT_EYE_DISSENTER_ANGRY", Tuple.Create(85, "EYE_DISSENTER_ANGRY") },
        { "LEFT_EYE_ENLIGHTENED", Tuple.Create(85, "EYE_ENLIGHTENED") },
        { "LEFT_EYE_HALF_CLOSED", Tuple.Create(85, "EYE_HALF_CLOSED") },
        { "LEFT_EYE_HALF_CLOSED_ANGRY", Tuple.Create(85, "EYE_HALF_CLOSED_ANGRY_LEFT") },
        { "LEFT_EYE_INSANE", Tuple.Create(85, "EYE_INSANE_LEFT") },
        { "LEFT_EYE_SACRIFICE_1", Tuple.Create(85, "EYE_SACRIFICE_1") },
        { "LEFT_EYE_SACRIFICE_2", Tuple.Create(85, "EYE_SACRIFICE_2") },
        { "LEFT_EYE_SHOCKED", Tuple.Create(85, "EYE_SHOCKED") },
        { "LEFT_EYE_SICK_2", Tuple.Create(85, "EYE_SICK_2_LEFT") },
        { "LEFT_EYE_SICK", Tuple.Create(85, "EYE_SICK_LEFT") },
        { "LEFT_EYE_SLEEPING", Tuple.Create(85, "EYE_SLEEPING") },
        { "LEFT_EYE_SLEEPING_SICK", Tuple.Create(85, "EYE_SLEEPING_SICK") },
        { "LEFT_EYE_SLEEPING_TIRED", Tuple.Create(85, "EYE_SLEEPING_TIRED") },
        { "LEFT_EYE_SLEEPY", Tuple.Create(85, "EYE_SLEEPY_LEFT") },
        { "LEFT_EYE_SMILE", Tuple.Create(85, "EYE_SMILE") },
        { "LEFT_EYE_SMILE_DOWN", Tuple.Create(85, "EYE_SMILE_DOWN") },
        { "LEFT_EYE_SMILE_UP", Tuple.Create(85, "EYE_SMILE_UP") },
        { "LEFT_EYE_SQUINT", Tuple.Create(85, "EYE_SQUINT") },
        { "LEFT_EYE_STARS", Tuple.Create(85, "EYE_STARS") },
        { "LEFT_EYE_UNCONVERTED", Tuple.Create(85, "EYE_UNCONVERTED") },
        { "LEFT_EYE_UP", Tuple.Create(85, "EYE_UP") },
        { "LEFT_EYE_VERY_ANGRY", Tuple.Create(85, "EYE_VERY_ANGRY") },
        { "LEFT_EYE_WHITE", Tuple.Create(85, "EYE_WHITE") },
        { "LEFT_EYE_WORRIED", Tuple.Create(85, "EYE_WORRIED_LEFT") },
        { "LEFT_EYE_FIRE_1", Tuple.Create(85, "Face/EYE_FIRE1") },
        { "LEFT_EYE_FIRE_2", Tuple.Create(85, "Face/EYE_FIRE2") },
        { "LEFT_EYE_FIRE_3", Tuple.Create(85, "Face/EYE_FIRE3") },
        { "LEFT_EYE_FIRE_4", Tuple.Create(85, "Face/EYE_FIRE4") },
        { "LEFT_EYE_FIRE_5", Tuple.Create(85, "Face/EYE_FIRE5") },
        { "LEFT_EYE_FIRE_6", Tuple.Create(85, "Face/EYE_FIRE6") },
        { "LEFT_EYE_FIRE_7", Tuple.Create(85, "Face/EYE_FIRE7") }
    };
    
    public static void Add(CustomSkin skin)
    {
        string atlasText = skin.GenerateAtlasText();
        AddCustomSkin(skin.Name, skin.Texture, atlasText, skin.Colors, skin.Hidden, skin.Unlocked, skin.TwitchPremium,
            skin.Invariant);
    }

    public static void AddCustomSkin(string name, Texture2D sheet, string atlasText, List<WorshipperData.SlotsAndColours> colors, bool hidden = false, bool unlocked = true, bool twitchPremium = false, bool invariant = false)
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

        List<Tuple<int, string>> overrideRegions = new();
        
        foreach (AtlasRegion region in atlas.GetAtlas().regions)
        {
            string simpleName = region.name;
            if (simpleName.Contains("#")) simpleName = simpleName.Split('#')[0];
            
            if (SimplifiedSkinNames.TryGetValue(simpleName, out Tuple<int, string> simplified))
            {
                overrideRegions.Add(simplified);
                region.name = (SlotsEnum)simplified.Item1 + ":" + simplified.Item2;
                continue;
            }
            else if (simpleName.Contains(":"))
            {
                try {
                    string rName = simpleName.Split(':')[1];
                    int regionIndex = (int)(SlotsEnum)Enum.Parse(typeof(SlotsEnum), simpleName.Split(':')[0]);
                    overrideRegions.Add(Tuple.Create(regionIndex, rName));
                    continue;
                } catch (Exception e) {
                    // ignored
                }
            }
            Plugin.Logger.LogError($"Failed to parse region with name: {simpleName}");
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
                    Plugin.Logger.LogWarning($"Invalid scale length: {scale.Length}");
                }

                atlasRegion.name = nameSplit[0];
            }
            overrides.Add(Tuple.Create(overrideRegions[index].Item1, overrideRegions[index].Item2, scale[0], scale[1], scale[2], scale[3]));
        }

        CreateNewFollowerType(name, colors, hidden, unlocked, twitchPremium, invariant);
        CreateSkin(name, overrides);
    }

    internal static void CreateNewFollowerType(string name, List<WorshipperData.SlotsAndColours> colors, bool hidden = false, bool unlocked = true, bool twitchPremium = false, bool invariant = false)
    {
        WorshipperData.Instance.Characters.Add(new WorshipperData.SkinAndData() {
            Title = name,
            Skin = new List<WorshipperData.CharacterSkin> {
                new() {
                    Skin = name
                }
            },
            SlotAndColours = colors,
            TwitchPremium = twitchPremium,
            _hidden = hidden,
            _dropLocation = WorshipperData.DropLocation.Other,
            _invariant = invariant,
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
        _cachedTextures.Clear();
        AtlasUtilities.ClearCache();
        
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
    
    private static Dictionary<string, Texture2D> _cachedTextures = new Dictionary<string, Texture2D>();

    [HarmonyPatch(typeof(Graphics), "CopyTexture", new[] { typeof(Texture), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(Texture), typeof(int), typeof(int), typeof(int), typeof(int) })]
    [HarmonyPrefix]
    public static bool Graphics_CopyTexture(ref Texture src, int srcElement, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight, ref Texture dst, int dstElement, int dstMip, int dstX, int dstY)
    {
        if (src is Texture2D s2d)
        {
            if (src.graphicsFormat != dst.graphicsFormat)
            {
                Texture2D orig;
                if (_cachedTextures.TryGetValue(src.name, out var cached))
                {
                    Plugin.Logger.LogDebug($"Using cached texture {src.name} ({cached.width}x{cached.height})");
                    orig = cached;
                }
                else
                {
                    Plugin.Logger.LogDebug($"Copying texture {src.name} ({src.width}x{src.height}) to {dst.name} ({src.width}x{src.height} with different formats: {src.graphicsFormat} to {dst.graphicsFormat}");
                    orig = DuplicateTexture(s2d, dst.graphicsFormat);
                    _cachedTextures[src.name] = orig;
                }
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