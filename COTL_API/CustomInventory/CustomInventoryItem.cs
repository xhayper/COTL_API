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

/*
			this._itemHeader.text = LocalizationManager.GetTranslation(string.Format("Inventory/{0}", config), true, 0, true, false, null, null, true);
			this._itemLore.text = LocalizationManager.GetTranslation(string.Format("Inventory/{0}/Lore", config), true, 0, true, false, null, null, true);
			this._itemDescription.text = LocalizationManager.GetTranslation(string.Format("Inventory/{0}/Description", config), true, 0, true, false, null, null, true);
*/

    public virtual string LocalizedName() { return LocalizationManager.GetTranslation(string.Format("Inventory/{0}", ItemType), true, 0, true, false, null, null, true); }
    public virtual string LocalizedLore() { return LocalizationManager.GetTranslation(string.Format("Inventory/{0}/Lore", ItemType), true, 0, true, false, null, null, true); }
    public virtual string LocalizedDescription() { return LocalizationManager.GetTranslation(string.Format("Inventory/{0}/Description", ItemType), true, 0, true, false, null, null, true); }

    public int FuelWeight;
    public int FoodSatitation;

    public bool IsFish;
    public bool IsFood;
    public bool IsBigFish;

    public bool CanBeGivenToFollower;

    public string CapacityString(int minimum) { return ""; }

}