using System.Collections.Generic;
using COTL_API.CustomTarotCard;
using COTL_API.Helpers;
using COTL_API.Skins;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using System.IO;
using Lamb.UI;

namespace COTL_API.Debug;

[HarmonyPatch]
public class DebugCode
{
    public static void CreateSkin()
    {
        Texture2D customTex =
            TextureHelper.CreateTextureFromPath(PluginPaths.ResolveAssetPath("placeholder_sheet.png"));
        string atlasText = File.ReadAllText(PluginPaths.ResolveAssetPath("basic_atlas.txt"));

        SkinManager.AddCustomSkin("Test", customTex, atlasText);
    }

    [HarmonyPatch(typeof(InventoryMenu), nameof(InventoryMenu.OnShowStarted))]
    [HarmonyPrefix]
    public static void InventoryMenu_OnShowStarted(InventoryMenu __instance)
    {
        if (!Plugin.Debug) return;

        Inventory.AddItem(Plugin.DebugItem, 1, true);
        Inventory.AddItem(Plugin.DebugItem2, 1, true);
        Inventory.AddItem(Plugin.DebugItem3, 1, true);
    }

    [HarmonyPatch(typeof(UITarotChoiceOverlayController), nameof(UITarotChoiceOverlayController.Show),
        new[] { typeof(TarotCards.TarotCard), typeof(TarotCards.TarotCard), typeof(bool) })]
    [HarmonyPrefix]
    public static bool UITarotChoiceOverlayController_Show(UITarotChoiceOverlayController __instance,
        TarotCards.TarotCard card1, TarotCards.TarotCard card2, bool instant)
    {
        if (!Plugin.Debug) return true;
        
        __instance._card1 = GetRandModdedCard();
        __instance._card2 = GetRandVanillaCard();
        __instance._uiCard1.Play(__instance._card1);
        __instance._uiCard2.Play(__instance._card2);
        __instance.Show(instant);

        return false;
    }
    
    internal static TarotCards.TarotCard GetRandVanillaCard()
    {
        List<TarotCards.Card> vanillaCardList = new(DataManager.Instance.PlayerFoundTrinkets);
        vanillaCardList.RemoveAll(c =>
            CustomTarotCardManager.CustomTarotCards.ContainsKey(c));

        return new TarotCards.TarotCard(
            vanillaCardList.ElementAt(Random.Range(0,
                vanillaCardList.Count)), 0);
    }

    internal static TarotCards.TarotCard GetRandModdedCard()
    {
        return new TarotCards.TarotCard(
            CustomTarotCardManager.CustomTarotCards.Keys.ElementAt(Random.Range(0,
                CustomTarotCardManager.CustomTarotCards.Count)), 0);
    }

    internal static int getTarotMult(TarotCards.Card obj)
    {
        int mult = 0;
        if (DataManager.Instance.dungeonRun < 5) return Mathf.Min(mult, TarotCards.GetMaxTarotCardLevel(obj));

        while (Random.Range(0f, 1f) < 0.275f * DataManager.Instance.GetLuckMultiplier()) mult++;

        return Mathf.Min(mult, TarotCards.GetMaxTarotCardLevel(obj));
    }
}