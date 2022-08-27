namespace COTL_API.INDEV;

public class DEBUG_ITEM_CLASS_2 : CustomInventory.CustomInventoryItem
{
    public override string InternalName => "DEBUG_ITEM_2";
    public override string LocalizedName() { return "DEBUG_ITEM_2"; }
    public override string LocalizedDescription() { return "COTL_API'S 2ND DEBUG ITEM"; }

    public override bool IsFood => true;
}