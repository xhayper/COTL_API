using HorizontalSelector = COTL_API.CustomSettings.Elements.HorizontalSelector;
using Slider = COTL_API.CustomSettings.Elements.Slider;
using Toggle = COTL_API.CustomSettings.Elements.Toggle;
using Object = UnityEngine.Object;
using COTL_API.CustomSettings;
using Lamb.UI.SettingsMenu;
using Lamb.UI.Settings;
using src.UINavigator;
using UnityEngine.UI;
using UnityEngine;
using HarmonyLib;
using Lamb.UI;
using I2.Loc;
using TMPro;

namespace COTL_API.UI;

[HarmonyPatch]
public static class UIManager
{
    public static Action OnSettingsLoaded { get; set; } = delegate { };

    [HarmonyPatch(typeof(UISettingsMenuController), nameof(UISettingsMenuController.OnShowStarted))]
    [HarmonyPostfix]
    public static void UISettingsMenuController_OnShowStarted(UISettingsMenuController __instance)
    {
        if (SettingsUtils.SliderTemplate == null)
        {
            SettingsUtils.SliderTemplate = __instance._gameSettings.GetComponentInChildren<ScrollRect>().content
                .GetChild(1).gameObject;
            SettingsUtils.ToggleTemplate = __instance._gameSettings.GetComponentInChildren<ScrollRect>().content
                .GetChild(2).gameObject;
            SettingsUtils.HorizontalSelectorTemplate = __instance._gameSettings.GetComponentInChildren<ScrollRect>()
                .content.GetChild(3).gameObject;
            SettingsUtils.HeaderTemplate = __instance._graphicsSettings.GetComponentInChildren<ScrollRect>().content
                .GetChild(0).gameObject;
        }

        var originalGraphicsSettings = __instance._graphicsSettings;
        var stnb = __instance.transform.GetComponentInChildren<SettingsTabNavigatorBase>();
        var hlg = stnb.GetComponentInChildren<HorizontalLayoutGroup>();
        var graphicsSettingsTab = hlg.transform.GetChild(2);
        var newSettings = Object.Instantiate(graphicsSettingsTab, hlg.transform);
        newSettings.SetSiblingIndex(hlg.transform.childCount - 2);
        newSettings.name = "Mod Settings Button";
        var text = newSettings.GetComponentInChildren<TMP_Text>();
        Object.Destroy(newSettings.GetComponentInChildren<Localize>());
        text.text = "Mods";
        var content = __instance.GetComponentInChildren<GameSettings>().transform.parent.gameObject;
        var graphicsSettings = content.transform.GetChild(1);
        var newGraphicsSettings = Object.Instantiate(graphicsSettings, content.transform);
        newGraphicsSettings.name = "Mod Settings Content";
        newGraphicsSettings.gameObject.SetActive(true);
        var copy = newGraphicsSettings.GetComponentInChildren<GraphicsSettings>();
        var tab = newSettings.GetComponent<SettingsTab>();
        tab._menu = copy;
        copy._defaultSelectable = copy._scrollRect.content.GetComponentInChildren<Selectable>();
        copy._defaultSelectableFallbacks = Array.Empty<Selectable>();
        var onShow = originalGraphicsSettings.OnShow;
        var onHide = originalGraphicsSettings.OnHide;
        var onShowDelegates = onShow.GetInvocationList();
        var onHideDelegates = onHide.GetInvocationList();
        var showDelegate = (Action)onShowDelegates[1];
        var hideDelegateAll = (Action)onHideDelegates[1];
        var hideDelegate = (Action)onHideDelegates[2];
        copy.OnShow += showDelegate;
        copy.OnHide += hideDelegate;
        copy.OnHide += hideDelegateAll;
        originalGraphicsSettings.OnShow -= showDelegate;
        originalGraphicsSettings.OnHide -= hideDelegate;

        __instance.transform.GetComponentInChildren<SettingsTabNavigatorBase>()._tabs = __instance.transform
            .GetComponentInChildren<SettingsTabNavigatorBase>()._tabs.Append(tab).ToArray();
        var button = newSettings.GetComponentInChildren<MMButton>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => stnb.TransitionTo(tab));
    }

    [HarmonyPatch(typeof(GraphicsSettings), nameof(GraphicsSettings.OnShowStarted))]
    [HarmonyPrefix]
    public static void UISettingsMenuController_OnShowStarted(GraphicsSettings __instance)
    {
        if (__instance.name != "Mod Settings Content") return;

        __instance._targetFpsSelectable.HorizontalSelector._canvasGroup = __instance._canvasGroup;
        __instance._defaultSelectable = __instance._scrollRect.content.GetComponentInChildren<Selectable>();
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(__instance._defaultSelectable as IMMSelectable);

    }

    [HarmonyPatch(typeof(GraphicsSettings), nameof(GraphicsSettings.Start))]
    [HarmonyPrefix]
    public static bool GraphicsSettings_Start(GraphicsSettings __instance)
    {
        if (__instance.name != "Mod Settings Content") return true;

        Transform scrollContent = __instance._scrollRect.content;
        foreach (Transform child in scrollContent)
            Object.Destroy(child.gameObject);

        OnSettingsLoaded.Invoke();
        OnSettingsLoaded = delegate { };

        string? currentCategory = null;
        foreach (var element in CustomSettingsManager.SettingsElements.OrderBy(x => x.Category)
                     .ThenBy(x => x.Text))
        {
            if (element.Category != currentCategory)
            {
                currentCategory = element.Category;
                SettingsUtils.AddHeader(scrollContent, currentCategory);
            }

            switch (element)
            {
                case HorizontalSelector dropdown:
                    {

                        void OnValueChanged(int i)
                        {
                            dropdown.Value = dropdown.Options[i];
                            dropdown.OnValueChanged?.Invoke(i);
                        }

                        SettingsUtils.AddHorizontalSelector(scrollContent, dropdown.Text, dropdown.Options, -1,
                            OnValueChanged, dropdown.Value);
                        break;
                    }
                case Slider slider:
                    {
                        void OnValueChanged(float i)
                        {
                            slider.Value = i;
                            slider.OnValueChanged?.Invoke(i);
                        }

                        SettingsUtils.AddSlider(scrollContent, slider.Text, slider.Value, slider.Min, slider.Max,
                            slider.Increment, slider.DisplayFormat, OnValueChanged);
                        break;
                    }
                case Toggle toggle:
                    {
                        void OnValueChanged(bool i)
                        {
                            toggle.Value = i;
                            toggle.OnValueChanged?.Invoke(i);
                        }

                        SettingsUtils.AddToggle(scrollContent, toggle.Text, toggle.Value, OnValueChanged);
                        break;
                    }
            }
        }

        if (!CustomSettingsManager.SettingsElements.Any())
            SettingsUtils.AddHeader(scrollContent, "(No mods have settings)");

        return false;
    }
}
