using System.Collections.Generic;
using Lamb.UI.PauseMenu;
using Lamb.UI.MainMenu;
using UnityEngine;
using System.Linq;
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

            var pauseMenuItems =
                PauseMenuQueue.Select(x => container.AddComponent(x) as UIMenuBase).ToList();
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


            // I have been struggling with this for a while now. I'll get back to this later.
            // API container.
            /*
            GameObject Container = new GameObject("COTL_API_MenuContainer");
            Container.transform.SetParent(menuContainer);
            Container.layer = UIHelpers.UILayer;
            Container.transform.position = Vector3.zero;
            Container.transform.localScale = Vector3.one;

            UIMenuBase.Parent = Container.transform;

            List<UIMenuBase> StartMenuItems = StartMenuQueue.Select(x => Container.AddComponent(x) as UIMenuBase).ToList();
            */
        }
    }
}