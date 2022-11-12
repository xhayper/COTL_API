using System.Collections.Generic;
using COTL_API.Helpers;
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
        foreach (var customRitual in CustomRitualList.Keys)
        {
            LogHelper.LogInfo("Custom Ritual: " + customRitual);
            __instance.ConfigureItem(__instance._ritualItemTemplate.Instantiate(__instance._ritualsContent),
                customRitual);
        }
    }

    [HarmonyPatch(typeof(RitualItem), nameof(RitualItem.Configure))]
    [HarmonyPrefix]
    public static bool RitualItem_Configure(RitualItem __instance, UpgradeSystem.Type ritualType)
    {
        if (!CustomRitualList.ContainsKey(ritualType)) return true;

        if (!DataManager.Instance.UnlockedUpgrades.Contains(ritualType))
            DataManager.Instance.UnlockedUpgrades.Add(ritualType);
        return true;
    }

    [HarmonyPatch(typeof(DoctrineUpgradeSystem), nameof(DoctrineUpgradeSystem.GetIconForRitual))]
    [HarmonyPrefix]
    public static bool DoctrineUpgradeSystem_GetIconForRitual(ref Sprite __result, UpgradeSystem.Type type)
    {
        if (!CustomRitualList.ContainsKey(type)) return true;

        __result = CustomRitualList[type].Sprite;
        return false;
    }

    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.GetCost))]
    [HarmonyPrefix]
    public static bool UpgradeSystem_GetCost(ref List<StructuresData.ItemCost> __result, UpgradeSystem.Type Type)
    {
        if (!CustomRitualList.ContainsKey(Type)) return true;

        __result = CustomRitualList[Type].ItemCosts;
        return false;
    }

    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.GetLocalizedName))]
    [HarmonyPrefix]
    public static bool UpgradeSystem_GetLocalizedName(ref string __result, UpgradeSystem.Type Type)
    {
        if (!CustomRitualList.ContainsKey(Type)) return true;

        __result = CustomRitualList[Type].GetLocalizedName;
        return false;
    }

    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.GetLocalizedDescription))]
    [HarmonyPrefix]
    public static bool UpgradeSystem_GetLocalizedDescription(ref string __result, UpgradeSystem.Type Type)
    {
        if (!CustomRitualList.ContainsKey(Type)) return true;

        __result = CustomRitualList[Type].GetLocalizedDescription;
        return false;
    }

    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.GetRitualFaithChange))]
    [HarmonyPrefix]
    public static bool UpgradeSystem_GetRitualFaithChange(ref float __result, UpgradeSystem.Type Type)
    {
        if (!CustomRitualList.ContainsKey(Type)) return true;

        __result = CustomRitualList[Type].FaithChange;
        return false;
    }

    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.GetRitualTrait))]
    [HarmonyPrefix]
    public static bool UpgradeSystem_GetRitualTrait(ref FollowerTrait.TraitType __result, UpgradeSystem.Type Type)
    {
        if (!CustomRitualList.ContainsKey(Type)) return true;

        __result = CustomRitualList[Type].RitualTrait;
        return false;
    }

    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.PerformRitual))]
    [HarmonyPostfix]
    public static void Interaction_TempleAltar_PerformRitual(Interaction_TempleAltar __instance,
        UpgradeSystem.Type RitualType)
    {
        if (!CustomRitualList.ContainsKey(RitualType)) return;

        var ritual =
            (CustomRitual)__instance.gameObject.AddComponent(CustomRitualList[RitualType].GetType());
        ritual.UpgradeType = CustomRitualList[RitualType].UpgradeType;
        ritual.ModPrefix = CustomRitualList[RitualType].ModPrefix;
        __instance.CurrentRitual = ritual;
        __instance.CurrentRitual.Play();
    }

    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.RitualOnEnd))]
    [HarmonyPostfix]
    public static void Interaction_TempleAltar_RitualOnEnd(Interaction_TempleAltar __instance, bool cancelled)
    {
        if (!CustomRitualList.ContainsKey(__instance.RitualType)) return;

        if (!cancelled)
            UpgradeSystem.AddCooldown(__instance.RitualType,
                CustomRitualList[__instance.RitualType].Cooldown);
    }
}