---
title: Settings
description: Documentation on how to add custom settings using the Cult of the Lamb API
---

## Creating Settings

There are three diffent type of settings: Sliders, Dropdowns, HorizontalSelectors, and Toggles.  
**Sliders** have a range of numbers.  
**Dropdowns** have an array of strings.
**HorizontalSelectors** have an array of strings.  
**Toggles** have a boolean.

To create a setting, you use the respective method in `CustomSettingsManager`:

```csharp
AddSlider(string? category, string text, float value, float min, float max,
    int increment, MMSlider.ValueDisplayFormat displayFormat, Action<float>? onValueChanged = null)

AddSavedSlider(string? category, string guid, string text, float value, float min, float max,
    int increment, MMSlider.ValueDisplayFormat displayFormat, Action<float>? onValueChanged = null)

AddDropdown(string? category, string text, string? value, string?[] options,
    Action<int>? onValueChanged = null)

AddSavedDropdown(string? category, string guid, string text, string value, string?[] options,
    Action<int>? onValueChanged = null)

AddHorizontalSelector(string? category, string text, string? value, string?[] options,
    Action<int>? onValueChanged = null)

AddSavedHorizontalSelector(string? category, string guid, string text, string value, string?[] options,
    Action<int>? onValueChanged = null)

AddHorizontalSelector(string? category, string text, string? value, string?[] options,
    Action<int>? onValueChanged = null)

AddSavedHorizontalSelector(string? category, string guid, string text, string value, string?[] options,
    Action<int>? onValueChanged = null)

AddToggle(string? category, string text, bool value, Action<bool>? onValueChaginged = null)

AddSavedToggle(string? category, string guid, string text, bool value,
        Action<bool>? onValueChanged = null)

AddBepInExConfig(string? category, string text, ConfigEntry<string> entry, Action<int>? onValueChanged = null)

AddBepInExConfig(string? category, string text, ConfigEntry<float> entry, int increment, MMSlider.ValueDisplayFormat displayFormat, Action<float>? onValueChanged = null)

AddBepInExConfig(string? category, string text, ConfigEntry<int> entry, int increment, MMSlider.ValueDisplayFormat displayFormat, Action<int>? onValueChanged = null)

AddBepInExConfig(string? category, string text, ConfigEntry<bool> entry, Action<bool>? onValueChanged = null)
```

'Saved' settings are automatically saved in the save file (`modded_settings.json`).<br>
'BepInEx" settings are automatically bined to ConfigEntry, when you update it in-game, the BepInEx config file also update.

**Universal Parameters:**  
`category` determines the header under which the settings appears.  
`text` is the visual name of the setting.  
`value` is the default value of the setting.  
`onValueChanged` is the callback that runs when the setting is changed.

**Saved Setting Parameters:**  
`guid` is the unique identifier prefixed to the category and name. This should usually be the GUID of your plugin. `guid`.`category`.`name` should be unique for all settings across all mods.

**Slider Parameters:**  
`min` and `max` are the minimum and maximum values of the slider.  
`increment` is the increment at which the slider changes when dragged.  
`displayFormat` determines how the text is displayed. It can be either `Percentage` or `RawValue`.

**Dropdown Parameters:**  
`options` is the list of options to choose from.

**Horizontal Selector Parameters:**  
`options` is the list of options to choose from.

## Dynamic settings

If you want a setting that can have different options at runtime, you can add a delegate to `UIManager.OnSettingsLoaded` that updates the setting.  
Example:

```csharp
var selector = CustomSettingsManager.AddSavedHorizontalSelector(...)
UIManager.OnSettingsLoaded += delegate () =>
{
    selector!.Options = ...
};
```
