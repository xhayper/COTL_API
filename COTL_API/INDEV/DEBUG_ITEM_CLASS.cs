using COTL_API.CustomInventory;

namespace COTL_API.INDEV;

public class DEBUG_ITEM_CLASS : CustomInventory.CustomInventoryItem
{
    public override string InternalName { get => "DEBUG_ITEM"; }
    public override string LocalizedName() { return "DEBUG_ITEM"; }
    public override string LocalizedDescription() { return "COTL_API'S DEBUG ITEM"; }

    public override bool IsCurrency { get => true; }
}