namespace COTL_API.CustomInventory;

public class CustomInventoryItem
{
    public static InventoryItem.ITEM_CATEGORIES ItemCategory;
    public static InventoryItem.ITEM_TYPE SeedType;

    public static string Name() { return LocalizedName(); }
    public static string Lore() { return LocalizedDescription(); }
    public static string Description() { return ""; }

    public static string LocalizedName() { return ""; }
    public static string LocalizedLore() { return ""; }
    public static string LocalizedDescription() { return ""; }

    public static int FuelWeight;
    public static int FoodSatitation;

    public static bool IsFish;
    public static bool IsFood;
    public static bool IsBigFish;

    public static bool CanBeGivenToFollower;

    public static string CapacityString(int minimum) { return ""; }
}