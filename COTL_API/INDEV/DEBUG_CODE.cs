using System.Collections.Generic;
using COTL_API.Helpers;
using COTL_API.Skins;
using System.Text;
using UnityEngine;
using HarmonyLib;
using System.IO;
using System;

namespace COTL_API.INDEV;

[HarmonyPatch]
public class DEBUG_CODE
{
    public static void CreateSkin()
    {
        var customTex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        byte[] imgBytes = File.ReadAllBytes(PluginPaths.ResolveAssetPath("placeholder_sheet.png"));
        customTex.LoadImage(imgBytes);
        var atlasText = File.ReadAllText(PluginPaths.ResolveAssetPath("basic_atlas.txt"));

        SkinManager.AddCustomSkin("Test", customTex, atlasText);
    }

    [HarmonyPatch(typeof(Lamb.UI.InventoryMenu), "OnShowStarted")]
    [HarmonyPrefix]
    public static void _____(Lamb.UI.InventoryMenu __instance)
    {
        Inventory.AddItem(Plugin.DEBUG_ITEM, 1, true);
        Inventory.AddItem(Plugin.DEBUG_ITEM_2, 1, true);
        Inventory.AddItem(Plugin.DEBUG_ITEM_3, 1, true);
    }
}