---
title: Meals
description: Documentation on how to add a custom Meal using the Cult Of The Lamb API
---
## Creating Meals

Creating meals is almost identical to creating Custom items. You first create a class overriding `CustomMeal`. Example:

```csharp
using COTL_API.CustomInventory;
using COTL_API.Helpers;
using UnityEngine;
using System.IO;

public class ExampleMeal : CustomMeal
{
    override string InternalName => "Example_Meal";
    public override string LocalizedName() { return "Example Meal"; }
    public override string LocalizedDescription() { return "This is an example meal"; }
    //used for spawning object in the world
    public override Sprite Sprite => TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "Assets", "example_meal.png"));

    // A list of Effects that will occur when eating this Meal
    override MealEffect[] MealEffects =>
    [
        new MealEffect()
        {
            MealEffectType = CookingData.MealEffectType.InstantlyVomit,
            Chance = 50;
        }
    ];
    // used for cooking the meal. 
    //the outer list is for storing different recipes for the same Meal 
    //the inner list is for the items and quantities that the recipe requires
    public override List<List<InventoryItem>> Recipe =>
    [
        [
        ]
    ];
}

// this is the "Star Rating" of the meal. Range: 0-3
public override int SatiationLevel => 3;

//this is the amount that the "hunger circle" is filled when cooking the meal. Range: 0-1
public override float TummyRating => 0.6f;
```


Custom Meals support all the overrides that custom items do, despite the fact that many of them are non-functional. all overrides that modify the behaviour of the item in-inventory don't function, as you don't pick up meals into your inventory.

> WARNING:
> Overriding `ItemPickupToImitate` with any item that isn't a meal will cause an error!

## Adding Items

To add an meal to the game, simply use `CustomItemManager.Add()`.  
Example:

```csharp
using COTL_API.CustomInventory;

public static InventoryItem.ITEM_TYPE ExampleMeal { get; private set; }

private void Awake()
{
    ExampleMeal = CustomItemManager.Add(new ExampleMeal());
}
```

Assigning the result of `CustomItemManager.Add()` allows you to reference that meal elsewhere in your code using `Plugin.ExampleMeal`.

## Creating MealEffects

MealEffects occur when a follower or player consume a meal. To create a custom MealEffect you first create a class overriding `CustomMealEffect`. Example:

```csharp
using COTL_API.CustomInventory;
using COTL_API.Helpers;
using UnityEngine;
using System.IO;

public class ExampleMealEffect : CustomMealEffect
{
    public override string Internal name => "ExampleEffect";
    public override Action<FollowerBrain> Effect = DoSomething:

    public void DoSomething(FollowerBrain follower)
    {
        LogInfo($"my name is {FollowerBrain.Info.Name} and I just ate something.");
    }
}
```

To add the effect to the game, simply use `CustomMealEffectManager.Add()`.  
Example:

```csharp
using COTL_API.CustomInventory;

public static CookingData.MealEffectType ExampleMealEffect { get; private set; }

private void Awake()
{
    ExampleMealEffect = CustomMealEffectManager.Add(new ExampleMealEffect());
}
```

Assigning the result of `CustomMealEffectmManager.Add()` allows you to reference that meal elsewhere in your code using `Plugin.ExampleMealEffect`.

## Final Steps

For the icon to load, you need to put it in the appropriate location. For the example, this would be `/Assets/example_item.png` relative to the root folder containing the .dll  
Directory structure:

```
📂plugins
 ┣📂Assets
 ┃ ┗🖼️example_meal.png
 ┗📜mod_name.dll
```