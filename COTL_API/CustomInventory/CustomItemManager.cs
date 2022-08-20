using System.Collections.Generic;
using HarmonyLib;

namespace COTL_API.CustomInventory;

[HarmonyPatch]
public class CustomItemManager
{
    // Dictionary<modPrefix+ItemName, CustomInventoryItem>
    private static Dictionary<InventoryItem.ITEM_TYPE, CustomInventoryItem> customItems = new();

    public static InventoryItem.ITEM_TYPE Add(CustomInventoryItem item)
    {
        // TODO: Implement this
        // Current Plan: Use the same system that InscryptionAPI use
        // Assembly.GetCallingAssembly + Item name
        // Then resolve assign that to an ID

        var generatedItemType = (InventoryItem.ITEM_TYPE)5000;

        customItems.Add(generatedItemType, item);

        return generatedItemType;
    }

}