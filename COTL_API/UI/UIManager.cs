using COTL_API.UI.Settings;
using HarmonyLib;
using Lamb.UI.SettingsMenu;

namespace COTL_API.UI;

[HarmonyPatch]
public class UIManager
{
    [HarmonyPatch(typeof(UISettingsMenuController), nameof(UISettingsMenuController.OnShowStarted))]
    [HarmonyPrefix]
    public static void UISettingsMenuController_OnShowStarted(UISettingsMenuController __instance)
    {
        //__instance.PushInstance(new SkinSettings());
        __instance.Push(new SkinSettings());
    }
}