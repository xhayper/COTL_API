using Lamb.UI;

namespace COTL_API.CustomSettings.Elements;

public interface ISettingsElement
{
    public string? Category { get; }
    public string Text { get; }
}

public class Slider : ISettingsElement
{
    public string? Category { get; set; }

    string? ISettingsElement.Category => Category;

    public string Text { get; set; }

    string ISettingsElement.Text => Text;

    public float Value { get; set; }
    public float Min { get; set; }
    public float Max { get; set; }
    public int Increment { get; set; }
    public MMSlider.ValueDisplayFormat DisplayFormat { get; set; }

    public Action<float>? OnValueChanged { get; set; }

    public Slider(string? category, string text, float value, float min, float max, int increment, MMSlider.ValueDisplayFormat displayFormat, Action<float>? onValueChanged)
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


public class Dropdown : ISettingsElement
{
    public string? Category { get; set; }

    string? ISettingsElement.Category => Category;

    public string Text { get; set; }

    string ISettingsElement.Text => Text;

    public string? Value { get; set; }
    public string?[] Options { get; set; }
    public Action<int>? OnValueChanged { get; set; }

    public Dropdown(string? category, string text, string? value, string?[] options, Action<int>? onValueChanged)
    {
        Category = category;
        Text = text;
        Value = value;
        Options = options;
        OnValueChanged = onValueChanged;
    }
}

public class HorizontalSelector : ISettingsElement
{
    public string? Category { get; set; }

    string? ISettingsElement.Category => Category;

    public string Text { get; set; }

    string ISettingsElement.Text => Text;

    public string? Value { get; set; }
    public string?[] Options { get; set; }
    public Action<int>? OnValueChanged { get; set; }

    public HorizontalSelector(string? category, string text, string? value, string?[] options, Action<int>? onValueChanged)
    {
        Category = category;
        Text = text;
        Value = value;
        Options = options;
        OnValueChanged = onValueChanged;
    }
}

public class Toggle : ISettingsElement
{
    public string? Category { get; set; }

    string? ISettingsElement.Category => Category;

    public string Text { get; set; }

    string ISettingsElement.Text => Text;

    public bool Value { get; set; }
    public Action<bool>? OnValueChanged { get; set; }

    public Toggle(string? category, string text, bool value, Action<bool>? onValueChanged)
    {
        Category = category;
        Text = text;
        Value = value;
        OnValueChanged = onValueChanged;
    }
}