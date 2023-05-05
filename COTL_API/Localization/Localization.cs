using COTL_API.Helpers;
using HarmonyLib;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.Settings;
using UnityEngine.ProBuilder;

namespace COTL_API.Localization;

[HarmonyPatch]
public static class Localization
{
    public static Dictionary<string, Dictionary<string, string>> LocalizationMap { get; } = new();
    public static List<string> LanguageList { get; } = new();

    public static void LoadLocalization(string name, string path)
    {
        LocalizationMap.Add(name, new Dictionary<string, string>());
        LanguageList.Add(name);
        if (File.Exists(path))
        {
            var lines = File.ReadAllLines(path);
            var isInsideQuotes = false;
            foreach (var line in lines)
            {
                var key = "";
                var value = "";
                for (var i = 0; i < line.Length; i++)
                    if (line[i] == '"')
                    {
                        isInsideQuotes = !isInsideQuotes;
                    }
                    else if (line[i] == ',' && !isInsideQuotes)
                    {
                        key = line.Substring(1, i - 2);
                        value = line.Substring(i + 3, line.Length - i - 4);
                        break;
                    }

                LocalizationMap[name].Add(key, value);
            }

            LogHelper.LogInfo($"Loaded localization: {name}");
        }
        else
        {
            LogHelper.LogError(
                $"Localization file not found! Please make sure that the path \"{path}\" contains the localization file.");
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(TermData), "GetTranslation")]
    internal static bool GetTranslation(ref string __result, TermData __instance)
    {
        if (__instance.Term.StartsWith("\"\"") && __instance.Term.EndsWith("\"\""))
        {
            var literal = __instance.Term.Substring(2, __instance.Term.Length - 4);
            __result = literal;
            return false;
        }

        var lang = SettingsManager.Settings.Game.Language;
        if (!LocalizationMap.ContainsKey(lang)) return true;
        if (!LocalizationMap[lang].ContainsKey(__instance.Term)) return true;
        __result = LocalizationMap[lang][__instance.Term];
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
            if (TwitchAuthentication.IsAuthenticated) TwitchRequest.SendEBSData();

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

    [HarmonyPatch(typeof(MMHorizontalSelector), nameof(MMHorizontalSelector.UpdateContent))]
    [HarmonyPrefix]
    private static void MMHorizontalSelector_UpdateContent(MMHorizontalSelector __instance, string[] newContent)
    {
        if (__instance._contentIndex >= newContent.Length) __instance._contentIndex = 0;
    }
}