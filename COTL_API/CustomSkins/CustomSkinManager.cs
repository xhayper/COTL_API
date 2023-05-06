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
    
    internal static int NumGenericAtlases = 0;

    internal static readonly Dictionary<string, CustomPlayerSkin> CustomPlayerSkins = new();

    internal static readonly Dictionary<string, Tuple<int, string>> FollowerSkinDict = new()
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
        { "HEAD_SKIN_BTM", Tuple.Create(78, "HEAD_SKIN_BTM") },
        { "HEAD_SKIN_BTM_BACK", Tuple.Create(78, "HEAD_SKIN_BTM_BACK") },
        { "MARKINGS", Tuple.Create(79, "MARKINGS") },
        { "HEAD_SKIN_TOP", Tuple.Create(80, "HEAD_SKIN_TOP") },
        { "HEAD_SKIN_TOP_BACK", Tuple.Create(80, "HEAD_SKIN_TOP_BACK") },
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

    internal static readonly Dictionary<string, Tuple<int, string>> PlayerSkinDict = new()
    {
        { "Crown_Particle1", Tuple.Create(0, "Crown_Particle1") },
        { "Crown_Particle2", Tuple.Create(1, "Crown_Particle2") },
        { "Crown_Particle6", Tuple.Create(2, "Crown_Particle6") },
        { "effects/Crown_Particle3", Tuple.Create(3, "effects/Crown_Particle3") },
        { "effects/Crown_Particle4", Tuple.Create(4, "effects/Crown_Particle4") },
        { "effects/Crown_Particle5", Tuple.Create(5, "effects/Crown_Particle5") },
        { "sunburst", Tuple.Create(9, "sunburst") },
        { "sunburst2", Tuple.Create(9, "sunburst2") },
        { "Corpse", Tuple.Create(11, "Corpse") },
        { "Corpse2", Tuple.Create(12, "Corpse") },
        { "Halo", Tuple.Create(13, "Halo") },
        { "ARM_LEFT", Tuple.Create(14, "ARM_LEFT") },
        { "PonchoShoulder", Tuple.Create(15, "PonchoShoulder") },
        { "Tools/PITCHFORK", Tuple.Create(16, "Tools/PITCHFORK") },
        { "Tools/SEED_BAG", Tuple.Create(16, "Tools/SEED_BAG") },
        { "Tools/SPADE", Tuple.Create(16, "Tools/SPADE") },
        { "Tools/WATERING_CAN", Tuple.Create(16, "Tools/WATERING_CAN") },
        { "Tools/FishingRod", Tuple.Create(16, "Tools/FishingRod") },
        { "Tools/FishingRod2", Tuple.Create(16, "Tools/FishingRod2") },
        { "Tools/Mop", Tuple.Create(16, "Tools/Mop") },
        { "FishingRod_Front", Tuple.Create(16, "FishingRod_Front") },
        { "GauntletHeavya", Tuple.Create(16, "GauntletHeavy") },
        { "GauntletHeavy2a", Tuple.Create(16, "GauntletHeavy2") },
        { "images/AttackHand1", Tuple.Create(16, "images/AttackHand1") },
        { "images/AttackHand2", Tuple.Create(16, "images/AttackHand2") },
        { "LEG_LEFT", Tuple.Create(17, "LEG_LEFT") },
        { "LEG_RIGHT", Tuple.Create(18, "LEG_RIGHT") },
        { "Body", Tuple.Create(19, "Body") },
        { "PonchoLeft", Tuple.Create(20, "PonchoLeft") },
        { "PonchoLeft2", Tuple.Create(20, "PonchoLeft2") },
        { "Weapons/Axe", Tuple.Create(21, "Weapons/Axe") },
        { "Weapons/Blunderbuss", Tuple.Create(21, "Weapons/Blunderbuss") },
        { "Weapons/Dagger", Tuple.Create(21, "Weapons/Dagger") },
        { "Weapons/Hammer", Tuple.Create(21, "Weapons/Hammer") },
        { "Weapons/Sword", Tuple.Create(21, "Weapons/Sword") },
        { "DaggerFlipped", Tuple.Create(21, "DaggerFlipped") },
        { "ARM_RIGHT", Tuple.Create(22, "ARM_RIGHT") },
        { "ArmSpikes", Tuple.Create(23, "ArmSpikes") },
        { "PonchoRight", Tuple.Create(24, "PonchoRight") },
        { "PonchoRight2", Tuple.Create(24, "PonchoRight2") },
        { "PonchoExtra", Tuple.Create(25, "PonchoExtra") },
        { "images/Rope", Tuple.Create(26, "images/Rope") },
        { "images/RopeTopRight", Tuple.Create(27, "images/RopeTopRight") },
        { "images/RopeTopLeft", Tuple.Create(28, "images/RopeTopLeft") },
        { "Bell", Tuple.Create(29, "Bell") },
        { "Antler", Tuple.Create(30, "Antler") },
        { "Antler_RITUAL", Tuple.Create(30, "Antler_RITUAL") },
        { "Antler_SERMON", Tuple.Create(30, "Antler_SERMON") },
        { "Antler_Horn", Tuple.Create(30, "Antler_Horn") },
        { "Antler2", Tuple.Create(31, "Antler") },
        { "Antler_SERMON2", Tuple.Create(31, "Antler_SERMON") },
        { "Antler_RITUAL2", Tuple.Create(31, "Antler_RITUAL") },
        { "Antler_Horn2", Tuple.Create(31, "Antler_Horn") },
        { "EAR_LEFT", Tuple.Create(32, "EAR_LEFT") },
        { "EAR_RITUAL", Tuple.Create(32, "EAR_RITUAL") },
        { "EAR_SERMON", Tuple.Create(32, "EAR_SERMON") },
        { "CrownGlow", Tuple.Create(33, "CrownGlow") },
        { "images/CrownSpikesc", Tuple.Create(34, "images/CrownSpikes") },
        { "CROWN", Tuple.Create(35, "CROWN") },
        { "CROWN_RITUAL", Tuple.Create(35, "CROWN_RITUAL") },
        { "CROWN_SERMON", Tuple.Create(35, "CROWN_SERMON") },
        { "BigCrown", Tuple.Create(35, "BigCrown") },
        { "CROWN_WHITE", Tuple.Create(35, "CROWN_WHITE") },
        { "images/CrownEyeShut3", Tuple.Create(36, "images/CrownEyeShut3") },
        { "images/CrownEyeShut2", Tuple.Create(36, "images/CrownEyeShut2") },
        { "images/CrownEyeShut", Tuple.Create(36, "images/CrownEyeShut") },
        { "CROWN_EYE", Tuple.Create(36, "CROWN_EYE") },
        { "images/CrownEye_RITUAL", Tuple.Create(36, "images/CrownEye_RITUAL") },
        { "images/CrownEye_SERMON", Tuple.Create(36, "images/CrownEye_SERMON") },
        { "images/CrownEyeBig", Tuple.Create(36, "images/CrownEyeBig") },
        { "HeadBack", Tuple.Create(39, "HeadBack") },
        { "HeadBackDown", Tuple.Create(39, "HeadBackDown") },
        { "HeadBackDown_RITUAL", Tuple.Create(39, "HeadBackDown_RITUAL") },
        { "HeadBackDown_SERMON", Tuple.Create(39, "HeadBackDown_SERMON") },
        { "HeadFront", Tuple.Create(40, "HeadFront") },
        { "HeadFrontDown", Tuple.Create(40, "HeadFrontDown") },
        { "EAR_RIGHT", Tuple.Create(42, "EAR_RIGHT") },
        { "EAR_RIGHT_RITUAL", Tuple.Create(42, "EAR_RIGHT_RITUAL") },
        { "EAR_RIGHT_SERMON", Tuple.Create(42, "EAR_RIGHT_SERMON") },
        { "effects/eye_blood", Tuple.Create(43, "effects/eye_blood") },
        { "effects/eye_tears", Tuple.Create(43, "effects/eye_tears") },
        { "effects/eye_blood2", Tuple.Create(44, "effects/eye_blood") },
        { "effects/eye_tears2", Tuple.Create(44, "effects/eye_tears") },
        { "MOUTH_NORMAL", Tuple.Create(45, "MOUTH_NORMAL") },
        { "Face/MOUTH_CHEEKY", Tuple.Create(45, "Face/MOUTH_CHEEKY") },
        { "Face/MOUTH_CHUBBY", Tuple.Create(45, "Face/MOUTH_CHUBBY") },
        { "Face/MOUTH_DEAD", Tuple.Create(45, "Face/MOUTH_DEAD") },
        { "Face/MOUTH_GRUMPY", Tuple.Create(45, "Face/MOUTH_GRUMPY") },
        { "Face/MOUTH_HAPPY", Tuple.Create(45, "Face/MOUTH_HAPPY") },
        { "Face/MOUTH_INDIFFERENT", Tuple.Create(45, "Face/MOUTH_INDIFFERENT") },
        { "Face/MOUTH_KAWAII", Tuple.Create(45, "Face/MOUTH_KAWAII") },
        { "Face/MOUTH_OO", Tuple.Create(45, "Face/MOUTH_OO") },
        { "Face/MOUTH_OPEN", Tuple.Create(45, "Face/MOUTH_OPEN") },
        { "Face/MOUTH_SAD", Tuple.Create(45, "Face/MOUTH_SAD") },
        { "Face/MOUTH_SCARED", Tuple.Create(45, "Face/MOUTH_SCARED") },
        { "Face/MOUTH_SLEEP_0", Tuple.Create(45, "Face/MOUTH_SLEEP_0") },
        { "Face/MOUTH_SLEEP_1", Tuple.Create(45, "Face/MOUTH_SLEEP_1") },
        { "Face/MOUTH_TONGUE", Tuple.Create(45, "Face/MOUTH_TONGUE") },
        { "Face/MOUTH_UNCONVERTED", Tuple.Create(45, "Face/MOUTH_UNCONVERTED") },
        { "MOUTH_TALK", Tuple.Create(45, "MOUTH_TALK") },
        { "MOUTH_TALK_HAPPY", Tuple.Create(45, "MOUTH_TALK_HAPPY") },
        { "MOUTH_UNCONVERTED_SPEAK", Tuple.Create(45, "MOUTH_UNCONVERTED_SPEAK") },
        { "MOUTH_GRIMACE", Tuple.Create(45, "MOUTH_GRIMACE") },
        { "MOUTH_SNARL", Tuple.Create(45, "MOUTH_SNARL") },
        { "MOUTH_TALK1", Tuple.Create(45, "MOUTH_TALK1") },
        { "MOUTH_TALK2", Tuple.Create(45, "MOUTH_TALK2") },
        { "MOUTH_TALK3", Tuple.Create(45, "MOUTH_TALK3") },
        { "MOUTH_TALK4", Tuple.Create(45, "MOUTH_TALK4") },
        { "MOUTH_TALK5", Tuple.Create(45, "MOUTH_TALK5") },
        { "EYE_LEFT", Tuple.Create(46, "EYE") },
        { "EYE_ANGRY_LEFT", Tuple.Create(46, "EYE_ANGRY_LEFT") },
        { "EYE_BACK_LEFT", Tuple.Create(46, "EYE_BACK") },
        { "EYE_DETERMINED_DOWN_LEFT", Tuple.Create(46, "EYE_DETERMINED_DOWN_LEFT") },
        { "EYE_DETERMINED_LEFT", Tuple.Create(46, "EYE_DETERMINED_LEFT") },
        { "EYE_DOWN_LEFT", Tuple.Create(46, "EYE_DOWN") },
        { "EYE_HALF_CLOSED_LEFT", Tuple.Create(46, "EYE_HALF_CLOSED") },
        { "EYE_HAPPY_LEFT", Tuple.Create(46, "EYE_HAPPY") },
        { "EYE_UP_LEFT", Tuple.Create(46, "EYE_UP") },
        { "EYE_WORRIED_LEFT", Tuple.Create(46, "EYE_WORRIED_LEFT") },
        { "Face/EYE_CLOSED_LEFT_LEFT", Tuple.Create(46, "Face/EYE_CLOSED") },
        { "Face/EYE_DEAD_LEFT", Tuple.Create(46, "Face/EYE_DEAD") },
        { "Face/EYE_RED_LEFT", Tuple.Create(46, "Face/EYE_RED") },
        { "Face/EYE_SHOCKED_LEFT", Tuple.Create(46, "Face/EYE_SHOCKED") },
        { "Face/EYE_SLEEPING_LEFT", Tuple.Create(46, "Face/EYE_SLEEPING") },
        { "Face/EYE_SQUINT_LEFT", Tuple.Create(46, "Face/EYE_SQUINT") },
        { "Face/EYE_UNCONVERTED_LEFT", Tuple.Create(46, "Face/EYE_UNCONVERTED") },
        { "Face/EYE_UNCONVERTED_WORRIED_LEFT", Tuple.Create(46, "Face/EYE_UNCONVERTED_WORRIED") },
        { "EYE_ANGRY_LEFT_UP", Tuple.Create(46, "EYE_ANGRY_LEFT_UP") },
        { "EYE_WHITE_LEFT", Tuple.Create(46, "EYE_WHITE") },
        { "EYE_WEARY_LEFT", Tuple.Create(46, "EYE_WEARY_LEFT") },
        { "EYE_GRIMACE_LEFT", Tuple.Create(46, "EYE_GRIMACE") },
        { "EYE_WEARY_LEFT_DOWN", Tuple.Create(46, "EYE_WEARY_LEFT_DOWN") },
        { "EYE_HAPPY2_LEFT", Tuple.Create(46, "EYE_HAPPY2") },
        { "Face/EYE_RED_ANGRY_LEFT", Tuple.Create(46, "Face/EYE_RED_ANGRY") },
        { "EYE_WHITE_ANGRY_LEFT", Tuple.Create(46, "EYE_WHITE_ANGRY") },
        { "EYE_RIGHT", Tuple.Create(47, "EYE") },
        { "EYE_ANGRY_RIGHT", Tuple.Create(47, "EYE_ANGRY_RIGHT") },
        { "EYE_BACK_RIGHT", Tuple.Create(47, "EYE_BACK") },
        { "EYE_DETERMINED_DOWN_RIGHT", Tuple.Create(47, "EYE_DETERMINED_DOWN_RIGHT") },
        { "EYE_DETERMINED_RIGHT", Tuple.Create(47, "EYE_DETERMINED_RIGHT") },
        { "EYE_DOWN_RIGHT", Tuple.Create(47, "EYE_DOWN") },
        { "EYE_HALF_CLOSED_RIGHT", Tuple.Create(47, "EYE_HALF_CLOSED") },
        { "EYE_HAPPY_RIGHT", Tuple.Create(47, "EYE_HAPPY") },
        { "EYE_UP_RIGHT", Tuple.Create(47, "EYE_UP") },
        { "EYE_WORRIED_RIGHT", Tuple.Create(47, "EYE_WORRIED_RIGHT") },
        { "Face/EYE_CLOSED_RIGHT", Tuple.Create(47, "Face/EYE_CLOSED") },
        { "Face/EYE_DEAD_RIGHT", Tuple.Create(47, "Face/EYE_DEAD") },
        { "Face/EYE_RED_RIGHT", Tuple.Create(47, "Face/EYE_RED") },
        { "Face/EYE_SHOCKED_RIGHT", Tuple.Create(47, "Face/EYE_SHOCKED") },
        { "Face/EYE_SLEEPING_RIGHT", Tuple.Create(47, "Face/EYE_SLEEPING") },
        { "Face/EYE_SQUINT_RIGHT", Tuple.Create(47, "Face/EYE_SQUINT") },
        { "Face/EYE_UNCONVERTED_RIGHT", Tuple.Create(47, "Face/EYE_UNCONVERTED") },
        { "Face/EYE_UNCONVERTED_WORRIED_RIGHT", Tuple.Create(47, "Face/EYE_UNCONVERTED_WORRIED") },
        { "EYE_ANGRY_RIGHT_UP", Tuple.Create(47, "EYE_ANGRY_RIGHT_UP") },
        { "EYE_WHITE_RIGHT", Tuple.Create(47, "EYE_WHITE") },
        { "EYE_WEARY_RIGHT", Tuple.Create(47, "EYE_WEARY_RIGHT") },
        { "EYE_GRIMACE_RIGHT", Tuple.Create(47, "EYE_GRIMACE") },
        { "EYE_WEARY_RIGHT_DOWN", Tuple.Create(47, "EYE_WEARY_RIGHT_DOWN") },
        { "EYE_HAPPY2_RIGHT", Tuple.Create(47, "EYE_HAPPY2") },
        { "Face/EYE_RED_ANGRY_RIGHT", Tuple.Create(47, "Face/EYE_RED_ANGRY") },
        { "EYE_WHITE_ANGRY_RIGHT", Tuple.Create(47, "EYE_WHITE_ANGRY") },
        { "HairTuft", Tuple.Create(48, "HairTuft") },
        { "Tools/Book_open", Tuple.Create(49, "Tools/Book_open") },
        { "Tools/Book_closed", Tuple.Create(49, "Tools/Book_closed") },
        { "Tools/BookFlipping_3", Tuple.Create(49, "Tools/BookFlipping_3") },
        { "Tools/BookFlipping_2", Tuple.Create(49, "Tools/BookFlipping_2") },
        { "Tools/BookFlipping_1", Tuple.Create(49, "Tools/BookFlipping_1") },
        { "Tools/BookFlipping_4", Tuple.Create(49, "Tools/BookFlipping_4") },
        { "PonchoRightCorner", Tuple.Create(51, "PonchoRightCorner") },
        { "PonchoRightCorner2", Tuple.Create(52, "PonchoRightCorner") },
        { "images/CrownMouth", Tuple.Create(53, "images/CrownMouth") },
        { "images/CrownMouthOpen", Tuple.Create(53, "images/CrownMouthOpen") },
        { "Tools/Chalice", Tuple.Create(54, "Tools/Chalice") },
        { "Tools/Chalice_Skull", Tuple.Create(54, "Tools/Chalice_Skull") },
        { "Tools/Chalice_Skull_Drink", Tuple.Create(54, "Tools/Chalice_Skull_Drink") },
        { "effects/slam_effect0006", Tuple.Create(55, "effects/slam_effect0006") },
        { "effects/slam_effect0005", Tuple.Create(55, "effects/slam_effect0005") },
        { "effects/slam_effect0004", Tuple.Create(55, "effects/slam_effect0004") },
        { "effects/slam_effect0003", Tuple.Create(55, "effects/slam_effect0003") },
        { "effects/slam_effect0002", Tuple.Create(55, "effects/slam_effect0002") },
        { "effects/slam_effect0001", Tuple.Create(55, "effects/slam_effect0001") },
        { "images/CrownSpikesa", Tuple.Create(56, "images/CrownSpikes") },
        { "images/CrownSpikes2a", Tuple.Create(57, "images/CrownSpikes2") },
        { "images/CrownSpikesb", Tuple.Create(58, "images/CrownSpikes") },
        { "images/CrownSpikes2b", Tuple.Create(59, "images/CrownSpikes2") },
        { "AttackHand1", Tuple.Create(60, "AttackHand1") },
        { "AttackHand2", Tuple.Create(60, "AttackHand2") },
        { "GauntletHeavyb", Tuple.Create(61, "GauntletHeavy") },
        { "GauntletHeavy2b", Tuple.Create(61, "GauntletHeavy2") },
        { "Weapons/Sling", Tuple.Create(62, "Weapons/Sling") },
        { "Weapons/SlingRope", Tuple.Create(63, "Weapons/SlingRope") },
        { "SlingHand", Tuple.Create(64, "SlingHand") },
        { "Arm_frontbit", Tuple.Create(65, "Arm_frontbit") },
        { "whiteball", Tuple.Create(66, "whiteball") },
        { "effects/whiteball", Tuple.Create(67, "effects/whiteball") },
        { "Weapons/SlingHand", Tuple.Create(68, "Weapons/SlingHand") },
        { "effects/portal_btm", Tuple.Create(69, "effects/portal_btm") },
        { "effects/portal_top", Tuple.Create(70, "effects/portal_top") },
        { "portal_splash", Tuple.Create(71, "portal_splash") },
        { "GrappleHook", Tuple.Create(72, "GrappleHook") },
        { "Weapons/Lute", Tuple.Create(73, "Weapons/Lute") },
        { "Weapons/SlingHand2", Tuple.Create(74, "Weapons/SlingHand") },
        { "images/Crown_half_left", Tuple.Create(75, "images/Crown_half_left") },
        { "images/Crown_half_right", Tuple.Create(76, "images/Crown_half_right") },
        { "Sparks1a", Tuple.Create(80, "Sparks1") },
        { "Sparks1b", Tuple.Create(81, "Sparks1") },
        { "Sparks2a", Tuple.Create(82, "Sparks2") },
        { "Sparks2b", Tuple.Create(83, "Sparks2") },
        { "Weapons/SpecialSword_1", Tuple.Create(84, "Weapons/SpecialSword_1") },
        { "Weapons/SpecialSword_2", Tuple.Create(84, "Weapons/SpecialSword_2") },
        { "Weapons/SpecialSword_3", Tuple.Create(84, "Weapons/SpecialSword_3") },
        { "Weapons/SpecialSword_4", Tuple.Create(84, "Weapons/SpecialSword_4") },
        { "MonsterHeart_glow", Tuple.Create(85, "MonsterHeart_glow") },
        { "KnowledgeParchment", Tuple.Create(85, "KnowledgeParchment") },
        { "Knowledge_Trinket", Tuple.Create(85, "Knowledge_Trinket") },
        { "Knowledge_Curse", Tuple.Create(85, "Knowledge_Curse") },
        { "Knowledge_Decoration", Tuple.Create(85, "Knowledge_Decoration") },
        { "Knowledge_Weapon", Tuple.Create(85, "Knowledge_Weapon") },
        { "Tools/Woodaxe", Tuple.Create(85, "Tools/Woodaxe") },
        { "Tools/Woodaxe2", Tuple.Create(85, "Tools/Woodaxe2") },
        { "Tools/Pickaxe", Tuple.Create(85, "Tools/Pickaxe") },
        { "Tools/Pickaxe2", Tuple.Create(85, "Tools/Pickaxe2") },
        { "Tools/Hammer", Tuple.Create(85, "Tools/Hammer") },
        { "Net", Tuple.Create(85, "Net") },
        { "Items/WebberSkull", Tuple.Create(85, "Items/WebberSkull") },
        { "Tools/Book_open2", Tuple.Create(85, "Tools/Book_open") },
        { "Tools/Book_closed2", Tuple.Create(85, "Tools/Book_closed") },
        { "MonsterHeart_glow2", Tuple.Create(86, "MonsterHeart_glow") },
        { "GiftSmall", Tuple.Create(86, "GiftSmall") },
        { "GiftMedium", Tuple.Create(86, "GiftMedium") },
        { "effects/MonsterBlood1a", Tuple.Create(87, "effects/MonsterBlood1") },
        { "effects/MonsterBlood1b", Tuple.Create(88, "effects/MonsterBlood1") },
        { "MonsterBlood2", Tuple.Create(89, "MonsterBlood2") },
        { "Tools/CardBack", Tuple.Create(90, "Tools/CardBack") },
        { "Tools/CardFront", Tuple.Create(90, "Tools/CardFront") },
        { "Tools/CardBack2", Tuple.Create(91, "Tools/CardBack") },
        { "Tools/CardFront2", Tuple.Create(91, "Tools/CardFront") },
        { "Tools/CardBack3", Tuple.Create(92, "Tools/CardBack") },
        { "Tools/CardFront3", Tuple.Create(92, "Tools/CardFront") },
        { "Tools/CardBack4", Tuple.Create(93, "Tools/CardBack") },
        { "Tools/CardFront4", Tuple.Create(93, "Tools/CardFront") },
        { "Tools/CardBack5", Tuple.Create(94, "Tools/CardBack") },
        { "Tools/CardFront5", Tuple.Create(94, "Tools/CardFront") },
        { "Tools/CardBack6", Tuple.Create(95, "Tools/CardBack") },
        { "Tools/CardFront6", Tuple.Create(95, "Tools/CardFront") },
        { "RitualSymbolHalo", Tuple.Create(96, "RitualSymbolHalo") },
        { "RitualSymbol_1", Tuple.Create(97, "RitualSymbol_1") },
        { "RitualSymbol_2", Tuple.Create(97, "RitualSymbol_2") },
        { "effects/RitualRing2", Tuple.Create(98, "effects/RitualRing2") },
        { "effects/SermonRing2", Tuple.Create(98, "effects/SermonRing2") },
        { "AttackSlash1", Tuple.Create(99, "AttackSlash1") },
        { "AttackSlash2", Tuple.Create(99, "AttackSlash2") },
        { "effects/RitualRing", Tuple.Create(100, "effects/RitualRing") },
        { "effects/SermonRing", Tuple.Create(100, "effects/SermonRing") },
        { "CollarPiece1", Tuple.Create(101, "CollarPiece1") },
        { "CollarPiece2", Tuple.Create(102, "CollarPiece2") },
        { "ChainBit1", Tuple.Create(103, "ChainBit1") },
        { "ChainBit2", Tuple.Create(104, "ChainBit2") },
        { "ChainBit1b", Tuple.Create(105, "ChainBit1") },
        { "ChainBit3", Tuple.Create(106, "ChainBit3") },
        { "SwordHeavy", Tuple.Create(108, "SwordHeavy") },
        { "Weapons/SwordHeavy_Necromancy", Tuple.Create(108, "Weapons/SwordHeavy_Necromancy") },
        { "Weapons/SwordHeavy_Ice", Tuple.Create(108, "Weapons/SwordHeavy_Ice") },
        { "Weapons/SwordHeavy_Charm", Tuple.Create(108, "Weapons/SwordHeavy_Charm") },
        { "AxeHeavy", Tuple.Create(108, "AxeHeavy") },
        { "HammerHeavy", Tuple.Create(108, "HammerHeavy") },
        { "effects/SpawnHeavy_1", Tuple.Create(109, "effects/SpawnHeavy_1") },
        { "effects/SpawnHeavy_2", Tuple.Create(109, "effects/SpawnHeavy_2") },
        { "effects/SpawnHeavy_3", Tuple.Create(109, "effects/SpawnHeavy_3") },
        { "effects/SpawnHeavy_4", Tuple.Create(109, "effects/SpawnHeavy_4") },
        { "SpawnHeavy_glow", Tuple.Create(110, "SpawnHeavy_glow") },
        { "FireSmall_0001", Tuple.Create(111, "FireSmall_0001") },
        { "FireSmall_0002", Tuple.Create(111, "FireSmall_0002") },
        { "FireSmall_0003", Tuple.Create(111, "FireSmall_0003") },
        { "FireSmall_0004", Tuple.Create(111, "FireSmall_0004") },
        { "FireSmall_0005", Tuple.Create(111, "FireSmall_0005") },
        { "FireSmall_0006", Tuple.Create(111, "FireSmall_0006") },
        { "FireSmall_0007", Tuple.Create(111, "FireSmall_0007") },
        { "FireWild_0001", Tuple.Create(112, "FireWild_0001") },
        { "FireWild_0002", Tuple.Create(112, "FireWild_0002") },
        { "FireWild_0003", Tuple.Create(112, "FireWild_0003") },
        { "FireWild_0004", Tuple.Create(112, "FireWild_0004") },
        { "FireWild_0005", Tuple.Create(112, "FireWild_0005") },
        { "FireWild_0006", Tuple.Create(112, "FireWild_0006") },
        { "FireWild_0007", Tuple.Create(112, "FireWild_0007") },
        { "FireWild_0008", Tuple.Create(112, "FireWild_0008") },
        { "FireWild_0009", Tuple.Create(112, "FireWild_0009") },
        { "effects/chunder_1", Tuple.Create(113, "effects/chunder_1") },
        { "effects/chunder_2", Tuple.Create(113, "effects/chunder_2") },
        { "effects/chunder_3", Tuple.Create(113, "effects/chunder_3") },
        { "Curses/Icon_Curse_Blast", Tuple.Create(114, "Curses/Icon_Curse_Blast") },
        { "Curses/Icon_Curse_Fireball", Tuple.Create(114, "Curses/Icon_Curse_Fireball") },
        { "Curses/Icon_Curse_Slash", Tuple.Create(114, "Curses/Icon_Curse_Slash") },
        { "Curses/Icon_Curse_Splatter", Tuple.Create(114, "Curses/Icon_Curse_Splatter") },
        { "Curses/Icon_Curse_Tentacle", Tuple.Create(114, "Curses/Icon_Curse_Tentacle") }
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
            return new List<Tuple<int, string>> { simplified };
        }

        if (!simpleName.Contains(":")) return new List<Tuple<int, string>>();

        try
        {
            var rName = simpleName.Split(':')[1];
            var regionIndex = (int)(SkinSlots)Enum.Parse(typeof(SkinSlots), simpleName.Split(':')[0]);
            region.name = regionIndex + ":" + rName + "#" + add;
            return new List<Tuple<int, string>> { Tuple.Create(regionIndex, rName) };
        }
        catch (Exception)
        {
            // ignored
        }

        return new List<Tuple<int, string>>();
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
        string name = $"CustomTarotSkin_{internalName}";
        if (!TarotSprites.ContainsKey(name)) TarotSprites.Add(name, skin);
        return name;
    }
    
    internal static Skin CreateOrGetTarotSkinFromTemplate(SkeletonData template, string skinName)
    {
        return TarotSkins.TryGetValue(skinName, out var fromTemplate) ? fromTemplate : CreateTarotSkin(template.Skins.ToList()[1], skinName);
    }

    private static Skin CreateTarotSkin(Skin template, string skinName)
    {
        Sprite sprite = TarotSprites[skinName];
        SpineAtlasAsset atlas = CreateSingleTextureAtlas(sprite);

        Skin skin = new(skinName);

        AtlasRegion atlasRegion = atlas.GetAtlas().FindRegion("GENERIC_ATTACHMENT").Clone();
        Skin.SkinEntry back = template.Attachments.ToList()[0];
        back = new Skin.SkinEntry(back.SlotIndex, back.Name, back.Attachment.Copy());
        skin.SetAttachment(back.SlotIndex, back.Name, back.Attachment);
        Skin.SkinEntry front = template.Attachments.ToList()[1];
        front = new Skin.SkinEntry(front.SlotIndex, front.Name, front.Attachment.Copy());
        if (front.Attachment is MeshAttachment customAttachment)
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
            customAttachment.Name = "CustomTarotSkin_" + skinName;
            customAttachment.SetRegion(atlasRegion);
            atlasRegion.name = "CustomTarotSkin_" + atlasRegion.name;
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

            skin.SetAttachment(front.SlotIndex, front.Name, customAttachment);
        }
        TarotSkins.Add(skinName, skin);
        return skin;
    }

    private static SpineAtlasAsset CreateSingleTextureAtlas(Sprite sprite)
    {
        int n = NumGenericAtlases;
        int w = (int) sprite.bounds.size.x;
        int h = (int) sprite.bounds.size.y;
        string atlasText = $"\r\ngeneric_sheet_{n}\r\nsize: {w},{h}\r\nformat: RGBA8888\r\nfilter: Linear,Linear"
            + "\r\nrepeat: none\r\nGENERIC_ATTACHMENT\r\nrotate: false\r\nxy: 0, 0\r\n"
            + $"size: {w}, {h}\r\norig: {w}, {h}\r\noffset: 0, 0\r\nindex: -1";
        Texture tex = sprite.texture;
        tex.name = $"generic_sheet_{n}";

        Material mat = new(Shader.Find("Spine/Skeleton")) {
            mainTexture = tex
        };

        Material[] materials = { mat };
        SpineAtlasAsset atlas = SpineAtlasAsset.CreateRuntimeInstance(new TextAsset(atlasText), materials, true);
        NumGenericAtlases++;
        return atlas;
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