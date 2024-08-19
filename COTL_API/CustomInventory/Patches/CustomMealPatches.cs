using HarmonyLib;
using Socket.Newtonsoft.Json;
using src.UI.InfoCards;
using System;

namespace COTL_API.CustomInventory
{
    [HarmonyPatch]
    public static partial class CustomItemManager
    {
        [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetRecipe)), HarmonyPostfix]
        public static void AddCustomRecepies(ref List<List<InventoryItem>> __result, InventoryItem.ITEM_TYPE mealType)
        {
            if (!CustomItemList.ContainsKey(mealType))
                return;

            __result = (CustomItemList[mealType] as CustomMeal)!.Recipe;
        }

        [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetMealEffects)), HarmonyPostfix]
        public static void AddCustomMealEffects(ref CookingData.MealEffect[] __result, InventoryItem.ITEM_TYPE mealType)
        {
            if (!CustomItemList.ContainsKey(mealType))
                return;

            __result = (CustomItemList[mealType] as CustomMeal)!.MealEffects;
        }


        [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetTummyRating)), HarmonyPostfix]
        public static void GetCustomMealTummyRating(ref float __result, InventoryItem.ITEM_TYPE meal)
        {
            if (!CustomItemList.ContainsKey(meal))
                return;

            __result = (CustomItemList[meal] as CustomMeal)!.TummyRating;
        }

        [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetSatationLevel)), HarmonyPostfix]
        public static void GetCustomSatiationLevel(ref int __result, InventoryItem.ITEM_TYPE meal)
        {
            if (!CustomItemList.ContainsKey(meal))
                return;

            __result = (CustomItemList[meal] as CustomMeal)!.SatiationLevel;
        }

        [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetAllMeals)), HarmonyPostfix]
        public static void AddCustomMeal(ref InventoryItem.ITEM_TYPE[] __result)
        {
            var customMeals = CustomItemList.Values.Where(x => x.GetType().IsSubclassOf(typeof(CustomMeal))).ToArray();
            var customItems = CustomItemList.Keys.Where(x => CustomItemList[x].GetType().IsSubclassOf(typeof(CustomMeal))).ToArray(); // ????
            var newResult = new InventoryItem.ITEM_TYPE[__result.Length + customMeals.Length];

            for (var i = 0; i < __result.Length; i++)
            {
                newResult[i] = __result[i];
            }

            for (var i = 0; i < customMeals.Length; i++)
            {
                newResult[__result.Length + i] = customItems[i];
            }

            __result = newResult;
        }

        [HarmonyPatch(typeof(RecipeInfoCard), nameof(RecipeInfoCard.Configure)), HarmonyPrefix]
        public static void AddCustomStarRating(RecipeInfoCard __instance, InventoryItem.ITEM_TYPE config)
        {
            var satationLevel = CookingData.GetSatationLevel(config);
            for (var index = 0; index < __instance._starFills.Length; ++index)
                __instance._starFills[index].SetActive(satationLevel >= index + 1);
        }
    }
}
