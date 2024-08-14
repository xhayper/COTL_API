using HarmonyLib;
using I2.Loc;
using Lamb.UI.Settings;
using UnityEngine.ProBuilder;

namespace COTL_API.CustomLocalization;

[HarmonyPatch]
public partial class CustomLocalizationManager
{
    [HarmonyPatch(typeof(TermData), "GetTranslation")]
    [HarmonyPrefix]
    private static bool TermData_GetTranslation(ref string __result, TermData __instance)
    {
        if (__instance.Term.StartsWith("\"\"") && __instance.Term.EndsWith("\"\""))
        {
            var literal = __instance.Term.Substring(2, __instance.Term.Length - 4);
            __result = literal;
            return false;
        }

        var lang = SettingsManager.Settings.Game.Language;
        if (!LocalizationMap.TryGetValue(lang, out var value)) return true;
        if (!value.ContainsKey(__instance.Term)) return true;
        __result = LocalizationMap[lang][__instance.Term];

        return false;
    }

    [HarmonyPatch(typeof(LanguageSourceData), "TryGetTranslation")]
    [HarmonyPrefix]
    private static bool LanguageSourceData_TryGetTranslation(string term, ref string Translation, ref bool __result)
    {
        var lang = SettingsManager.Settings.Game.Language;
        if (!LocalizationMap.TryGetValue(lang, out var value)) return true;
        if (!value.ContainsKey(term)) return true;
        Translation = LocalizationMap[lang][term];
        __result = true;

        return false;
    }

    [HarmonyPatch(typeof(GameSettings), nameof(GameSettings.OnLanguageChanged))]
    [HarmonyPrefix]
    private static bool GameSettings_OnLanguageChanged(GameSettings __instance, int index)
    {
        if (index >= LanguageUtilities.AllLanguages.Length)
        {
            var text = LanguageList[index - LanguageUtilities.AllLanguages.Length];
            SettingsManager.Settings.Game.Language = text;
            LocalizationManager.CurrentLanguage = text;
            __instance._cachedLanguage = text;
            
            if (TwitchAuthentication.IsAuthenticated)
                TwitchManager.SetLanguage(LocalizationManager.CurrentLanguageCode);

            UnityEngine.Debug.Log("GameSettings - Change Language to " + text);
            LocalizationManager.LocalizeAll(true);
            return false;
        }

        LocalizationManager.LocalizeAll(true);
        return true;
    }

    [HarmonyPatch(typeof(GameSettings), nameof(GameSettings.GetLanguageIndex))]
    [HarmonyPrefix]
    private static bool GameSettings_GetLanguageIndex(GameSettings __instance, ref int __result)
    {
        __instance._languageSelector._prefilledContent =
            __instance._languageSelector._prefilledContent.AddRange(LocalizationMap.Keys.ToArray());
        __instance._languageSelector.UpdateContent(__instance._languageSelector._prefilledContent);
        if (!LanguageList.Contains(SettingsManager.Settings.Game.Language)) return true;
        __result = LanguageUtilities.AllLanguages.Length + LanguageList.IndexOf(SettingsManager.Settings.Game.Language);
        return false;
    }
}