using COTL_API.CustomInventory;

namespace COTL_API.INDEV;

public class DEBUG_ITEM_CLASS_2 : CustomInventoryItem
{
    public override string InternalName { get => "DEBUG_ITEM_2"; }
    public override string LocalizedName() { return "DEBUG_ITEM_2"; }
    public override string LocalizedDescription() { return "COTL_API'S 2ND DEBUG ITEM"; }
}