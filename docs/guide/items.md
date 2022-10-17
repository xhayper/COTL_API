# Items

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

    //used for inventory icons
    public override Sprite InventoryIcon => TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "Assets", "example_item.png"));

    //used for spawning object in the world
    public override Sprite Sprite => TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "Assets", "example_item.png"));
}
```

`CustomInventoryItem` supports the following overrides:<br>
| Type                          | Name                                                | Default                                                                                 |
|-------------------------------|-----------------------------------------------------|-----------------------------------------------------------------------------------------|
| string                        | InternalName                                        | \[REQUIRED\]                                                                            |
| Sprite                        | InventoryIcon                                       | TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"))     |
| Sprite                        | Sprite                                              | TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"))     |
| InventoryItem.ITEM_CATEGORIES | ItemCategory                                        | InventoryItem.ITEM_CATEGORIES.NONE                                                      |
| InventoryItem.ITEM_TYPE       | SeedType                                            | InventoryItem.ITEM_TYPE.NONE;                                                           |
| string                        | LocalizedName()                                     | LocalizationManager.GetTranslation($"Inventory/{ModPrefix}.{InternalName}")             |
| string                        | LocalizedLore()                                     | LocalizationManager.GetTranslation($"Inventory/{ModPrefix}.{InternalName}/Lore")        |
| string                        | LocalizedDescription()                              | LocalizationManager.GetTranslation($"Inventory/{ModPrefix}.{InternalName}/Description") |
| int                           | FuelWeight                                          | 1                                                                                       |
| int                           | FoodSatitation                                      | 75                                                                                      |
| bool                          | IsFish                                              | false                                                                                   |
| bool                          | IsFood                                              | false                                                                                   |
| bool                          | IsBigFish                                           | false                                                                                   |
| bool                          | IsCurrency                                          | false                                                                                   |
| bool                          | IsSeed                                              | false                                                                                   |
| bool                          | IsPlantable                                         | false                                                                                   |
| bool                          | IsBurnableFuel                                      | false                                                                                   |
| bool                          | CanBeGivenToFollower                                | false                                                                                   |
| string                        | GiftTitle(Follower follower)                        | $"{Name()} ({Inventory.GetItemQuantity(ItemType)})"                                     |
| FollowerCommands              | GiftCommand                                         | FollowerCommands.None                                                                   |
| void                          | OnGiftTo(Follower follower, System.Action onFinish) | onFinish()                                                                              |
| CustomItemManager.ItemRarity  | Rarity                                              | CustomItemManager.ItemRarity.COMMON                                                     |
| Vector3                       | LocalScale                                          | new(0.5f, 0.5f, 0.5f)                                                                   |
| bool                          | AddItemToOfferingShrine                             | false;                                                                                  |
| InventoryItem.ITEM_TYPE       | ItemPickUpToImitate                                 | InventoryItem.ITEM_TYPE.LOG                                                             |
| bool                          | AddItemToDungeonChests                              | false                                                                                   |
| int                           | DungeonChestSpawnChance                             | 100                                                                                     |
| int                           | DungeonChestMinAmount                               | 1                                                                                       |
| int                           | DungeonChestMaxAmount                               | 1                                                                                       |
| bool                          | CanBeRefined                                        | false                                                                                   |
| InventoryItem.ITEM_TYPE       | RefineryInput                                       | InventoryItem.ITEM_TYPE.LOG                                                             |
| int                           | RefineryInputQty                                    | 15                                                                                      |
| float                         | CustomRefineryDuration                              | 0                                                                                       |

## Helpers

| Type | Name                                                                | Purpose                                                                                        |
| ---- | ------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------- |
| bool | CustomItemManager.DropLoot(CustomInventoryItem customInventoryItem) | Used to determine if a custom item should drop or not based on the items chance configuration. |

## Adding Items

To add an item to the game, simply use `CustomItemManager.Add()`.  
Example:

```csharp
using COTL_API.CustomInventory;
public static InventoryItem.ITEM_TYPE ExampleItem { get; private set; }
```

```csharp
private void Awake()
{
    ExampleItem = CustomItemManager.Add(new ExampleItem());
}
```

Assigning the result of `CustomItemManager.Add()` allows you to reference that item elsewhere in your code using `Plugin.ExampleItem`. Example usage below.

## Spawning Items

To spawn an item into the world, add the neccessary overrides (see table above) and simply use `InventoryItem.Spawn(Plugin.ExampleItem, 5, Position);`.
The properties the item will take on (such as bounce, speed etc) are determined by `ItemPickUpToImitate`.
The sprite that is used for spawning is the same as the inventory icon.

- Items can be added to dungeon chests. 
- You can control the chance to spawn, and the minimum/maximum amount to spawn.
- There is a helper method to aid in determining chance, simply call `CustomItemManager.DropLoot(Plugin.ExampleItem);` which returns a boolean true/false.
- Keep in mind the chance is affected by the players current LuckModifier.
- Items can be added to offering shrines. The shrines have two pools, COMMON and RARE. The default for custom items is COMMON.

## Refinery

Custom items can be added to the refinery as a refinable resource. Simply follow the steps below:
- Override the refinery fields.
- **CanBeRefined** needs to be set to true. The default is false.
- **RefineryInput** is the item/materials required.
- **RefineryInputQty** is how many of the above items are required.

`The default time used by the refinery is 128f, with that then being modified based on the followers attributes.`
- **CustomRefineryDuration** is optional. Leaving it at 0 or not overriding is off, anything greater than 0 becomes the time to refine.

## Final Steps

For the icon to load, you need to put it in the appropriate location. For the example, this would be `/Assets/example_item.png` relative to the root folder containing the .dll  
Directory structure:

```
📂plugins
 ┣📂Assets
 ┃ ┗🖼️example_item.png
 ┗📜mod_name.dll
```