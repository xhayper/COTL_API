using COTL_API.Saves;
using COTL_API.UI;
using Lamb.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace COTL_API.CustomSettings;

public static class CustomSettingsManager
{
    internal static List<Slider> Sliders { get; set; } = new();
    internal static List<Dropdown> Dropdowns { get; set; } = new();
    internal static List<Toggle> Toggles { get; set; } = new();
    internal static List<SettingsElement> SettingsElements => new List<SettingsElement>().Concat(Sliders).Concat(Dropdowns).Concat(Toggles).ToList();
    
    public static void AddSlider(string category, string text, float value, float min, float max, int increment, MMSlider.ValueDisplayFormat displayFormat, Action<float> onValueChanged = null)
    {
        onValueChanged ??= delegate { };
        Sliders.Add(new Slider(category, text, value, min, max, increment, displayFormat, onValueChanged));
    }

    public static void AddSavedSlider(string category, string guid, string text, float value, float min, float max, int increment, MMSlider.ValueDisplayFormat displayFormat, Action<float> onValueChanged = null)
    {
        string fullGuid = "Settings_" + guid + "_" + text;
        onValueChanged ??= delegate { };
        if (!SettingsDataManager.SettingsData.Data.ContainsKey(fullGuid))
            SettingsDataManager.SettingsData.Data.Add(fullGuid, value);
        Slider slider = new Slider(category, text, (float)SettingsDataManager.SettingsData.Data[fullGuid], min, max,
            increment, displayFormat,
            delegate(float newValue)
            {
                SettingsDataManager.SettingsData.Data[fullGuid] = newValue;
                SettingsDataManager.Save();
                onValueChanged(newValue);
            });
        Sliders.Add(slider);
    }
    
    public static void AddDropdown(string category, string text, string value, string[] options, Action<int> onValueChanged = null)
    {
        onValueChanged ??= delegate { };
        Dropdowns.Add(new Dropdown(category, text, value, options, onValueChanged));
    }

    public static void AddSavedDropdown(string category, string guid, string text, string value, string[] options, Action<int> onValueChanged = null)
    {
        string fullGuid = "Settings_" + guid + "_" + text;
        onValueChanged ??= delegate { };
        if (!SettingsDataManager.SettingsData.Data.ContainsKey(fullGuid))
            SettingsDataManager.SettingsData.Data.Add(fullGuid, value);
        Dropdown dropdown = new Dropdown(category, text, (string)SettingsDataManager.SettingsData.Data[fullGuid], options,
            delegate(int newValue)
            {
                SettingsDataManager.SettingsData.Data[fullGuid] = options[newValue];
                SettingsDataManager.Save();
                onValueChanged(newValue);
            });
        Dropdowns.Add(dropdown);
    }
    
    public static void AddToggle(string category, string text, bool value, Action<bool> onValueChanged = null)
    {
        onValueChanged ??= delegate { };
        Toggles.Add(new Toggle(category, text, value, onValueChanged));
    }

    public static void AddSavedToggle(string category, string guid, string text, bool value, Action<bool> onValueChanged = null)
    {
        string fullGuid = "Settings_" + guid + "_" + text;
        onValueChanged ??= delegate { };
        if (!SettingsDataManager.SettingsData.Data.ContainsKey(fullGuid))
            SettingsDataManager.SettingsData.Data.Add(fullGuid, value);
        Toggle toggle = new Toggle(category, text, (bool)SettingsDataManager.SettingsData.Data[fullGuid],
            delegate(bool newValue)
            {
                SettingsDataManager.SettingsData.Data[fullGuid] = newValue;
                SettingsDataManager.Save();
                onValueChanged(newValue);
            });
        Toggles.Add(toggle);
    }

}