using HarmonyLib;
using Lamb.UI.FollowerInteractionWheel;

namespace COTL_API.CustomInventory;

[HarmonyPatch]
public static partial class CustomItemManager
{
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetAllDrinks))]
    [HarmonyPostfix]
    private static void CookingData_GetAllDrinks(ref InventoryItem.ITEM_TYPE[] __result)
    {
        var customDrinks = CustomDrinkList.Values.ToArray();
        var customItems = CustomDrinkList.Keys.ToArray();
        var newResult = new InventoryItem.ITEM_TYPE[__result.Length + customDrinks.Length];

        for (var i = 0; i < __result.Length; i++) newResult[i] = __result[i];

        for (var i = 0; i < customDrinks.Length; i++) newResult[__result.Length + i] = customItems[i];

        __result = newResult;
    }
    
    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.DrinkCommands))]
    [HarmonyPostfix]
    private static void FollowerCommandGroups_DrinkCommands(ref List<InventoryItem.ITEM_TYPE> availableDrinks,
        ref List<CommandItem> __result)
    {
        foreach (var item in CustomDrinkList.Keys)
        {
            if (availableDrinks.Contains(item)) continue;

            __result.Add(new FollowerCommandItems.FoodCommandItem
            {
                Command = CustomDrinkList[item].FollowerCommand
            });
        }
    }

    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetPleasure))]
    [HarmonyPostfix]
    private static void CookingData_GetPleasure(ref FollowerBrain.PleasureActions __result,
        ref InventoryItem.ITEM_TYPE item)
    {
        if (CustomDrinkList.TryGetValue(item, out var value)) __result = value.PleasureAction;
    }
}