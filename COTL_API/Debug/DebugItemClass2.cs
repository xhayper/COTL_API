using COTL_API.CustomInventory;

namespace COTL_API.Debug;

public class DebugItemClass2 : CustomCrop
{
    public override string InternalName => "DEBUG_ITEM_2";

    public override CustomInventoryItemType InventoryItemType => CustomInventoryItemType.FOOD;

    public override bool IsFood => true;

    public override List<InventoryItem.ITEM_TYPE> HarvestResult { get; } = 
        [
            Plugin.Instance.DebugItem,
            Plugin.Instance.DebugItem2,
        ];
}