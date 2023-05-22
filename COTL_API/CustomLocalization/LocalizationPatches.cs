using COTL_API.CustomLocalization;

namespace COTL_API.Localization;

[Obsolete("Use CustomLocalizationManager instead.")]
public static class LocalizationPatches
{
    [Obsolete("Use CustomLocalizationManager.LocalizationMap instead.")]
    public static Dictionary<string, Dictionary<string, string>> LocalizationMap =>
        CustomLocalizationManager.LocalizationMap;

    [Obsolete("Use CustomLocalizationManager.LanguageList instead.")]
    public static List<string> LanguageList => CustomLocalizationManager.LanguageList;

    /// <summary>
    ///     Loads a localization file from a path.
    /// </summary>
    /// <param name="name">The name of the language.</param>
    /// <param name="path">The path to the localization file.</param>
    [Obsolete("Use CustomLocalizationManager.LoadLocalization instead.")]
    public static void LoadLocalization(string name, string path)
    {
        CustomLocalizationManager.LoadLocalization(name, path);
    }
}