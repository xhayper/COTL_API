using HarmonyLib;
using I2.Loc;
using Lamb.UI.Rituals;
using Socket.Newtonsoft.Json.Utilities.LinqBridge;
using src.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace COTL_API.CustomRituals;

[HarmonyPatch]
internal class RitualPatches
{
    [HarmonyPatch(typeof(UIRitualsMenuController), nameof(UIRitualsMenuController.OnShowStarted))]
    [HarmonyPrefix]
    public static void UIRitualsMenuController_OnShowStarted(UIRitualsMenuController __instance)
    {
        foreach (UpgradeSystem.Type customRitual in CustomRitualManager.CustomRituals.Keys)
        {
            Plugin.Logger.LogInfo("Custom Ritual: " + customRitual);
            __instance.ConfigureItem(__instance._ritualItemTemplate.Instantiate(__instance._ritualsContent), customRitual);
        }
    }

    [HarmonyPatch(typeof(RitualItem), nameof(RitualItem.Configure))]
    [HarmonyPrefix]
    public static bool RitualItem_Configure(RitualItem __instance, UpgradeSystem.Type ritualType)
    {
        if (!CustomRitualManager.CustomRituals.ContainsKey(ritualType)) return true;

        if (!DataManager.Instance.UnlockedUpgrades.Contains(ritualType)) DataManager.Instance.UnlockedUpgrades.Add(ritualType);
        return true;
    }

    [HarmonyPatch(typeof(DoctrineUpgradeSystem), nameof(DoctrineUpgradeSystem.GetIconForRitual))]
    [HarmonyPrefix]
    public static bool DoctrineUpgradeSystem_GetIconForRitual(ref Sprite __result, UpgradeSystem.Type type)
    {
        if (!CustomRitualManager.CustomRituals.ContainsKey(type)) return true;

        __result = CustomRitualManager.CustomRituals[type].Sprite;
        return false;
    }

    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.GetCost))]
    [HarmonyPrefix]
    public static bool UpgradeSystem_GetCost(ref List<StructuresData.ItemCost> __result, UpgradeSystem.Type Type)
    {
        if (!CustomRitualManager.CustomRituals.ContainsKey(Type)) return true;

        __result = CustomRitualManager.CustomRituals[Type].ItemCosts;
        return false;
    }

    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.GetLocalizedName))]
    [HarmonyPrefix]
    public static bool UpgradeSystem_GetLocalizedName(ref string __result, UpgradeSystem.Type Type)
    {
        if (!CustomRitualManager.CustomRituals.ContainsKey(Type)) return true;

        __result = CustomRitualManager.CustomRituals[Type].GetLocalizedName;
        return false;
    }

    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.GetLocalizedDescription))]
    [HarmonyPrefix]
    public static bool UpgradeSystem_GetLocalizedDescription(ref string __result, UpgradeSystem.Type Type)
    {
        if (!CustomRitualManager.CustomRituals.ContainsKey(Type)) return true;

        __result = CustomRitualManager.CustomRituals[Type].GetLocalizedDescription;
        return false;
    }

    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.GetRitualFaithChange))]
    [HarmonyPrefix]
    public static bool UpgradeSystem_GetRitualFaithChange(ref float __result, UpgradeSystem.Type Type)
    {
        if (!CustomRitualManager.CustomRituals.ContainsKey(Type)) return true;

        __result = CustomRitualManager.CustomRituals[Type].FaithChange;
        return false;
    }

    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.GetRitualTrait))]
    [HarmonyPrefix]
    public static bool UpgradeSystem_GetRitualTrait(ref FollowerTrait.TraitType __result, UpgradeSystem.Type Type)
    {
        if (!CustomRitualManager.CustomRituals.ContainsKey(Type)) return true;

        __result = CustomRitualManager.CustomRituals[Type].RitualTrait;
        return false;
    }

    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.PerformRitual))]
    [HarmonyPostfix]
    public static void Interaction_TempleAltar_PerformRitual(Interaction_TempleAltar __instance, UpgradeSystem.Type RitualType)
    {
        if (!CustomRitualManager.CustomRituals.ContainsKey(RitualType)) return;

        var ritual = (CustomRitual)__instance.gameObject.AddComponent(CustomRitualManager.CustomRituals[RitualType].GetType());
        ritual.upgradeType = CustomRitualManager.CustomRituals[RitualType].upgradeType;
        ritual.ModPrefix = CustomRitualManager.CustomRituals[RitualType].ModPrefix;
        __instance.CurrentRitual = ritual;
        __instance.CurrentRitual.Play();
    }

    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.RitualOnEnd))]
    [HarmonyPostfix]
    public static void Interaction_TempleAltar_RitualOnEnd(Interaction_TempleAltar __instance, bool cancelled)
    {
        if (!CustomRitualManager.CustomRituals.ContainsKey(__instance.RitualType)) return;

        if (!cancelled)
            UpgradeSystem.AddCooldown(__instance.RitualType, CustomRitualManager.CustomRituals[__instance.RitualType].Cooldown);
    }
}
