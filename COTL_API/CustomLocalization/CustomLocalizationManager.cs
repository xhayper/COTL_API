using HarmonyLib;

namespace COTL_API.CustomLocalization;

[HarmonyPatch]
public partial class CustomLocalizationManager
{
    public static Dictionary<string, Dictionary<string, string>> LocalizationMap { get; } = new();
    public static List<string> LanguageList { get; } = [];

    /// <summary>
    ///     Loads a localization file from a path.
    /// </summary>
    /// <param name="name">The name of the language.</param>
    /// <param name="path">The path to the localization file.</param>
    public static void LoadLocalization(string name, string path)
    {
        if (!LocalizationMap.ContainsKey(name))
            LocalizationMap.Add(name, new Dictionary<string, string>());

        if (!LanguageList.Contains(name))
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
                    if (line[i] == '"' && (0 >= i || line[i - 1] != '\\') && (1 >= i || line[i - 2] != '\\'))
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

            LogDebug($"Loaded localization: {name}");
        }
        else
        {
            LogError(
                $"Localization file not found! Please make sure that the path \"{path}\" contains the localization file.");
        }
    }
}