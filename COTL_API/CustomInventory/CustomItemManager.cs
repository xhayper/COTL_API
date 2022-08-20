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
        var itemType = GuidManager.GetEnumValue<InventoryItem.ITEM_TYPE>(TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly()), item.Name());
        item.ItemType = itemType;

        customItems.Add(itemType, item);

        return itemType;
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

    [HarmonyPatch(typeof(Lamb.UI.InventoryMenu), "OnShowStarted")]
    [HarmonyPrefix]
    public static void _____(Lamb.UI.InventoryMenu __instance)
    {
        __instance._currencyFilter.Add(Plugin.ITEM);
        Inventory.AddItem(Plugin.ITEM, 1, true);
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

    [HarmonyPatch(typeof(InventoryItem), "GetItemCategory")]
    [HarmonyPrefix]
    public static bool InventoryItem_ItemCategory(InventoryItem.ITEM_TYPE type, ref InventoryItem.ITEM_CATEGORIES __result)
    {
        if (!customItems.ContainsKey(type)) return true;

        __result = customItems[type].ItemCategory;

        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), "GetSeedType")]
    [HarmonyPrefix]
    public static bool InventoryItem_GetSeedType(InventoryItem.ITEM_TYPE type, ref InventoryItem.ITEM_TYPE __result)
    {
        if (!customItems.ContainsKey(type)) return true;

        __result = customItems[type].SeedType;

        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), "FuelWeight", typeof(InventoryItem.ITEM_TYPE))]
    [HarmonyPrefix]
    public static bool InventoryItem_FuelWeight(InventoryItem.ITEM_TYPE type, ref int __result)
    {
        if (!customItems.ContainsKey(type)) return true;

        __result = customItems[type].FuelWeight;

        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), "FoodSatitation")]
    [HarmonyPrefix]
    public static bool InventoryItem_FoodSatitation(InventoryItem.ITEM_TYPE Type, ref int __result)
    {
        if (!customItems.ContainsKey(Type)) return true;

        __result = customItems[Type].FoodSatitation;

        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), "IsFish")]
    [HarmonyPrefix]
    public static bool InventoryItem_IsFish(InventoryItem.ITEM_TYPE Type, ref bool __result)
    {
        if (!customItems.ContainsKey(Type)) return true;

        __result = customItems[Type].IsFish;

        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), "IsFood")]
    [HarmonyPrefix]
    public static bool InventoryItem_IsFood(InventoryItem.ITEM_TYPE Type, ref bool __result)
    {
        if (!customItems.ContainsKey(Type)) return true;

        __result = customItems[Type].IsFood;

        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), "IsBigFish")]
    [HarmonyPrefix]
    public static bool InventoryItem_IsBigFish(InventoryItem.ITEM_TYPE Type, ref bool __result)
    {
        if (!customItems.ContainsKey(Type)) return true;

        __result = customItems[Type].IsBigFish;

        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), "CanBeGivenToFollower")]
    [HarmonyPrefix]
    public static bool InventoryItem_CanBeGivenToFollower(InventoryItem.ITEM_TYPE Type, ref bool __result)
    {
        if (!customItems.ContainsKey(Type)) return true;

        __result = customItems[Type].IsBigFish;

        return false;
    }
}