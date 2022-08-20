using System.Collections.Generic;
using System.Reflection;
using COTL_API.Guid;
using HarmonyLib;

namespace COTL_API.CustomInventory;

[HarmonyPatch]
public class CustomItemManager
{
    public static Dictionary<InventoryItem.ITEM_TYPE, CustomInventoryItem> customItems = new();

    public static InventoryItem.ITEM_TYPE Add(CustomInventoryItem item)
    {
        var itemEnum = GuidManager.GetEnumValue<InventoryItem.ITEM_TYPE>(TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly()), item.Name());

        customItems.Add(itemEnum, item);

        return itemEnum;
    }

}