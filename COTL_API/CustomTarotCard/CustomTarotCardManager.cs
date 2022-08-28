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
    public static readonly Dictionary<TarotCards.Card, CustomTarotCard> CustomTarotCards = new();

    public static TarotCards.Card Add(CustomTarotCard card)
    {
        string guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        TarotCards.Card cardType = GuidManager.GetEnumValue<TarotCards.Card>(guid, card.InternalName);
        card.CardType = cardType;
        card.ModPrefix = guid;

        CustomTarotCards.Add(cardType, card);

        return cardType;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [HarmonyPatch(typeof(UIManager), nameof(UIManager.ShowTarotChoice))]
    [HarmonyPrefix]
    public static bool UIManager_ShowTarotChoice(UIManager __instance, TarotCards.TarotCard card1,
        TarotCards.TarotCard card2, ref UITarotChoiceOverlayController __result)
    {
        if (!CustomTarotCards.ContainsKey(card1.CardType) && !CustomTarotCards.ContainsKey(card2.CardType)) return true;

        UITarotChoiceOverlayController uiTarotChoiceOverlayController =
            __instance.TarotChoiceOverlayTemplate.Instantiate();
        uiTarotChoiceOverlayController.Show(card1, card2);
        __instance.SetMenuInstance(uiTarotChoiceOverlayController, 1f);

        uiTarotChoiceOverlayController.OnTarotCardSelected += delegate(TarotCards.TarotCard obj)
        {
            if (!CustomTarotCards.ContainsKey(obj.CardType)) return;

            CustomTarotCards[obj.CardType].OnPickup();
        };

        __result = uiTarotChoiceOverlayController;

        return false;
    }

    [HarmonyPatch(typeof(UIWeaponCard), nameof(UIWeaponCard.Play))]
    [HarmonyPostfix]
    public static void UIWeaponCard_Play(UIWeaponCard __instance, TarotCards.TarotCard Card)
    {
        if (!CustomTarotCards.ContainsKey(Card.CardType)) return;

        __instance.NameText.text = CustomTarotCards[Card.CardType].LocalisedName();
        __instance.SubtitleText.text = CustomTarotCards[Card.CardType].LocalisedLore();
        __instance.EffectText.text = CustomTarotCards[Card.CardType].LocalisedDescription();
    }

    [HarmonyPatch(typeof(UIWeaponCard), nameof(UIWeaponCard.Show))]
    [HarmonyPostfix]
    public static void UIWeaponCard_Show(UIWeaponCard __instance, TarotCards.TarotCard Card)
    {
        if (!CustomTarotCards.ContainsKey(Card.CardType)) return;

        __instance.NameText.text = CustomTarotCards[Card.CardType].LocalisedName();
        __instance.SubtitleText.text = CustomTarotCards[Card.CardType].LocalisedLore();
        __instance.EffectText.text = CustomTarotCards[Card.CardType].LocalisedDescription();
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetCardCategory))]
    [HarmonyPrefix]
    public static bool TarotCards_GetCardCategory(TarotCards.Card Type, ref TarotCards.CardCategory __result)
    {
        if (!CustomTarotCards.ContainsKey(Type)) return true;

        __result = CustomTarotCards[Type].Category;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.LocalisedName), new[] { typeof(TarotCards.Card) })]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedName(TarotCards.Card type, ref string __result)
    {
        if (!CustomTarotCards.ContainsKey(type)) return true;

        __result = CustomTarotCards[type].LocalisedName();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.LocalisedName), new[] { typeof(TarotCards.Card), typeof(int) })]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedName(TarotCards.Card Card, int upgradeIndex, ref string __result)
    {
        if (!CustomTarotCards.ContainsKey(Card)) return true;

        __result = CustomTarotCards[Card].LocalisedName(upgradeIndex);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.LocalisedDescription), typeof(TarotCards.Card))]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedDescription(TarotCards.Card Type, ref string __result)
    {
        if (!CustomTarotCards.ContainsKey(Type)) return true;

        __result = CustomTarotCards[Type].LocalisedDescription();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.LocalisedDescription), typeof(TarotCards.Card), typeof(int))]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedDescription(TarotCards.Card Type, int upgradeIndex, ref string __result)
    {
        if (!CustomTarotCards.ContainsKey(Type)) return true;

        __result = CustomTarotCards[Type].LocalisedDescription(upgradeIndex);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.LocalisedLore))]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedLore(TarotCards.Card Type, ref string __result)
    {
        if (!CustomTarotCards.ContainsKey(Type)) return true;

        __result = CustomTarotCards[Type].LocalisedLore();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.Skin))]
    [HarmonyPrefix]
    public static bool TarotCards_Skin(TarotCards.Card Type, ref string __result)
    {
        if (!CustomTarotCards.ContainsKey(Type)) return true;

        __result = CustomTarotCards[Type].Skin;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetTarotCardWeight))]
    [HarmonyPrefix]
    public static bool TarotCards_GetTarotCardWeight(TarotCards.Card cardType, ref int __result)
    {
        if (!CustomTarotCards.ContainsKey(cardType)) return true;

        __result = CustomTarotCards[cardType].TarotCardWeight;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetMaxTarotCardLevel))]
    [HarmonyPrefix]
    public static bool TarotCards_GetMaxTarotCardLevel(TarotCards.Card cardType, ref int __result)
    {
        if (!CustomTarotCards.ContainsKey(cardType)) return true;

        __result = CustomTarotCards[cardType].MaxTarotCardLevel;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.AnimationSuffix))]
    [HarmonyPrefix]
    public static bool TarotCards_AnimationSuffix(TarotCards.Card Type, ref string __result)
    {
        if (!CustomTarotCards.ContainsKey(Type)) return true;

        __result = CustomTarotCards[Type].AnimationSuffix;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.IsCurseRelatedTarotCard))]
    [HarmonyPrefix]
    public static bool TarotCards_IsCurseRelatedTarotCard(TarotCards.Card card, ref bool __result)
    {
        if (!CustomTarotCards.ContainsKey(card)) return true;

        __result = CustomTarotCards[card].IsCursedRelated;

        return false;
    }
}