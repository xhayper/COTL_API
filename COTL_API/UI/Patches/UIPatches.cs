using System;
using HarmonyLib;
using Lamb.UI;
using System.Linq;
using Lamb.UI.PauseMenu;
using UnityEngine;
using System.Collections.Generic;
using COTL_API.UI.Helpers;
using TMPro;

namespace COTL_API.UI.Patches;
internal static class UIPatches
{
    public static List<Type> PauseMenuQueue = new List<Type>();

    [HarmonyPatch]
    public static class PauseMenuPatch
    {

        [HarmonyPatch(typeof(UIPauseMenuController), nameof(UIPauseMenuController.Start))]
        [HarmonyPostfix]
        static void AddUIItems (UIPauseMenuController __instance)
        {
            Transform parentMenu = __instance.gameObject.transform.Find("PauseMenuContainer");

            // Font
            TextMeshProUGUI textMesh = parentMenu.Find("Left").Find("Transform").Find("MenuContainer").Find("Settings").Find("Text").GetComponent<TextMeshProUGUI>();
            FontHelpers._pauseMenu = textMesh.font;

            GameObject Container = new GameObject("Container");
            Container.transform.SetParent(parentMenu);
            Container.layer = UIHelpers.UILayer;
            Container.transform.localPosition = new Vector3(0f, 360f, 0);
            Container.transform.localScale = Vector3.one;

            PauseMenuBase.Parent = Container.transform;

            List<PauseMenuBase> PauseMenuItems = PauseMenuQueue.Select(x => Container.AddComponent(x) as PauseMenuBase).ToList();

            /*foreach(Type type in PauseMenuQueue)
            {
                PauseMenuBase woo = Container.AddComponent(type) as PauseMenuBase;
            }*/
        }
    }
}
