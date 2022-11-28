using COTL_API.CustomSettings.Elements;
using BepInEx.Configuration;
using Lamb.UI;

namespace COTL_API.CustomSettings;

public static class CustomSettingsManager
{
    internal static List<Slider> Sliders { get; } = new();
    internal static List<HorizontalSelector> HorizontalSelectors { get; } = new();
    internal static List<Toggle> Toggles { get; } = new();

    internal static IEnumerable<ISettingsElement> SettingsElements =>
        Sliders.Cast<ISettingsElement>().Concat(HorizontalSelectors).Concat(Toggles);

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

        var fullGuid = $"{guid}.{category}.{text}";
        onValueChanged ??= delegate { };
        if (!Plugin.SettingsData.ContainsKey(fullGuid))
            Plugin.SettingsData.Add(fullGuid, value);
        var slider = new Slider(category, text, Plugin.SettingsData.GetValueAsFloat(fullGuid), min, max,
            increment, displayFormat,
            delegate (float newValue)
            {
                if (Plugin.Instance != null)
                {
                    Plugin.SettingsData.SetValue(fullGuid, newValue);
                    Plugin.Instance.ModdedSettingsData.Save();
                }

                onValueChanged(newValue);
            });
        Sliders.Add(slider);
        return slider;
    }

    public static HorizontalSelector AddHorizontalSelector(string? category, string text, string? value, string?[] options,
        Action<int>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };
        var horizontalSelector = new HorizontalSelector(category, text, value, options, onValueChanged);
        HorizontalSelectors.Add(horizontalSelector);
        return horizontalSelector;
    }

    public static HorizontalSelector? AddSavedHorizontalSelector(string? category, string guid, string text, string value, string?[] options,
        Action<int>? onValueChanged = null)
    {
        if (Plugin.SettingsData == null) return null;

        var fullGuid = $"{guid}.{category}.{text}";
        onValueChanged ??= delegate { };

        if (!Plugin.SettingsData.ContainsKey(fullGuid))
            Plugin.SettingsData.Add(fullGuid, value);
        var horizontalSelector = new HorizontalSelector(category, text, Plugin.SettingsData.GetValueAsString(fullGuid), options, null);
        horizontalSelector.OnValueChanged = delegate (int newValue)
        {
            if (Plugin.Instance != null)
            {
                Plugin.SettingsData.SetValue(fullGuid, horizontalSelector.Options[newValue]);
                Plugin.Instance.ModdedSettingsData.Save();
            }

            onValueChanged(newValue);
        };
        HorizontalSelectors.Add(horizontalSelector);



        return horizontalSelector;
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

        var fullGuid = $"Settings.{guid}.{category}.{text}";
        onValueChanged ??= delegate { };
        if (!Plugin.SettingsData.ContainsKey(fullGuid))
            Plugin.SettingsData.Add(fullGuid, value);
        var toggle = new Toggle(category, text, Plugin.SettingsData.GetValueAsBoolean(fullGuid),
            delegate (bool newValue)
            {
                if (Plugin.Instance != null)
                {
                    Plugin.SettingsData.SetValue(fullGuid, newValue);
                    Plugin.Instance.ModdedSettingsData.Save();
                }

                onValueChanged(newValue);
            });
        Toggles.Add(toggle);
        return toggle;
    }

    public static ISettingsElement? AddBepInExConfig<T>(string modName, ConfigEntry<T> entry, Action<T>? onValueChanged = null)
    {
        if (typeof(T) == typeof(bool))
        {
            onValueChanged ??= delegate { };

            var toggle = new Toggle($"{modName}'s {entry.Definition.Section}", entry.Definition.Key, (bool)Convert.ChangeType(entry.Value, typeof(bool)),
                delegate (bool newValue)
                {
                    T val = (T)Convert.ChangeType(newValue, typeof(T));
                    entry.Value = val;
                    onValueChanged?.Invoke(val);
                });

            entry.SettingChanged += delegate (object sender, EventArgs e)
            {
                toggle.Value = (bool)Convert.ChangeType(entry.Value, typeof(bool));
            };

            Toggles.Add(toggle);
            return toggle;
        }

        return null;
    }
}