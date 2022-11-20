using COTL_API.CustomSettings;
using COTL_API.CustomSkins;
using Object = UnityEngine.Object;
using Lamb.UI.SettingsMenu;
using HarmonyLib;
using Lamb.UI;
using Lamb.UI.Settings;
using Sirenix.Utilities;
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

    [HarmonyPatch(typeof(UISettingsMenuController), nameof(UISettingsMenuController.OnShowStarted))]
    [HarmonyPostfix]
    public static void UISettingsMenuController_OnShowStarted(UISettingsMenuController __instance)
    {
        if (SettingsUtils._sliderTemplate == null)
        {
            SettingsUtils._sliderTemplate = __instance._gameSettings.GetComponentInChildren<ScrollRect>().content.GetChild(1).gameObject;
            SettingsUtils._toggleTemplate = __instance._gameSettings.GetComponentInChildren<ScrollRect>().content.GetChild(2).gameObject;
            SettingsUtils._horizontalSelectorTemplate = __instance._gameSettings.GetComponentInChildren<ScrollRect>().content.GetChild(3).gameObject;
            SettingsUtils._headerTemplate = __instance._graphicsSettings.GetComponentInChildren<ScrollRect>().content.GetChild(0).gameObject;
        }
        
        GraphicsSettings originalGraphicsSettings = __instance._graphicsSettings;
        SettingsTabNavigatorBase stnb = __instance.transform.GetComponentInChildren<SettingsTabNavigatorBase>();
        HorizontalLayoutGroup hlg = stnb.GetComponentInChildren<HorizontalLayoutGroup>();
        Transform graphicsSettingsTab = hlg.transform.GetChild(2);
        Transform newSettings = Object.Instantiate(graphicsSettingsTab, hlg.transform);
        newSettings.SetSiblingIndex(hlg.transform.childCount - 2);
        newSettings.name = "Mod Settings Button";
        TMP_Text text = newSettings.GetComponentInChildren<TMP_Text>();
        text.text = "Mods";
        GameObject content = __instance.GetComponentInChildren<GameSettings>().transform.parent.gameObject;
        Transform graphicsSettings = content.transform.GetChild(1);
        Transform newGraphicsSettings = Object.Instantiate(graphicsSettings, content.transform);
        newGraphicsSettings.name = "Mod Settings Content";
        newGraphicsSettings.gameObject.SetActive(true);
        GraphicsSettings copy = newGraphicsSettings.GetComponentInChildren<GraphicsSettings>();
        SettingsTab tab = newSettings.GetComponent<SettingsTab>();
        tab._menu = copy;
        copy._defaultSelectable = graphicsSettingsTab.GetComponentInChildren<Selectable>();
        Action onShow = originalGraphicsSettings.OnShow;
        Action onHide = originalGraphicsSettings.OnHide;
        Delegate[] onShowDelegates = onShow.GetInvocationList();
        Delegate[] onHideDelegates = onHide.GetInvocationList();
        Action showDelegate = (Action)onHideDelegates[1];
        Action hideDelegate = (Action)onShowDelegates[1];
        copy.OnShow += showDelegate;
        copy.OnHide += hideDelegate;
        originalGraphicsSettings.OnShow -= showDelegate;
        originalGraphicsSettings.OnHide -= hideDelegate;
        
        __instance.transform.GetComponentInChildren<SettingsTabNavigatorBase>()._tabs = __instance.transform.GetComponentInChildren<SettingsTabNavigatorBase>()._tabs.Append(tab).ToArray();
        MMButton button = newSettings.GetComponentInChildren<MMButton>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => stnb.TransitionTo(tab));
    }
    
    [HarmonyPatch(typeof(GraphicsSettings), nameof(GraphicsSettings.Start))]
    [HarmonyPrefix]
    public static bool GraphicsSettings_Start(GraphicsSettings __instance)
    {
        if (__instance.name == "Mod Settings Content")
        {
            Transform scrollContent = __instance._scrollRect.content;
            foreach (Transform child in scrollContent)
            {
                Object.Destroy(child.gameObject);
            }
            string currentCategory = null;
            foreach (SettingsElement element in CustomSettingsManager.SettingsElements.OrderBy(x => x.Category).ThenBy(x => x.Text))
            {
                if (element.Category != currentCategory)
                {
                    currentCategory = element.Category;
                    SettingsUtils.AddHeader(scrollContent, currentCategory);
                }
                if (element is Dropdown dropdown)
                {
                    SettingsUtils.AddHorizontalSelector(scrollContent, dropdown.Text, dropdown.Options, -1,
                        dropdown.OnValueChanged, dropdown.Value);
                }
                else if (element is Slider slider)
                {
                    UnityAction<float> onValueChanged = delegate (float i)
                    {
                        slider.OnValueChanged(i);
                    };
                    SettingsUtils.AddSlider(scrollContent, slider.Text, slider.Value, slider.Min, slider.Max, slider.Increment, slider.DisplayFormat, onValueChanged);
                }
                else if (element is Toggle toggle)
                {
                    SettingsUtils.AddToggle(scrollContent, toggle.Text, toggle.Value, toggle.OnValueChanged);
                }
            }

            return false;
        }
        return true;
    }
    
        
    [HarmonyPatch(typeof(GraphicsSettings), nameof(GraphicsSettings.OnShowStarted))]
    [HarmonyPrefix]
    public static void UISettingsMenuController_OnShowStarted(GraphicsSettings __instance)
    {

        if (__instance.name == "Mod Settings Content")
        {
            __instance._targetFpsSelectable.HorizontalSelector._canvasGroup = __instance._canvasGroup;
        }
    }
}