using System.Collections.Generic;
using System.Reflection;
using src.Extensions;
using COTL_API.Guid;
using HarmonyLib;
using Lamb.UI;

namespace COTL_API.CustomTarotCard;

[HarmonyPatch]
public class CustomTarotCardManager
{
    public static Dictionary<TarotCards.Card, CustomTarotCard> customTarotCards = new();

    public static TarotCards.Card Add(CustomTarotCard card)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var cardType = GuidManager.GetEnumValue<TarotCards.Card>(guid, card.InternalName);
        card.CardType = cardType;
        card.ModPrefix = guid;

        customTarotCards.Add(cardType, card);

        return cardType;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [HarmonyPatch(typeof(UIManager), nameof(UIManager.ShowTarotChoice))]
    [HarmonyPrefix]
    public static bool UIManager_ShowTarotChoice(UIManager __instance, TarotCards.TarotCard card1, TarotCards.TarotCard card2, ref UITarotChoiceOverlayController __result)
    {
        if (!customTarotCards.ContainsKey(card1.CardType) && !customTarotCards.ContainsKey(card2.CardType)) return true;

        var uiTarotChoiceOverlayController = __instance.TarotChoiceOverlayTemplate.Instantiate<UITarotChoiceOverlayController>();
        uiTarotChoiceOverlayController.Show(card1, card2, false);
        __instance.SetMenuInstance(uiTarotChoiceOverlayController, 1f, false);

        uiTarotChoiceOverlayController.OnTarotCardSelected += delegate (TarotCards.TarotCard obj)
        {
            if (!customTarotCards.ContainsKey(obj.CardType)) return;

            customTarotCards[obj.CardType].OnPickup();
        };

        __result = uiTarotChoiceOverlayController;

        return false;
    }

    [HarmonyPatch(typeof(UIWeaponCard), nameof(UIWeaponCard.Play))]
    [HarmonyPostfix]
    public static void UIWeaponCard_Play(UIWeaponCard __instance, TarotCards.TarotCard Card)
    {
        if (!customTarotCards.ContainsKey(Card.CardType)) return;

        __instance.NameText.text = customTarotCards[Card.CardType].LocalisedName();
        __instance.SubtitleText.text = customTarotCards[Card.CardType].LocalisedLore();
        __instance.EffectText.text = customTarotCards[Card.CardType].LocalisedDescription();
    }
    
    [HarmonyPatch(typeof(UIWeaponCard), nameof(UIWeaponCard.Show))]
    [HarmonyPostfix]
    public static void UIWeaponCard_Show(UIWeaponCard __instance, TarotCards.TarotCard Card)
    {
        if (!customTarotCards.ContainsKey(Card.CardType)) return;

        __instance.NameText.text = customTarotCards[Card.CardType].LocalisedName();
        __instance.SubtitleText.text = customTarotCards[Card.CardType].LocalisedLore();
        __instance.EffectText.text = customTarotCards[Card.CardType].LocalisedDescription();
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetCardCategory))]
    [HarmonyPrefix]
    public static bool TarotCards_GetCardCategory(TarotCards.Card Type, ref TarotCards.CardCategory __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].CardCategory;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.LocalisedName), new System.Type[] { typeof(TarotCards.Card) })]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedName(TarotCards.Card type, ref string __result)
    {
        if (!customTarotCards.ContainsKey(type)) return true;

        __result = customTarotCards[type].LocalisedName();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.LocalisedName), new System.Type[] { typeof(TarotCards.Card), typeof(int) })]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedName(TarotCards.Card Card, int upgradeIndex, ref string __result)
    {
        if (!customTarotCards.ContainsKey(Card)) return true;

        __result = customTarotCards[Card].LocalisedName(upgradeIndex);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.LocalisedDescription), typeof(TarotCards.Card))]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedDescription(TarotCards.Card Type, ref string __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].LocalisedDescription();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.LocalisedDescription), typeof(TarotCards.Card), typeof(int))]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedDescription(TarotCards.Card Type, int upgradeIndex, ref string __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].LocalisedDescription(upgradeIndex);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.LocalisedLore))]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedLore(TarotCards.Card Type, ref string __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].LocalisedLore();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.Skin))]
    [HarmonyPrefix]
    public static bool TarotCards_Skin(TarotCards.Card Type, ref string __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].Skin;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetTarotCardWeight))]
    [HarmonyPrefix]
    public static bool TarotCards_GetTarotCardWeight(TarotCards.Card cardType, ref int __result)
    {
        if (!customTarotCards.ContainsKey(cardType)) return true;

        __result = customTarotCards[cardType].TarotCardWeight;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetMaxTarotCardLevel))]
    [HarmonyPrefix]
    public static bool TarotCards_GetMaxTarotCardLevel(TarotCards.Card cardType, ref int __result)
    {
        if (!customTarotCards.ContainsKey(cardType)) return true;

        __result = customTarotCards[cardType].MaxTarotCardLevel;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.AnimationSuffix))]
    [HarmonyPrefix]
    public static bool TarotCards_AnimationSuffix(TarotCards.Card Type, ref string __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].AnimationSuffix;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.IsCurseRelatedTarotCard))]
    [HarmonyPrefix]
    public static bool TarotCards_IsCurseRelatedTarotCard(TarotCards.Card card, ref bool __result)
    {
        if (!customTarotCards.ContainsKey(card)) return true;

        __result = customTarotCards[card].IsCursedRelated;

        return false;
    }
}