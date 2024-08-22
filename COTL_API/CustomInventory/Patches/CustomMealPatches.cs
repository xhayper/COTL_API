using COTL_API.CustomStructures;
using HarmonyLib;
using Lamb.UI.FollowerInteractionWheel;
using Socket.Newtonsoft.Json;
using src.UI.InfoCards;
using System;

namespace COTL_API.CustomInventory
{
    [HarmonyPatch]
    public static partial class CustomItemManager
    {
        [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetRecipe)), HarmonyPostfix]
        private static void AddCustomRecepies(ref List<List<InventoryItem>> __result, InventoryItem.ITEM_TYPE mealType)
        {
            if (!CustomMealList.ContainsKey(mealType))
                return;

            __result = CustomMealList[mealType].Recipe;
        }

        [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetMealEffects)), HarmonyPostfix]
        private static void AddCustomMealEffects(ref CookingData.MealEffect[] __result, InventoryItem.ITEM_TYPE mealType)
        {
            if (!CustomMealList.ContainsKey(mealType))
                return;

            __result = CustomMealList[mealType].MealEffects;
        }


        [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetTummyRating)), HarmonyPostfix]
        private static void GetCustomMealTummyRating(ref float __result, InventoryItem.ITEM_TYPE meal)
        {
            if (!CustomMealList.ContainsKey(meal))
                return;

            __result = CustomMealList[meal].TummyRating;
        }

        [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetSatationLevel)), HarmonyPostfix]
        private static void GetCustomSatiationLevel(ref int __result, InventoryItem.ITEM_TYPE meal)
        {
            if (!CustomMealList.ContainsKey(meal))
                return;

            __result = CustomMealList[meal].SatiationLevel;
        }

        [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetAllMeals)), HarmonyPostfix]
        private static void AddCustomMeal(ref InventoryItem.ITEM_TYPE[] __result)
        {
            var customMeals = CustomMealList.Values.ToArray();
            var customItems = CustomMealList.Keys.ToArray();
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

        // TODO: Add support for Localizations
        [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetLocalizedName)), HarmonyPostfix]
        private static void GetCustomMealName(InventoryItem.ITEM_TYPE mealType, ref string __result)
        {
            if (CustomMealList.Keys.Contains(mealType))
            {
                __result = CustomMealList[mealType].LocalizedName();
            }
        }

        // TODO: Add support for Localizations
        [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetLocalizedDescription)), HarmonyPostfix]
        private static void GetCustomMealDescription(InventoryItem.ITEM_TYPE mealType, ref string __result)
        {
            if (CustomMealList.Keys.Contains(mealType))
            {
                __result = CustomMealList[mealType].Description();
            }
        }

        // TODO : rewrite, i don't like it
        [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetMealFromStructureType)), HarmonyPostfix]
        private static void GetCustomMealFromStructure(StructureBrain.TYPES structureType, ref InventoryItem.ITEM_TYPE __result)
        {
            var item = CustomMealList.Keys
                .FirstOrDefault(x => CustomMealList[x].StructureType == structureType);

            if (item != InventoryItem.ITEM_TYPE.NONE)
                __result = item;
        }

        [HarmonyPatch(typeof(RecipeInfoCard), nameof(RecipeInfoCard.Configure)), HarmonyPrefix]
        private static void AddCustomStarRating(RecipeInfoCard __instance, InventoryItem.ITEM_TYPE config)
        {
            var satationLevel = CookingData.GetSatationLevel(config);
            for (var index = 0; index < __instance._starFills.Length; ++index)
                __instance._starFills[index].SetActive(satationLevel >= index + 1);
        }

        [HarmonyPatch(typeof(StructuresData), nameof(StructuresData.GetInfoByType)), HarmonyPostfix]
        private static void GetCustomInfoByType(StructureBrain.TYPES Type, ref StructuresData __result)
        {
            if (CustomMealList.Values.Any(x => x.StructureType == Type))
            {
                var data = new StructuresData
                {
                    PrefabPath = "Prefabs/Structures/Other/Meal",
                    IgnoreGrid = true,
                    Location = FollowerLocation.Base,
                };
                data.Type = Type;
                __result = data;
            }
        }

        [HarmonyPatch(typeof(StructuresData), nameof(StructuresData.GetMealType))]
        private static void GetCustomMealType(StructureBrain.TYPES structureType, ref InventoryItem.ITEM_TYPE __result)
        {
            var inventoryItem = CustomMealList.Values.FirstOrDefault(x => x.StructureType == structureType);

            if (inventoryItem != null)
                __result = inventoryItem.ItemType;
        }
        private static void GetCustomStructureMealType(InventoryItem.ITEM_TYPE mealType, ref StructureBrain.TYPES __result)
        {
            // TODO: Investigate if this can cause an NRE.
            // in theory, No. since this method will never be called on a mealType that isn't a CustomMeal AND is in the CustomItemList.Keys
            if (CustomMealList.Keys.Contains(mealType))
                __result = CustomMealList.Values.First(x => x.ItemType == mealType).StructureType;
        }

        // TODO: rewrite this to be a Postfix so that it doesn't have to skip other patches
        // TODO: maybe(?) rewrite this such that it's not a carbon copy of the original method
        [HarmonyPatch(typeof(StructureBrain), nameof(StructureBrain.CreateBrain)), HarmonyPrefix]
        private static bool CreateBrainForCustomMeal(ref StructuresData data, ref StructureBrain __result)
        {
            StructureBrain sb;
            var type = data.Type;
            if (CustomMealList.Values.Any(x => x.StructureType == type))
            {
                sb = new Structures_Meal();

                StructureBrain.ApplyConfigToData(data);
                sb.Init(data);
                StructureBrain._brainsByID.Add(data.ID, sb);
                StructureManager.StructuresAtLocation(data.Location).Add(sb);
                __result = sb;

                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.MealCommands)), HarmonyPostfix]
        private static void AddCustomMealCommands(ref List<InventoryItem.ITEM_TYPE> availableMeals, ref List<CommandItem> __result)
        {
            foreach (var item in CustomMealList.Keys)
            {
                if (availableMeals.Contains(item))
                {
                    __result.Add(new FollowerCommandItems.FoodCommandItem()
                    {
                        Command = CustomMealList[item].FollowerCommand
                    }); ;
                }
            }
        }

        [HarmonyPatch(typeof(FollowerCommandItems.FoodCommandItem), nameof(FollowerCommandItems.FoodCommandItem.GetTitle))]
        private static void GetFoodCommandItemTitle(ref FollowerCommandItems.FoodCommandItem __instance, ref string __result)
        { 
            var command = __instance.Command;
            if (CustomMealList.Values.Any(x => x.FollowerCommand == command))
            { 
                __result = CustomMealList.Values.First(x => x.FollowerCommand == command).LocalizedName();
            }
        }
        
        [HarmonyPatch(typeof(FollowerCommandItems.FoodCommandItem), nameof(FollowerCommandItems.FoodCommandItem.GetDescription))]
        public static void GetFoodCommandItemDescription(ref FollowerCommandItems.FoodCommandItem __instance, ref string __result)
        { 
            var command = __instance.Command;
            if (CustomMealList.Values.Any(x => x.FollowerCommand == command))
            { 
                __result = CustomMealList.Values.First(x => x.FollowerCommand == command).LocalizedDescription();
            }
        }
    }
}
