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
        
        copy.OnShow += (Action)onShowDelegates[1];
        copy.OnHide += (Action)onHideDelegates[1];
        originalGraphicsSettings.OnShow -= (Action)onShowDelegates[1];
        originalGraphicsSettings.OnHide -= (Action)onHideDelegates[1];
        
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
            SettingsUtils.AddHeader(scrollContent, "Mod Settings");
            SettingsUtils.AddHorizontalSelector(scrollContent, "Lamb Skin", new string[] { "Default" }.Concat(CustomSkinManager.CustomPlayerSkins.Keys).ToArray(), 0,
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
    public static void UISettingsMenuController_OnShowStarted(GraphicsSettings __instance)
    {

        if (__instance.name == "Mod Settings Content")
        {
            __instance._targetFpsSelectable.HorizontalSelector._canvasGroup = __instance._canvasGroup;
        }
    }
}