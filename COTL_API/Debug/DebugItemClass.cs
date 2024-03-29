using COTL_API.CustomInventory;

namespace COTL_API.Debug;

public class DebugItemClass : CustomInventoryItem
{
    public override string InternalName => "DEBUG_ITEM";

    public override CustomInventoryItemType InventoryItemType => CustomInventoryItemType.CURRENCY;

    public override bool IsCurrency => true;

    public override bool IsBurnableFuel => true;
    public override int FuelWeight => 100;

    public override bool CanBeGivenToFollower => true;
    public override FollowerCommands GiftCommand => Plugin.Instance!.DebugGiftFollowerCommand;
}