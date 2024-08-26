namespace COTL_API.CustomInventory;

public abstract class CustomMeal : CustomFood
{
    /// <summary>
    ///     This is the amount that "Hunger Circle" is filled in when cooking the meal.
    ///     Range: 0-1
    /// </summary>
    public abstract float TummyRating { get; }

    public override InventoryItem.ITEM_TYPE ItemPickUpToImitate { get; } = InventoryItem.ITEM_TYPE.MEAL;
    public virtual MealQuality Quality { get; } = MealQuality.NORMAL;
}

public enum MealQuality
{
    BAD,
    NORMAL,
    GOOD
}