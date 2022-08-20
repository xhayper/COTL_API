using I2.Loc;
namespace COTL_API.CustomInventory;

public class CustomInventoryItem
{

    public InventoryItem.ITEM_CATEGORIES ItemCategory;
    public InventoryItem.ITEM_TYPE ItemType;
    public InventoryItem.ITEM_TYPE SeedType;

    public virtual string Name() { return LocalizedName(); }
    public virtual string Lore() { return LocalizedLore(); }
    public virtual string Description() { return LocalizedDescription(); }
    public virtual string LocalizedName() { return LocalizationManager.GetTranslation(string.Format("Inventory/{0}", ItemType), true, 0, true, false, null, null, true); }
    public virtual string LocalizedLore() { return LocalizationManager.GetTranslation(string.Format("Inventory/{0}/Lore", ItemType), true, 0, true, false, null, null, true); }
    public virtual string LocalizedDescription() { return LocalizationManager.GetTranslation(string.Format("Inventory/{0}/Description", ItemType), true, 0, true, false, null, null, true); }

    public int FuelWeight = 1;
    public int FoodSatitation = 75;

    public bool IsFish = false;
    public bool IsFood = false;
    public bool IsBigFish = false;

    public bool CanBeGivenToFollower = false;

    public virtual string CapacityString(int minimum)
    {
        int itemQuantity = Inventory.GetItemQuantity(this.ItemType);
        string text = string.Format("{0} {1}/{2}", FontImageNames.GetIconByType(this.ItemType), itemQuantity, minimum);
        return itemQuantity < minimum ? text.Colour(StaticColors.RedColor) : text;
    }

}