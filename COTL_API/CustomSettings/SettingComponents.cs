using Lamb.UI;

namespace COTL_API.UI;

internal interface ISettingsElement
{
    internal string? Category { get; }
    internal string Text { get; }
}

internal class Slider : ISettingsElement
{
    internal string? Category { get; set; }

    string? ISettingsElement.Category => this.Category;

    internal string Text { get; set; }

    string ISettingsElement.Text => this.Text;

    internal float Value { get; set; }
    internal float Min { get; set; }
    internal float Max { get; set; }
    internal int Increment { get; set; }
    internal MMSlider.ValueDisplayFormat DisplayFormat { get; set; }
    
    internal Action<float>? OnValueChanged { get; set; }
    
    internal Slider(string? category, string text, float value, float min, float max, int increment, MMSlider.ValueDisplayFormat displayFormat, Action<float>? onValueChanged)
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
}

internal class Dropdown : ISettingsElement
{
    internal string? Category { get; set; }

    string? ISettingsElement.Category => this.Category;

    internal string Text { get; set; }

    string ISettingsElement.Text => this.Text;

    internal string? Value { get; set; }
    internal string?[] Options { get; set; }
    internal Action<int>? OnValueChanged { get; set; }
    
    internal Dropdown(string? category, string text, string? value, string?[] options, Action<int>? onValueChanged)
    {
        Category = category;
        Text = text;
        Value = value;
        Options = options;
        OnValueChanged = onValueChanged;
    }
}

internal class Toggle : ISettingsElement
{
    internal string? Category { get; set; }

    string? ISettingsElement.Category => this.Category;

    internal string Text { get; set; }

    string ISettingsElement.Text => this.Text;

    internal bool Value { get; set; }
    internal Action<bool>? OnValueChanged { get; set; }
    
    internal Toggle(string? category, string text, bool value, Action<bool>? onValueChanged)
    {
        Category = category;
        Text = text;
        Value = value;
        OnValueChanged = onValueChanged;
    }
}