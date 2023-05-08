using System.Collections.ObjectModel;
using BepInEx.Configuration;
using COTL_API.CustomSettings.Elements;
using Lamb.UI;
using UnityEngine;

namespace COTL_API.CustomSettings;

public static class CustomSettingsManager
{
    internal static List<ISettingsElement> SettingsElements { get; } = new();

    internal static ReadOnlyCollection<Slider> Sliders => SettingsElements.OfType<Slider>().ToList().AsReadOnly();
    internal static ReadOnlyCollection<Dropdown> Dropdowns => SettingsElements.OfType<Dropdown>().ToList().AsReadOnly();

    internal static ReadOnlyCollection<HorizontalSelector> HorizontalSelectors =>
        SettingsElements.OfType<HorizontalSelector>().ToList().AsReadOnly();

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
            delegate(float newValue)
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

    public static KeyboardShortcutDropdown AddKeyboardShortcutDropdown(string? category, string text,
        ConfigEntry<KeyboardShortcut> entry,
        Action<KeyboardShortcut>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };
        var dropdown = new KeyboardShortcutDropdown(category, text, entry.Value.MainKey,
            (Enum.GetValues(typeof(KeyCode)) as KeyCode?[])!, onValueChanged);
        SettingsElements.Add(dropdown);
        return dropdown;
    }

    public static Dropdown AddDropdown(string? category, string text, string? value, string?[] options,
        Action<int>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };
        var dropdown = new Dropdown(category, text, value, options, onValueChanged);
        SettingsElements.Add(dropdown);
        return dropdown;
    }

    public static KeyboardShortcutDropdown? AddSavedKeyboardShortcutDropdown(string? category, string guid, string text,
        string value,
        KeyCode?[] options,
        Action<KeyboardShortcut>? onValueChanged = null)
    {
        if (Plugin.SettingsData == null) return null;

        var fullGuid = $"{guid}.{category}.{text}";
        onValueChanged ??= delegate { };

        if (!Plugin.SettingsData.ContainsKey(fullGuid))
            Plugin.SettingsData.Add(fullGuid, value);

        // Convert the string value stored in Plugin.SettingsData to KeyCode
        Enum.TryParse(Plugin.SettingsData.GetValueAsString(fullGuid), out KeyCode storedKeyCode);

        var dropdown = new KeyboardShortcutDropdown(category, text, storedKeyCode, options, onValueChanged)
        {
            OnValueChanged = delegate(KeyboardShortcut newValue)
            {
                if (Plugin.Instance != null)
                {
                    Plugin.SettingsData.SetValue(fullGuid, newValue.ToString());
                    Plugin.Instance.ModdedSettingsData.Save();
                }

                // Call the onValueChanged action with the newValue
                onValueChanged(newValue);
            }
        };
        SettingsElements.Add(dropdown);

        return dropdown;
    }


    public static Dropdown? AddSavedDropdown(string? category, string guid, string text, string value,
        string?[] options,
        Action<int>? onValueChanged = null)
    {
        if (Plugin.SettingsData == null) return null;

        var fullGuid = $"{guid}.{category}.{text}";
        onValueChanged ??= delegate { };

        if (!Plugin.SettingsData.ContainsKey(fullGuid))
            Plugin.SettingsData.Add(fullGuid, value);
        var dropdown = new Dropdown(category, text, Plugin.SettingsData.GetValueAsString(fullGuid), options, null);
        dropdown.OnValueChanged = delegate(int newValue)
        {
            if (Plugin.Instance != null)
            {
                Plugin.SettingsData.SetValue(fullGuid, dropdown.Options[newValue]);
                Plugin.Instance.ModdedSettingsData.Save();
            }

            onValueChanged(newValue);
        };
        SettingsElements.Add(dropdown);

        return dropdown;
    }

    public static HorizontalSelector AddHorizontalSelector(string? category, string text, string? value,
        string?[] options,
        Action<int>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };
        var horizontalSelector = new HorizontalSelector(category, text, value, options, onValueChanged);
        SettingsElements.Add(horizontalSelector);
        return horizontalSelector;
    }

    public static HorizontalSelector? AddSavedHorizontalSelector(string? category, string guid, string text,
        string value, string?[] options,
        Action<int>? onValueChanged = null)
    {
        if (Plugin.SettingsData == null) return null;

        var fullGuid = $"{guid}.{category}.{text}";
        onValueChanged ??= delegate { };

        if (!Plugin.SettingsData.ContainsKey(fullGuid))
            Plugin.SettingsData.Add(fullGuid, value);
        var horizontalSelector = new HorizontalSelector(category, text, Plugin.SettingsData.GetValueAsString(fullGuid),
            options, null);
        horizontalSelector.OnValueChanged = delegate(int newValue)
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
            delegate(bool newValue)
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

    public static KeyboardShortcutDropdown AddBepInExConfig(string? category, string text,
        ConfigEntry<KeyboardShortcut> entry,
        Action<KeyboardShortcut>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };

        var dropdown = new KeyboardShortcutDropdown(category, text, entry.Value.MainKey,
            (Enum.GetValues(typeof(KeyCode)) as KeyCode?[])!,
            delegate(KeyboardShortcut newKeyCode)
            {
                entry.Value = newKeyCode;
                onValueChanged(entry.Value);
            });

        entry.SettingChanged += delegate { dropdown.Value = entry.Value.MainKey; };

        SettingsElements.Add(dropdown);
        return dropdown;
    }

    public static HorizontalSelector? AddBepInExConfig(string? category, string text, ConfigEntry<string> entry,
        Action<int>? onValueChanged = null)
    {
        if (entry.Description.AcceptableValues is not AcceptableValueList<string> acceptedValue) return null;
        onValueChanged ??= delegate { };

        var selector = new HorizontalSelector(category, text, entry.Value, acceptedValue.AcceptableValues,
            delegate(int newValue)
            {
                entry.Value = acceptedValue.AcceptableValues[newValue];
                onValueChanged(newValue);
            });

        entry.SettingChanged += delegate { selector.Value = entry.Value; };

        SettingsElements.Add(selector);
        return selector;
    }

    public static Slider? AddBepInExConfig(string? category, string text, ConfigEntry<float> entry, int increment,
        MMSlider.ValueDisplayFormat displayFormat, Action<float>? onValueChanged = null)
    {
        if (entry.Description.AcceptableValues is not AcceptableValueRange<float> acceptedValue) return null;
        onValueChanged ??= delegate { };

        var slider = new Slider(category, text, entry.Value, acceptedValue.MinValue, acceptedValue.MaxValue, increment,
            displayFormat,
            delegate(float newValue)
            {
                entry.Value = newValue;
                onValueChanged(newValue);
            });

        entry.SettingChanged += delegate { slider.Value = entry.Value; };

        SettingsElements.Add(slider);
        return slider;
    }

    public static Slider? AddBepInExConfig(string? category, string text, ConfigEntry<int> entry, int increment,
        MMSlider.ValueDisplayFormat displayFormat, Action<int>? onValueChanged = null)
    {
        if (entry.Description.AcceptableValues is not AcceptableValueRange<int> acceptedValue) return null;
        onValueChanged ??= delegate { };

        var slider = new Slider(category, text, entry.Value, acceptedValue.MinValue, acceptedValue.MaxValue, increment,
            displayFormat,
            delegate(float newValue)
            {
                var newVal = (int)Math.Floor(newValue);

                entry.Value = newVal;
                onValueChanged(newVal);
            });

        entry.SettingChanged += delegate { slider.Value = entry.Value; };

        SettingsElements.Add(slider);
        return slider;
    }

    public static Toggle AddBepInExConfig(string? category, string text, ConfigEntry<bool> entry,
        Action<bool>? onValueChanged = null)
    {
        onValueChanged ??= delegate { };

        var toggle = new Toggle(category, text, entry.Value,
            delegate(bool newValue)
            {
                entry.BoxedValue = newValue;
                onValueChanged(newValue);
            });

        entry.SettingChanged += delegate { toggle.Value = entry.Value; };

        SettingsElements.Add(toggle);
        return toggle;
    }
}