using static CookingData;

namespace COTL_API.CustomInventory;

public abstract class CustomMeal : CustomInventoryItem
{
    internal StructureBrain.TYPES StructureType { get; set; }

    internal FollowerCommands FollowerCommand { get; set; }

    /// <summary>
    /// This Meal's Recipe
    /// </summary>
    public abstract List<List<InventoryItem>> Recipe { get; }

    /// <summary>
    /// A list of the effects that will occur when eating this meal
    /// </summary>
    public virtual MealEffect[] MealEffects { get; } = [];

    public override bool IsFood => true;
    public override InventoryItem.ITEM_TYPE ItemPickUpToImitate { get; } = InventoryItem.ITEM_TYPE.MEAL_BERRIES;
    /// <summary>
    /// "Star" level of this food
    /// Range: 0-3
    /// </summary>
    public abstract int SatiationLevel { get; }

    /// <summary>
    /// This is the amount that "Hunger Circle" is filled in when cooking the meal.
    /// Range: 0-1
    /// </summary>
    public abstract float TummyRating { get; }

    public virtual MealQuality Quality { get; } = MealQuality.NORMAL;
}

public enum MealQuality
{ 
    BAD,
    NORMAL,
    GOOD,
}
