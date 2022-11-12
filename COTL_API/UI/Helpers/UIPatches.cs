using System.Collections.Generic;
using Lamb.UI.PauseMenu;
using Lamb.UI.MainMenu;
using UnityEngine;
using HarmonyLib;
using System;
using TMPro;

namespace COTL_API.UI.Helpers;

internal static class UIPatches
{
    public static readonly List<Type> PauseMenuQueue = new();
    public static readonly List<Type> StartMenuQueue = new();

    [HarmonyPatch]
    public static class PauseMenuPatch
    {
        [HarmonyPatch(typeof(UIPauseMenuController), nameof(UIPauseMenuController.Start))]
        [HarmonyPostfix]
        public static void AddUIItems(UIPauseMenuController __instance)
        {
            var menu = __instance.gameObject.transform.Find("PauseMenuContainer");

            // FontAsset reference.
            var getTextAsset = menu.Find("Left")
                .transform.Find("Transform").transform.Find("MenuContainer")
                .transform.Find("Settings")
                .transform.Find("Text").GetComponent<TextMeshProUGUI>();
            FontHelpers._pauseMenu = getTextAsset.font;

            // API container.
            var container = new GameObject("COTL_API_MenuContainer");
            container.transform.SetParent(menu);
            container.layer = UIHelpers.UILayer;
            container.transform.position = Vector3.zero;
            container.transform.localScale = Vector3.one;

            UIMenuBase.Parent = container.transform;
        }
    }

    [HarmonyPatch]
    public static class StartMenuPatch
    {
        [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.Start))]
        [HarmonyPostfix]
        public static void AddUIItems(MainMenuController __instance)
        {
            var menu = __instance.gameObject.transform.Find("Main Menu");

            // FontAsset reference.
            var getTextAsset = menu.transform.Find("MainMenuContainer")
                .transform.Find("Left").transform.Find("Transform")
                .transform.Find("MenusContainer").transform.Find("MainMenu")
                .transform.Find("Settings").transform.Find("Text (TMP)")
                .GetComponent<TextMeshProUGUI>();
            FontHelpers._startMenu = getTextAsset.font;
        }
    }
}