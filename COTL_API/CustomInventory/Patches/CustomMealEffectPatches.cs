using HarmonyLib;
using UnityEngine;

namespace COTL_API.CustomInventory;

// TODO: Test this proper, in theory this works but i haven't tested it yet
[HarmonyPatch]
public static partial class CustomMealEffectManager
{
    [HarmonyPatch(typeof(CookingData),nameof(CookingData.DoMealEffect)), HarmonyPostfix]
    public static void DoCustomEffect(InventoryItem.ITEM_TYPE meal, FollowerBrain follower) 
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
}
