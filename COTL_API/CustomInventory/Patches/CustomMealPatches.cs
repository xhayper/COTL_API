using COTL_API.CustomStructures;
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
        [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetLocalizedName)), HarmonyPostfix]
        public static void GetCustomMealName(InventoryItem.ITEM_TYPE mealType, ref string __result)
        {
            if (CustomItemList.Keys.Contains(mealType))
            {
                __result = CustomItemList[mealType].LocalizedName();
            }
        }

        [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetLocalizedDescription)), HarmonyPostfix]
        public static void GetCustomMealDescription(InventoryItem.ITEM_TYPE mealType, ref string __result)
        {
            if (CustomItemList.Keys.Contains(mealType))
            {
                __result = CustomItemList[mealType].Description();
            }
        }

        [HarmonyPatch(typeof(RecipeInfoCard), nameof(RecipeInfoCard.Configure)), HarmonyPrefix]
        public static void AddCustomStarRating(RecipeInfoCard __instance, InventoryItem.ITEM_TYPE config)
        {
            var satationLevel = CookingData.GetSatationLevel(config);
            for (var index = 0; index < __instance._starFills.Length; ++index)
                __instance._starFills[index].SetActive(satationLevel >= index + 1);
        }


        // TODO : rewrite, i don't like it
        [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetMealFromStructureType)), HarmonyPostfix]
        public static void GetCustomMealFromStructure(StructureBrain.TYPES structureType, ref InventoryItem.ITEM_TYPE __result)
        {
            var item = CustomItemList.Keys
                .FirstOrDefault(x => CustomItemList[x].GetType().IsSubclassOf(typeof(CustomMeal)) && 
                (CustomItemList[x] as CustomMeal)!._mealTypel == structureType);

            if (item != InventoryItem.ITEM_TYPE.NONE)
                __result = item;
        }

        [HarmonyPatch(typeof(StructuresData), nameof(StructuresData.GetInfoByType)), HarmonyPostfix]
        public static void GetCustomInfoByType(StructureBrain.TYPES Type, ref StructuresData __result)
        {
            if (CustomItemList.Values.Any(x => x.GetType().IsSubclassOf(typeof(CustomMeal)) && (x as CustomMeal)!._mealTypel == Type))
            {
                var data = new StructuresData
                {
                    PrefabPath = "Prefabs/Structures/Other/Meal",
                    IgnoreGrid = true,
                };
                data.Type = Type;
                __result = data;
            }
        }
    }
}
