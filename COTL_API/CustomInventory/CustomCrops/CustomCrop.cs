using UnityEngine;

namespace COTL_API.CustomInventory;

public abstract class CustomCrop : CustomInventoryItem
{
    internal StructureBrain.TYPES StructureType { get; set; }
    internal int CropStatesCount => CropStates.Count;
    //public sealed override bool IsPlantable => true;
    
    /// <summary>
    ///     The States of this crop, the last state should be the fully grown state. Requires at least two states.
    /// </summary>
    public virtual List<Sprite> CropStates { get; } = [];

    /// <summary>
    ///     The time it takes for this crop to grow, in game ticks.
    /// </summary>
    public virtual float CropGrowthTime => 9f;

    /// <summary>
    ///     The Crop and Seed that drops when this is harvested. Must be size 2
    /// </summary>
    public abstract List<InventoryItem.ITEM_TYPE> HarvestResult { get; }

    /// <summary>
    ///     How long (in seconds) it takes to pick this crop. 
    /// </summary>
    public virtual float PickingTime => 2.5f;

    /// <summary>
    ///     The range in how many resources will drop when collecting.
    /// </summary>
    public virtual Vector2Int CropCountToDropRange => new(3, 4);

    /// <summary>
    ///     Shows when hovering the crop to harvest it.
    /// </summary>
    public virtual string HarvestText => "Pick <color=#FD1D03>Berries</color>";
}