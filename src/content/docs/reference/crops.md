---
title: Crops
description: Documentation on how to add Custom Seeds that grow crops using the Cult Of The Lamb API
---

Custom Crops are actually custom items, the item created is a seed that is planeted in a farm plot and grows the custom crops.

## Creating Crops

To create a custom crop, create a class that overrides `CustomCrop`:

```csharp
using COTL_API.CustomInventory;
using COTL_API.Helpers;
using UnityEngine;
using System.IO;

public class ExampleSeed : CustomCrop
{
    override string InternalName => "Example_Seed";
    public override string LocalizedName() { return "Example Seed"; }
    public override string LocalizedDescription() { return "This is an example Seed"; }
    //used for spawning item in the world
    public override Sprite Sprite => TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "Assets", "example_seed.png"));
    // used for the inventory
    public override Sprite InventoryIcon => Sprite;

    // these will be the stages of growth for the crop. the last stage is the stage that will get harvested
    public override List<Sprite> CropStates { get; } =
    [
        TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "Assets", "crop_stage_1.png")),
        TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "Assets", "crop_stage_2.png")),
        TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "Assets", "crop_stage_3.png")),
        TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "Assets", "crop_stage_4.png")),
    ];

    public override float CropGrowthTime => 9f;

    public override float PickingTime => 10f;

    // this will be the result of picking the crop. the first Must be the "crop" result (e.g: pumpkins) and the second Must be the seed (e.g: pumpking seed). it must be at least size 2, anything after 2 is ignored.
    public override List<InventoryItem.ITEM_TYPE> HarvestResult =>
    [
        InvetoryItem.ITEM_TYPE.GOD_TEAR,
        Plugin.ExampleSeed,
    ];
    public override Vector2Int CropCountToDropRange => new(6, 10);
    public override string HarvestText => "Pick <color=#DB36DB>Example Seed</color>";

}
```

`CustomCrop` support the following overrides:

| Type                            | Name                 | Default                               |
| ------------------------------- | -------------------- | ------------------------------------- |
| List\<InventoryItem.ITEM_TYPE\> | HarvestResult        | REQUIRED                              |
| List\<Sprite\>                  | CropStates           | []                                    |
| float                           | CropGrowthTime       | 9f                                    |
| float                           | PickingTime          | 2.5f                                  |
| Vector2Int                      | CropCountToDropRange | (3,4)                                 |
| string                          | HarvestText          | "Pick <color=#FD1D03>Berries</color>" |

Custom Crops also support all overrides available for Custom Items.

## Adding Crops

To add a custom crop into the game, simply use `CustomItemManager.Add(CustomCrop crop)`.  
Example:

```csharp
using COTL_API.CustomInventory;

public static InventoryItem.ITEM_TYPE ExampleSeed { get; private set; }

private void Awake()
{
    ExampleSeed = CustomItemManager.Add(new ExampleSeed());
}
```

Assigning the result of `CustomItemManager.Add()` allows you to reference that crop elsewhere in your code using `Plugin.ExampleSeed`.

## Final Steps

For the icon to load, you need to put it in the appropriate location. For the example, this would be `/Assets/example_seed.png` relative to the root folder containing the .dll  
Directory structure:

```
📂plugins
 ┣📂Assets
 ┃ ┗🖼️example_seed.png
 ┃ ┗🖼️crop_stage_1.png
 ┃ ┗🖼️crop_stage_2.png
 ┃ ┗🖼️crop_stage_3.png
 ┃ ┗🖼️crop_stage_4.png
 ┗📜mod_name.dll
```
