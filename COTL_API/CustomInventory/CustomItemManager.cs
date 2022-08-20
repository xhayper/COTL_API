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

    // Patch `ItemInfoCard` not using `InventoryItem`'s method
    [HarmonyPatch(typeof(Lamb.UI.ItemInfoCard), "Configure")]
    [HarmonyPrefix]
    public static bool ItemInfoCard_Configure(Lamb.UI.ItemInfoCard __instance, InventoryItem.ITEM_TYPE config)
    {
        if (!customItems.ContainsKey(config)) return true;

        __instance._inventoryIcon.Configure(config, false);
        __instance._itemHeader.text = InventoryItem.Name(config);
        __instance._itemLore.text = InventoryItem.Lore(config);
        __instance._itemDescription.text = InventoryItem.Description(config);
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), "Name")]
    [HarmonyPrefix]
    public static bool InventoryItem_Name(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!customItems.ContainsKey(Type)) return true;

        __result = customItems[Type].Name();

        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), "LocalizedName")]
    [HarmonyPrefix]
    public static bool InventoryItem_LocalizedName(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!customItems.ContainsKey(Type)) return true;

        __result = customItems[Type].LocalizedName();

        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), "Description")]
    [HarmonyPrefix]
    public static bool InventoryItem_Description(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!customItems.ContainsKey(Type)) return true;

        __result = customItems[Type].Description();

        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), "LocalizedDescription")]
    [HarmonyPrefix]
    public static bool InventoryItem_LocalizedDescription(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!customItems.ContainsKey(Type)) return true;

        __result = customItems[Type].LocalizedDescription();

        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), "Lore")]
    [HarmonyPrefix]
    public static bool InventoryItem_Lore(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!customItems.ContainsKey(Type)) return true;

        __result = customItems[Type].Lore();

        return false;
    }
}