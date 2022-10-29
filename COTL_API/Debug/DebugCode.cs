using COTL_API.CustomSkins;
using System.Collections.Generic;
using COTL_API.CustomTarotCard;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using Lamb.UI;

namespace COTL_API.Debug;

[HarmonyPatch]
public class DebugCode
{
    private static int _currentOverride = -1;
    private static string[] _testSkinOverrides = {
        "Goat", "Owl", "Snake", "Lamb_0", "Lamb_0_BW", "Lamb_1", "Lamb_1_BW", "Lamb_2", "Lamb_2_BW", "Lamb_3",
        "Lamb_3_BW", "Lamb_4", "Lamb_4_BW", "Lamb_5", "Lamb_5_BW", "Lamb_Intro", "Lamb_BW", "Lamb"
    };
    
    [HarmonyPatch(typeof(InventoryMenu), nameof(InventoryMenu.OnShowStarted))]
    [HarmonyPrefix]
    public static void InventoryMenu_OnShowStarted(InventoryMenu __instance)
    {
        if (!Plugin.Debug) return;

        Inventory.AddItem(Plugin.DebugItem, 1, true);
        Inventory.AddItem(Plugin.DebugItem2, 1, true);
        Inventory.AddItem(Plugin.DebugItem3, 1, true);
        
        // Cycle skin overrides
        _currentOverride = (_currentOverride + 1) % _testSkinOverrides.Length;
        CustomSkinManager.SetPlayerSkinOverride(PlayerFarming.Instance.Spine.Skeleton.Data.FindSkin(_testSkinOverrides[_currentOverride]));
        PlayerFarming.Instance.SetSkin();
    }
    
    [HarmonyPatch(typeof(UITarotChoiceOverlayController), nameof(UITarotChoiceOverlayController.Show))]
    [HarmonyPrefix]
    public static bool UITarotChoiceOverlayController_Show(UITarotChoiceOverlayController __instance,
        TarotCards.TarotCard card1, TarotCards.TarotCard card2, bool instant)
    {
        if (!Plugin.Debug) return true;

        DataManager.Instance.PlayerRunTrinkets.Remove(card1);
        DataManager.Instance.PlayerRunTrinkets.Remove(card2);
        
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
            CustomTarotCardManager.CustomTarotCards.ContainsKey(c) || DataManager.Instance.PlayerRunTrinkets.Any((t) => t.CardType == c));

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