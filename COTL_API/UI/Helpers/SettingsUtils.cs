using Object = UnityEngine.Object;
using UnityEngine.Events;
using COTL_API.Helpers;
using UnityEngine;
using Lamb.UI;
using TMPro;

namespace COTL_API.UI;

internal static class SettingsUtils
{
    internal static GameObject? SliderTemplate { get; set; }
    internal static GameObject? ToggleTemplate { get; set; }
    internal static GameObject? HorizontalSelectorTemplate { get; set; }
    internal static GameObject? HeaderTemplate { get; set; }

    public static void AddHeader(Transform parent, string? text)
    {
        if (HeaderTemplate == null)
        {
            LogHelper.LogError("Unable to find header template!");
            return;
        }

        var header = Object.Instantiate(HeaderTemplate, parent);
        header.name = text;
        var headerText = header.GetComponentInChildren<TextMeshProUGUI>();
        headerText.text = text;
    }

    public static void AddSlider(Transform parent, string text, float value, float minValue, float maxValue,
        int increment, MMSlider.ValueDisplayFormat format = MMSlider.ValueDisplayFormat.RawValue,
        UnityAction<float>? onChange = null)
    {
        if (SliderTemplate == null)
        {
            LogHelper.LogError("Unable to find slider template!");
            return;
        }

        var slider = Object.Instantiate(SliderTemplate, parent);
        slider.name = text;
        var sliderText = slider.GetComponentInChildren<TextMeshProUGUI>();
        sliderText.text = text;
        var sliderSlider = slider.GetComponentInChildren<MMSlider>();
        sliderSlider.value = value;
        sliderSlider.minValue = minValue;
        sliderSlider.maxValue = maxValue;
        sliderSlider._increment = increment;
        sliderSlider._valueDisplayFormat = format;
        sliderSlider.UpdateVisuals();
        if (onChange != null) sliderSlider.onValueChanged.AddListener(onChange);
    }

    public static void AddToggle(Transform parent, string text, bool value, Action<bool>? onChange = null)
    {
        if (ToggleTemplate == null)
        {
            LogHelper.LogError("Unable to find toggle template!");
            return;
        }

        var toggle = Object.Instantiate(ToggleTemplate, parent);
        toggle.name = text;
        var toggleText = toggle.GetComponentInChildren<TextMeshProUGUI>();
        toggleText.text = text;
        var toggleToggle = toggle.GetComponentInChildren<MMToggle>();
        toggleToggle.Value = value;
        toggleToggle.UpdateState(true);
        if (onChange != null) toggleToggle.OnValueChanged += onChange;
    }

    public static void AddHorizontalSelector(Transform parent, string text, string?[] options, int index = 0,
        Action<int>? onChange = null, string? indexStringOverride = null)
    {
        if (HorizontalSelectorTemplate == null)
        {
            LogHelper.LogError("Unable to find horizontal selector template!");
            return;
        }

        var horizontalSelector = Object.Instantiate(HorizontalSelectorTemplate, parent);
        horizontalSelector.name = text;
        var horizontalSelectorText = horizontalSelector.GetComponentInChildren<TextMeshProUGUI>();
        horizontalSelectorText.text = text;
        var selector = horizontalSelector.GetComponentInChildren<MMHorizontalSelector>();
        selector._localizeContent = false;
        selector.UpdateContent(options);
        var indexOverride = Math.Max(0, options.IndexOf(indexStringOverride));
        selector.ContentIndex = indexStringOverride != null ? indexOverride : index;
        if (onChange != null) selector.OnSelectionChanged += onChange;
    }
}