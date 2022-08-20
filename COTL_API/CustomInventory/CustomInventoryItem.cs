using System.Reflection;
using COTL_API.Guid;

namespace COTL_API.CustomInventory;

public class CustomInventoryItem
{
    public InventoryItem.ITEM_CATEGORIES ItemCategory;
    public InventoryItem.ITEM_TYPE SeedType;

    public string Name() { return LocalizedName(); }
    public string Lore() { return LocalizedDescription(); }
    public string Description() { return ""; }

    public string LocalizedName() { return ""; }
    public string LocalizedLore() { return ""; }
    public string LocalizedDescription() { return ""; }

    public int FuelWeight;
    public int FoodSatitation;

    public bool IsFish;
    public bool IsFood;
    public bool IsBigFish;

    public bool CanBeGivenToFollower;

    public string CapacityString(int minimum) { return ""; }

}