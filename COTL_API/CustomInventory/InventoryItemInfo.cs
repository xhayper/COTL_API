namespace COTL_API.CustomInventory;

public class CustomInventoryItem
{
    public static InventoryItem.ITEM_CATEGORIES ItemCategory;
    public static InventoryItem.ITEM_TYPE SeedType;

    public static string Name = LocalizedName;
    public static string Lore = LocalizedDescription;
    public static string Description;

    public static string LocalizedName;
    public static string LocalizedLore;
    public static string LocalizedDescription;

    public static int FuelWeight;
    public static int FoodSatitation;

    public static bool IsFish;
    public static bool IsFood;
    public static bool IsBigFish;

    public static bool CanBeGivenToFollower;

    public static string CapacityString(int amount, int minimum) { return ""; }
}