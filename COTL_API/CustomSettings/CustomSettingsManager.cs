using COTL_API.CustomSettings.Elements;
using System.Collections.ObjectModel;
using BepInEx.Configuration;
using Lamb.UI;

namespace COTL_API.CustomSettings;

public static class CustomSettingsManager
{
    internal static List<ISettingsElement> SettingsElements { get; } = new();

    internal static ReadOnlyCollection<Slider> Sliders => SettingsElements.OfType<Slider>().ToList().AsReadOnly();
    internal static ReadOnlyCollection<HorizontalSelector> HorizontalSelectors => SettingsElements.OfType<HorizontalSelector>().ToList().AsReadOnly();
    internal static ReadOnlyCollection<Toggle> Toggles => SettingsElements.OfType<Toggle>().ToList().AsReadOnly();

    public static Slider AddSlider(string? category, string text, float value, float min, float max, int increment,
        MMSlider.ValueDisplayFormat displayFormat, Action<float>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };
        var slider = new Slider(category, text, value, min, max, increment, displayFormat, onValueChanged);
        SettingsElements.Add(slider);
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
        SettingsElements.Add(slider);
        return slider;
    }

    public static HorizontalSelector AddHorizontalSelector(string? category, string text, string? value, string?[] options,
        Action<int>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };
        var horizontalSelector = new HorizontalSelector(category, text, value, options, onValueChanged);
        SettingsElements.Add(horizontalSelector);
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
        SettingsElements.Add(horizontalSelector);

        return horizontalSelector;
    }

    public static Toggle AddToggle(string? category, string text, bool value, Action<bool>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };
        var toggle = new Toggle(category, text, value, onValueChanged);
        SettingsElements.Add(toggle);
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
        SettingsElements.Add(toggle);
        return toggle;
    }

    //--- BEPINEX CONFIG BINDING ---//

    // TODO: Improve BepInEx number config

    public static HorizontalSelector? AddBepInExConfig(string? category, string text, ConfigEntry<string> entry, Action<int>? onValueChanged = null)
    {
        if (!(entry.Description.AcceptableValues is AcceptableValueList<string>)) return null;
        onValueChanged ??= delegate { };

        var acceptedValue = ((AcceptableValueList<string>)entry.Description.AcceptableValues);

        var selector = new HorizontalSelector(category, text, entry.Value, acceptedValue.AcceptableValues,
            delegate (int newValue)
            {
                entry.Value = acceptedValue.AcceptableValues[newValue];
                onValueChanged(newValue);
            });

        entry.SettingChanged += delegate (object sender, EventArgs e)
        {
            selector.Value = entry.Value;
        };

        SettingsElements.Add(selector);
        return selector;
    }

    public static Slider? AddBepInExConfig(string? category, string text, ConfigEntry<float> entry, int increment, MMSlider.ValueDisplayFormat displayFormat, Action<float>? onValueChanged = null)
    {
        if (!(entry.Description.AcceptableValues is AcceptableValueRange<float>)) return null;
        onValueChanged ??= delegate { };

        var acceptedValue = ((AcceptableValueRange<float>)entry.Description.AcceptableValues);

        var slider = new Slider(category, text, entry.Value, acceptedValue.MinValue, acceptedValue.MaxValue, increment, displayFormat,
            delegate (float newValue)
            {
                entry.Value = newValue;
                onValueChanged(newValue);
            });

        entry.SettingChanged += delegate (object sender, EventArgs e)
        {
            slider.Value = entry.Value;
        };

        SettingsElements.Add(slider);
        return slider;
    }

    public static Slider? AddBepInExConfig(string? category, string text, ConfigEntry<int> entry, int increment, MMSlider.ValueDisplayFormat displayFormat, Action<int>? onValueChanged = null)
    {
        if (!(entry.Description.AcceptableValues is AcceptableValueRange<int>)) return null;
        onValueChanged ??= delegate { };

        var acceptedValue = ((AcceptableValueRange<int>)entry.Description.AcceptableValues);

        var slider = new Slider(category, text, entry.Value, acceptedValue.MinValue, acceptedValue.MaxValue, increment, displayFormat,
            delegate (float newValue)
            {
                int newVal = (int)Math.Floor(newValue);

                entry.Value = newVal;
                onValueChanged(newVal);
            });

        entry.SettingChanged += delegate (object sender, EventArgs e)
        {
            slider.Value = entry.Value;
        };

        SettingsElements.Add(slider);
        return slider;
    }

    public static Toggle AddBepInExConfig(string? category, string text, ConfigEntry<bool> entry, Action<bool>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };

        var toggle = new Toggle(category, text, entry.Value,
            delegate (bool newValue)
            {
                entry.BoxedValue = newValue;
                onValueChanged(newValue);
            });

        entry.SettingChanged += delegate (object sender, EventArgs e)
        {
            toggle.Value = entry.Value;
        };

        SettingsElements.Add(toggle);
        return toggle;
    }
}