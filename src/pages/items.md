---
title: Items
description: Items Docs
layout: ../layouts/MainLayout.astro
---

## Creating Items

To create an item, you first need to make a class overriding `CustomInventoryItem`.  
Example:

```csharp
using COTL_API.CustomInventory;
using COTL_API.Helpers;
using System.IO;
using UnityEngine;
```

```csharp
internal class ExampleItem : CustomInventoryItem
{
    public override string InternalName => "Example_Item";
    public override string LocalizedName() { return "Example Item"; }
    public override string LocalizedDescription() { return "This is an example item"; }

    public override Sprite InventoryIcon => TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "Assets", "example_item.png"));
}
```

`CustomInventoryItem` supports the following overrides:
| Type | Name | Default |
|-|-|-|
| string | InternalName | \[REQUIRED\] |
| Sprite | InventoryIcon | TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png")) |
| InventoryItem.ITEM_CATEGORIES | ItemCategory | InventoryItem.ITEM_CATEGORIES.NONE |
| InventoryItem.ITEM_TYPE | SeedType | InventoryItem.ITEM_TYPE.NONE; |
| string | LocalizedName() | LocalizationManager.GetTranslation($"Inventory/{ModPrefix}.{InternalName}") |
| string | LocalizedLore() | LocalizationManager.GetTranslation($"Inventory/{ModPrefix}.{InternalName}/Lore") |
| string | LocalizedDescription() | LocalizationManager.GetTranslation($"Inventory/{ModPrefix}.{InternalName}/Description") |
| int | FuelWeight | 1
| int | FoodSatitation | 75
| bool | IsFish | false
| bool | IsFood | false
| bool | IsBigFish | false
| bool | IsCurrency | false
| bool | IsSeed | false
| bool | IsPlantable | false
| bool | IsBurnableFuel | false
| bool | CanBeGivenToFollower | false
| string | GiftTitle(Follower follower) | $"{Name()} ({Inventory.GetItemQuantity(ItemType)})"
| FollowerCommands | GiftCommand | FollowerCommands.None
| void | OnGiftTo(Follower follower, System.Action onFinish) | onFinish()

## Adding Items

To add an item to the game, simply use `CustomItemManager.Add()`.  
Example:

```csharp
using COTL_API.CustomInventory;
```

```csharp
CustomItemManager.Add(new ExampleItem());
```

## Final Steps

For the icon to load, you need to put it in the appropriate location. For the example, this would be `/Assets/example_item.png` relative to the root folder containing the .dll  
Directory structure:

```
📂plugins
 ┣📂Assets
 ┃ ┗🖼️example_item.png
 ┗📜mod_name.dll
```
