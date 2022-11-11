using COTL_API.CustomSkins;
using Object = UnityEngine.Object;
using Lamb.UI.SettingsMenu;
using HarmonyLib;
using Lamb.UI;
using Lamb.UI.Settings;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace COTL_API.UI;

[HarmonyPatch]
public class UIManager
{
    private static GameObject _sliderTemplate;
    private static GameObject _toggleTemplate;
    private static GameObject _horizontalSelectorTemplate;
    private static GameObject _headerTemplate;
    
    [HarmonyPatch(typeof(UISettingsMenuController), nameof(UISettingsMenuController.OnShowStarted))]
    [HarmonyPostfix]
    public static void UISettingsMenuController_OnShowStarted(UISettingsMenuController __instance)
    {
        if (_sliderTemplate == null)
        {
            _sliderTemplate = __instance._gameSettings.GetComponentInChildren<ScrollRect>().content.GetChild(1).gameObject;
            _toggleTemplate = __instance._gameSettings.GetComponentInChildren<ScrollRect>().content.GetChild(2).gameObject;
            _horizontalSelectorTemplate = __instance._gameSettings.GetComponentInChildren<ScrollRect>().content.GetChild(3).gameObject;
            _headerTemplate = __instance._graphicsSettings.GetComponentInChildren<ScrollRect>().content.GetChild(0).gameObject;
        }
        
        SettingsTabNavigatorBase stnb = __instance.transform.GetComponentInChildren<SettingsTabNavigatorBase>();
        HorizontalLayoutGroup hlg = stnb.GetComponentInChildren<HorizontalLayoutGroup>();
        Transform graphicsSettingsTab = hlg.transform.GetChild(2);
        Transform newSettings = Object.Instantiate(graphicsSettingsTab, hlg.transform);
        newSettings.SetSiblingIndex(hlg.transform.childCount - 2);
        newSettings.name = "API Settings Button";
        TMP_Text text = newSettings.GetComponentInChildren<TMP_Text>();
        text.text = "Mods";
        GameObject content = __instance.GetComponentInChildren<GameSettings>().transform.parent.gameObject;
        Transform graphicsSettings = content.transform.GetChild(1);
        Transform newGraphicsSettings = Object.Instantiate(graphicsSettings, content.transform);
        newGraphicsSettings.name = "API Settings Content";
        newGraphicsSettings.gameObject.SetActive(true);
        GraphicsSettings copy = newGraphicsSettings.GetComponentInChildren<GraphicsSettings>();
        SettingsTab tab = newSettings.GetComponent<SettingsTab>();
        tab._menu = newGraphicsSettings.gameObject.GetComponentInChildren<UIMenuBase>();
        copy._defaultSelectable = graphicsSettingsTab.GetComponentInChildren<Selectable>();
        __instance.transform.GetComponentInChildren<SettingsTabNavigatorBase>()._tabs = __instance.transform.GetComponentInChildren<SettingsTabNavigatorBase>()._tabs.Append(tab).ToArray();
        MMButton button = newSettings.GetComponentInChildren<MMButton>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => stnb.TransitionTo(tab));
        button.animator.runtimeAnimatorController = new RuntimeAnimatorController();
    }
    
    [HarmonyPatch(typeof(GraphicsSettings), nameof(GraphicsSettings.Start))]
    [HarmonyPrefix]
    public static bool GraphicsSettings_Start(GraphicsSettings __instance)
    {
        if (__instance.name == "API Settings Content")
        {
            ScrollRect scrollRect = __instance.GetComponentInChildren<ScrollRect>();
            Transform scrollContent = scrollRect.content;
            foreach (Transform child in scrollContent)
            {
                Object.Destroy(child.gameObject);
            }
            AddHeader(scrollContent, "API Settings");
            AddHorizontalSelector(scrollContent, "Lamb Skin", new string[] { "Default" }.Concat(CustomSkinManager.CustomPlayerSkins.Keys).ToArray(), 0,
                i =>
                {
                    if (i == 0)
                    {
                        CustomSkinManager.ResetPlayerSkin();
                    }
                    else
                    {
                        CustomSkinManager.SetPlayerSkinOverride(CustomSkinManager.CustomPlayerSkins.Values.ElementAt(i - 1));
                    }
                }, CustomSkinManager.OverrideSkinName);
            return false;
        }
        return true;
    }
    
    [HarmonyPatch(typeof(GraphicsSettings), nameof(GraphicsSettings.OnShowStarted))]
    [HarmonyPrefix]
    public static bool UISettingsMenuController_OnShowStarted(GraphicsSettings __instance)
    {
        if (__instance.name == "API Settings Content") return false;
        return true;
    }
    
    [HarmonyPatch(typeof(GraphicsSettings), nameof(GraphicsSettings.OnHideStarted))]
    [HarmonyPrefix]
    public static bool UISettingsMenuController_OnHideStarted(GraphicsSettings __instance)
    {
        if (__instance.name == "API Settings Content") return false;
        return true;
    }
    
    private static void AddHeader(Transform parent, string text)
    {
        GameObject header = Object.Instantiate(_headerTemplate, parent);
        header.name = text;
        TextMeshProUGUI headerText = header.GetComponentInChildren<TextMeshProUGUI>();
        headerText.text = text;
    }
    
    private static void AddSlider(Transform parent, string text, float value, float minValue, float maxValue, int increment, MMSlider.ValueDisplayFormat format = MMSlider.ValueDisplayFormat.RawValue, UnityAction<float> onChange = null)
    {
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
        sliderSlider.onValueChanged.AddListener(onChange);
    }
    
    private static void AddToggle(Transform parent, string text, bool value, Action<bool> onChange = null)
    {
        GameObject toggle = Object.Instantiate(_toggleTemplate, parent);
        toggle.name = text;
        TextMeshProUGUI toggleText = toggle.GetComponentInChildren<TextMeshProUGUI>();
        toggleText.text = text;
        MMToggle toggleToggle = toggle.GetComponentInChildren<MMToggle>();
        toggleToggle.Value = value;
        toggleToggle.OnValueChanged += (bool b) => onChange?.Invoke(b);
    }
    
    private static void AddHorizontalSelector(Transform parent, string text, string[] options, int index = 0, Action<int> onChange = null, string indexStringOverride = null)
    {
        GameObject horizontalSelector = Object.Instantiate(_horizontalSelectorTemplate, parent);
        horizontalSelector.name = text;
        TextMeshProUGUI horizontalSelectorText = horizontalSelector.GetComponentInChildren<TextMeshProUGUI>();
        horizontalSelectorText.text = text;
        MMHorizontalSelector selector = horizontalSelector.GetComponentInChildren<MMHorizontalSelector>();
        selector._localizeContent = false;
        selector.UpdateContent(options);
        selector.ContentIndex = indexStringOverride != null ? (options.IndexOf(indexStringOverride) == -1 ? 0 : options.IndexOf(indexStringOverride)) : index;
        selector.OnSelectionChanged += (int i) => onChange?.Invoke(i);
    }
}