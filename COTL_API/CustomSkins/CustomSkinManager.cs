using COTL_API.Helpers;
using Spine.Unity;
using UnityEngine;
using HarmonyLib;
using Spine;

namespace COTL_API.CustomSkins;

[HarmonyPatch]
public static partial class CustomSkinManager
{
    internal static readonly Dictionary<string, SpineAtlasAsset> CustomAtlases = new();
    internal static readonly Dictionary<string, Skin?> CustomFollowerSkins = new();
    internal static readonly Dictionary<string, bool> AlwaysUnlockedSkins = new();
    internal static readonly Dictionary<string, Texture2D> SkinTextures = new();
    internal static readonly Dictionary<string, Material> SkinMaterials = new();

    internal static readonly Dictionary<string, CustomPlayerSkin> CustomPlayerSkins = new();

    internal static string OverrideSkinName { get; set; } = "Default";

    internal static List<Skin?>? PlayerSkinOverride { get; set; }

    internal static readonly List<Tuple<int, string>> SkinSlots = new()
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
        Tuple.Create(85, "Face/EYE_FIRE7")
    };

    internal static readonly Dictionary<string, Tuple<int, string>> SimplifiedSkinNames = new()
    {
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

    public static void AddFollowerSkin(CustomFollowerSkin followerSkin)
    {
        var atlasText = followerSkin.GenerateAtlasText();
        AddFollowerSkin(followerSkin.Name, followerSkin.Texture, atlasText, followerSkin.Colors, followerSkin.Hidden,
            followerSkin.Unlocked, followerSkin.TwitchPremium,
            followerSkin.Invariant);
    }

    public static void AddFollowerSkin(string name, Texture2D sheet, string atlasText,
        List<WorshipperData.SlotsAndColours> colors, bool hidden = false, bool unlocked = true,
        bool twitchPremium = false, bool invariant = false)
    {
        Material mat;
        SpineAtlasAsset atlas;
        var overrides = SkinUtils.CreateSkinAtlas(name, sheet, atlasText, RegionOverrideFunction, out mat, out atlas);
        SkinTextures.Add(name, sheet);
        SkinMaterials.Add(name, mat);
        CustomAtlases.Add(name, atlas);

        CreateNewFollowerType(name, colors, hidden, twitchPremium, invariant);
        CreateSkin(name, overrides, unlocked);
    }

    public static void AddPlayerSkin(CustomPlayerSkin playerSkin)
    {
        CustomPlayerSkins.Add(playerSkin.Name, playerSkin);
    }

    private static List<Tuple<int, string>>? RegionOverrideFunction(AtlasRegion region)
    {
        var simpleName = region.name;
        var add = "";
        if (simpleName.Contains("#"))
        {
            var split = simpleName.Split('#');
            add = "#" + split[1];
            simpleName = split[0];
        }

        if (SimplifiedSkinNames.TryGetValue(simpleName, out var simplified))
        {
            region.name = simplified.Item1 + ":" + simplified.Item2 + add;
            return new List<Tuple<int, string>>() {simplified};
        }

        if (!simpleName.Contains(":")) return null;

        try
        {
            var rName = simpleName.Split(':')[1];
            var regionIndex = (int)(SkinSlots)Enum.Parse(typeof(SkinSlots), simpleName.Split(':')[0]);
            region.name = regionIndex + ":" + rName + "#" + add;
            return new List<Tuple<int, string>>() {Tuple.Create(regionIndex, rName)};
        }
        catch (Exception)
        {
            // ignored
        }

        return null;
    }

    internal static void CreateNewFollowerType(string name, List<WorshipperData.SlotsAndColours> colors,
        bool hidden = false, bool twitchPremium = false, bool invariant = false)
    {
        WorshipperData.Instance.Characters.Add(new WorshipperData.SkinAndData
        {
            Title = name,
            Skin = new List<WorshipperData.CharacterSkin>
            {
                new()
                {
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

    internal static void CreateSkin(string name, List<Tuple<int, string, float, float, float, float>> overrides,
        bool unlocked)
    {
        void Action()
        {
            Skin skin = new(name);
            var dog = WorshipperData.Instance.SkeletonData.Skeleton.Data.FindSkin("Dog");
            var skin2 = SkinUtils.ApplyAllOverrides(dog, skin, overrides, SkinMaterials[name], CustomAtlases[name]);

            CustomFollowerSkins.Add(name, skin2);
            AlwaysUnlockedSkins.Add(name, unlocked);
        }

        if (Plugin.Started)
        {
            Action();
        }
        else
        {
            Plugin.OnStart += Action;
        }
    }

    public static void SetPlayerSkinOverride(Skin? normalSkin, Skin? hurtSkin = null, Skin? hurtSkin2 = null)
    {
        List<Skin?> skins = new()
        {
            normalSkin,
            hurtSkin,
            hurtSkin2
        };
        if (PlayerSkinOverride != null)
            LogHelper.LogDebug("PlayerSkinOverride already exists. Overwriting.");
        PlayerSkinOverride = skins;
    }

    public static void SetPlayerSkinOverride(CustomPlayerSkin skin)
    {
        skin.Apply();
        OverrideSkinName = skin.Name;
    }

    public static void ResetPlayerSkin()
    {
        PlayerSkinOverride = null;
        SkinUtils.SkinToLoad = null;
        OverrideSkinName = "Default";
    }
}