using HarmonyLib;
using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using UnityEngine;

namespace COTL_API.CustomSkins;

[HarmonyPatch]
public static partial class CustomSkinManager
{
    internal static readonly Dictionary<string, SpineAtlasAsset> CustomAtlases = new();
    internal static readonly Dictionary<string, Skin?> CustomFollowerSkins = new();
    internal static readonly Dictionary<string, bool> AlwaysUnlockedSkins = new();
    internal static readonly Dictionary<string, Texture2D> SkinTextures = new();
    internal static readonly Dictionary<string, Material> SkinMaterials = new();
    internal static readonly Dictionary<string, Skin> TarotSkins = new();
    internal static readonly Dictionary<string, Sprite> TarotSprites = new();

    internal static int NumGenericAtlases;

    internal static readonly Dictionary<string, CustomPlayerSkin> CustomPlayerSkins = new();

    internal static readonly Dictionary<string, Tuple<int, string>> FollowerSkinDict = new()
    {
        { "LEFT_ARM_SKIN", Tuple.Create(26, "ARM_LEFT_SKIN") },
        { "LEFT_SLEEVE", Tuple.Create(27, "SLEEVE_LEFT_BTM") },
        { "WEAPON_HAND_SKIN", Tuple.Create(37, "WEAPON_HAND_SKIN") },
        { "LEFT_LEG_SKIN", Tuple.Create(38, "LEG_LEFT_SKIN") },
        { "RIGHT_LEG_SKIN", Tuple.Create(39, "LEG_RIGHT_SKIN") },
        { "BODY_SKIN", Tuple.Create(44, "Body_Naked") },
        { "BODY_SKIN_BOWED", Tuple.Create(44, "Body_Naked_Up") },
        { "BODY_SKIN_UP", Tuple.Create(45, "BODY_TOP") },
        { "Body_Lvl3", Tuple.Create(45, "BODY_TOP_UP") },
        { "BowlBtm", Tuple.Create(55, "BowlBtm") },
        { "BowlFood", Tuple.Create(56, "BowlFood") },
        { "BowlFront", Tuple.Create(57, "BowlFront") },
        { "RIGHT_ARM_SKIN", Tuple.Create(63, "ARM_RIGHT_SKIN") },
        { "RIGHT_SLEEVE", Tuple.Create(64, "SLEEVE_RIGHT_BTM") },
        { "HEAD_SKIN_BTM", Tuple.Create(78, "HEAD_SKIN_BTM") },
        { "HEAD_SKIN_BTM_BACK", Tuple.Create(78, "HEAD_SKIN_BTM_BACK") },
        { "MARKINGS", Tuple.Create(81, "MARKINGS") },
        { "HEAD_SKIN_TOP", Tuple.Create(80, "HEAD_SKIN_TOP") },
        { "HEAD_SKIN_TOP_BACK", Tuple.Create(80, "HEAD_SKIN_TOP_BACK") },
        { "Angry_Colouring", Tuple.Create(82, "Angry_Colouring") },
        { "Embarrassed_Colouring", Tuple.Create(82, "Embarrassed_Colouring") },
        { "Possessed_Colouring", Tuple.Create(82, "Possessed_Colouring") },
        { "Sick_Colouring", Tuple.Create(82, "Sick_Colouring") },
        { "MOUTH_BEDREST", Tuple.Create(85, "Face/MOUTH_BEDREST") },
        { "MOUTH_CHEEKY", Tuple.Create(85, "Face/MOUTH_CHEEKY") },
        { "MOUTH_DEAD", Tuple.Create(85, "Face/MOUTH_DEAD") },
        { "MOUTH_DERP", Tuple.Create(85, "Face/MOUTH_DERP") },
        { "MOUTH_ENLIGHTENED", Tuple.Create(85, "Face/MOUTH_ENLIGHTENED") },
        { "MOUTH_GRIN", Tuple.Create(85, "Face/MOUTH_GRIN") },
        { "MOUTH_HAPPY", Tuple.Create(85, "Face/MOUTH_HAPPY") },
        { "MOUTH_HAPPY_2", Tuple.Create(85, "Face/MOUTH_HAPPY-2") },
        { "MOUTH_HUNGRY_1", Tuple.Create(85, "Face/MOUTH_HUNGRY1") },
        { "MOUTH_HUNGRY_2", Tuple.Create(85, "Face/MOUTH_HUNGRY2") },
        { "MOUTH_INDIFFERENT", Tuple.Create(85, "Face/MOUTH_INDIFFERENT") },
        { "MOUTH_INDIFFERENT_HUNGRY", Tuple.Create(85, "Face/MOUTH_INDIFFERENT_HUNGRY") },
        { "MOUTH_INSANE", Tuple.Create(85, "Face/MOUTH_INSANE") },
        { "MOUTH_KISS", Tuple.Create(85, "Face/MOUTH_KISS") },
        { "MOUTH_KISS_BIG", Tuple.Create(85, "Face/MOUTH_KISS_BIG") },
        { "MOUTH_MUMBLE", Tuple.Create(85, "Face/MOUTH_MUMBLE") },
        { "MOUTH_MUMBLE_HUNGRY", Tuple.Create(85, "Face/MOUTH_MUMBLE_HUNGRY") },
        { "MOUTH_RED", Tuple.Create(85, "Face/MOUTH_RED") },
        { "MOUTH_SACRIFICE", Tuple.Create(85, "Face/MOUTH_SACRIFICE") },
        { "MOUTH_SAD", Tuple.Create(85, "Face/MOUTH_SAD") },
        { "MOUTH_SADDER", Tuple.Create(85, "Face/MOUTH_SADDER") },
        { "MOUTH_SCARED", Tuple.Create(85, "Face/MOUTH_SCARED") },
        { "MOUTH_SICK", Tuple.Create(85, "Face/MOUTH_SICK") },
        { "MOUTH_SLEEP_0", Tuple.Create(85, "Face/MOUTH_SLEEP_0") },
        { "MOUTH_SLEEP_1", Tuple.Create(85, "Face/MOUTH_SLEEP_1") },
        { "MOUTH_TALK_HAPPY", Tuple.Create(85, "Face/MOUTH_TALK_HAPPY") },
        { "MOUTH_TALK_INDIFFERENT", Tuple.Create(85, "Face/MOUTH_TALK_INDIFFERENT") },
        { "MOUTH_TONGUE_1", Tuple.Create(85, "Face/MOUTH_TONGUE_1") },
        { "MOUTH_TONGUE_2", Tuple.Create(85, "Face/MOUTH_TONGUE_2") },
        { "MOUTH_TONGUE_3", Tuple.Create(85, "Face/MOUTH_TONGUE_3") },
        { "MOUTH_TONGUE_4", Tuple.Create(85, "Face/MOUTH_TONGUE_4") },
        { "MOUTH_WORRIED", Tuple.Create(85, "Face/MOUTH_WORRIED") },
        { "MOUTH_ENLIGHTENED_2", Tuple.Create(85, "MOUTH_ENLIGHTENED2") },
        { "RIGHT_EYE", Tuple.Create(90, "EYE") },
        { "RIGHT_EYE_ANGRY", Tuple.Create(90, "EYE_ANGRY") },
        { "RIGHT_EYE_ANGRY_UP", Tuple.Create(90, "EYE_ANGRY_UP") },
        { "RIGHT_EYE_BLACK", Tuple.Create(90, "EYE_BLACK") },
        { "RIGHT_EYE_BRAINWASHED", Tuple.Create(90, "EYE_BRAINWASHED") },
        { "RIGHT_EYE_CLOSED", Tuple.Create(90, "EYE_CLOSED") },
        { "RIGHT_EYE_CRAZY", Tuple.Create(90, "EYE_CRAZY") },
        { "RIGHT_EYE_DEAD", Tuple.Create(90, "EYE_DEAD") },
        { "RIGHT_EYE_DISSENTER", Tuple.Create(90, "EYE_DISSENTER") },
        { "RIGHT_EYE_DISSENTER_ANGRY", Tuple.Create(90, "EYE_DISSENTER_ANGRY") },
        { "RIGHT_EYE_ENLIGHTENED", Tuple.Create(90, "EYE_ENLIGHTENED") },
        { "RIGHT_EYE_HALF_CLOSED", Tuple.Create(90, "EYE_HALF_CLOSED") },
        { "RIGHT_EYE_HALF_CLOSED_ANGRY", Tuple.Create(90, "EYE_HALF_CLOSED_ANGRY") },
        { "RIGHT_EYE_INSANE", Tuple.Create(90, "EYE_INSANE") },
        { "RIGHT_EYE_SACRIFICE_1", Tuple.Create(90, "EYE_SACRIFICE_1") },
        { "RIGHT_EYE_SACRIFICE_2", Tuple.Create(90, "EYE_SACRIFICE_2") },
        { "RIGHT_EYE_SHOCKED", Tuple.Create(90, "EYE_SHOCKED") },
        { "RIGHT_EYE_SICK_2", Tuple.Create(90, "EYE_SICK_2_RIGHT") },
        { "RIGHT_EYE_SICK", Tuple.Create(90, "EYE_SICK_RIGHT") },
        { "RIGHT_EYE_SLEEPING", Tuple.Create(90, "EYE_SLEEPING") },
        { "RIGHT_EYE_SLEEPING_SICK", Tuple.Create(90, "EYE_SLEEPING_SICK") },
        { "RIGHT_EYE_SLEEPING_TIRED", Tuple.Create(90, "EYE_SLEEPING_TIRED") },
        { "RIGHT_EYE_SLEEPY", Tuple.Create(90, "EYE_SLEEPY_RIGHT") },
        { "RIGHT_EYE_SMILE", Tuple.Create(90, "EYE_SMILE") },
        { "RIGHT_EYE_SMILE_DOWN", Tuple.Create(90, "EYE_SMILE_DOWN") },
        { "RIGHT_EYE_SMILE_UP", Tuple.Create(90, "EYE_SMILE_UP") },
        { "RIGHT_EYE_SQUINT", Tuple.Create(90, "EYE_SQUINT") },
        { "RIGHT_EYE_STARS", Tuple.Create(90, "EYE_STARS") },
        { "RIGHT_EYE_UNCONVERTED", Tuple.Create(90, "EYE_UNCONVERTED") },
        { "RIGHT_EYE_UP", Tuple.Create(90, "EYE_UP") },
        { "RIGHT_EYE_VERY_ANGRY", Tuple.Create(90, "EYE_VERY_ANGRY") },
        { "RIGHT_EYE_WHITE", Tuple.Create(90, "EYE_WHITE") },
        { "RIGHT_EYE_WORRIED", Tuple.Create(90, "EYE_WORRIED_RIGHT") },
        { "RIGHT_EYE_FIRE_1", Tuple.Create(90, "Face/EYE_FIRE1") },
        { "RIGHT_EYE_FIRE_2", Tuple.Create(90, "Face/EYE_FIRE2") },
        { "RIGHT_EYE_FIRE_3", Tuple.Create(90, "Face/EYE_FIRE3") },
        { "RIGHT_EYE_FIRE_4", Tuple.Create(90, "Face/EYE_FIRE4") },
        { "RIGHT_EYE_FIRE_5", Tuple.Create(90, "Face/EYE_FIRE5") },
        { "RIGHT_EYE_FIRE_6", Tuple.Create(90, "Face/EYE_FIRE6") },
        { "RIGHT_EYE_FIRE_7", Tuple.Create(90, "Face/EYE_FIRE7") },
        { "LEFT_EYE", Tuple.Create(91, "EYE") },
        { "LEFT_EYE_ANGRY", Tuple.Create(91, "EYE_ANGRY_LEFT") },
        { "LEFT_EYE_ANGRY_UP", Tuple.Create(91, "EYE_ANGRY_UP_LEFT") },
        { "LEFT_EYE_BLACK", Tuple.Create(91, "EYE_BLACK") },
        { "LEFT_EYE_BRAINWASHED", Tuple.Create(91, "EYE_BRAINWASHED") },
        { "LEFT_EYE_CLOSED", Tuple.Create(91, "EYE_CLOSED") },
        { "LEFT_EYE_CRAZY", Tuple.Create(91, "EYE_CRAZY") },
        { "LEFT_EYE_DEAD", Tuple.Create(91, "EYE_DEAD") },
        { "LEFT_EYE_DISSENTER", Tuple.Create(91, "EYE_DISSENTER") },
        { "LEFT_EYE_DISSENTER_ANGRY", Tuple.Create(91, "EYE_DISSENTER_ANGRY") },
        { "LEFT_EYE_ENLIGHTENED", Tuple.Create(91, "EYE_ENLIGHTENED") },
        { "LEFT_EYE_HALF_CLOSED", Tuple.Create(91, "EYE_HALF_CLOSED") },
        { "LEFT_EYE_HALF_CLOSED_ANGRY", Tuple.Create(91, "EYE_HALF_CLOSED_ANGRY_LEFT") },
        { "LEFT_EYE_INSANE", Tuple.Create(91, "EYE_INSANE_LEFT") },
        { "LEFT_EYE_SACRIFICE_1", Tuple.Create(91, "EYE_SACRIFICE_1") },
        { "LEFT_EYE_SACRIFICE_2", Tuple.Create(91, "EYE_SACRIFICE_2") },
        { "LEFT_EYE_SHOCKED", Tuple.Create(91, "EYE_SHOCKED") },
        { "LEFT_EYE_SICK_2", Tuple.Create(91, "EYE_SICK_2_LEFT") },
        { "LEFT_EYE_SICK", Tuple.Create(91, "EYE_SICK_LEFT") },
        { "LEFT_EYE_SLEEPING", Tuple.Create(91, "EYE_SLEEPING") },
        { "LEFT_EYE_SLEEPING_SICK", Tuple.Create(91, "EYE_SLEEPING_SICK") },
        { "LEFT_EYE_SLEEPING_TIRED", Tuple.Create(91, "EYE_SLEEPING_TIRED") },
        { "LEFT_EYE_SLEEPY", Tuple.Create(91, "EYE_SLEEPY_LEFT") },
        { "LEFT_EYE_SMILE", Tuple.Create(91, "EYE_SMILE") },
        { "LEFT_EYE_SMILE_DOWN", Tuple.Create(91, "EYE_SMILE_DOWN") },
        { "LEFT_EYE_SMILE_UP", Tuple.Create(91, "EYE_SMILE_UP") },
        { "LEFT_EYE_SQUINT", Tuple.Create(91, "EYE_SQUINT") },
        { "LEFT_EYE_STARS", Tuple.Create(91, "EYE_STARS") },
        { "LEFT_EYE_UNCONVERTED", Tuple.Create(91, "EYE_UNCONVERTED") },
        { "LEFT_EYE_UP", Tuple.Create(91, "EYE_UP") },
        { "LEFT_EYE_VERY_ANGRY", Tuple.Create(91, "EYE_VERY_ANGRY") },
        { "LEFT_EYE_WHITE", Tuple.Create(91, "EYE_WHITE") },
        { "LEFT_EYE_WORRIED", Tuple.Create(91, "EYE_WORRIED_LEFT") },
        { "LEFT_EYE_FIRE_1", Tuple.Create(91, "Face/EYE_FIRE1") },
        { "LEFT_EYE_FIRE_2", Tuple.Create(91, "Face/EYE_FIRE2") },
        { "LEFT_EYE_FIRE_3", Tuple.Create(91, "Face/EYE_FIRE3") },
        { "LEFT_EYE_FIRE_4", Tuple.Create(91, "Face/EYE_FIRE4") },
        { "LEFT_EYE_FIRE_5", Tuple.Create(91, "Face/EYE_FIRE5") },
        { "LEFT_EYE_FIRE_6", Tuple.Create(91, "Face/EYE_FIRE6") },
        { "LEFT_EYE_FIRE_7", Tuple.Create(91, "Face/EYE_FIRE7") }
    };

    internal static readonly Dictionary<string, Tuple<int, string>> PlayerSkinDict = new()
    {
        { "ARM_LEFT", Tuple.Create(14, "ARM_LEFT") },
        { "ArmSpikes", Tuple.Create(25, "ArmSpikes") },
        { "Tools/Book_open", Tuple.Create(53, "Tools/Book_open") },
        { "Tools/Book_closed", Tuple.Create(53, "Tools/Book_closed") },
        { "Tools/BookFlipping_3", Tuple.Create(53, "Tools/BookFlipping_3") },
        { "Tools/BookFlipping_2", Tuple.Create(53, "Tools/BookFlipping_2") },
        { "Tools/BookFlipping_1", Tuple.Create(53, "Tools/BookFlipping_1") },
        { "Tools/BookFlipping_4", Tuple.Create(53, "Tools/BookFlipping_4") },
        { "Tools/Chalice", Tuple.Create(59, "Tools/Chalice") },
        { "Tools/Chalice_Skull", Tuple.Create(59, "Tools/Chalice_Skull") },
        { "Tools/Chalice_Skull_Drink", Tuple.Create(59, "Tools/Chalice_Skull_Drink") },
        { "CROWN", Tuple.Create(37, "CROWN") },
        { "CROWN_RITUAL", Tuple.Create(37, "CROWN_RITUAL") },
        { "CROWN_SERMON", Tuple.Create(37, "CROWN_SERMON") },
        { "BigCrown", Tuple.Create(37, "BigCrown") },
        { "CROWN_WHITE", Tuple.Create(37, "CROWN_WHITE") },
        { "images/CrownEyeShut3", Tuple.Create(39, "images/CrownEyeShut3") },
        { "images/CrownEyeShut2", Tuple.Create(39, "images/CrownEyeShut2") },
        { "images/CrownEyeShut", Tuple.Create(39, "images/CrownEyeShut") },
        { "CROWN_EYE", Tuple.Create(39, "CROWN_EYE") },
        { "images/CrownEye_RITUAL", Tuple.Create(39, "images/CrownEye_RITUAL") },
        { "images/CrownEye_SERMON", Tuple.Create(39, "images/CrownEye_SERMON") },
        { "images/CrownEyeBig", Tuple.Create(39, "images/CrownEyeBig") },
        { "images/CrownEyeBig2", Tuple.Create(39, "images/CrownEyeBig2") },
        { "CrownEyeSin", Tuple.Create(39, "CrownEyeSin") },
        { "images/CrownMouth", Tuple.Create(57, "images/CrownMouth") },
        { "images/CrownMouthOpen", Tuple.Create(57, "images/CrownMouthOpen") },
        { "CROWN_EYE", Tuple.Create(40, "CROWN_EYE") },
        { "images/CrownEyeBig", Tuple.Create(40, "images/CrownEyeBig") },
        { "images/CrownEyeBig2", Tuple.Create(40, "images/CrownEyeBig2") },
        { "images/CrownEyeShut", Tuple.Create(40, "images/CrownEyeShut") },
        { "images/CrownEyeShut2", Tuple.Create(40, "images/CrownEyeShut2") },
        { "images/CrownEyeShut3", Tuple.Create(40, "images/CrownEyeShut3") },
        { "images/CrownEye_RITUAL", Tuple.Create(40, "images/CrownEye_RITUAL") },
        { "images/CrownEye_SERMON", Tuple.Create(40, "images/CrownEye_SERMON") },
        { "CrownEyeSin", Tuple.Create(40, "CrownEyeSin") },
        { "Tools/CardBack", Tuple.Create(98, "Tools/CardBack") },
        { "Tools/CardFront", Tuple.Create(98, "Tools/CardFront") },
        { "Tools/CardBack", Tuple.Create(102, "Tools/CardBack") },
        { "Tools/CardFront", Tuple.Create(102, "Tools/CardFront") },
        { "Tools/CardBack", Tuple.Create(103, "Tools/CardBack") },
        { "Tools/CardFront", Tuple.Create(103, "Tools/CardFront") },
        { "Tools/CardBack", Tuple.Create(101, "Tools/CardBack") },
        { "Tools/CardFront", Tuple.Create(101, "Tools/CardFront") },
        { "Tools/CardBack", Tuple.Create(99, "Tools/CardBack") },
        { "Tools/CardFront", Tuple.Create(99, "Tools/CardFront") },
        { "Tools/CardBack", Tuple.Create(100, "Tools/CardBack") },
        { "Tools/CardFront", Tuple.Create(100, "Tools/CardFront") },
        { "Crown_Particle1", Tuple.Create(0, "Crown_Particle1") },
        { "Crown_Particle2", Tuple.Create(1, "Crown_Particle2") },
        { "effects/Crown_Particle3", Tuple.Create(3, "effects/Crown_Particle3") },
        { "effects/Crown_Particle4", Tuple.Create(4, "effects/Crown_Particle4") },
        { "effects/Crown_Particle5", Tuple.Create(5, "effects/Crown_Particle5") },
        { "Crown_Particle6", Tuple.Create(2, "Crown_Particle6") },
        { "Curses/Icon_Curse_Blast", Tuple.Create(122, "Curses/Icon_Curse_Blast") },
        { "Curses/Icon_Curse_Fireball", Tuple.Create(122, "Curses/Icon_Curse_Fireball") },
        { "Curses/Icon_Curse_Slash", Tuple.Create(122, "Curses/Icon_Curse_Slash") },
        { "Curses/Icon_Curse_Splatter", Tuple.Create(122, "Curses/Icon_Curse_Splatter") },
        { "Curses/Icon_Curse_Tentacle", Tuple.Create(122, "Curses/Icon_Curse_Tentacle") },
        { "DRINK", Tuple.Create(58, "DRINK") },
        { "EAR_LEFT", Tuple.Create(34, "EAR_LEFT") },
        { "EAR_RITUAL", Tuple.Create(34, "EAR_RITUAL") },
        { "EAR_SERMON", Tuple.Create(34, "EAR_SERMON") },
        { "EAR_RIGHT", Tuple.Create(46, "EAR_RIGHT") },
        { "EAR_RIGHT_RITUAL", Tuple.Create(46, "EAR_RIGHT_RITUAL") },
        { "EAR_RIGHT_SERMON", Tuple.Create(46, "EAR_RIGHT_SERMON") },
        { "EYE", Tuple.Create(50, "EYE") },
        { "EYE_ANGRY_LEFT", Tuple.Create(50, "EYE_ANGRY_LEFT") },
        { "EYE_BACK", Tuple.Create(50, "EYE_BACK") },
        { "EYE_DETERMINED_DOWN_LEFT", Tuple.Create(50, "EYE_DETERMINED_DOWN_LEFT") },
        { "EYE_DETERMINED_LEFT", Tuple.Create(50, "EYE_DETERMINED_LEFT") },
        { "EYE_DOWN", Tuple.Create(50, "EYE_DOWN") },
        { "EYE_HALF_CLOSED", Tuple.Create(50, "EYE_HALF_CLOSED") },
        { "EYE_HAPPY", Tuple.Create(50, "EYE_HAPPY") },
        { "EYE_UP", Tuple.Create(50, "EYE_UP") },
        { "EYE_WORRIED_LEFT", Tuple.Create(50, "EYE_WORRIED_LEFT") },
        { "Face/EYE_CLOSED", Tuple.Create(50, "Face/EYE_CLOSED") },
        { "Face/EYE_DEAD", Tuple.Create(50, "Face/EYE_DEAD") },
        { "Face/EYE_RED", Tuple.Create(50, "Face/EYE_RED") },
        { "Face/EYE_SHOCKED", Tuple.Create(50, "Face/EYE_SHOCKED") },
        { "Face/EYE_SLEEPING", Tuple.Create(50, "Face/EYE_SLEEPING") },
        { "Face/EYE_SQUINT", Tuple.Create(50, "Face/EYE_SQUINT") },
        { "Face/EYE_UNCONVERTED", Tuple.Create(50, "Face/EYE_UNCONVERTED") },
        { "Face/EYE_UNCONVERTED_WORRIED", Tuple.Create(50, "Face/EYE_UNCONVERTED_WORRIED") },
        { "EYE_ANGRY_LEFT_UP", Tuple.Create(50, "EYE_ANGRY_LEFT_UP") },
        { "EYE_WHITE", Tuple.Create(50, "EYE_WHITE") },
        { "EYE_WEARY_LEFT", Tuple.Create(50, "EYE_WEARY_LEFT") },
        { "EYE_GRIMACE", Tuple.Create(50, "EYE_GRIMACE") },
        { "EYE_WEARY_LEFT_DOWN", Tuple.Create(50, "EYE_WEARY_LEFT_DOWN") },
        { "EYE_HAPPY2", Tuple.Create(50, "EYE_HAPPY2") },
        { "Face/EYE_RED_ANGRY", Tuple.Create(50, "Face/EYE_RED_ANGRY") },
        { "EYE_WHITE_ANGRY", Tuple.Create(50, "EYE_WHITE_ANGRY") },
        { "Face/EYE_HAPPY_UP", Tuple.Create(50, "Face/EYE_HAPPY_UP") },
        { "EYE", Tuple.Create(51, "EYE") },
        { "EYE_ANGRY_RIGHT", Tuple.Create(51, "EYE_ANGRY_RIGHT") },
        { "EYE_BACK", Tuple.Create(51, "EYE_BACK") },
        { "EYE_DETERMINED_DOWN_RIGHT", Tuple.Create(51, "EYE_DETERMINED_DOWN_RIGHT") },
        { "EYE_DETERMINED_RIGHT", Tuple.Create(51, "EYE_DETERMINED_RIGHT") },
        { "EYE_DOWN", Tuple.Create(51, "EYE_DOWN") },
        { "EYE_HALF_CLOSED", Tuple.Create(51, "EYE_HALF_CLOSED") },
        { "EYE_HAPPY", Tuple.Create(51, "EYE_HAPPY") },
        { "EYE_UP", Tuple.Create(51, "EYE_UP") },
        { "EYE_WORRIED_RIGHT", Tuple.Create(51, "EYE_WORRIED_RIGHT") },
        { "Face/EYE_CLOSED", Tuple.Create(51, "Face/EYE_CLOSED") },
        { "Face/EYE_DEAD", Tuple.Create(51, "Face/EYE_DEAD") },
        { "Face/EYE_RED", Tuple.Create(51, "Face/EYE_RED") },
        { "Face/EYE_SHOCKED", Tuple.Create(51, "Face/EYE_SHOCKED") },
        { "Face/EYE_SLEEPING", Tuple.Create(51, "Face/EYE_SLEEPING") },
        { "Face/EYE_SQUINT", Tuple.Create(51, "Face/EYE_SQUINT") },
        { "Face/EYE_UNCONVERTED", Tuple.Create(51, "Face/EYE_UNCONVERTED") },
        { "Face/EYE_UNCONVERTED_WORRIED", Tuple.Create(51, "Face/EYE_UNCONVERTED_WORRIED") },
        { "EYE_ANGRY_RIGHT_UP", Tuple.Create(51, "EYE_ANGRY_RIGHT_UP") },
        { "EYE_WHITE", Tuple.Create(51, "EYE_WHITE") },
        { "EYE_WEARY_RIGHT", Tuple.Create(51, "EYE_WEARY_RIGHT") },
        { "EYE_GRIMACE", Tuple.Create(51, "EYE_GRIMACE") },
        { "EYE_WEARY_RIGHT_DOWN", Tuple.Create(51, "EYE_WEARY_RIGHT_DOWN") },
        { "EYE_HAPPY2", Tuple.Create(51, "EYE_HAPPY2") },
        { "Face/EYE_RED_ANGRY", Tuple.Create(51, "Face/EYE_RED_ANGRY") },
        { "EYE_WHITE_ANGRY", Tuple.Create(51, "EYE_WHITE_ANGRY") },
        { "Face/EYE_HAPPY_UP", Tuple.Create(51, "Face/EYE_HAPPY_UP") },
        { "AttackHand1", Tuple.Create(65, "AttackHand1") },
        { "AttackHand2", Tuple.Create(65, "AttackHand2") },
        { "GauntletHeavy", Tuple.Create(66, "GauntletHeavy") },
        { "GauntletHeavy2", Tuple.Create(66, "GauntletHeavy2") },
        { "GunHand", Tuple.Create(69, "GunHand") },
        { "MonsterHeart_glow", Tuple.Create(93, "MonsterHeart_glow") },
        { "KnowledgeParchment", Tuple.Create(93, "KnowledgeParchment") },
        { "Knowledge_Trinket", Tuple.Create(93, "Knowledge_Trinket") },
        { "Knowledge_Curse", Tuple.Create(93, "Knowledge_Curse") },
        { "Knowledge_Decoration", Tuple.Create(93, "Knowledge_Decoration") },
        { "Knowledge_Weapon", Tuple.Create(93, "Knowledge_Weapon") },
        { "Tools/Woodaxe", Tuple.Create(93, "Tools/Woodaxe") },
        { "Tools/Woodaxe2", Tuple.Create(93, "Tools/Woodaxe2") },
        { "Tools/Pickaxe", Tuple.Create(93, "Tools/Pickaxe") },
        { "Tools/Pickaxe2", Tuple.Create(93, "Tools/Pickaxe2") },
        { "Tools/Hammer", Tuple.Create(93, "Tools/Hammer") },
        { "Net", Tuple.Create(93, "Net") },
        { "Items/WebberSkull", Tuple.Create(93, "Items/WebberSkull") },
        { "Tools/Book_open", Tuple.Create(93, "Tools/Book_open") },
        { "Tools/Book_closed", Tuple.Create(93, "Tools/Book_closed") },
        { "MonsterHeart_glow", Tuple.Create(94, "MonsterHeart_glow") },
        { "GiftSmall", Tuple.Create(94, "GiftSmall") },
        { "GiftMedium", Tuple.Create(94, "GiftMedium") },
        { "Red Heart Pick Up", Tuple.Create(94, "Red Heart Pick Up") },
        { "MOUTH_NORMAL", Tuple.Create(49, "MOUTH_NORMAL") },
        { "Face/MOUTH_CHEEKY", Tuple.Create(49, "Face/MOUTH_CHEEKY") },
        { "Face/MOUTH_CHUBBY", Tuple.Create(49, "Face/MOUTH_CHUBBY") },
        { "Face/MOUTH_DEAD", Tuple.Create(49, "Face/MOUTH_DEAD") },
        { "Face/MOUTH_GRUMPY", Tuple.Create(49, "Face/MOUTH_GRUMPY") },
        { "Face/MOUTH_HAPPY", Tuple.Create(49, "Face/MOUTH_HAPPY") },
        { "Face/MOUTH_INDIFFERENT", Tuple.Create(49, "Face/MOUTH_INDIFFERENT") },
        { "Face/MOUTH_KAWAII", Tuple.Create(49, "Face/MOUTH_KAWAII") },
        { "Face/MOUTH_OO", Tuple.Create(49, "Face/MOUTH_OO") },
        { "Face/MOUTH_OPEN", Tuple.Create(49, "Face/MOUTH_OPEN") },
        { "Face/MOUTH_SAD", Tuple.Create(49, "Face/MOUTH_SAD") },
        { "Face/MOUTH_SCARED", Tuple.Create(49, "Face/MOUTH_SCARED") },
        { "Face/MOUTH_SLEEP_0", Tuple.Create(49, "Face/MOUTH_SLEEP_0") },
        { "Face/MOUTH_SLEEP_1", Tuple.Create(49, "Face/MOUTH_SLEEP_1") },
        { "Face/MOUTH_TONGUE", Tuple.Create(49, "Face/MOUTH_TONGUE") },
        { "Face/MOUTH_UNCONVERTED", Tuple.Create(49, "Face/MOUTH_UNCONVERTED") },
        { "MOUTH_TALK", Tuple.Create(49, "MOUTH_TALK") },
        { "MOUTH_TALK_HAPPY", Tuple.Create(49, "MOUTH_TALK_HAPPY") },
        { "MOUTH_UNCONVERTED_SPEAK", Tuple.Create(49, "MOUTH_UNCONVERTED_SPEAK") },
        { "MOUTH_GRIMACE", Tuple.Create(49, "MOUTH_GRIMACE") },
        { "MOUTH_SNARL", Tuple.Create(49, "MOUTH_SNARL") },
        { "MOUTH_TALK1", Tuple.Create(49, "MOUTH_TALK1") },
        { "MOUTH_TALK2", Tuple.Create(49, "MOUTH_TALK2") },
        { "MOUTH_TALK3", Tuple.Create(49, "MOUTH_TALK3") },
        { "MOUTH_TALK4", Tuple.Create(49, "MOUTH_TALK4") },
        { "MOUTH_TALK5", Tuple.Create(49, "MOUTH_TALK5") },
        { "Face/MOUTH_HUNGRY1", Tuple.Create(49, "Face/MOUTH_HUNGRY1") },
        { "Face/MOUTH_HUNGRY2", Tuple.Create(49, "Face/MOUTH_HUNGRY2") },
        { "effects/MonsterBlood1", Tuple.Create(95, "effects/MonsterBlood1") },
        { "MonsterBlood2", Tuple.Create(97, "MonsterBlood2") },
        { "effects/MonsterBlood1", Tuple.Create(96, "effects/MonsterBlood1") },
        { "images/RopeTopLeft", Tuple.Create(30, "images/RopeTopLeft") },
        { "images/RopeTopRight", Tuple.Create(29, "images/RopeTopRight") },
        { "effects/chunder_1", Tuple.Create(121, "effects/chunder_1") },
        { "effects/chunder_2", Tuple.Create(121, "effects/chunder_2") },
        { "effects/chunder_3", Tuple.Create(121, "effects/chunder_3") },
        { "Weapons/Sling", Tuple.Create(67, "Weapons/Sling") },
        { "Weapons/SpecialSword_1", Tuple.Create(91, "Weapons/SpecialSword_1") },
        { "Weapons/SpecialSword_2", Tuple.Create(91, "Weapons/SpecialSword_2") },
        { "Weapons/SpecialSword_3", Tuple.Create(91, "Weapons/SpecialSword_3") },
        { "Weapons/SpecialSword_4", Tuple.Create(91, "Weapons/SpecialSword_4") },
        { "Tools/PITCHFORK", Tuple.Create(16, "Tools/PITCHFORK") },
        { "Tools/SEED_BAG", Tuple.Create(16, "Tools/SEED_BAG") },
        { "Tools/SPADE", Tuple.Create(16, "Tools/SPADE") },
        { "Tools/WATERING_CAN", Tuple.Create(16, "Tools/WATERING_CAN") },
        { "Tools/FishingRod", Tuple.Create(16, "Tools/FishingRod") },
        { "Tools/FishingRod2", Tuple.Create(16, "Tools/FishingRod2") },
        { "Tools/Mop", Tuple.Create(16, "Tools/Mop") },
        { "FishingRod_Front", Tuple.Create(16, "FishingRod_Front") },
        { "GauntletHeavy", Tuple.Create(16, "GauntletHeavy") },
        { "GauntletHeavy2", Tuple.Create(16, "GauntletHeavy2") },
        { "images/AttackHand1", Tuple.Create(16, "images/AttackHand1") },
        { "images/AttackHand2", Tuple.Create(16, "images/AttackHand2") },
        { "Weapons/Axe", Tuple.Create(22, "Weapons/Axe") },
        { "Weapons/Blunderbuss", Tuple.Create(22, "Weapons/Blunderbuss") },
        { "Weapons/Dagger", Tuple.Create(22, "Weapons/Dagger") },
        { "Weapons/Hammer", Tuple.Create(22, "Weapons/Hammer") },
        { "Weapons/Sword", Tuple.Create(22, "Weapons/Sword") },
        { "DaggerFlipped", Tuple.Create(22, "DaggerFlipped") },
        { "Shield", Tuple.Create(22, "Shield") },
        { "Blunderbuss/Flipped", Tuple.Create(22, "Blunderbuss/Flipped") },
        { "GrappleHook", Tuple.Create(79, "GrappleHook") },
        { "Weapons/Lute", Tuple.Create(80, "Weapons/Lute") },
        { "SlingHand", Tuple.Create(70, "SlingHand") },
        { "Weapons/SlingHand", Tuple.Create(81, "Weapons/SlingHand") },
        { "Weapons/SlingRope", Tuple.Create(68, "Weapons/SlingRope") },
        { "SwordHeavy", Tuple.Create(116, "SwordHeavy") },
        { "Weapons/SwordHeavy_Necromancy", Tuple.Create(116, "Weapons/SwordHeavy_Necromancy") },
        { "Weapons/SwordHeavy_Ice", Tuple.Create(116, "Weapons/SwordHeavy_Ice") },
        { "Weapons/SwordHeavy_Charm", Tuple.Create(116, "Weapons/SwordHeavy_Charm") },
        { "AxeHeavy", Tuple.Create(116, "AxeHeavy") },
        { "HammerHeavy", Tuple.Create(116, "HammerHeavy") },
        { "Blunderbuss_Heavy", Tuple.Create(116, "Blunderbuss_Heavy") },
        { "Weapons/SlingHand", Tuple.Create(75, "Weapons/SlingHand") },
        { "AttackSlash1", Tuple.Create(107, "AttackSlash1") },
        { "AttackSlash2", Tuple.Create(107, "AttackSlash2") },
        { "ChainBit1", Tuple.Create(111, "ChainBit1") },
        { "ChainBit2", Tuple.Create(112, "ChainBit2") },
        { "ChainBit3", Tuple.Create(114, "ChainBit3") },
        { "ChainBit1", Tuple.Create(113, "ChainBit1") },
        { "CollarPiece1", Tuple.Create(109, "CollarPiece1") },
        { "CollarPiece2", Tuple.Create(110, "CollarPiece2") },
        { "Halo", Tuple.Create(13, "Halo") },
        { "RitualSymbolHalo", Tuple.Create(104, "RitualSymbolHalo") },
        { "RitualSymbol_1", Tuple.Create(105, "RitualSymbol_1") },
        { "RitualSymbol_2", Tuple.Create(105, "RitualSymbol_2") },
        { "Sparks1", Tuple.Create(87, "Sparks1") },
        { "Sparks2", Tuple.Create(89, "Sparks2") },
        { "Sparks2", Tuple.Create(90, "Sparks2") },
        { "Sparks1", Tuple.Create(88, "Sparks1") },
        { "effects/SpawnHeavy_1", Tuple.Create(117, "effects/SpawnHeavy_1") },
        { "effects/SpawnHeavy_2", Tuple.Create(117, "effects/SpawnHeavy_2") },
        { "effects/SpawnHeavy_3", Tuple.Create(117, "effects/SpawnHeavy_3") },
        { "effects/SpawnHeavy_4", Tuple.Create(117, "effects/SpawnHeavy_4") },
        { "SpawnHeavy_glow", Tuple.Create(118, "SpawnHeavy_glow") },
        { "effects/eye_blood", Tuple.Create(47, "effects/eye_blood") },
        { "effects/eye_tears", Tuple.Create(47, "effects/eye_tears") },
        { "effects/eye_blood", Tuple.Create(48, "effects/eye_blood") },
        { "effects/eye_tears", Tuple.Create(48, "effects/eye_tears") },
        { "effects/portal_btm", Tuple.Create(76, "effects/portal_btm") },
        { "portal_splash", Tuple.Create(78, "portal_splash") },
        { "effects/portal_top", Tuple.Create(77, "effects/portal_top") },
        { "whiteball", Tuple.Create(73, "whiteball") },
        { "effects/whiteball", Tuple.Create(74, "effects/whiteball") },
        { "Antler", Tuple.Create(32, "Antler") },
        { "Antler_RITUAL", Tuple.Create(32, "Antler_RITUAL") },
        { "Antler_SERMON", Tuple.Create(32, "Antler_SERMON") },
        { "Antler_Horn", Tuple.Create(32, "Antler_Horn") },
        { "Antler", Tuple.Create(33, "Antler") },
        { "Antler_SERMON", Tuple.Create(33, "Antler_SERMON") },
        { "Antler_RITUAL", Tuple.Create(33, "Antler_RITUAL") },
        { "Antler_Horn", Tuple.Create(33, "Antler_Horn") },
        { "ARM_RIGHT", Tuple.Create(23, "ARM_RIGHT") },
        { "Arm_frontbit", Tuple.Create(72, "Arm_frontbit") },
        { "Arm_frontbit", Tuple.Create(71, "Arm_frontbit") },
        { "Bell", Tuple.Create(31, "Bell") },
        { "Body", Tuple.Create(19, "Body") },
        { "Corpse", Tuple.Create(11, "Corpse") },
        { "Corpse", Tuple.Create(12, "Corpse") },
        { "CrownGlow", Tuple.Create(35, "CrownGlow") },
        { "images/CrownSpikes", Tuple.Create(36, "images/CrownSpikes") },
        { "images/CrownSpikes", Tuple.Create(61, "images/CrownSpikes") },
        { "images/CrownSpikes2", Tuple.Create(62, "images/CrownSpikes2") },
        { "images/CrownSpikes", Tuple.Create(63, "images/CrownSpikes") },
        { "images/CrownSpikes2", Tuple.Create(64, "images/CrownSpikes2") },
        { "images/Crown_half_left", Tuple.Create(82, "images/Crown_half_left") },
        { "images/Crown_half_right", Tuple.Create(83, "images/Crown_half_right") },
        { "FireSmall_0001", Tuple.Create(119, "FireSmall_0001") },
        { "FireSmall_0002", Tuple.Create(119, "FireSmall_0002") },
        { "FireSmall_0003", Tuple.Create(119, "FireSmall_0003") },
        { "FireSmall_0004", Tuple.Create(119, "FireSmall_0004") },
        { "FireSmall_0005", Tuple.Create(119, "FireSmall_0005") },
        { "FireSmall_0006", Tuple.Create(119, "FireSmall_0006") },
        { "FireSmall_0007", Tuple.Create(119, "FireSmall_0007") },
        { "FireWild_0001", Tuple.Create(120, "FireWild_0001") },
        { "FireWild_0002", Tuple.Create(120, "FireWild_0002") },
        { "FireWild_0003", Tuple.Create(120, "FireWild_0003") },
        { "FireWild_0004", Tuple.Create(120, "FireWild_0004") },
        { "FireWild_0005", Tuple.Create(120, "FireWild_0005") },
        { "FireWild_0006", Tuple.Create(120, "FireWild_0006") },
        { "FireWild_0007", Tuple.Create(120, "FireWild_0007") },
        { "FireWild_0008", Tuple.Create(120, "FireWild_0008") },
        { "FireWild_0009", Tuple.Create(120, "FireWild_0009") },
        { "HairTuft", Tuple.Create(52, "HairTuft") },
        { "HeadBack", Tuple.Create(43, "HeadBack") },
        { "HeadBackDown", Tuple.Create(43, "HeadBackDown") },
        { "HeadBackDown_RITUAL", Tuple.Create(43, "HeadBackDown_RITUAL") },
        { "HeadBackDown_SERMON", Tuple.Create(43, "HeadBackDown_SERMON") },
        { "HeadFront", Tuple.Create(44, "HeadFront") },
        { "HeadFrontDown", Tuple.Create(44, "HeadFrontDown") },
        { "LEG_RIGHT", Tuple.Create(18, "LEG_RIGHT") },
        { "LEG_LEFT", Tuple.Create(17, "LEG_LEFT") },
        { "PonchoExtra", Tuple.Create(27, "PonchoExtra") },
        { "PonchoLeft", Tuple.Create(21, "PonchoLeft") },
        { "PonchoLeft2", Tuple.Create(21, "PonchoLeft2") },
        { "PonchoRight", Tuple.Create(26, "PonchoRight") },
        { "PonchoRight2", Tuple.Create(26, "PonchoRight2") },
        { "PonchoRightCorner", Tuple.Create(55, "PonchoRightCorner") },
        { "PonchoRightCorner", Tuple.Create(56, "PonchoRightCorner") },
        { "PonchoShoulder", Tuple.Create(15, "PonchoShoulder") },
        { "PonchoShoulder_Right", Tuple.Create(24, "PonchoShoulder_Right") },
        { "images/Rope", Tuple.Create(28, "images/Rope") },
        { "sunburst", Tuple.Create(9, "sunburst") },
        { "sunburst2", Tuple.Create(9, "sunburst2") },
        { "effects/RitualRing", Tuple.Create(108, "effects/RitualRing") },
        { "effects/SermonRing", Tuple.Create(108, "effects/SermonRing") },
        { "effects/RitualRing2", Tuple.Create(106, "effects/RitualRing2") },
        { "effects/SermonRing2", Tuple.Create(106, "effects/SermonRing2") },
        { "effects/slam_effect0006", Tuple.Create(60, "effects/slam_effect0006") },
        { "effects/slam_effect0005", Tuple.Create(60, "effects/slam_effect0005") },
        { "effects/slam_effect0004", Tuple.Create(60, "effects/slam_effect0004") },
        { "effects/slam_effect0003", Tuple.Create(60, "effects/slam_effect0003") },
        { "effects/slam_effect0002", Tuple.Create(60, "effects/slam_effect0002") },
        { "effects/slam_effect0001", Tuple.Create(60, "effects/slam_effect0001") },
    };

    internal static List<Skin?>? PlayerSkinOverride { get; set; }

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
        var overrides =
            SkinUtils.CreateSkinAtlas(name, sheet, atlasText, RegionOverrideFunction, out var mat, out var atlas);

        SkinTextures.Add(name, sheet);
        SkinMaterials.Add(name, mat);
        CustomAtlases.Add(name, atlas);

        CreateNewFollowerType(name, colors, hidden, twitchPremium, invariant);
        CreateFollowerSkin(name, overrides, unlocked);
    }

    public static void AddPlayerSkin(CustomPlayerSkin playerSkin)
    {
        CustomPlayerSkins.Add(playerSkin.Name, playerSkin);

        if (Plugin.SkinSettings != null)
            Plugin.SkinSettings.Options =
                new[] { "Default" }.Concat(CustomPlayerSkins.Keys).ToArray();
    }

    public static void AddTarotBackSkin(string internalName, Sprite skin)
    {
        var spriteName = $"CustomTarotBack_{internalName}";
        if (!TarotSprites.ContainsKey(spriteName)) TarotSprites.Add(spriteName, skin);
    }

    private static List<Tuple<int, string>> RegionOverrideFunction(AtlasRegion region)
    {
        var simpleName = region.name;
        var add = "";
        if (simpleName.Contains("#"))
        {
            var split = simpleName.Split('#');
            add = "#" + split[1];
            simpleName = split[0];
        }

        if (FollowerSkinDict.TryGetValue(simpleName, out var simplified))
        {
            region.name = simplified.Item1 + ":" + simplified.Item2 + add;
            return [simplified];
        }

        if (!simpleName.Contains(":")) return [];

        try
        {
            var rName = simpleName.Split(':')[1];
            var regionIndex = (int)(SkinSlots)Enum.Parse(typeof(SkinSlots), simpleName.Split(':')[0]);
            region.name = regionIndex + ":" + rName + "#" + add;
            return [Tuple.Create(regionIndex, rName)];
        }
        catch (Exception)
        {
            // ignored
        }

        return [];
    }

    internal static void CreateNewFollowerType(string name, List<WorshipperData.SlotsAndColours> colors,
        bool hidden = false, bool twitchPremium = false, bool invariant = false)
    {
        WorshipperData.Instance.Characters.Add(new WorshipperData.SkinAndData
        {
            Title = name,
            Skin =
            [
                new WorshipperData.CharacterSkin
                {
                    Skin = name
                }
            ],
            SlotAndColours = colors,
            TwitchPremium = twitchPremium,
            _hidden = hidden,
            _dropLocation = WorshipperData.DropLocation.Other,
            _invariant = invariant
        });
    }

    internal static void CreateFollowerSkin(string name, List<Tuple<int, string, float, float, float, float>> overrides,
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
            Action();
        else
            Plugin.OnStart += Action;
    }

    internal static string GetOrCreateTarotSkin(string internalName, Sprite skin)
    {
        var name = $"CustomTarotSkin_{internalName}";
        if (!TarotSprites.ContainsKey(name)) TarotSprites.Add(name, skin);
        return name;
    }

    internal static Skin CreateOrGetTarotSkinFromTemplate(SkeletonData template, string skinName)
    {
        return TarotSkins.TryGetValue(skinName, out var fromTemplate)
            ? fromTemplate
            : CreateTarotSkin(template.Skins.ToList()[1], skinName);
    }

    private static Skin CreateTarotSkin(Skin template, string skinName)
    {
        var sprite = TarotSprites[skinName];
        var atlas = CreateSingleTextureAtlas(sprite);

        Skin skin = new(skinName);

        var atlasRegion = atlas.GetAtlas().FindRegion("GENERIC_ATTACHMENT").Clone();
        var back = template.Attachments.ToList()[0];
        back = new Skin.SkinEntry(back.SlotIndex, back.Name, back.Attachment.Copy());

        if (back.Attachment is MeshAttachment customAttachmentBack && TarotSprites.TryGetValue("CustomTarotBack_" + skinName, out var backSprite))
        {
            var backAtlas = CreateSingleTextureAtlas(backSprite);
            var backAtlasRegion = backAtlas.GetAtlas().FindRegion("GENERIC_ATTACHMENT").Clone();

            float minX = int.MaxValue;
            float maxX = int.MinValue;
            float minY = int.MaxValue;
            float maxY = int.MinValue;

            for (var j = 0; j < customAttachmentBack.Vertices.Length; j++)
                switch (j % 3)
                {
                    case 0:
                        minY = Math.Min(minY, customAttachmentBack.Vertices[j]);
                        maxY = Math.Max(maxY, customAttachmentBack.Vertices[j]);
                        break;
                    case 1:
                        minX = Math.Min(minX, customAttachmentBack.Vertices[j]);
                        maxX = Math.Max(maxX, customAttachmentBack.Vertices[j]);
                        break;
                }

            customAttachmentBack.Name = "CustomTarotSkin_" + skinName;
            customAttachmentBack.SetRegion(backAtlasRegion, false);
            backAtlasRegion.name = "CustomTarotSkin_" + atlasRegion.name;
            customAttachmentBack.HullLength = 4;
            customAttachmentBack.Triangles = [1, 2, 3, 1, 3, 0];

            float pw = backAtlasRegion.page.width;
            float ph = backAtlasRegion.page.height;
            float x = backAtlasRegion.x;
            float y = backAtlasRegion.y;
            float w = backAtlasRegion.width;
            float h = backAtlasRegion.height;
            customAttachmentBack.UVs =
            [
                (x + w) / pw, y / ph, (x + w) / pw, (y + h) / ph, x / pw, (y + h) / ph, x / pw, y / ph
            ];
            customAttachmentBack.Vertices = [minY, minX, 1, maxY, minX, 1, maxY, maxX, 1, minY, maxX, 1];
            customAttachmentBack.WorldVerticesLength = 8;
            customAttachmentBack.UpdateUVs();

            skin.SetAttachment(back.SlotIndex, back.Name, customAttachmentBack);
        }
        else
        {
            skin.SetAttachment(back.SlotIndex, back.Name, back.Attachment);
        }

        var front = template.Attachments.ToList()[1];
        front = new Skin.SkinEntry(front.SlotIndex, front.Name, front.Attachment.Copy());
        if (front.Attachment is MeshAttachment customAttachment)
        {
            float minX = int.MaxValue;
            float maxX = int.MinValue;
            float minY = int.MaxValue;
            float maxY = int.MinValue;

            for (var j = 0; j < customAttachment.Vertices.Length; j++)
                switch (j % 3)
                {
                    case 0:
                        minY = Math.Min(minY, customAttachment.Vertices[j]);
                        maxY = Math.Max(maxY, customAttachment.Vertices[j]);
                        break;
                    case 1:
                        minX = Math.Min(minX, customAttachment.Vertices[j]);
                        maxX = Math.Max(maxX, customAttachment.Vertices[j]);
                        break;
                }

            customAttachment.Name = "CustomTarotSkin_" + skinName;
            customAttachment.SetRegion(atlasRegion, false);
            atlasRegion.name = "CustomTarotSkin_" + atlasRegion.name;
            customAttachment.HullLength = 4;
            customAttachment.Triangles = [1, 2, 3, 1, 3, 0];

            float pw = atlasRegion.page.width;
            float ph = atlasRegion.page.height;
            float x = atlasRegion.x;
            float y = atlasRegion.y;
            float w = atlasRegion.width;
            float h = atlasRegion.height;
            customAttachment.UVs =
            [
                (x + w) / pw, y / ph, (x + w) / pw, (y + h) / ph, x / pw, (y + h) / ph, x / pw, y / ph
            ];
            customAttachment.Vertices = [minY, minX, 1, maxY, minX, 1, maxY, maxX, 1, minY, maxX, 1];
            customAttachment.WorldVerticesLength = 8;
            customAttachment.UpdateUVs();

            skin.SetAttachment(front.SlotIndex, front.Name, customAttachment);
        }

        TarotSkins.Add(skinName, skin);
        return skin;
    }

    private static SpineAtlasAsset CreateSingleTextureAtlas(Sprite sprite)
    {
        var n = NumGenericAtlases;
        var w = (int)sprite.bounds.size.x;
        var h = (int)sprite.bounds.size.y;
        var atlasText = $"\r\ngeneric_sheet_{n}\r\nsize: {w},{h}\r\nformat: RGBA8888\r\nfilter: Linear,Linear"
                        + "\r\nrepeat: none\r\nGENERIC_ATTACHMENT\r\nrotate: false\r\nxy: 0, 0\r\n"
                        + $"size: {w}, {h}\r\norig: {w}, {h}\r\noffset: 0, 0\r\nindex: -1";
        Texture tex = sprite.texture;
        tex.name = $"generic_sheet_{n}";

        Material mat = new(Shader.Find("Spine/Skeleton"))
        {
            mainTexture = tex
        };

        Material[] materials = [mat];
        var atlas = SpineAtlasAsset.CreateRuntimeInstance(new TextAsset(atlasText), materials, true);
        NumGenericAtlases++;
        return atlas;
    }

    public static void SetPlayerSkinOverride(Skin? normalSkin, Skin? hurtSkin = null, Skin? hurtSkin2 = null)
    {
        List<Skin?> skins =
        [
            normalSkin,
            hurtSkin,
            hurtSkin2
        ];

        if (PlayerSkinOverride != null)
            LogDebug("PlayerSkinOverride already exists. Overwriting.");

        PlayerSkinOverride = skins;
    }

    public static void SetPlayerSkinOverride(CustomPlayerSkin skin)
    {
        skin.Apply();
    }

    public static void ResetPlayerSkin()
    {
        PlayerSkinOverride = null;
        SkinUtils.SkinToLoad = null;
    }
}