using COTL_API.Helpers;
using UnityEngine;
using I2.Loc;

namespace COTL_API.CustomInventory;

public class CustomInventoryItem
{

    public virtual string InternalName { get; set; }
    public InventoryItem.ITEM_TYPE ItemType;
    public string ModPrefix;

    public virtual Sprite InventoryIcon { get; set; }

    public virtual InventoryItem.ITEM_CATEGORIES ItemCategory { get; set; }
    public virtual InventoryItem.ITEM_TYPE SeedType { get; set; }

    public virtual string Name() { return LocalizedName(); }
    public virtual string Lore() { return LocalizedLore(); }
    public virtual string Description() { return LocalizedDescription(); }
    public virtual string LocalizedName() { return LocalizationManager.GetTranslation($"Inventory/{ModPrefix}.{InternalName}"); }
    public virtual string LocalizedLore() { return LocalizationManager.GetTranslation($"Inventory/{ModPrefix}.{InternalName}/Lore"); }
    public virtual string LocalizedDescription() { return LocalizationManager.GetTranslation($"Inventory/{ModPrefix}.{InternalName}/Description"); }

    public virtual int FuelWeight { get; set; }
    public virtual int FoodSatitation { get; set; }

    public virtual bool IsFish { get; set; }
    public virtual bool IsFood { get; set; }
    public virtual bool IsBigFish { get; set; }
    public virtual bool IsCurrency { get; set; }

    public virtual bool CanBeGivenToFollower { get; set; }

    public virtual string CapacityString(int minimum)
    {
        int itemQuantity = Inventory.GetItemQuantity(ItemType);
        string text = $"{FontImageNames.GetIconByType(ItemType)} {itemQuantity}/{minimum}";
        return itemQuantity < minimum ? text.Colour(StaticColors.RedColor) : text;
    }

    public CustomInventoryItem()
    {
        ItemCategory = InventoryItem.ITEM_CATEGORIES.NONE;
        SeedType = InventoryItem.ITEM_TYPE.NONE;

        FuelWeight = 1;
        FoodSatitation = 75;

        IsFish = false;
        IsFood = false;
        IsBigFish = false;
        IsCurrency = false;

        CanBeGivenToFollower = false;

        InventoryIcon = TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));
    }
}
