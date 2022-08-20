using I2.Loc;
namespace COTL_API.CustomInventory;

public class CustomInventoryItem
{

    public virtual InventoryItem.ITEM_CATEGORIES ItemCategory { get; set; }
    public InventoryItem.ITEM_TYPE ItemType;
    public virtual InventoryItem.ITEM_TYPE SeedType { get; set; }

    public virtual string Name() { return LocalizedName(); }
    public virtual string Lore() { return LocalizedLore(); }
    public virtual string Description() { return LocalizedDescription(); }
    public virtual string LocalizedName() { return LocalizationManager.GetTranslation(string.Format("Inventory/{0}", ItemType), true, 0, true, false, null, null, true); }
    public virtual string LocalizedLore() { return LocalizationManager.GetTranslation(string.Format("Inventory/{0}/Lore", ItemType), true, 0, true, false, null, null, true); }
    public virtual string LocalizedDescription() { return LocalizationManager.GetTranslation(string.Format("Inventory/{0}/Description", ItemType), true, 0, true, false, null, null, true); }

    public virtual int FuelWeight { get; set; }
    public virtual int FoodSatitation { get; set; }

    public virtual bool IsFish { get; set; }
    public virtual bool IsFood { get; set; }
    public virtual bool IsBigFish { get; set; }

    public virtual bool CanBeGivenToFollower { get; set; }

    public virtual string CapacityString(int minimum)
    {
        int itemQuantity = Inventory.GetItemQuantity(this.ItemType);
        string text = string.Format("{0} {1}/{2}", FontImageNames.GetIconByType(this.ItemType), itemQuantity, minimum);
        return itemQuantity < minimum ? text.Colour(StaticColors.RedColor) : text;
    }

    public CustomInventoryItem()
    {
        this.ItemCategory = InventoryItem.ITEM_CATEGORIES.NONE;
        this.SeedType = InventoryItem.ITEM_TYPE.NONE;

        this.FuelWeight = 1;
        this.FoodSatitation = 75;

        this.IsFish = false;
        this.IsFood = false;
        this.IsBigFish = false;

        this.CanBeGivenToFollower = false;
    }

}