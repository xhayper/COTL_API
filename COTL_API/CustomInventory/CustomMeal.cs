using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CookingData;

namespace COTL_API.CustomInventory;

public abstract class CustomMeal : CustomInventoryItem
{
    internal StructureBrain.TYPES StructureType { get; set; }

    internal FollowerCommands FollowerCommand { get; set; }

    /// <summary>
    /// This Meal's Recepie <br/>
    /// This is a List of Lists, but the first list should only be of length 1, containing a list of all the items for this recepie
    /// </summary>
    public abstract List<List<InventoryItem>> Recipe { get; }

    /// <summary>
    /// A list of the effects that will occur when eating this meal
    /// </summary>
    public virtual MealEffect[] MealEffects { get; } = [];

    public override bool IsFood => true;

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

    public virtual MealQuality Quality { get; } = MealQuality.Normal;
}

public enum MealQuality
{ 
    Bad,
    Normal,
    Good,
}
