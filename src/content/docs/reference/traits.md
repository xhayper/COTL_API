---
title: Traits
description: Documentation on how to add a custom trait using Cult of the Lamb API
---

## Creating Traits

To create an trait, you first need to make a class overriding `CustomTrait`.  
Example:

```csharp
using COTL_API.CustomTraits;
using UnityEngine;
using System.IO;
```

```csharp
[HarmonyPatch]
internal class ExampleTrait : CustomTrait
     {
         public override string InternalName => "ExampleTrait";
     
         public override bool Positive => true;
     
        // exclusive traits are traits that can't appear along with this trait!
        // if they are also custom defined trait, you only need to exclude it on 
        // one of the traits.
         public override List<FollowerTrait.TraitType> ExclusiveTraits =>
         [
             FollowerTrait.TraitType.RoyalPooper
         ];

         public override TraitFlags TraitFlags => TraitFlags.RareStartingTrait;
         
         public override string LocalizedTitle() => "Example Trait";
         
         public override string LocalizedDescription() => "this trait is just an example :).";
     
         public override Sprite Icon => TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "Assets", "ExampleTrait.png"));
     
        // by default, no behaviour for custom traits is added. use patches check in 
        // your own code for the presence of the trait, and change the game's 
        // behvaiour accordingly.
         [HarmonyPatch(typeof(FollowerBrain), nameof(FollowerBrain.GetPoopType))]
         [HarmonyPrefix]
         private static bool FollowerBrain_GetPoopType(ref FollowerBrain __instance, ref StructureBrain.TYPES __result)
         {
             if (!__instance.Info.Traits.Contains(Plugin.ExampleTrait)) return true;
             
             __result = StructureBrain.TYPES.POOP_RAINBOW;
             DataManager.Instance.DaySinceLastSpecialPoop = TimeManager.CurrentDay;
             return false;
     
         }
}
```

There is no diffrence between cult traits and regular traits. Cult traits are added by `FollowerTrait.AddCultTrait(FollowerTrait.TraitType);` and regular ones by `followerBrain.AddTrait(FollowerTrait.TraitType);` on a follower's brain.

`CustomTrait` supports the following overrides:
| Type | Name | Default |
|-|-|-|
| string | InternalName | \[REQUIRED\] |
|bool|Positive| true|
|bool|IsTraitUnavailable()|false|
|Sprite|Icon|TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));|
|List<FollowerTrait.TraitType>| ExclusiveTraits|[]|
TraitFlags|TraitFlags|TraitFlags.None|
|string|LocalizedTitle()|LocalizationManager.GetTranslation($"Traits/{ModPrefix}.{InternalName}")|
|string|LocalizedDescription()|LocalizationManager.GetTranslation($"Traits/{InternalName}.description")|

## Adding Traits

To add a trait to the game, simply use `CustomTraitManager.Add()`.  
Example:

```csharp
using COTL_API.CustomTraits;
public static FollowerTrait.TraitType ExampleTrait { get; private set; }
```

```csharp
private void Awake()
{
    ExampleTrait = CustomTraitManager.Add(new ExampleTrait());
}
```

Assigning the result of `CustomTraitManager.Add()` allows you to reference that trait elsewhere in your code using `Plugin.ExampleTrait`.

## Final Steps

For the icon to load, you need to put it in the appropriate location. For the example, this would be `/Assets/ExampleTrait.png` relative to the root folder containing the .dll  
Directory structure:

```
📂plugins
 ┣📂Assets
 ┃ ┗🖼️ExampleTrait.png
 ┗📜mod_name.dll
```
