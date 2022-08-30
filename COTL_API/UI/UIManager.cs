using Object = UnityEngine.Object;
using COTL_API.UI.Settings;
using Lamb.UI.SettingsMenu;
using HarmonyLib;
using UnityEngine;

namespace COTL_API.UI;

[HarmonyPatch]
public class UIManager
{
    private static SkinSettings _skinSettings;

    static UIManager()
    {
        GameObject tempGO = new();

        _skinSettings = tempGO.AddComponent<SkinSettings>();
        _skinSettings.Init();
    }
    
    [HarmonyPatch(typeof(UISettingsMenuController), nameof(UISettingsMenuController.OnShowStarted))]
    [HarmonyPrefix]
    public static void UISettingsMenuController_OnShowStarted(UISettingsMenuController __instance)
    {
        __instance.Push(_skinSettings);
    }
}