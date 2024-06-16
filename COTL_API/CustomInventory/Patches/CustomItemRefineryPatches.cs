using HarmonyLib;
using Lamb.UI;
using Lamb.UI.RefineryMenu;
using src.UI.InfoCards;
using Object = UnityEngine.Object;

namespace COTL_API.CustomInventory;

[HarmonyPatch]
public static partial class CustomItemManager
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Refinery), nameof(Structures_Refinery.GetCost))]
    public static void Structures_Refinery_GetCost(Structures_Refinery __instance, InventoryItem.ITEM_TYPE Item,
        ref List<StructuresData.ItemCost> __result)
    {
        if (!CustomItemList.ContainsKey(Item)) return;

        __result = new List<StructuresData.ItemCost>
        {
            new(CustomItemList[Item].RefineryInput, CustomItemList[Item].RefineryInputQty)
        };
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Refinery), nameof(Structures_Refinery.RefineryDuration))]
    public static void Structures_Refinery_RefineryDuration(Structures_Refinery __instance,
        InventoryItem.ITEM_TYPE ItemType, ref float __result)
    {
        if (!CustomItemList.ContainsKey(ItemType)) return;

        if (CustomItemList[ItemType].CustomRefineryDuration > 0)
            __result = CustomItemList[ItemType].CustomRefineryDuration;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIMenuBase), nameof(UIMenuBase.Show), typeof(bool))]
    public static void UIRefineryMenuController_Show(ref UIMenuBase __instance)
    {
        if (__instance is not UIRefineryMenuController menu) return;

        foreach (var item in CustomItemList.Where(item =>
                     item.Value.CanBeRefined))
        {
            var refineryItem = Object.Instantiate(menu.refineryIconPrefab, menu.Container);
            refineryItem.OnItemSelected += menu.OnItemSelected;
            var button = refineryItem.Button;
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
        if (!CustomItemList.TryGetValue(config, out var value)) return;

        __instance._descriptionText.text = value.LocalizedDescription();
        __instance._headerText.text = CustomItemList[config].LocalizedName();
    }
}