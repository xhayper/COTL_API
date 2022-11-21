using COTL_API.UI;
using Lamb.UI;

namespace COTL_API.CustomSettings;

public static class CustomSettingsManager
{
    internal static List<Slider> Sliders { get; set; } = new();
    internal static List<Dropdown> Dropdowns { get; set; } = new();
    internal static List<Toggle> Toggles { get; set; } = new();
    
    internal static IEnumerable<SettingsElement> SettingsElements => new List<SettingsElement>().Concat(Sliders).Concat(Dropdowns).Concat(Toggles).ToList();
    
    public static void AddSlider(string? category, string text, float value, float min, float max, int increment, MMSlider.ValueDisplayFormat displayFormat, Action<float>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };
        Sliders.Add(new Slider(category, text, value, min, max, increment, displayFormat, onValueChanged));
    }

    public static void AddSavedSlider(string? category, string guid, string text, float value, float min, float max, int increment, MMSlider.ValueDisplayFormat displayFormat, Action<float>? onValueChanged = null)
    {
        if (Plugin.SettingsData == null) return;
        
        var fullGuid = "Settings_" + guid + "_" + text;
        onValueChanged ??= delegate { };
        if (!Plugin.Instance!.APIData.Data!.SettingsData.ContainsKey(fullGuid))
            Plugin.SettingsData.Add(fullGuid, value);
        var slider = new Slider(category, text, (float)Plugin.SettingsData[fullGuid], min, max,
            increment, displayFormat,
            delegate(float newValue)
            {
                if (Plugin.Instance != null)
                {
                    Plugin.SettingsData[fullGuid] = newValue;
                    Plugin.Instance.APIData.Save();
                }

                onValueChanged(newValue);
            });
        Sliders.Add(slider);
    }
    
    public static void AddDropdown(string? category, string text, string? value, string?[] options, Action<int>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };
        Dropdowns.Add(new Dropdown(category, text, value, options, onValueChanged));
    }

    public static void AddSavedDropdown(string? category, string guid, string text, string value, string?[] options, Action<int>? onValueChanged = null)
    {
        if (Plugin.SettingsData == null) return;
        
        var fullGuid = "Settings_" + guid + "_" + text;
        onValueChanged ??= delegate { };
        if (!Plugin.SettingsData.ContainsKey(fullGuid))
            Plugin.SettingsData.Add(fullGuid, value);
        var dropdown = new Dropdown(category, text, (string)Plugin.SettingsData[fullGuid], options,
            delegate(int newValue)
            {
                if (Plugin.Instance != null)
                {
                    Plugin.SettingsData[fullGuid] = options[newValue] ?? string.Empty;
                    Plugin.Instance.APIData.Save();
                }

                onValueChanged(newValue);
            });
        Dropdowns.Add(dropdown);
    }
    
    public static void AddToggle(string? category, string text, bool value, Action<bool>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };
        Toggles.Add(new Toggle(category, text, value, onValueChanged));
    }

    public static void AddSavedToggle(string? category, string guid, string text, bool value, Action<bool>? onValueChanged = null)
    {
        if (Plugin.SettingsData == null) return;
        
        var fullGuid = "Settings_" + guid + "_" + text;
        onValueChanged ??= delegate { };
        if (!Plugin.SettingsData.ContainsKey(fullGuid))
            Plugin.SettingsData.Add(fullGuid, value);
        var toggle = new Toggle(category, text, (bool)Plugin.SettingsData[fullGuid],
            delegate(bool newValue)
            {
                if (Plugin.Instance != null)
                {
                    Plugin.SettingsData[fullGuid] = newValue;
                    Plugin.Instance.APIData.Save();
                }

                onValueChanged(newValue);
            });
        Toggles.Add(toggle);
    }

}