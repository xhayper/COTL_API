using COTL_API.CustomObjectives;
using COTL_API.CustomTarotCard;
using HarmonyLib;
using Lamb.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace COTL_API.Debug;

[HarmonyPatch]
public class DebugCode
{
    [HarmonyPatch(typeof(InventoryMenu), nameof(InventoryMenu.OnShowStarted))]
    [HarmonyPrefix]
    private static void InventoryMenu_OnShowStarted(InventoryMenu __instance)
    {
        if (Plugin.Instance == null || !Plugin.Instance.Debug) return;

        Inventory.AddItem(Plugin.Instance.DebugItem, 1, true);
        Inventory.AddItem(Plugin.Instance.DebugItem2, 1, true);
        Inventory.AddItem(Plugin.Instance.DebugItem3, 1, true);
        Inventory.AddItem(Plugin.Instance.DebugItem4, 1, true);

        if (PlayerFarming.Instance.playerRelic.CurrentRelic == null ||
            PlayerFarming.Instance.playerRelic.CurrentRelic.RelicType != Plugin.Instance.DebugRelic)
            PlayerFarming.Instance.playerRelic.EquipRelic(EquipmentManager.GetRelicData(Plugin.Instance.DebugRelic));

        var test = CustomObjectiveManager.BedRest("Test");
        test.InitialQuestText = "This is my custom quest text for this objective.";
    }

    [HarmonyPatch(typeof(UITarotChoiceOverlayController), nameof(UITarotChoiceOverlayController.Show))]
    [HarmonyPrefix]
    private static bool UITarotChoiceOverlayController_Show(UITarotChoiceOverlayController __instance,
        TarotCards.TarotCard card1, TarotCards.TarotCard card2, bool instant)
    {
        if (Plugin.Instance == null || !Plugin.Instance.Debug) return true;

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
            CustomTarotCardManager.CustomTarotCardList.ContainsKey(c) ||
            PlayerFarming.Instance.RunTrinkets.Any(t => t.CardType == c));

        return new TarotCards.TarotCard(
            vanillaCardList.ElementAt(Random.Range(0,
                vanillaCardList.Count)), 0);
    }

    internal static TarotCards.TarotCard GetRandModdedCard()
    {
        return new TarotCards.TarotCard(
            CustomTarotCardManager.CustomTarotCardList.Keys.ElementAt(Random.Range(0,
                CustomTarotCardManager.CustomTarotCardList.Count)), 0);
    }

    internal static int getTarotMult(TarotCards.Card obj)
    {
        var mult = 0;
        if (DataManager.Instance.dungeonRun < 5) return Mathf.Min(mult, TarotCards.GetMaxTarotCardLevel(obj));

        while (Random.Range(0f, 1f) < 0.275f * DataManager.Instance.GetLuckMultiplier()) mult++;

        return Mathf.Min(mult, TarotCards.GetMaxTarotCardLevel(obj));
    }
}