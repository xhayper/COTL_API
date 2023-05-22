using COTL_API.CustomSkins;
using HarmonyLib;
using Lamb.UI;
using Lamb.UI.MainMenu;

namespace COTL_API.Patches;

[HarmonyPatch]
public static class SettingsPatches
{
    [HarmonyPatch(typeof(MMHorizontalSelector), nameof(MMHorizontalSelector.UpdateContent))]
    [HarmonyPrefix]
    private static void MMHorizontalSelector_UpdateContent(MMHorizontalSelector __instance, string[] newContent)
    {
        if (__instance._contentIndex >= newContent.Length) __instance._contentIndex = 0;
    }
    
    [HarmonyPatch(typeof(LoadMenu), nameof(LoadMenu.OnTryLoadSaveSlot))]
    [HarmonyPostfix]
    private static void LoadMenu_OnTryLoadSaveSlot()
    {
        if (Plugin.SkinSettings?.Value is null or "Default") return;

        if (CustomSkinManager.CustomPlayerSkins.TryGetValue(Plugin.SkinSettings.Value, out var skin))
            CustomSkinManager.SetPlayerSkinOverride(skin);
        else
            Plugin.SkinSettings.Value = "Default";
    }
}