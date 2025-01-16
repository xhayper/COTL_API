using HarmonyLib;
using UnityEngine;

namespace FoodPlus.CustomTraits;

[HarmonyPatch]
public static partial class CustomTraitManager
{
    [HarmonyPatch(typeof(FollowerTrait), nameof(FollowerTrait.GetIcon))]
    [HarmonyPostfix]
    private static void FollowerTrait_GetIcon(FollowerTrait.TraitType Type, ref Sprite __result)
    {
        if (!CustomTraitList.TryGetValue(Type, out var value)) return;
        __result = value.Icon;
    }
    
    [HarmonyPatch(typeof(FollowerTrait), nameof(FollowerTrait.IsPositiveTrait))]
    [HarmonyPostfix]
    private static void FollowerTrait_IsPositiveTrait(FollowerTrait.TraitType traitType, ref bool __result)
    {
        if (!CustomTraitList.TryGetValue(traitType, out var value)) return;
        __result = value.Positive;
    }
    
    [HarmonyPatch(typeof(FollowerTrait), nameof(FollowerTrait.GetLocalizedTitle))]
    [HarmonyPostfix]
    private static void FollowerTrait_GetLocalizedTitle(FollowerTrait.TraitType Type, ref string __result)
    {
        if (!CustomTraitList.TryGetValue(Type, out var value)) return;
        __result = value.LocalizedTitle();
    }

    [HarmonyPatch(typeof(FollowerTrait), nameof(FollowerTrait.GetLocalizedDescription))]
    [HarmonyPostfix]
    private static void FollowerTrait_GetLocalizedDescription(FollowerTrait.TraitType Type, ref string __result)
    {
        if (!CustomTraitList.TryGetValue(Type, out var value)) return;
        __result = value.LocalizedDescription();
    }

    [HarmonyPatch(typeof(FollowerTrait), nameof(FollowerTrait.IsTraitUnavailable))]
    [HarmonyPostfix]
    private static void FollowerTrait_IsTraitUnavailable(FollowerTrait.TraitType trait, ref bool __result)
    {
        if (!CustomTraitList.TryGetValue(trait, out var value)) return;
        __result = value.IsTraitUnavailable();
    }
}