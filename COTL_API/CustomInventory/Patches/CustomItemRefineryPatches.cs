using HarmonyLib;
using Lamb.UI;
using Lamb.UI.RefineryMenu;
using src.UI.InfoCards;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//if it asks, choose "Does not introduce namespace"
namespace COTL_API.CustomInventory;

[HarmonyPatch]
public static partial class CustomItemManager
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Refinery), nameof(Structures_Refinery.GetCost))]
    public static void Structures_Refinery_GetCost(Structures_Refinery __instance, InventoryItem.ITEM_TYPE Item, ref List<StructuresData.ItemCost> __result)
    {
        if (!CustomItems.ContainsKey(Item)) return;

        __result = new List<StructuresData.ItemCost> {
            new(CustomItems[Item].RefineryInput, CustomItems[Item].RefineryInputQty)
        };
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Refinery), nameof(Structures_Refinery.RefineryDuration))]
    public static void Structures_Refinery_RefineryDuration(Structures_Refinery __instance, InventoryItem.ITEM_TYPE ItemType, ref float __result)
    {
        if (!CustomItems.ContainsKey(ItemType)) return;

        if (CustomItems[ItemType].CustomRefineryDuration > 0)
        {
            __result = CustomItems[ItemType].CustomRefineryDuration;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIMenuBase), nameof(UIMenuBase.Show), typeof(bool))]
    public static void UIRefineryMenuController_Show(ref UIMenuBase __instance)
    {
        if (__instance is not UIRefineryMenuController menu) return;

        foreach (KeyValuePair<InventoryItem.ITEM_TYPE, CustomInventoryItem> item in CustomItems.Where(item => item.Value.CanBeRefined))
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
    public static void RefineryInfoCard_Configure(ref RefineryInfoCard __instance, ref InventoryItem.ITEM_TYPE config)
    {
        if (!CustomItems.ContainsKey(config)) return;

        __instance._descriptionText.text = CustomItems[config].LocalizedDescription();
        __instance._headerText.text = CustomItems[config].LocalizedName();
    }
}