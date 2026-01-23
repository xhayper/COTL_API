using HarmonyLib;
using Lamb.UI.FollowerInteractionWheel;

namespace COTL_API.CustomInventory;

[HarmonyPatch]
public static partial class CustomItemManager
{
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetTummyRating))]
    [HarmonyPostfix]
    private static void CookingData_GetTummyRating(ref float __result, InventoryItem.ITEM_TYPE meal)
    {
        if (CustomMealList.TryGetValue(meal, out var value)) __result = value.TummyRating;
    }

    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetAllMeals))]
    [HarmonyPostfix]
    private static void CookingData_GetAllMeals(ref InventoryItem.ITEM_TYPE[] __result)
    {
        var customMeals = CustomMealList.Values.ToArray();
        var customItems = CustomMealList.Keys.ToArray();
        var newResult = new InventoryItem.ITEM_TYPE[__result.Length + customMeals.Length];

        for (var i = 0; i < __result.Length; i++) newResult[i] = __result[i];

        for (var i = 0; i < customMeals.Length; i++) newResult[__result.Length + i] = customItems[i];

        __result = newResult;
    }

    [HarmonyPatch(typeof(Meal), nameof(Meal.MealSafeToEat))]
    [HarmonyPostfix]
    private static void Meal_MealSafeToEat(Meal __instance, ref bool __result)
    {
        if (__instance.StructureInfo == null) return;

        var type = CookingData.GetMealFromStructureType(__instance.StructureInfo.Type);

        if (CustomMealList.TryGetValue(type, out var value))
            __result = value.MealSafeToEat;
    }

    [HarmonyPatch(typeof(StructuresData), nameof(StructuresData.GetInfoByType))]
    [HarmonyPostfix]
    private static void StructuresData_GetInfoByType(StructureBrain.TYPES Type, ref StructuresData __result)
    {
        if (CustomMealList.Values.Any(x => x.StructureType == Type))
        {
            var data = new StructuresData
            {
                PrefabPath = "Prefabs/Structures/Other/Meal",
                IgnoreGrid = true,
                Location = FollowerLocation.Base
            };
            data.Type = Type;
            __result = data;
        }
    }

    // TODO: rewrite this to be a Postfix so that it doesn't have to skip other patches
    // TODO: maybe(?) rewrite this such that it's not a carbon copy of the original method
    [HarmonyPatch(typeof(StructureBrain), nameof(StructureBrain.CreateBrain))]
    [HarmonyPrefix]
    private static bool StructureBrain_CreateBrain(ref StructuresData data, ref StructureBrain __result)
    {
        StructureBrain sb;
        var type = data.Type;
        if (CustomMealList.Values.Any(x => x.StructureType == type))
        {
            sb = new Structures_Meal();

            StructureBrain.ApplyConfigToData(data);
            sb.Init(data);
            StructureBrain.TryAddBrain(data.ID, sb);
            StructureManager.StructuresAtLocation(data.Location).Add(sb);
            __result = sb;

            return false;
        }

        return true;
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.MealCommands))]
    [HarmonyPostfix]
    private static void FollowerCommandGroups_MealCommands(ref List<InventoryItem.ITEM_TYPE> availableMeals,
        ref List<CommandItem> __result)
    {
        foreach (var item in CustomMealList.Keys)
        {
            if (!availableMeals.Contains(item)) continue;

            __result.Add(new FollowerCommandItems.FoodCommandItem
            {
                Command = CustomMealList[item].FollowerCommand
            });
        }
    }

    [HarmonyPatch(typeof(FollowerTask_EatMeal), nameof(FollowerTask_EatMeal.GetMealReaction))]
    [HarmonyPostfix]
    public static void FollowerTask_EatMeal_GetMealReaction(StructureBrain.TYPES type, ref string __result)
    {
        if (CustomMealList.Values.All(x => x.StructureType != type)) return;

        var quality = CustomMealList.Values.First(x => x.StructureType == type).Quality;
        __result = quality switch
        {
            MealQuality.BAD => "Food/food-finish-bad",
            MealQuality.NORMAL => "Food/food-finish",
            MealQuality.GOOD => "Food/Food-finish-good",
            _ => "Food/Food-finish"
        };
    }

    [HarmonyPatch(typeof(StructuresData), nameof(StructuresData.GetMealType))]
    [HarmonyPostfix]
    private static void StructuresData_GetMealType(StructureBrain.TYPES structureType,
        ref InventoryItem.ITEM_TYPE __result)
    {
        if (CustomMealList.Values.Any(x => x.StructureType == structureType))
            __result = CustomMealList.Values.First(x => x.StructureType == structureType).ItemType;
    }
}