//NOSONAR

namespace COTL_API.Debug;

public class DebugItemClass2 : CustomInventory.CustomInventoryItem
{
    public override string InternalName => "DEBUG_ITEM_2";

    public override string LocalizedName()
    {
        return "DEBUG_ITEM_2";
    }

    public override string LocalizedDescription()
    {
        return "COTL_API'S 2ND DEBUG ITEM";
    }

    public override bool IsFood => true;
    public override bool IsSeed => true;
}