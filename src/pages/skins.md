---
title: Skins
description: Documentation on how to add a custom skin using the Cult of the Lamb API
layout: ../layouts/MainLayout.astro
---

## Creating Skins

**This applies to both follower skins and player skins.**

To create a skin, you first need to make a class overriding `CustomFollowerSkin` or `CustomPlayerSkin`.  
Example:

```csharp
using COTL_API.CustomSkins;
using COTL_API.Helpers;
using UnityEngine;
```

```csharp
internal class ExampleFollowerSkin : CustomFollowerSkin
{
    public override string Name => "Example Skin";

    public override Texture2D Texture =>
        TextureHelper.CreateTextureFromPath(PluginPaths.ResolveAssetPath("example_sheet.png"));

    public override List<SkinOverride> Overrides => new()
    {
        new SkinOverride("HEAD_SKIN_TOP_BACK", new Rect(0, 127, 111, 127)),
        new SkinOverride("HEAD_SKIN_BTM_BACK", new Rect(0, 0, 111, 127)),
        new SkinOverride("HEAD_SKIN_TOP", new Rect(111, 127, 149, 127)),
        new SkinOverride("HEAD_SKIN_BTM", new Rect(111, 0, 149, 127))
    };
}
```

`CustomFollowerSkin` supports the following overrides:
| Type | Name | Default | Type
|-|-|-|-|
| string | Name | \[REQUIRED\] | Both |
| Texture2D | Texture | \[REQUIRED\] | Both |
| List\<SkinOverride\> | Overrides | \[REQUIRED\] | Both |
| List\<WorshipperData.SlotsAndColours\> | Colors | ((Magenta & Black)) | Follower Skins |
| bool | TwitchPremium | false | Follower Skins |
| bool | Hidden | false | Follower Skins |
| bool | Unlocked | true | Follower Skins |
| bool | Invariant | false | Follower Skins |

## Skin Sheets

The skin sheet must be a single image with rectangle boundaries for each 'attachment' to override.  
Example sheets:<br>
![Example Follower Sheet](/img/follower_skin_example.png)<br>
![Example Player Sheet](/img/player_skin_example.png)

#### Follower Skin Overrides

The following overrides are supported for follower skins:

```

LEFT_ARM_SKIN,
LEFT_SLEEVE,
WEAPON_HAND_SKIN,
LEFT_LEG_SKIN,
RIGHT_LEG_SKIN,
BODY_SKIN,
BODY_SKIN_BOWED,
BODY_SKIN_UP,
Body_Lvl3,
BowlBtm,
BowlFood,
BowlFront,
RIGHT_ARM_SKIN,
RIGHT_SLEEVE,
HEAD_SKIN_BTM,
HEAD_SKIN_BTM_BACK,
HEAD_SKIN_TOP,
HEAD_SKIN_TOP_BACK,
MARKINGS,
Angry_Colouring,
Embarrassed_Colouring,
Possessed_Colouring,
Sick_Colouring,
MOUTH_BEDREST,
MOUTH_CHEEKY,
MOUTH_DEAD,
MOUTH_DERP,
MOUTH_ENLIGHTENED,
MOUTH_GRIN,
MOUTH_HAPPY,
MOUTH_HAPPY_2,
MOUTH_HUNGRY_1,
MOUTH_HUNGRY_2,
MOUTH_INDIFFERENT,
MOUTH_INDIFFERENT_HUNGRY,
MOUTH_INSANE,
MOUTH_KISS,
MOUTH_KISS_BIG,
MOUTH_MUMBLE,
MOUTH_MUMBLE_HUNGRY,
MOUTH_RED,
MOUTH_SACRIFICE,
MOUTH_SAD,
MOUTH_SADDER,
MOUTH_SCARED,
MOUTH_SICK,
MOUTH_SLEEP_0,
MOUTH_SLEEP_1,
MOUTH_TALK_HAPPY,
MOUTH_TALK_INDIFFERENT,
MOUTH_TONGUE_1,
MOUTH_TONGUE_2,
MOUTH_TONGUE_3,
MOUTH_TONGUE_4,
MOUTH_WORRIED,
MOUTH_ENLIGHTENED_2,
RIGHT_EYE,
RIGHT_EYE_ANGRY,
RIGHT_EYE_ANGRY_UP,
RIGHT_EYE_BLACK,
RIGHT_EYE_BRAINWASHED,
RIGHT_EYE_CLOSED,
RIGHT_EYE_CRAZY,
RIGHT_EYE_DEAD,
RIGHT_EYE_DISSENTER,
RIGHT_EYE_DISSENTER_ANGRY,
RIGHT_EYE_ENLIGHTENED,
RIGHT_EYE_HALF_CLOSED,
RIGHT_EYE_HALF_CLOSED_ANGRY,
RIGHT_EYE_INSANE,
RIGHT_EYE_SACRIFICE_1,
RIGHT_EYE_SACRIFICE_2,
RIGHT_EYE_SHOCKED,
RIGHT_EYE_SICK_2,
RIGHT_EYE_SICK,
RIGHT_EYE_SLEEPING,
RIGHT_EYE_SLEEPING_SICK,
RIGHT_EYE_SLEEPING_TIRED,
RIGHT_EYE_SLEEPY,
RIGHT_EYE_SMILE,
RIGHT_EYE_SMILE_DOWN,
RIGHT_EYE_SMILE_UP,
RIGHT_EYE_SQUINT,
RIGHT_EYE_STARS,
RIGHT_EYE_UNCONVERTED,
RIGHT_EYE_UP,
RIGHT_EYE_VERY_ANGRY,
RIGHT_EYE_WHITE,
RIGHT_EYE_WORRIED,
RIGHT_EYE_FIRE_1,
RIGHT_EYE_FIRE_2,
RIGHT_EYE_FIRE_3,
RIGHT_EYE_FIRE_4,
RIGHT_EYE_FIRE_5,
RIGHT_EYE_FIRE_6,
RIGHT_EYE_FIRE_7,
LEFT_EYE,
LEFT_EYE_ANGRY,
LEFT_EYE_ANGRY_UP,
LEFT_EYE_BLACK,
LEFT_EYE_BRAINWASHED,
LEFT_EYE_CLOSED,
LEFT_EYE_CRAZY,
LEFT_EYE_DEAD,
LEFT_EYE_DISSENTER,
LEFT_EYE_DISSENTER_ANGRY,
LEFT_EYE_ENLIGHTENED,
LEFT_EYE_HALF_CLOSED,
LEFT_EYE_HALF_CLOSED_ANGRY,
LEFT_EYE_INSANE,
LEFT_EYE_SACRIFICE_1,
LEFT_EYE_SACRIFICE_2,
LEFT_EYE_SHOCKED,
LEFT_EYE_SICK_2,
LEFT_EYE_SICK,
LEFT_EYE_SLEEPING,
LEFT_EYE_SLEEPING_SICK,
LEFT_EYE_SLEEPING_TIRED,
LEFT_EYE_SLEEPY,
LEFT_EYE_SMILE,
LEFT_EYE_SMILE_DOWN,
LEFT_EYE_SMILE_UP,
LEFT_EYE_SQUINT,
LEFT_EYE_STARS,
LEFT_EYE_UNCONVERTED,
LEFT_EYE_UP,
LEFT_EYE_VERY_ANGRY,
LEFT_EYE_WHITE,
LEFT_EYE_WORRIED,
LEFT_EYE_FIRE_1,
LEFT_EYE_FIRE_2,
LEFT_EYE_FIRE_3,
LEFT_EYE_FIRE_4,
LEFT_EYE_FIRE_5,
LEFT_EYE_FIRE_6,
LEFT_EYE_FIRE_7
```

#### Player Skin Overrides

The following overrrides are supported for player skins:

```

Crown_Particle1,
Crown_Particle2,
Crown_Particle6,
effects/Crown_Particle3,
effects/Crown_Particle4,
effects/Crown_Particle5,
sunburst,
sunburst2,
Corpse,
Halo,
ARM_LEFT,
PonchoShoulder,
FishingRod_Front,
Tools/FishingRod,
Tools/FishingRod2,
Tools/Mop,
Tools/PITCHFORK,
Tools/SEED_BAG,
Tools/SPADE,
Tools/WATERING_CAN,
LEG_LEFT,
LEG_RIGHT,
Body,
PonchoLeft,
PonchoLeft2,
DaggerFlipped,
Weapons/Axe,
Weapons/Blunderbuss,
Weapons/Dagger,
Weapons/Hammer,
Weapons/Sword,
ARM_RIGHT,
ArmSpikes,
PonchoRight,
PonchoRight2,
PonchoExtra,
images/Rope,
images/RopeTopRight,
images/RopeTopLeft,
Bell,
Antler,
Antler_Horn,
Antler_RITUAL,
Antler_SERMON,
EAR_LEFT,
EAR_RITUAL,
EAR_SERMON,
CrownGlow,
images/CrownSpikes,
BigCrown,
CROWN,
CROWN_RITUAL,
CROWN_SERMON,
CROWN_WHITE,
CROWN_EYE,
images/CrownEyeBig,
images/CrownEyeShut,
images/CrownEyeShut2,
images/CrownEyeShut3,
images/CrownEye_RITUAL,
images/CrownEye_SERMON,
HeadBack,
HeadBackDown,
HeadBackDown_RITUAL,
HeadBackDown_SERMON,
HeadFront,
HeadFrontDown,
EAR_RIGHT,
EAR_RIGHT_RITUAL,
EAR_RIGHT_SERMON,
effects/eye_blood,
effects/eye_tears,
Face/MOUTH_CHEEKY,
Face/MOUTH_CHUBBY,
Face/MOUTH_DEAD,
Face/MOUTH_GRUMPY,
Face/MOUTH_HAPPY,
Face/MOUTH_INDIFFERENT,
Face/MOUTH_KAWAII,
Face/MOUTH_OO,
Face/MOUTH_OPEN,
Face/MOUTH_SAD,
Face/MOUTH_SCARED,
Face/MOUTH_SLEEP_0,
Face/MOUTH_SLEEP_1,
Face/MOUTH_TONGUE,
Face/MOUTH_UNCONVERTED,
MOUTH_GRIMACE,
MOUTH_NORMAL,
MOUTH_SNARL,
MOUTH_TALK,
MOUTH_TALK1,
MOUTH_TALK2,
MOUTH_TALK3,
MOUTH_TALK4,
MOUTH_TALK5,
MOUTH_TALK_HAPPY,
MOUTH_UNCONVERTED_SPEAK,
EYE,
EYE_ANGRY_LEFT,
EYE_ANGRY_LEFT_UP,
EYE_BACK,
EYE_DETERMINED_DOWN_LEFT,
EYE_DETERMINED_LEFT,
EYE_DOWN,
EYE_GRIMACE,
EYE_HALF_CLOSED,
EYE_HAPPY,
EYE_HAPPY2,
EYE_UP,
EYE_WEARY_LEFT,
EYE_WEARY_LEFT_DOWN,
EYE_WHITE,
EYE_WHITE_ANGRY,
EYE_WORRIED_LEFT,
Face/EYE_CLOSED,
Face/EYE_DEAD,
Face/EYE_RED,
Face/EYE_RED_ANGRY,
Face/EYE_SHOCKED,
Face/EYE_SLEEPING,
Face/EYE_SQUINT,
Face/EYE_UNCONVERTED,
Face/EYE_UNCONVERTED_WORRIED,
EYE_ANGRY_RIGHT,
EYE_ANGRY_RIGHT_UP,
EYE_DETERMINED_DOWN_RIGHT,
EYE_DETERMINED_RIGHT,
EYE_WEARY_RIGHT,
EYE_WEARY_RIGHT_DOWN,
EYE_WORRIED_RIGHT,
HairTuft,
Tools/BookFlipping_1,
Tools/BookFlipping_2,
Tools/BookFlipping_3,
Tools/BookFlipping_4,
Tools/Book_closed,
Tools/Book_open,
PonchoRightCorner,
images/CrownMouth,
images/CrownMouthOpen,
Tools/Chalice,
Tools/Chalice_Skull,
Tools/Chalice_Skull_Drink,
effects/slam_effect0001,
effects/slam_effect0002,
effects/slam_effect0003,
effects/slam_effect0004,
effects/slam_effect0005,
effects/slam_effect0006,
images/CrownSpikes2,
images/AttackHand1,
images/AttackHand2,
Weapons/Sling,
Weapons/SlingRope,
SlingHand,
Arm_frontbit,
whiteball,
effects/whiteball,
effects/portal_btm,
effects/portal_top,
portal_splash,
GrappleHook,
Weapons/Lute,
Weapons/SlingHand,
images/Crown_half_left,
images/Crown_half_right,
Sparks1,
Sparks2,
Weapons/SpecialSword_1,
Weapons/SpecialSword_2,
Weapons/SpecialSword_3,
Weapons/SpecialSword_4,
KnowledgeParchment,
Knowledge_Curse,
Knowledge_Decoration,
Knowledge_Trinket,
Knowledge_Weapon,
MonsterHeart_glow,
Net,
Tools/Hammer,
Tools/Pickaxe,
Tools/Pickaxe2,
Tools/Woodaxe,
Tools/Woodaxe2,
GiftMedium,
GiftSmall,
effects/MonsterBlood1,
MonsterBlood2,
Tools/CardBack,
Tools/CardFront,
RitualSymbolHalo,
RitualSymbol_1,
RitualSymbol_2,
effects/RitualRing2,
effects/SermonRing2,
AttackSlash1,
AttackSlash2,
effects/RitualRing,
effects/SermonRing,
CollarPiece1,
CollarPiece2,
ChainBit1,
ChainBit2,
ChainBit3,
SwordHeavy,
Weapons/SwordHeavy_Charm,
Weapons/SwordHeavy_Ice,
Weapons/SwordHeavy_Necromancy,
effects/SpawnHeavy_1,
effects/SpawnHeavy_2,
effects/SpawnHeavy_3,
effects/SpawnHeavy_4,
SpawnHeavy_glow,
FireSmall_0001,
FireSmall_0002,
FireSmall_0003,
FireSmall_0004,
FireSmall_0005,
FireSmall_0006,
FireSmall_0007,
FireWild_0001,
FireWild_0002,
FireWild_0003,
FireWild_0004,
FireWild_0005,
FireWild_0006,
FireWild_0007,
FireWild_0008,
FireWild_0009,
effects/chunder_1,
effects/chunder_2,
effects/chunder_3,
Curses/Icon_Curse_Blast,
Curses/Icon_Curse_Fireball,
Curses/Icon_Curse_Slash,
Curses/Icon_Curse_Splatter,
Curses/Icon_Curse_Tentacle
```

## Colors

Followers can have unique colors by overriding attachments. All white pixels in the overrides defined in Colors get replaced by the selected color.  
Example Colors override:

```csharp
public override List<WorshipperData.SlotsAndColours> Colors { get; } = new()
  {
      new WorshipperData.SlotsAndColours
      {
          SlotAndColours = new List<WorshipperData.SlotAndColor>
          {
              new("ARM_LEFT_SKIN", new Color(1, 0, 0)),
              new("ARM_RIGHT_SKIN", new Color(1, 0, 0)),
              new("LEG_LEFT_SKIN", new Color(1, 0, 0)),
              new("LEG_RIGHT_SKIN", new Color(1, 0, 0)),
              new("BODY_SKIN", new Color(1, 0, 0)),
              new("BODY_SKIN_BOWED", new Color(1, 0, 0)),
              new("BODY_SKIN_UP", new Color(1, 0, 0)),
              new("HEAD_SKIN_BTM", new Color(1, 0, 0)),
              new("HEAD_SKIN_TOP", new Color(1, 0.5f, 0)),
          }
      },
      new WorshipperData.SlotsAndColours
      {
          SlotAndColours = new List<WorshipperData.SlotAndColor>
          {
              new("ARM_LEFT_SKIN", new Color(0, 1, 0)),
              new("ARM_RIGHT_SKIN", new Color(0, 1, 0)),
              new("LEG_LEFT_SKIN", new Color(0, 1, 0)),
              new("LEG_RIGHT_SKIN", new Color(0, 1, 0)),
              new("BODY_SKIN", new Color(0, 1, 0)),
              new("BODY_SKIN_BOWED", new Color(0, 1, 0)),
              new("BODY_SKIN_UP", new Color(0, 1, 0)),
              new("HEAD_SKIN_BTM", new Color(0, 1, 0)),
              new("HEAD_SKIN_TOP", new Color(0, 1, 0.5f)),
          }
      },
      new WorshipperData.SlotsAndColours
      {
          SlotAndColours = new List<WorshipperData.SlotAndColor>
          {
              new("ARM_LEFT_SKIN", new Color(0, 0, 1)),
              new("ARM_RIGHT_SKIN", new Color(0, 0, 1)),
              new("LEG_LEFT_SKIN", new Color(0, 0, 1)),
              new("LEG_RIGHT_SKIN", new Color(0, 0, 1)),
              new("BODY_SKIN", new Color(0, 0, 1)),
              new("BODY_SKIN_BOWED", new Color(0, 0, 1)),
              new("BODY_SKIN_UP", new Color(0, 0, 1)),
              new("HEAD_SKIN_BTM", new Color(0, 0, 1)),
              new("HEAD_SKIN_TOP", new Color(0.5f, 0, 1)),
          }
      }
  };
```

## Adding Skins

To add a skin to the game, simply use `CustomSkinManager.Add()`.  
Example:

```csharp
using COTL_API.CustomSkins;
```

```csharp
private void Awake()
{
    CustomSkinManager.AddFollowerSkin(new ExampleFollowerSkin());
    CustomSkinManager.AddPlayerSkin(new ExamplePlayerSkin());
}
```

## Final Steps

For the skin texture to load, you need to put it in the appropriate location. For the example, this would be `/Assets/example_sheet.png` relative to the root folder containing the .dll  
Directory structure:

```
📂plugins
 ┣📂Assets
 ┃ ┗🖼️example_sheet.png
 ┗📜mod_name.dll
```
