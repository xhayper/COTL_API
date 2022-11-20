using Lamb.UI;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace COTL_API.UI;

internal class SettingsUtils
{
    internal static GameObject _sliderTemplate;
    internal static GameObject _toggleTemplate;
    internal static GameObject _horizontalSelectorTemplate;
    internal static GameObject _headerTemplate;
        
    public static void AddHeader(Transform parent, string text)
    {
        if (_headerTemplate == null)
        {
            Plugin.Logger.LogError("Unable to find header template!");
            return;
        }
        GameObject header = Object.Instantiate(_headerTemplate, parent);
        header.name = text;
        TextMeshProUGUI headerText = header.GetComponentInChildren<TextMeshProUGUI>();
        headerText.text = text;
    }
    
    public static void AddSlider(Transform parent, string text, float value, float minValue, float maxValue, int increment, MMSlider.ValueDisplayFormat format = MMSlider.ValueDisplayFormat.RawValue, UnityAction<float> onChange = null)
    {
        if (_sliderTemplate == null)
        {
            Plugin.Logger.LogError("Unable to find slider template!");
            return;
        }
        GameObject slider = Object.Instantiate(_sliderTemplate, parent);
        slider.name = text;
        TextMeshProUGUI sliderText = slider.GetComponentInChildren<TextMeshProUGUI>();
        sliderText.text = text;
        MMSlider sliderSlider = slider.GetComponentInChildren<MMSlider>();
        sliderSlider.value = value;
        sliderSlider.minValue = minValue;
        sliderSlider.maxValue = maxValue;
        sliderSlider._increment = increment;
        sliderSlider._valueDisplayFormat = format;
        if (onChange != null) sliderSlider.onValueChanged.AddListener(onChange);
    }
    
    public static void AddToggle(Transform parent, string text, bool value, Action<bool> onChange = null)
    {
        if (_toggleTemplate == null)
        {
            Plugin.Logger.LogError("Unable to find toggle template!");
            return;
        }
        GameObject toggle = Object.Instantiate(_toggleTemplate, parent);
        toggle.name = text;
        TextMeshProUGUI toggleText = toggle.GetComponentInChildren<TextMeshProUGUI>();
        toggleText.text = text;
        MMToggle toggleToggle = toggle.GetComponentInChildren<MMToggle>();
        toggleToggle.Value = value;
        if (onChange != null) toggleToggle.OnValueChanged += onChange;
    }
    
    public static void AddHorizontalSelector(Transform parent, string text, string[] options, int index = 0, Action<int> onChange = null, string indexStringOverride = null)
    {
        if (_horizontalSelectorTemplate == null)
        {
            Plugin.Logger.LogError("Unable to find horizontal selector template!");
            return;
        }
        GameObject horizontalSelector = Object.Instantiate(_horizontalSelectorTemplate, parent);
        horizontalSelector.name = text;
        TextMeshProUGUI horizontalSelectorText = horizontalSelector.GetComponentInChildren<TextMeshProUGUI>();
        horizontalSelectorText.text = text;
        MMHorizontalSelector selector = horizontalSelector.GetComponentInChildren<MMHorizontalSelector>();
        selector._localizeContent = false;
        selector.UpdateContent(options);
        selector.ContentIndex = indexStringOverride != null ? (options.IndexOf(indexStringOverride) == -1 ? 0 : options.IndexOf(indexStringOverride)) : index;
        if (onChange != null) selector.OnSelectionChanged += onChange;
    }
}