using COTL_API.Helpers;
using UnityEngine;
using I2.Loc;

namespace COTL_API.CustomInventory;

public class CustomInventoryItem
{
    public virtual string InternalName { get; }
    public InventoryItem.ITEM_TYPE ItemType;
    public string ModPrefix;

    public virtual Sprite InventoryIcon { get; } =
        TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));

    public virtual string InventoryStringIcon()
    {
        return $"<sprite name=\"icon_ITEM_{ModPrefix}.{InternalName}\">";
    }

    public virtual InventoryItem.ITEM_CATEGORIES ItemCategory { get; } = InventoryItem.ITEM_CATEGORIES.NONE;
    public virtual InventoryItem.ITEM_TYPE SeedType { get; } = InventoryItem.ITEM_TYPE.NONE;

    public virtual string Name() { return LocalizedName(); }
    public virtual string Lore() { return LocalizedLore(); }
    public virtual string Description() { return LocalizedDescription(); }

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
    
    public virtual int FuelWeight { get; } = 1;
    public virtual int FoodSatitation { get; } = 75;

    public virtual bool IsFish { get; } = false;
    public virtual bool IsFood { get; } = false;
    public virtual bool IsBigFish { get; } = false;
    public virtual bool IsCurrency { get; } = false;
    public virtual bool IsSeed { get; } = false;
    public virtual bool IsPlantable { get; } = false;
    public virtual bool IsBurnableFuel { get; } = false;

    public virtual bool CanBeGivenToFollower => false;
    
    public virtual string GiftTitle(Follower follower)
    {
        return $"{Name()} ({Inventory.GetItemQuantity(ItemType)})";
    }

    public virtual FollowerCommands GiftCommand => FollowerCommands.None;

    public virtual System.Action<Follower, InventoryItem.ITEM_TYPE, System.Action> OnGiveToFollower()
    {
        return InventoryItem.OnGetGift;
    }

    public virtual string CapacityString(int minimum)
    {
        int itemQuantity = Inventory.GetItemQuantity(ItemType);
        string text = $"{InventoryStringIcon()} {itemQuantity}/{minimum}";
        return itemQuantity < minimum ? text.Colour(StaticColors.RedColor) : text;
    }
}