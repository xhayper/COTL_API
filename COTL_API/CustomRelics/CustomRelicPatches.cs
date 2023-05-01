using HarmonyLib;

namespace COTL_API.CustomRelics;

/*
 TODO: Add support for `EquipmentManager -> GetRandomRelicData`
 TODO: Add support for `PlayerRelic -> UseRelic`
 */

[HarmonyPatch]
public partial class CustomRelicManager
{
    [HarmonyPatch(typeof(RelicData), nameof(RelicData.GetTitleLocalisation), typeof(RelicType))]
    [HarmonyPrefix]
    private static bool RelicData_GetTitleLocalisation(RelicType type, ref string __result)
    {
        if (!CustomRelicDataList.ContainsKey(type)) return true;

        __result = CustomRelicDataList[type].GetTitleLocalisation();

        return false;
    }

    [HarmonyPatch(typeof(RelicData), nameof(RelicData.GetDescriptionLocalisation), typeof(RelicType))]
    [HarmonyPrefix]
    private static bool RelicData_GetDescriptionLocalisation(RelicType type, ref string __result)
    {
        if (!CustomRelicDataList.ContainsKey(type)) return true;

        __result = CustomRelicDataList[type].GetDescriptionLocalisation();

        return false;
    }

    [HarmonyPatch(typeof(RelicData), nameof(RelicData.GetLoreLocalization), typeof(RelicType))]
    [HarmonyPrefix]
    private static bool RelicData_GetLoreLocalization(RelicType type, ref string __result)
    {
        if (!CustomRelicDataList.ContainsKey(type)) return true;

        __result = CustomRelicDataList[type].GetLoreLocalization();

        return false;
    }

    [HarmonyPatch(typeof(RelicData), nameof(RelicData.GetChargeCategory), typeof(RelicType))]
    [HarmonyPrefix]
    private static bool RelicData_GetChargeCategory(RelicType type, ref RelicChargeCategory __result)
    {
        if (!CustomRelicDataList.ContainsKey(type)) return true;

        __result = CustomRelicDataList[type].GetChargeCategory();

        return false;
    }

    [HarmonyPatch(typeof(RelicData), nameof(RelicData.GetChargeCategory), typeof(RelicData))]
    [HarmonyPrefix]
    private static bool RelicData_GetChargeCategory(RelicData data, ref RelicChargeCategory __result)
    {
        if (!CustomRelicDataList.ContainsKey(data.RelicType)) return true;

        __result = CustomRelicDataList[data.RelicType].GetChargeCategory();

        return false;
    }
}