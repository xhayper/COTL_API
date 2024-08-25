using HarmonyLib;
using src.UI.InfoCards;
using UnityEngine;

namespace COTL_API.CustomInventory;

[HarmonyPatch]
public static partial class CustomMealEffectManager
{
    [HarmonyPatch(typeof(CookingData),nameof(CookingData.DoMealEffect)), HarmonyPostfix]
    public static void CookingData_DoMealEffect(InventoryItem.ITEM_TYPE meal, FollowerBrain follower) 
    {
        var customMealEffects = CookingData.GetMealEffects(meal).Where(x => CustomEffectList.Keys.Contains(x.MealEffectType));
        foreach (var effect in customMealEffects)
        {
            var chance = UnityEngine.Random.Range(0, 100);

            if (chance >= effect.Chance)
                continue;

            CustomEffectList[effect.MealEffectType].Effect(follower);
        }
    }
    
    [HarmonyPatch(typeof(RecipeInfoCard), nameof(RecipeInfoCard.Configure)), HarmonyPostfix]
    private static void RecipeInfoCard_Configure(RecipeInfoCard __instance, InventoryItem.ITEM_TYPE config)
    {
        var mealEffects = CookingData.GetMealEffects(config);
    }
    
    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetEffectDescription)), HarmonyPostfix]
    private static void CookingData_GetEffectDescription(ref CookingData.MealEffect mealEffect, ref string __result)
    {
        if (!CustomEffectList.Keys.Contains(mealEffect.MealEffectType)) return;

        var effect = CustomEffectList[mealEffect.MealEffectType];

        var description = effect.Description();
        var descriptionSuffix = effect.DescriptionSuffix();
        var icon = effect.Positive() ? "<sprite name=\"icon_FaithUp\">" : "<sprite name=\"icon_FaithDown\">";
        var enabled = effect.EffectEnabled();

        var formattedDescription = $"{icon} <color=#FFD201>{mealEffect.Chance}%</color> {description}";
        
        if (!enabled)
        {
            formattedDescription = $"<s>{formattedDescription}</s>";
        }
        
        __result = formattedDescription + descriptionSuffix;
    }
    
}
