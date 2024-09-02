namespace COTL_API.CustomInventory;

public abstract class CustomDrink : CustomFood
{
    internal FollowerBrain.PleasureActions PleasureAction { get; set; }
    public override InventoryItem.ITEM_TYPE ItemPickUpToImitate { get; } = InventoryItem.ITEM_TYPE.DRINK_GIN;

    /// <summary>
    ///     The amount of Sin gained by the follower. Range: 0-65
    /// </summary>
    public virtual int Pleasure { get; } = 0;

    public override int FoodSatitation { get; } = 0;
}