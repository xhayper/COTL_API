namespace COTL_API.CustomInventory;

public static partial class CustomItemManager
{
    public static Dictionary<InventoryItem.ITEM_TYPE, CustomMeal> CustomMealList { get; } = [];

    public static InventoryItem.ITEM_TYPE Add(CustomMeal meal)
    {
        if (!CookingData.GetAllMeals().Contains(meal.ItemPickUpToImitate))
            throw new ArgumentException("Custom Meal Imitation Item is not a meal!", meal.InternalName);

        var itemType = Add(meal as CustomFood);

        CustomMealList.Add(itemType, meal);

        return itemType;
    }
}