namespace COTL_API.Debug;

public class DebugItemClass : CustomInventory.CustomInventoryItem
{
    public override string InternalName => "DEBUG_ITEM";
    public override string LocalizedName() { return "DEBUG_ITEM"; }
    public override string LocalizedDescription() { return "COTL_API'S DEBUG ITEM"; }

    public override bool IsCurrency => true;
    public override bool IsBurnableFuel => true;

    public override InventoryItem.ITEM_CATEGORIES ItemCategory => InventoryItem.ITEM_CATEGORIES.LOGS;
    public override int FuelWeight => 100;
}