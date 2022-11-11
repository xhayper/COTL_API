using System.Collections.Generic;
using Lamb.UI.Rituals;
using src.Extensions;
using UnityEngine;
using HarmonyLib;

namespace COTL_API.CustomRituals;

[HarmonyPatch]
public static partial class CustomRitualManager
{
    [HarmonyPatch(typeof(UIRitualsMenuController), nameof(UIRitualsMenuController.OnShowStarted))]
    [HarmonyPrefix]
    public static void UIRitualsMenuController_OnShowStarted(UIRitualsMenuController __instance)
    {
        foreach (var customRitual in CustomRituals.Keys)
        {
            Plugin.Logger.LogInfo("Custom Ritual: " + customRitual);
            __instance.ConfigureItem(__instance._ritualItemTemplate.Instantiate(__instance._ritualsContent),
                customRitual);
        }
    }

    [HarmonyPatch(typeof(RitualItem), nameof(RitualItem.Configure))]
    [HarmonyPrefix]
    public static bool RitualItem_Configure(RitualItem __instance, UpgradeSystem.Type ritualType)
    {
        if (!CustomRituals.ContainsKey(ritualType)) return true;

        if (!DataManager.Instance.UnlockedUpgrades.Contains(ritualType))
            DataManager.Instance.UnlockedUpgrades.Add(ritualType);
        return true;
    }

    [HarmonyPatch(typeof(DoctrineUpgradeSystem), nameof(DoctrineUpgradeSystem.GetIconForRitual))]
    [HarmonyPrefix]
    public static bool DoctrineUpgradeSystem_GetIconForRitual(ref Sprite __result, UpgradeSystem.Type type)
    {
        if (!CustomRituals.ContainsKey(type)) return true;

        __result = CustomRituals[type].Sprite;
        return false;
    }

    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.GetCost))]
    [HarmonyPrefix]
    public static bool UpgradeSystem_GetCost(ref List<StructuresData.ItemCost> __result, UpgradeSystem.Type Type)
    {
        if (!CustomRituals.ContainsKey(Type)) return true;

        __result = CustomRituals[Type].ItemCosts;
        return false;
    }

    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.GetLocalizedName))]
    [HarmonyPrefix]
    public static bool UpgradeSystem_GetLocalizedName(ref string __result, UpgradeSystem.Type Type)
    {
        if (!CustomRituals.ContainsKey(Type)) return true;

        __result = CustomRituals[Type].GetLocalizedName;
        return false;
    }

    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.GetLocalizedDescription))]
    [HarmonyPrefix]
    public static bool UpgradeSystem_GetLocalizedDescription(ref string __result, UpgradeSystem.Type Type)
    {
        if (!CustomRituals.ContainsKey(Type)) return true;

        __result = CustomRituals[Type].GetLocalizedDescription;
        return false;
    }

    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.GetRitualFaithChange))]
    [HarmonyPrefix]
    public static bool UpgradeSystem_GetRitualFaithChange(ref float __result, UpgradeSystem.Type Type)
    {
        if (!CustomRituals.ContainsKey(Type)) return true;

        __result = CustomRituals[Type].FaithChange;
        return false;
    }

    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.GetRitualTrait))]
    [HarmonyPrefix]
    public static bool UpgradeSystem_GetRitualTrait(ref FollowerTrait.TraitType __result, UpgradeSystem.Type Type)
    {
        if (!CustomRituals.ContainsKey(Type)) return true;

        __result = CustomRituals[Type].RitualTrait;
        return false;
    }

    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.PerformRitual))]
    [HarmonyPostfix]
    public static void Interaction_TempleAltar_PerformRitual(Interaction_TempleAltar __instance,
        UpgradeSystem.Type RitualType)
    {
        if (!CustomRituals.ContainsKey(RitualType)) return;

        var ritual =
            (CustomRitual)__instance.gameObject.AddComponent(CustomRituals[RitualType].GetType());
        ritual.upgradeType = CustomRituals[RitualType].upgradeType;
        ritual.ModPrefix = CustomRituals[RitualType].ModPrefix;
        __instance.CurrentRitual = ritual;
        __instance.CurrentRitual.Play();
    }

    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.RitualOnEnd))]
    [HarmonyPostfix]
    public static void Interaction_TempleAltar_RitualOnEnd(Interaction_TempleAltar __instance, bool cancelled)
    {
        if (!CustomRituals.ContainsKey(__instance.RitualType)) return;

        if (!cancelled)
            UpgradeSystem.AddCooldown(__instance.RitualType,
                CustomRituals[__instance.RitualType].Cooldown);
    }
}