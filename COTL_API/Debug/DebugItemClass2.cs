using COTL_API.CustomInventory;

namespace COTL_API.Debug;

public class DebugItemClass2 : CustomInventoryItem
{
    public override string InternalName => "DEBUG_ITEM_2";

    public override CustomInventoryItemType InventoryItemType => CustomInventoryItemType.FOOD;

    public override bool IsFood => true;
    public override bool IsSeed => true;
}