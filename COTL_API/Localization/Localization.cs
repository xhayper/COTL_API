using HarmonyLib;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.Settings;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.ProBuilder;

namespace COTL_API.Localization;

[HarmonyPatch]
public static class Localization
{
    public static Dictionary<string,Dictionary<string, string>> Localizations = new();
    internal static List<string> Languages = new();
    
    public static void LoadLocalization(string name, string path)
    {
        Localizations.Add(name, new Dictionary<string, string>());
        Languages.Add(name);
        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            bool isInsideQuotes = false;
            foreach (string line in lines)
            {
                string key = "";
                string value = "";
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '"')
                    {
                        isInsideQuotes = !isInsideQuotes;
                    }
                    else if (line[i] == ',' && !isInsideQuotes)
                    {
                        key = line.Substring(1, i-2);
                        value = line.Substring(i+3, line.Length-i-4);
                        break;
                    }
                }
                Localizations[name].Add(key, value);
            }
            Plugin.Logger.LogInfo($"Loaded localization: {name}");
        }
        else
        {
            Plugin.Logger.LogError($"Localization file not found! Please make sure that the path \"{path}\" contains the localization file.");
        }
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TermData), "GetTranslation")]
    internal static bool GetTranslation(ref string __result, TermData __instance)
    {
        if (__instance.Term.StartsWith("\"\"") && __instance.Term.EndsWith("\"\""))
        {
            string literal = __instance.Term.Substring(2, __instance.Term.Length - 4);
            __result = literal;
            return false;
        }
        string lang = SettingsManager.Settings.Game.Language;
        if (Localizations.ContainsKey(lang))
        {
            if (Localizations[lang].ContainsKey(__instance.Term))
            {
                __result = Localizations[lang][__instance.Term];
                return false;
            }
        }

        return true;
    }

    [HarmonyPatch(typeof(GameSettings), nameof(GameSettings.OnLanguageChanged))]
    [HarmonyPrefix]
    public static bool GameSettings_OnLanguageChanged(GameSettings __instance, int index)
    {
        if (index >= LanguageUtilities.AllLanguages.Length)
        {
            string text = Languages[index - LanguageUtilities.AllLanguages.Length];
            SettingsManager.Settings.Game.Language = text;
            LocalizationManager.CurrentLanguage = text;
            __instance._cachedLanguage = text;
            if (TwitchAuthentication.IsAuthenticated)
            {
                TwitchRequest.SendEBSData();
            }
            UnityEngine.Debug.Log("GameSettings - Change Language to " + text);
            LocalizationManager.LocalizeAll(true);
            return false;
        }
        LocalizationManager.LocalizeAll(true);
        return true;
    }
    
    [HarmonyPatch(typeof(GameSettings), nameof(GameSettings.GetLanguageIndex))]
    [HarmonyPrefix]
    public static bool GameSettings_GetLanguageIndex(GameSettings __instance, ref int __result)
    {
        __instance._languageSelector._prefilledContent = __instance._languageSelector._prefilledContent.AddRange(Localizations.Keys.ToArray());
        __instance._languageSelector.UpdateContent(__instance._languageSelector._prefilledContent);
        if (Languages.Contains(SettingsManager.Settings.Game.Language))
        {
            __result = LanguageUtilities.AllLanguages.Length + Languages.IndexOf(SettingsManager.Settings.Game.Language);
            return false;
        }
        return true;
    }
    
    [HarmonyPatch(typeof(MMHorizontalSelector), nameof(MMHorizontalSelector.UpdateContent))]
    [HarmonyPrefix]
    public static void MMHorizontalSelector_UpdateContent(MMHorizontalSelector __instance, string[] newContent)
    {
        if (__instance._contentIndex >= newContent.Length)
        {
            __instance._contentIndex = 0;
        }
    }
}