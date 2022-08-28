namespace COTL_API.Debug;

public class DebugItemClass3 : CustomInventory.CustomInventoryItem
{
    public override string InternalName => "DEBUG_ITEM_3";
    public override string LocalizedName() { return "DEBUG_ITEM_3"; }
    public override string LocalizedDescription() { return "COTL_API'S 3RD DEBUG ITEM"; }

    public override bool IsPlantable => true;
}