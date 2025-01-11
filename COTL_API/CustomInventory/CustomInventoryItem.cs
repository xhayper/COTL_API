using I2.Loc;
using UnityEngine;

namespace COTL_API.CustomInventory;

public abstract class CustomInventoryItem
{
    /// <summary>
    ///     This is the name given to custom items when they are spawned into the world.
    /// </summary>
    internal string InternalObjectName = "";

    internal InventoryItem.ITEM_TYPE ItemType;
    internal string ModPrefix = "";
    public abstract string InternalName { get; }

    public virtual CustomInventoryItemType InventoryItemType { get; } = CustomInventoryItemType.ITEM;

    public virtual Sprite InventoryIcon { get; } =
        TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));

    public virtual InventoryItem.ITEM_CATEGORIES ItemCategory => InventoryItem.ITEM_CATEGORIES.NONE;
    public virtual InventoryItem.ITEM_TYPE SeedType => InventoryItem.ITEM_TYPE.NONE;

    public virtual int FuelWeight => 1;
    public virtual int FoodSatitation => 75;

    public virtual bool IsFish => false;
    public virtual bool IsFood => false;
    public virtual bool IsBigFish => false;
    public virtual bool IsCurrency => false;
    public virtual bool IsBurnableFuel => false;

    public virtual bool CanBeGivenToFollower => false;

    public virtual FollowerCommands GiftCommand => FollowerCommands.None;

    public virtual bool CanBeRefined { get; set; } = false;
    public virtual InventoryItem.ITEM_TYPE RefineryInput { get; set; } = InventoryItem.ITEM_TYPE.LOG;
    public virtual int RefineryInputQty { get; set; } = 15;

    //for comparison, the game default is 128f (which is then modified based on the follower using the refinery)
    public virtual float CustomRefineryDuration { get; set; } = 0f;

    public virtual string InventoryStringIcon()
    {
        return $"<sprite name=\"icon_ITEM_{ModPrefix}.{InternalName}\">";
    }

    public virtual string Name()
    {
        return LocalizedName();
    }

    public virtual string Lore()
    {
        return LocalizedLore();
    }

    public virtual string Description()
    {
        return LocalizedDescription();
    }

    public virtual string LocalizedName()
    {
        return LocalizationManager.GetTranslation($"Inventory/{ModPrefix}.{InternalName}");
    }

    public virtual string LocalizedLore()
    {
        return LocalizationManager.GetTranslation($"Inventory/{ModPrefix}.{InternalName}/Lore");
    }

    public virtual string LocalizedDescription()
    {
        return LocalizationManager.GetTranslation($"Inventory/{ModPrefix}.{InternalName}/Description");
    }

    public virtual string GiftTitle(Follower follower)
    {
        return $"{Name()} ({Inventory.GetItemQuantity(ItemType)})";
    }

    public virtual void OnGiftTo(Follower follower, Action onFinish)
    {
        onFinish();
    }

    public virtual string CapacityString(int minimum)
    {
        var itemQuantity = Inventory.GetItemQuantity(ItemType);
        var text = $"{InventoryStringIcon()} {itemQuantity}/{minimum}";
        return itemQuantity < minimum ? text.Colour(StaticColors.RedColor) : text;
    }

    #region ItemSpawning Region

    /// <summary>
    ///     This is the Sprite given to custom items when they are spawned into the world.
    /// </summary>
    public virtual Sprite Sprite { get; } =
        TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));


    /// <summary>
    ///     Defines rarity for offering shrine spawn chance.
    /// </summary>
    public virtual CustomItemManager.ItemRarity Rarity => CustomItemManager.ItemRarity.COMMON;

    /// <summary>
    ///     Defines the scale (size) of the object when spawned.
    /// </summary>
    public virtual Vector3 LocalScale { get; } = new(0.5f, 0.5f, 0.5f);


    /// <summary>
    ///     Defines whether to add the item to offering shrines.
    /// </summary>
    public virtual bool AddItemToOfferingShrine => false;


    /// <summary>
    ///     Defines the item properties (bounce, speed, magnet etc..) to imitate when spawned.
    /// </summary>
    public virtual InventoryItem.ITEM_TYPE ItemPickUpToImitate => InventoryItem.ITEM_TYPE.LOG;

    /// <summary>
    ///     Defines if the item should be added to dungeon chests.
    /// </summary>
    public virtual bool AddItemToDungeonChests => false;

    /// <summary>
    ///     The chance for the item to be spawned from dungeon chests. Keep in mind that the chance is affected by the current
    ///     Dungeon luck modifier.
    /// </summary>
    public virtual int DungeonChestSpawnChance => 100;

    /// <summary>
    ///     The minimum amount of items to spawn from dungeon chests.
    /// </summary>
    public virtual int DungeonChestMinAmount => 1;

    /// <summary>
    ///     The maximum amount of items to spawn from dungeon chests.
    /// </summary>
    public virtual int DungeonChestMaxAmount => 1;

    #endregion
}