using UnityEngine;

namespace COTL_API.CustomInventory;

public abstract class CustomSeed : CustomInventoryItem
{
    internal StructureBrain.TYPES StructureType { get; set; }
    internal int CropStatesCount => CropStates.Count;
    public sealed override bool IsPlantable { get; } = true;
    public sealed override bool IsSeed { get; } = true;

    /// <summary>
    ///     The States of this crop, the last state should be the fully grown state. 
    /// </summary>
    public virtual List<Sprite> CropStates { get; } = [];

    public virtual float CropGrowthTime => 9f;

    /// <summary>
    ///     How many resources this will drop upon collecting
    /// </summary>
    public virtual Vector2Int HarvestsPerSeedRange => new(2, 5);

    /// <summary>
    ///     The Items, (and probabilities) dropped when harvesting this crop
    /// </summary>
    public virtual DropMultipleLootOnDeath.ItemAndProbability[] HarvestResult => [];
    
    /// <summary>s
    ///     TODO: Find out what this does, it's probably how long it takes to break but that's an odd name for it
    /// </summary>
    public virtual int ProgressTarget => 10;

    /// <summary>
    ///     TODO: Find out what this does, it's probably how many of the item drops, but then what's HarvestsPerSeedRange?
    /// </summary>
    public virtual Vector2Int CropCountToDropRange => new(3, 4);
}