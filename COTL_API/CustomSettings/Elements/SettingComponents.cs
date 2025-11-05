using BepInEx.Configuration;
using Lamb.UI;
using UnityEngine;

namespace COTL_API.CustomSettings.Elements;

public interface ISettingsElement
{
    string? Category { get; }
    string Text { get; }
}

public class Slider(
    string? category,
    string text,
    float value,
    float min,
    float max,
    int increment,
    MMSlider.ValueDisplayFormat displayFormat,
    Action<float>? onValueChanged)
    : ISettingsElement
{
    public string? Category { get; set; } = category;

    public string Text { get; set; } = text;

    public float Value { get; set; } = value;
    public float Min { get; set; } = min;
    public float Max { get; set; } = max;
    public int Increment { get; set; } = increment;
    public MMSlider.ValueDisplayFormat DisplayFormat { get; set; } = displayFormat;

    public Action<float>? OnValueChanged { get; set; } = onValueChanged;

    string? ISettingsElement.Category => Category;

    string ISettingsElement.Text => Text;
}

public class Dropdown(string? category, string text, string? value, string?[] options, Action<int>? onValueChanged)
    : ISettingsElement
{
    public string? Category { get; set; } = category;

    public string Text { get; set; } = text;

    public string? Value { get; set; } = value;
    public string?[] Options { get; set; } = options;
    public Action<int>? OnValueChanged { get; set; } = onValueChanged;

    string? ISettingsElement.Category => Category;

    string ISettingsElement.Text => Text;
}

public class KeyboardShortcutDropdown(
    string? category,
    string text,
    KeyCode? value,
    Action<KeyboardShortcut>? onValueChanged)
    : ISettingsElement
{
    public KeyCode? Value { get; set; } = value;
    public Action<KeyboardShortcut>? OnValueChanged { get; set; } = onValueChanged;
    public string? Category { get; set; } = category;
    public string Text { get; set; } = text;
}

public class HorizontalSelector(
    string? category,
    string text,
    string? value,
    string?[] options,
    Action<int>? onValueChanged)
    : ISettingsElement
{
    public string? Category { get; set; } = category;

    public string Text { get; set; } = text;

    public string? Value { get; set; } = value;
    public string?[] Options { get; set; } = options;
    public Action<int>? OnValueChanged { get; set; } = onValueChanged;

    string? ISettingsElement.Category => Category;

    string ISettingsElement.Text => Text;
}

public class Toggle(string? category, string text, bool value, Action<bool>? onValueChanged)
    : ISettingsElement
{
    public string? Category { get; set; } = category;

    public string Text { get; set; } = text;

    public bool Value { get; set; } = value;
    public Action<bool>? OnValueChanged { get; set; } = onValueChanged;

    string? ISettingsElement.Category => Category;

    string ISettingsElement.Text => Text;
}