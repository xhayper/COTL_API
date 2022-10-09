using HarmonyLib;
using Lamb.UI;
using Lamb.UI.RefineryMenu;
using src.UI.InfoCards;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace COTL_API.CustomInventory;

[HarmonyPatch]
public static class CustomItemRefineryPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Refinery), nameof(Structures_Refinery.GetCost))]
    public static void Structures_Refinery_GetCost(Structures_Refinery __instance, InventoryItem.ITEM_TYPE Item, ref List<StructuresData.ItemCost> __result)
    {
        if (!CustomItemManager.CustomItems.ContainsKey(Item)) return;

        __result = new List<StructuresData.ItemCost> {
            new(CustomItemManager.CustomItems[Item].RefineryInput, CustomItemManager.CustomItems[Item].RefineryOutput)
        };
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Refinery), nameof(Structures_Refinery.RefineryDuration))]
    public static void Structures_Refinery_GetCost(Structures_Refinery __instance, InventoryItem.ITEM_TYPE ItemType, ref float __result)
    {
        if (!CustomItemManager.CustomItems.ContainsKey(ItemType)) return;

        if (CustomItemManager.CustomItems[ItemType].CustomRefineryDuration > 0)
        {
            __result = CustomItemManager.CustomItems[ItemType].CustomRefineryDuration;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIMenuBase), nameof(UIMenuBase.Show), typeof(bool))]
    public static void UIRefineryMenuController_Show(ref UIMenuBase __instance)
    {
        if (__instance is not UIRefineryMenuController menu) return;

        foreach (KeyValuePair<InventoryItem.ITEM_TYPE, CustomInventoryItem> item in CustomItemManager.CustomItems.Where(item => item.Value.CanBeRefined))
        {
            RefineryItem refineryItem = Object.Instantiate(menu.refineryIconPrefab, menu.Container);
            refineryItem.OnItemSelected += menu.OnItemSelected;
            MMButton button = refineryItem.Button;
            button.OnSelected += menu.OnQueueableItemSelected;
            refineryItem.Configure(item.Key);
            refineryItem.FadeIn(menu._refineryItems.Count * 0.03f);
            menu._refineryItems.Add(refineryItem);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(RefineryInfoCard), nameof(RefineryInfoCard.Configure), typeof(InventoryItem.ITEM_TYPE))]
    public static void RefineryItemInfoCard_Configure(ref RefineryInfoCard __instance, ref InventoryItem.ITEM_TYPE config)
    {
        if (!CustomItemManager.CustomItems.ContainsKey(config)) return;

        __instance._descriptionText.text = CustomItemManager.CustomItems[config].LocalizedDescription();
        __instance._headerText.text = CustomItemManager.CustomItems[config].LocalizedName();
    }
}