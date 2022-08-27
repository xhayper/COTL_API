using COTL_API.CustomTarotCard;
using COTL_API.Helpers;
using COTL_API.Skins;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using System.IO;
using Lamb.UI;

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
    public static void InventoryMenu_OnShowStarted(Lamb.UI.InventoryMenu __instance)
    {
        if (!Plugin.debugEnabled) return;

        Inventory.AddItem(Plugin.DEBUG_ITEM, 1, true);
        Inventory.AddItem(Plugin.DEBUG_ITEM_2, 1, true);
        Inventory.AddItem(Plugin.DEBUG_ITEM_3, 1, true);
    }

    [HarmonyPatch(typeof(UITarotChoiceOverlayController), nameof(UITarotChoiceOverlayController.Show), new System.Type[] { typeof(TarotCards.TarotCard), typeof(TarotCards.TarotCard), typeof(bool) })]
    [HarmonyPrefix]
    public static bool UITarotChoiceOverlayController_Show(UITarotChoiceOverlayController __instance, TarotCards.TarotCard card1, TarotCards.TarotCard card2, bool instant)
    {
        if (!Plugin.debugEnabled) return true;

        __instance._card1 = GetRandModdedCard();
        __instance._card2 = GetRandModdedCard();
        __instance._uiCard1.Play(__instance._card1);
        __instance._uiCard2.Play(__instance._card2);
        __instance.Show(instant);

        return false;
    }
    internal static TarotCards.TarotCard GetRandModdedCard()
    {
        return new TarotCards.TarotCard(CustomTarotCardManager.customTarotCards.Keys.ElementAt(Random.Range(0, CustomTarotCardManager.customTarotCards.Count)), 0); ;
    }
    internal static int getTarotMult(TarotCards.Card obj)
    {
        int mult = 0;
        if (DataManager.Instance.dungeonRun >= 5)
        {
            while (Random.Range(0f, 1f) < 0.275f * DataManager.Instance.GetLuckMultiplier())
            {
                mult++;
            }
        }

        return Mathf.Min(mult, TarotCards.GetMaxTarotCardLevel(obj));
    }
}