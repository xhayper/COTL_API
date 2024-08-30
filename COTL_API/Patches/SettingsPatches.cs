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
        // ReSharper disable once InvertIf
        if (Plugin.LambFleeceSkinSettings?.Value is not null)
        {
            if (CustomSkinManager.CustomPlayerSkins.TryGetValue(Plugin.LambFleeceSkinSettings.Value, out var skin))
                CustomSkinManager.SetPlayerSkinOverride(PlayerType.LAMB, skin);
            else
                Plugin.LambFleeceSkinSettings.Value = "Lamb";
        }

        // ReSharper disable once InvertIf
        if (Plugin.GoatFleeceSkinSettings?.Value is not null)
        {
            if (CustomSkinManager.CustomPlayerSkins.TryGetValue(Plugin.GoatFleeceSkinSettings.Value, out var skin))
                CustomSkinManager.SetPlayerSkinOverride(PlayerType.GOAT, skin);
            else
                Plugin.GoatFleeceSkinSettings.Value = "Goat";
        }

        if (Plugin.LambFleeceBleatSettings?.Value is not null && Plugin.LambFleeceBleatSettings.Value != "Default") 
            CustomSkinManager.SetPlayerBleatOverride(PlayerType.LAMB, (PlayerBleat)Enum.Parse(typeof(PlayerBleat), Plugin.LambFleeceBleatSettings.Value));

        if (Plugin.GoatFleeceBleatSettings?.Value is not null && Plugin.GoatFleeceBleatSettings.Value != "Default")
            CustomSkinManager.SetPlayerBleatOverride(PlayerType.GOAT, (PlayerBleat)Enum.Parse(typeof(PlayerBleat), Plugin.GoatFleeceBleatSettings.Value));
            
    }
}