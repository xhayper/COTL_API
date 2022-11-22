using COTL_API.CustomSettings.Elements;
using Lamb.UI;

namespace COTL_API.CustomSettings;

public static class CustomSettingsManager
{
    internal static List<Slider> Sliders { get; set; } = new();
    internal static List<Dropdown> Dropdowns { get; set; } = new();
    internal static List<Toggle> Toggles { get; set; } = new();

    internal static IEnumerable<ISettingsElement> SettingsElements =>
        Sliders.Cast<ISettingsElement>().Concat(Dropdowns).Concat(Toggles);

    public static Slider AddSlider(string? category, string text, float value, float min, float max, int increment,
        MMSlider.ValueDisplayFormat displayFormat, Action<float>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };
        var slider = new Slider(category, text, value, min, max, increment, displayFormat, onValueChanged);
        Sliders.Add(slider);
        return slider;
    }

    public static Slider? AddSavedSlider(string? category, string guid, string text, float value, float min, float max,
        int increment, MMSlider.ValueDisplayFormat displayFormat, Action<float>? onValueChanged = null)
    {
        if (Plugin.SettingsData == null) return null;

        var fullGuid = $"{guid}.{text}";
        onValueChanged ??= delegate { };
        if (!Plugin.SettingsData.ContainsKey(fullGuid))
            Plugin.SettingsData.Add(fullGuid, value);
        var slider = new Slider(category, text, Plugin.SettingsData.GetValueAsFloat(fullGuid), min, max,
            increment, displayFormat,
            delegate(float newValue)
            {
                if (Plugin.Instance != null)
                {
                    Plugin.SettingsData.SetValue(fullGuid, newValue);
                    Plugin.Instance.ModdedSettings.Save();
                }

                onValueChanged(newValue);
            });
        Sliders.Add(slider);
        return slider;
    }

    public static Dropdown AddDropdown(string? category, string text, string? value, string?[] options,
        Action<int>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };
        var dropdown = new Dropdown(category, text, value, options, onValueChanged);
        Dropdowns.Add(dropdown);
        return dropdown;
    }

    public static Dropdown? AddSavedDropdown(string? category, string guid, string text, string value, string?[] options,
        Action<int>? onValueChanged = null)
    {
        if (Plugin.SettingsData == null) return null;

        var fullGuid = $"{guid}.{text}";
        onValueChanged ??= delegate { };

        if (!Plugin.SettingsData.ContainsKey(fullGuid))
            Plugin.SettingsData.Add(fullGuid, value);
        var dropdown = new Dropdown(category, text, Plugin.SettingsData.GetValueAsString(fullGuid), options,
            delegate(int newValue)
            {
                if (Plugin.Instance != null)
                {
                    Plugin.SettingsData.SetValue(fullGuid, options[newValue]);
                    Plugin.Instance.ModdedSettings.Save();
                }
                
                onValueChanged(newValue);
            });
        Dropdowns.Add(dropdown);
        return dropdown;
    }

    public static Toggle AddToggle(string? category, string text, bool value, Action<bool>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };
        var toggle = new Toggle(category, text, value, onValueChanged);
        Toggles.Add(toggle);
        return toggle;
    }

    public static Toggle? AddSavedToggle(string? category, string guid, string text, bool value,
        Action<bool>? onValueChanged = null)
    {
        if (Plugin.SettingsData == null) return null;

        var fullGuid = $"Settings.{guid}.{text}";
        onValueChanged ??= delegate { };
        if (!Plugin.SettingsData.ContainsKey(fullGuid))
            Plugin.SettingsData.Add(fullGuid, value);
        var toggle = new Toggle(category, text, Plugin.SettingsData.GetValueAsBoolean(fullGuid),
            delegate(bool newValue)
            {
                if (Plugin.Instance != null)
                {
                    Plugin.SettingsData.SetValue(fullGuid, newValue);
                    Plugin.Instance.ModdedSettings.Save();
                }

                onValueChanged(newValue);
            });
        Toggles.Add(toggle);
        return toggle;
    }
}