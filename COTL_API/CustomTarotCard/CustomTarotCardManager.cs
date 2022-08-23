using System.Collections.Generic;
using System.Reflection;
using COTL_API.Guid;
using HarmonyLib;

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

    [HarmonyPatch(typeof(TarotCards), "GetCardCategory")]
    [HarmonyPrefix]
    public static bool TarotCards_GetCardCategory(TarotCards.Card Type, ref TarotCards.CardCategory __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].CardCategory;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "LocalisedName", new System.Type[] { typeof(TarotCards.Card) })]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedName(TarotCards.Card type, ref string __result)
    {
        if (!customTarotCards.ContainsKey(type)) return true;

        __result = customTarotCards[type].LocalisedName();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "LocalisedName", new System.Type[] { typeof(TarotCards.Card), typeof(int) })]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedName(TarotCards.Card Card, int upgradeIndex, ref string __result)
    {
        if (!customTarotCards.ContainsKey(Card)) return true;

        __result = customTarotCards[Card].LocalisedName(upgradeIndex);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "LocalisedDescription", typeof(TarotCards.Card))]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedDescription(TarotCards.Card Type, ref string __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].LocalisedDescription();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "LocalisedDescription", typeof(TarotCards.Card), typeof(int))]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedDescription(TarotCards.Card Type, int upgradeIndex, ref string __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].LocalisedDescription(upgradeIndex);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "LocalisedLore")]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedLore(TarotCards.Card Type, ref string __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].LocalisedLore();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "Skin")]
    [HarmonyPrefix]
    public static bool TarotCards_Skin(TarotCards.Card Type, ref string __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].Skin;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetTarotCardWeight")]
    [HarmonyPrefix]
    public static bool TarotCards_GetTarotCardWeight(TarotCards.Card cardType, ref int __result)
    {
        if (!customTarotCards.ContainsKey(cardType)) return true;

        __result = customTarotCards[cardType].TarotCardWeight;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetMaxTarotCardLevel")]
    [HarmonyPrefix]
    public static bool TarotCards_GetMaxTarotCardLevel(TarotCards.Card cardType, ref int __result)
    {
        if (!customTarotCards.ContainsKey(cardType)) return true;

        __result = customTarotCards[cardType].MaxTarotCardLevel;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "AnimationSuffix")]
    [HarmonyPrefix]
    public static bool TarotCards_AnimationSuffix(TarotCards.Card Type, ref string __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].AnimationSuffix;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "IsCurseRelatedTarotCard")]
    [HarmonyPrefix]
    public static bool TarotCards_IsCurseRelatedTarotCard(TarotCards.Card card, ref bool __result)
    {
        if (!customTarotCards.ContainsKey(card)) return true;

        __result = customTarotCards[card].IsCursedRelated;

        return false;
    }
}