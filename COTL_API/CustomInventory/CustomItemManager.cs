using System.Collections.Generic;
using System.Reflection;
using COTL_API.Guid;

namespace COTL_API.CustomInventory;

public static partial class CustomItemManager
{
    public static Dictionary<InventoryItem.ITEM_TYPE, CustomInventoryItem> CustomItems { get; } = new();

    public static InventoryItem.ITEM_TYPE Add(CustomInventoryItem item)
    {
        string guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        InventoryItem.ITEM_TYPE itemType = GuidManager.GetEnumValue<InventoryItem.ITEM_TYPE>(guid, item.InternalName);
        item.ItemType = itemType;
        item.ModPrefix = guid;

        CustomItems.Add(itemType, item);

        return itemType;
    }

    public static void AddGift(InventoryItem.ITEM_TYPE item)
    {
        if (!DataManager.AllGifts.Contains(item)) DataManager.AllGifts.Add(item);
    }

    public static void RemoveGift(InventoryItem.ITEM_TYPE item)
    {
        if (DataManager.AllGifts.Contains(item)) DataManager.AllGifts.Remove(item);
    }

    public static void AddNecklace(InventoryItem.ITEM_TYPE item)
    {
        if (!DataManager.AllNecklaces.Contains(item)) DataManager.AllNecklaces.Add(item);
    }

    public static void RemoveNecklace(InventoryItem.ITEM_TYPE item)
    {
        if (DataManager.AllNecklaces.Contains(item)) DataManager.AllNecklaces.Remove(item);
    }
}