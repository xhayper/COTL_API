using System.Collections.Generic;
using HarmonyLib;

namespace COTL_API.CustomInventory;

[HarmonyPatch]
public class CustomItemManager
{
    // Dictionary<modPrefix+ItemName, CustomInventoryItem>
    private static Dictionary<string, CustomInventoryItem> customItems = new();

    internal static void Add(CustomInventoryItem item)
    {
        // TODO: Implement this
        // Current Plan: Use the same system that InscryptionAPI use
        // Assembly.GetCallingAssembly + Item name
        // Then resolve assign that to an ID
    }

}