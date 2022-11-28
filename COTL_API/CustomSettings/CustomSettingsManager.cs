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

    //--- BEPINEX CONFIG BINDING ---//

    public static HorizontalSelector? AddBepInExConfig(string modName, ConfigEntry<string> entry, Action<int>? onValueChanged = null)
    {
        if (!(entry.Description.AcceptableValues is AcceptableValueList<string>)) return null;

        onValueChanged ??= delegate { };

        var acceptedValue = ((AcceptableValueList<string>)entry.Description.AcceptableValues);

        var selector = new HorizontalSelector($"{modName}'s {entry.Definition.Section}", entry.Definition.Key, entry.Value, acceptedValue.AcceptableValues,
            delegate (int newValue)
            {
                entry.Value = acceptedValue.AcceptableValues[newValue];
                onValueChanged(newValue);
            });

        entry.SettingChanged += delegate (object sender, EventArgs e)
        {
            selector.Value = entry.Value;
        };

        HorizontalSelectors.Add(selector);
        return selector;
    }

    public static Slider? AddBepInExConfig(string modName, ConfigEntry<float> entry, int increment, MMSlider.ValueDisplayFormat displayFormat, Action<float>? onValueChanged = null)
    {
        if (!(entry.Description.AcceptableValues is AcceptableValueRange<float>)) return null;

        onValueChanged ??= delegate { };

        var acceptedValue = ((AcceptableValueRange<float>)entry.Description.AcceptableValues);

        var slider = new Slider($"{modName}'s {entry.Definition.Section}", entry.Definition.Key, entry.Value, acceptedValue.MinValue, acceptedValue.MaxValue, increment, displayFormat,
            delegate (float newValue)
            {
                entry.Value = newValue;
                onValueChanged?.Invoke(newValue);
            });

        entry.SettingChanged += delegate (object sender, EventArgs e)
        {
            slider.Value = entry.Value;
        };

        Sliders.Add(slider);
        return slider;
    }

    public static Toggle AddBepInExConfig(string modName, ConfigEntry<bool> entry, Action<bool>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };

        var toggle = new Toggle($"{modName}'s {entry.Definition.Section}", entry.Definition.Key, entry.Value,
            delegate (bool newValue)
            {
                entry.BoxedValue = newValue;
                onValueChanged?.Invoke(newValue);
            });

        entry.SettingChanged += delegate (object sender, EventArgs e)
        {
            toggle.Value = entry.Value;
        };

        Toggles.Add(toggle);
        return toggle;
    }
}