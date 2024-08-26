using HarmonyLib;
using src.UI.InfoCards;
using UnityEngine;
using UnityEngine.Serialization;

namespace COTL_API.CustomInventory;

[HarmonyPatch]
public static partial class CustomItemManager
{
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetLocalizedName))]
    [HarmonyPostfix]
    private static void CookingData_GetLocalizedName(InventoryItem.ITEM_TYPE mealType, ref string __result)
    {
        if (CustomFoodList.TryGetValue(mealType, out var value)) __result = value.LocalizedName();
    }
    
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetLocalizedDescription))]
    [HarmonyPostfix]
    private static void CookingData_GetLocalizedDescription(InventoryItem.ITEM_TYPE mealType, ref string __result)
    {
        if (CustomFoodList.TryGetValue(mealType, out var value)) __result = value.Description();
    }
    
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetRecipe))]
    [HarmonyPostfix]
    private static void CookingData_GetRecipe(ref List<List<InventoryItem>> __result, InventoryItem.ITEM_TYPE mealType)
    {
        if (CustomFoodList.TryGetValue(mealType, out var value)) __result = value.Recipe;
    }

    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetMealEffects))]
    [HarmonyPostfix]
    private static void CookingData_GetMealEffects(ref CookingData.MealEffect[] __result,
        InventoryItem.ITEM_TYPE mealType)
    {
        if (CustomFoodList.TryGetValue(mealType, out var value)) __result = value.MealEffects;
    }
    
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetSatationLevel))]
    [HarmonyPostfix]
    private static void CookingData_GetSatationLevel(ref int __result, InventoryItem.ITEM_TYPE meal)
    {
        if (CustomFoodList.TryGetValue(meal, out var value))  __result = value.SatiationLevel;
    }
    
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetSatationAmount))]
    [HarmonyPostfix]
    private static void CookingData_GetSatationAmount(ref int __result, InventoryItem.ITEM_TYPE meal)
    {
        if (CustomFoodList.TryGetValue(meal, out var value)) __result = value.FoodSatitation;
    }
    
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetMealFromStructureType))]
    [HarmonyPostfix]
    private static void CookingData_GetMealFromStructureType(StructureBrain.TYPES structureType,
        ref InventoryItem.ITEM_TYPE __result)
    {
        var item = CustomFoodList.Keys
            .FirstOrDefault(x => CustomFoodList[x].StructureType == structureType);

        if (item != InventoryItem.ITEM_TYPE.NONE)
            __result = item;
    }

    [HarmonyPatch(typeof(RecipeInfoCard), nameof(RecipeInfoCard.Configure))]
    [HarmonyPostfix]
    private static void RecipeInfoCard_Configure(RecipeInfoCard __instance, InventoryItem.ITEM_TYPE config)
    {
        if (!CustomItemList.TryGetValue(config, out var value))
            return;

        __instance._itemDescription.text = value.Description();
        __instance._itemHeader.text = value.LocalizedName();
    }

    [HarmonyPatch(typeof(FollowerCommandItems.FoodCommandItem), nameof(FollowerCommandItems.FoodCommandItem.GetTitle))]
    [HarmonyPostfix]
    private static void FoodCommandItem_GetTitle(ref FollowerCommandItems.FoodCommandItem __instance,
        ref string __result)
    {
        var command = __instance.Command;
        if (CustomFoodList.Values.Any(x => x.FollowerCommand == command))
            __result = CustomFoodList.Values.First(x => x.FollowerCommand == command).LocalizedName();
    }

    [HarmonyPatch(typeof(FollowerCommandItems.FoodCommandItem),
        nameof(FollowerCommandItems.FoodCommandItem.GetDescription))]
    [HarmonyPostfix]
    private static void FoodCommandItem_GetDescription(ref FollowerCommandItems.FoodCommandItem __instance,
        ref string __result)
    {
        var command = __instance.Command;
        if (CustomFoodList.Values.Any(x => x.FollowerCommand == command))
            __result = CustomFoodList.Values.First(x => x.FollowerCommand == command).LocalizedDescription();
    }
    
    [HarmonyPatch(typeof(StructuresData), nameof(StructuresData.GetMealType))]
    [HarmonyPostfix]
    private static void StructuresData_GetMealType(StructureBrain.TYPES structureType,
        ref InventoryItem.ITEM_TYPE __result)
    {
        var inventoryItem = CustomFoodList.Values.FirstOrDefault(x => x.StructureType == structureType);

        if (inventoryItem != null) __result = inventoryItem.ItemType;
    }
        
    [HarmonyPatch(typeof(StructuresData), nameof(StructuresData.GetMealStructureType))]
    [HarmonyPostfix]
    private static void StructuresData_GetMealStructureType(InventoryItem.ITEM_TYPE mealType,
        ref StructureBrain.TYPES __result)
    {
        if (CustomFoodList.Keys.Contains(mealType))
            __result = CustomFoodList.Values.First(x => x.ItemType == mealType).StructureType;
    }

    [HarmonyPatch(typeof(interaction_FollowerInteraction),
        nameof(interaction_FollowerInteraction.OnFollowerCommandFinalized))]
    [HarmonyPrefix]
    private static void interaction_FollowerInteraction_OnFollowerCommandFinalized(
        ref FollowerCommands[] followerCommands,
        ref interaction_FollowerInteraction __instance)
    {
        var command = followerCommands[0];

        if (CustomFoodList.Values.All(x => x.FollowerCommand != command)) return;

        var food = CustomFoodList.Values.First(x => x.FollowerCommand == command);
        __instance.follower.Brain.CancelTargetedMeal(food.StructureType);
        __instance.eventListener.PlayFollowerVO(__instance.generalAcknowledgeVO);
        __instance.follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, food.StructureType);
        __instance.follower.Brain.CompleteCurrentTask();
    }

    [HarmonyPatch(typeof(InventoryItemDisplay), nameof(InventoryItemDisplay.SetImage),
        typeof(InventoryItem.ITEM_TYPE), typeof(bool))]
    [HarmonyPostfix]
    private static void InventoryItemDisplay_SetImage
        (ref InventoryItemDisplay __instance, ref InventoryItem.ITEM_TYPE Type)
    {
        var transform = __instance.spriteRenderer.transform;
        if (CustomFoodList.TryGetValue(Type, out var value))
        {
            transform.localScale = value.LocalScale;
            transform.localPosition += value.ItemDisplayOffset;
        }
        else
        {
            transform.localScale = Vector3.one;
            transform.localPosition = __instance.GetComponent<TransformHolder>().localPosition;
        }
    }
    
    // please forgive this hacky-ness it's the only way i could think of to do this
    [HarmonyPatch(typeof(InventoryItemDisplay), nameof(InventoryItemDisplay.Awake))]
    [HarmonyPostfix]
    private static void InventoryItemDisplay_Awake(ref InventoryItemDisplay __instance)
    {
        __instance.gameObject.AddComponent<TransformHolder>();
    }

    private class TransformHolder : MonoBehaviour
    {
        public Vector3 localPosition;

        public void Awake()
        {
            localPosition = transform.localPosition;
        }
    }
}