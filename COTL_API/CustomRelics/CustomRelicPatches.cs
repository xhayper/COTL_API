using HarmonyLib;
using I2.Loc;
using UnityEngine;

namespace COTL_API.CustomRelics;

[HarmonyPatch]
public partial class CustomRelicManager
{
    [HarmonyPatch(typeof(LocalizationManager), nameof(LocalizationManager.GetTranslation), typeof(string), typeof(bool),
        typeof(int), typeof(bool), typeof(bool), typeof(GameObject), typeof(string), typeof(bool))]
    [HarmonyPrefix]
    private static bool LocalizationManager_GetTranslation(string Term, ref string __result)
    {
        var split = Term.Split('/');
        if (split[0] != "Relics") return true;
        if (Enum.TryParse<RelicType>(split[1], out var relicType))
        {
            if (!CustomRelicDataList.ContainsKey(relicType)) return true;

            if (split.Length == 2) __result = CustomRelicDataList[relicType].GetTitleLocalisation();
            else if (split[2] == "Description") __result = CustomRelicDataList[relicType].GetDescriptionLocalisation();
            else if (split[2] == "Lore") __result = CustomRelicDataList[relicType].GetLoreLocalization();
            else return true;

            return false;
        }

        return true;
    }

    [HarmonyPatch(typeof(RelicData), nameof(RelicData.GetChargeCategory), typeof(RelicType))]
    [HarmonyPrefix]
    private static bool RelicData_GetChargeCategory(RelicType relicType, ref RelicChargeCategory __result)
    {
        if (!CustomRelicDataList.ContainsKey(relicType)) return true;

        __result = CustomRelicDataList[relicType].GetChargeCategory();

        return false;
    }

    [HarmonyPatch(typeof(RelicData), nameof(RelicData.GetChargeCategory), typeof(RelicData))]
    [HarmonyPrefix]
    private static bool RelicData_GetChargeCategory(RelicData relicData, ref RelicChargeCategory __result)
    {
        if (!CustomRelicDataList.ContainsKey(relicData.RelicType)) return true;

        __result = CustomRelicDataList[relicData.RelicType].GetChargeCategory();

        return false;
    }

    [HarmonyPatch(typeof(PlayerRelic), nameof(PlayerRelic.UseRelic), typeof(RelicType), typeof(bool))]
    [HarmonyPostfix]
    private static void PlayerRelic_UseRelic(RelicType relicType, bool forceConsumableAnimation)
    {
        if (!CustomRelicDataList.ContainsKey(relicType)) return;

        if (CustomRelicDataList[relicType].RelicSubType == RelicSubType.Blessed)
            CustomRelicDataList[relicType].OnUseBlessed(forceConsumableAnimation);
        else if (CustomRelicDataList[relicType].RelicSubType == RelicSubType.Dammed)
            CustomRelicDataList[relicType].OnUseDamned(forceConsumableAnimation);
        else
            CustomRelicDataList[relicType].OnUse(forceConsumableAnimation);
    }

    [HarmonyPatch(typeof(EquipmentManager), nameof(EquipmentManager.RelicData), MethodType.Getter)]
    [HarmonyPostfix]
    private static void EquipmentManager_RelicData(ref RelicData[] __result)
    {
        foreach (var relic in CustomRelicDataList)
        {
            __result = __result.Append(relic.Value).ToArray();
            if (relic.Value.CanBeBlessed)
                __result = __result.Append(relic.Value.ToBlessed()).ToArray();

            if (relic.Value.CanBeDamned)
                __result = __result.Append(relic.Value.ToDamned()).ToArray();
        }
    }
}