using HarmonyLib;
using UnityEngine;
using static CookingData;

namespace COTL_API.CustomInventory
{
    [HarmonyPatch]
    public static partial class CustomMealEffectManager
    {
        [HarmonyPatch(typeof(CookingData),nameof(CookingData.DoMealEffect)), HarmonyPostfix]
        public static void DoCustomEffect(InventoryItem.ITEM_TYPE meal, FollowerBrain follower) 
        {
            var customMealEffects = (MealEffect[])GetMealEffects(meal).Where(x => CustomEffectList.Keys.Contains(x.MealEffectType));
            foreach (var effect in customMealEffects)
            { 
                var chance = UnityEngine.Random.Range(0, 100);

                if(chance >= effect.Chance)
                    continue;

                CustomEffectList[effect.MealEffectType].Effect(follower);
            }
        }
    }
}
