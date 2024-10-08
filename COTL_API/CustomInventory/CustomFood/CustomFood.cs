using UnityEngine;

namespace COTL_API.CustomInventory;

public abstract class CustomFood : CustomInventoryItem
{
    internal StructureBrain.TYPES StructureType { get; set; }

    internal FollowerCommands FollowerCommand { get; set; }

    /// <summary>
    ///     This Food's Recipe
    /// </summary>
    public abstract List<List<InventoryItem>> Recipe { get; }
    
    /// <summary>
    ///     "Star" level of this food
    ///     Range: 0-3
    /// </summary>
    public abstract int SatiationLevel { get; }
    
    /// <summary>
    ///     A list of the effects that will occur when consuming this food
    /// </summary>
    public virtual CookingData.MealEffect[] MealEffects { get; } = [];
    
    /// <summary>
    ///     Item display offset when in inventory storage such as a follower kitchen or pub
    /// </summary>
    public virtual Vector3 ItemDisplayOffset { get; } = Vector3.zero;
    public override bool IsFood => true;

}