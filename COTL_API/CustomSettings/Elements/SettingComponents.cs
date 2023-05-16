using BepInEx.Configuration;
using Lamb.UI;
using UnityEngine;

namespace COTL_API.CustomSettings.Elements;

public interface ISettingsElement
{
    public string? Category { get; }
    public string Text { get; }
}

public class Slider : ISettingsElement
{
    public Slider(string? category, string text, float value, float min, float max, int increment,
        MMSlider.ValueDisplayFormat displayFormat, Action<float>? onValueChanged)
    {
        Category = category;
        Text = text;
        Value = value;
        Min = min;
        Max = max;
        Increment = increment;
        DisplayFormat = displayFormat;
        OnValueChanged = onValueChanged;
    }

    public string? Category { get; set; }

    public string Text { get; set; }

    public float Value { get; set; }
    public float Min { get; set; }
    public float Max { get; set; }
    public int Increment { get; set; }
    public MMSlider.ValueDisplayFormat DisplayFormat { get; set; }

    public Action<float>? OnValueChanged { get; set; }

    string? ISettingsElement.Category => Category;

    string ISettingsElement.Text => Text;
}

public class Dropdown : ISettingsElement
{
    public Dropdown(string? category, string text, string? value, string?[] options, Action<int>? onValueChanged)
    {
        Category = category;
        Text = text;
        Value = value;
        Options = options;
        OnValueChanged = onValueChanged;
    }

    public string? Category { get; set; }

    public string Text { get; set; }

    public string? Value { get; set; }
    public string?[] Options { get; set; }
    public Action<int>? OnValueChanged { get; set; }

    string? ISettingsElement.Category => Category;

    string ISettingsElement.Text => Text;
}

public class KeyboardShortcutDropdown : ISettingsElement
{
    public KeyboardShortcutDropdown(string? category, string text, KeyCode? value,
        Action<KeyboardShortcut>? onValueChanged)
    {
        Category = category;
        Text = text;
        Value = value;
        OnValueChanged = onValueChanged;
    }

    public KeyCode? Value { get; set; }
    public Action<KeyboardShortcut>? OnValueChanged { get; set; }
    public string? Category { get; set; }
    public string Text { get; set; }
}

public class HorizontalSelector : ISettingsElement
{
    public HorizontalSelector(string? category, string text, string? value, string?[] options,
        Action<int>? onValueChanged)
    {
        Category = category;
        Text = text;
        Value = value;
        Options = options;
        OnValueChanged = onValueChanged;
    }

    public string? Category { get; set; }

    public string Text { get; set; }

    public string? Value { get; set; }
    public string?[] Options { get; set; }
    public Action<int>? OnValueChanged { get; set; }

    string? ISettingsElement.Category => Category;

    string ISettingsElement.Text => Text;
}

public class Toggle : ISettingsElement
{
    public Toggle(string? category, string text, bool value, Action<bool>? onValueChanged)
    {
        Category = category;
        Text = text;
        Value = value;
        OnValueChanged = onValueChanged;
    }

    public string? Category { get; set; }

    public string Text { get; set; }

    public bool Value { get; set; }
    public Action<bool>? OnValueChanged { get; set; }

    string? ISettingsElement.Category => Category;

    string ISettingsElement.Text => Text;
}