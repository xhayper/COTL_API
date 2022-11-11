using System;
using HarmonyLib;
using System.Linq;
using Lamb.UI.PauseMenu;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Lamb.UI.MainMenu;

namespace COTL_API.UI.Helpers;
internal static class UIPatches
{
    public static List<Type> PauseMenuQueue = new List<Type>();
    public static List<Type> StartMenuQueue = new List<Type>();

    [HarmonyPatch]
    public static class PauseMenuPatch
    {

        [HarmonyPatch(typeof(UIPauseMenuController), nameof(UIPauseMenuController.Start))]
        [HarmonyPostfix]
        static void AddUIItems (UIPauseMenuController __instance)
        {
            Transform parentMenu = __instance.gameObject.transform.Find("PauseMenuContainer");

            // FontAsset reference.
            TextMeshProUGUI getTextAsset = parentMenu.Find("Left")
                .transform.Find("Transform").transform.Find("MenuContainer")
                .transform.Find("Settings")
                .transform.Find("Text").GetComponent<TextMeshProUGUI>();
            FontHelpers._pauseMenu = getTextAsset.font;

            // API container.
            GameObject Container = new GameObject("COTL_API_MenuContainer");
            Container.transform.SetParent(parentMenu);
            Container.layer = UIHelpers.UILayer;
            Container.transform.position = Vector3.zero;
            Container.transform.localScale = Vector3.one;

            UIMenuBase.Parent = Container.transform;

            List<UIMenuBase> PauseMenuItems = PauseMenuQueue.Select(x => Container.AddComponent(x) as UIMenuBase).ToList();
        }
    }

    [HarmonyPatch]
    public static class StartMenuPatch
    {

        [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.Start))]
        [HarmonyPostfix]
        static void AddUIItems(MainMenuController __instance)
        {
            Transform menuContainer = __instance.gameObject.transform.Find("Main Menu");

            // FontAsset reference.
            TextMeshProUGUI getTextAsset = menuContainer.transform.Find("MainMenuContainer")
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
