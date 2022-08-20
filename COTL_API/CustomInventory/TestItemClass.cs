namespace COTL_API.CustomInventory;

public class TestIItemClass : CustomInventoryItem
{

    public override string Name() { return "Test_Item"; }
    public override string Description() { return "This is a test!"; }

    public TestIItemClass()
    {
        this.ItemCategory = InventoryItem.ITEM_CATEGORIES.LOGS;
        this.ItemType = InventoryItem.ITEM_TYPE.LOG;
    }
}