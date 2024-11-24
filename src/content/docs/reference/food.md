---
title: Food
description: Documentation on how to add a custom Meal using the Cult Of The Lamb API
---
Creating custom food is almost identical to creating custom items, and custom food posses all the overrides that custom items do.
Custom food can also have custom MealEffects that trigger upon consuming the food. 

## Creating Meals

To create a custom meal, create a class that overrides `CustomMeal`:

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
    // used for the cooking menu/inventory
    public override Sprite InventoryIcon => Sprite;
    
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
        // first recipe
        [   // item, quantity
            new InventoryItem(InventoryItem.ITEM_TYPE.BEETROOT, 2),
        ],
        // second recipe
        [
            new InventoryItem(InventorItem.ITEM_TYPE.CAULIFLOWER, 2),
        ],
    ];

    // this is the "Star Rating" of the meal. Range: [0, 3]
    public override int SatiationLevel => 3;

    //this is the amount that the "hunger circle" is filled when cooking the meal. Range: [0, 1]
    public override float TummyRating => 0.6f;
}
```
Recipes defined first are crafted first, and will consume materials untill they can't be crafted. unfortuntely there is no way swap recipies at will, so it's advised to define "cheaper" recipes first.

Custom Meals support all the overrides that custom items do, despite the fact that many of them are non-functional. all overrides that modify the behaviour of the item in-inventory don't function, as you don't pick up meals into your inventory.

> WARNING: 
> Overriding `ItemPickupToImitate` with any item that isn't a meal will cause an error!

`CustomMeal` support the following overrides:

| Type | Name | Default |
|-|-|-|
|List<List<InventoryItem.ITEM_TYPE>>|Recipe|\[REQUIRED\]|
|CookingData.MealEffects[]|MealEffects|\[REQUIRED\]|
|Vector3|ItemDisplayOffset|null|
|MealQuality|Quality|MealQuality.NORMAL|
|float|TummyRating|0|
|int|SatiationLevel|0|
|bool|MealSafeToEat|true|

## Adding Meals

To add a custom meal into the game, simply use `CustomItemManager.Add()`.  
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

## Creating Drinks

To create a custom drink, create a class that overrides `CustomDrink`:

```csharp
using COTL_API.CustomInventory;
using COTL_API.Helpers;
using UnityEngine;
using System.IO;

public class ExampleDrink : CustomDrink
{
    override string InternalName => "Example_Drink";
    public override string LocalizedName() { return "Example Drink"; }
    public override string LocalizedDescription() { return "This is an example drink"; }
    //used for spawning object in the world
    public override Sprite Sprite => TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "Assets", "example_drink.png"));
    // used for the cooking menu/inventory
    public override Sprite InventoryIcon => Sprite;

    // A list of Effects that will occur when drinking this Drink
    override MealEffect[] MealEffects =>
    [
        new MealEffect()
        {
            MealEffectType = CookingData.MealEffectType.CausesDrunk,
            Chance = 75
        }
    ];
    // used for brewing the drink. 
    //the outer list is for storing different recipes for the same Drink 
    //the inner list is for the items and quantities that the recipe requires
    public override List<List<InventoryItem>> Recipe =>
    [
        // First recipe
        [
            // item, quantity
            new InventoryItem(InventoryItem.ITEM_TYPE.YOLK, 1)
        ],
        // Secon recipe
        [
            new InventoryItem(InventoryItem.ITEM_TYPE.GOD_TEAR, 1),
        ],
    ];
    
    // this is the "Star Rating" of the drink. Range: 0-3
    public override int SatiationLevel => 2;

    // this is the amount of Sin gained by the follower who drinks this Drink,
    // range: [0, 65]
    public override int Pleasure => 50;
}
```
>WARNING:
> Overriding `ItemPickupToImitate` with any item that isn't a drink will cause an error!

`CustomDrink` supports the following overrides:

| Type | Name | Default |
|-|-|-|
|List<List<InventoryItem.ITEM_TYPE>>|Recipe|\[REQUIRED\]|
|CookingData.MealEffects[]|MealEffects|\[REQUIRED\]|
|int|SatiationLevel|0|
|Vector3|ItemDisplayOffset|null|
|int|Pleasure|0|

## Adding Drinks

To add a custom drink into the game, simply use `CustomItemManager.Add()`.  
Example:

```csharp
using COTL_API.CustomInventory;

public static InventoryItem.ITEM_TYPE ExampleDrink { get; private set; }

private void Awake()
{
    ExampleDrink = CustomItemManager.Add(new ExampleDrink());
}
```

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
    public override Action<FollowerBrain> Effect = DoSomething;

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

> WARNING: Make sure to register Custom Meal Effects before you register your custom meals, otherwise any custom meal effects Won't work. Alternatively use Lazy loading.

`CustomMealEffect` supports the following overrides:

| Type | Name | Default |
|-|-|-|
| string | InternalName | \[REQUIRED\] |
| Action\<FollowerBrain\> | Effect | \[REQUIRED\]|
|bool | EffectEnabled()| true |
|bool|Positive()|true|
|string| Description()|$"CookingData/{InternalName}/Description"|
|string|DescriptionSuffix()|""|

## Final Steps

For the icon to load, you need to put it in the appropriate location. For the example, this would be `/Assets/example_food.png` relative to the root folder containing the .dll  
Directory structure:

```
📂plugins
 ┣📂Assets
 ┃ ┗🖼️example_meal.png
 ┃ ┗🖼️example_drink.png
 ┗📜mod_name.dll
```
