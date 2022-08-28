using System.Collections.Generic;
using System.Reflection;
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
        if (!DataManager.AllTrinkets.Contains(cardType))
            DataManager.AllTrinkets.Add(cardType);

        return cardType;
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

    [HarmonyPatch(typeof(TarotInfoCard), nameof(TarotInfoCard.Configure))]
    [HarmonyPostfix]
    public static void TarotInfoCard_Configure(TarotInfoCard __instance, TarotCards.TarotCard card)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return;

        __instance._tarotCard.Configure(card);
        __instance._itemHeader.text = CustomTarotCards[card.CardType].LocalisedName();
        __instance._itemLore.text = CustomTarotCards[card.CardType].LocalisedLore();
        __instance._itemDescription.text = CustomTarotCards[card.CardType].LocalisedDescription();
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

    //////////////////////////////////////////////////////////////////////////////////

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.ApplyInstantEffects))]
    [HarmonyPrefix]
    public static bool TarotCards_ApplyInstantEffects(TarotCards.TarotCard card)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        CustomTarotCards[card.CardType].ApplyInstantEffects(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetSpiritHeartCount))]
    [HarmonyPrefix]
    public static bool TarotCards_GetSpiritHeartCount(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetSpiritHeartCount(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetSpiritAmmoCount))]
    [HarmonyPrefix]
    public static bool TarotCards_GetSpiritAmmoCount(TarotCards.TarotCard card, ref int __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetSpiritAmmoCount(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetWeaponDamageMultiplerIncrease))]
    [HarmonyPrefix]
    public static bool TarotCards_GetWeaponDamageMultiplerIncrease(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetWeaponDamageMultiplerIncrease(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetCurseDamageMultiplerIncrease))]
    [HarmonyPrefix]
    public static bool TarotCards_GetCurseDamageMultiplerIncrease(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetCurseDamageMultiplerIncrease(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetWeaponCritChanceIncrease))]
    [HarmonyPrefix]
    public static bool TarotCards_GetWeaponCritChanceIncrease(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetWeaponCritChanceIncrease(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetLootIncreaseModifier))]
    [HarmonyPrefix]
    public static bool TarotCards_GetLootIncreaseModifier(
        TarotCards.TarotCard card,
        InventoryItem.ITEM_TYPE itemType, ref int __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetLootIncreaseModifier(card, itemType);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetMovementSpeedMultiplier))]
    [HarmonyPrefix]
    public static bool TarotCards_GetMovementSpeedMultiplier(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetMovementSpeedMultiplier(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetAttackRateMultiplier))]
    [HarmonyPrefix]
    public static bool TarotCards_GetAttackRateMultiplier(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetAttackRateMultiplier(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetBlackSoulsMultiplier))]
    [HarmonyPrefix]
    public static bool TarotCards_GetBlackSoulsMultiplier(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetBlackSoulsMultiplier(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetHealChance))]
    [HarmonyPrefix]
    public static bool TarotCards_GetHealChance(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetHealChance(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetNegateDamageChance))]
    [HarmonyPrefix]
    public static bool TarotCards_GetNegateDamageChance(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetNegateDamageChance(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetDamageAllEnemiesAmount))]
    [HarmonyPrefix]
    public static bool TarotCards_GetDamageAllEnemiesAmount(TarotCards.TarotCard card, ref int __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetDamageAllEnemiesAmount(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetHealthAmountMultiplier))]
    [HarmonyPrefix]
    public static bool TarotCards_GetHealthAmountMultiplier(TarotCards.TarotCard card, ref int __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetHealthAmountMultiplier(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetAmmoEfficiency))]
    [HarmonyPrefix]
    public static bool TarotCards_GetAmmoEfficiency(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetAmmoEfficiency(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetBlackSoulsOnDamage))]
    [HarmonyPrefix]
    public static bool TarotCards_GetBlackSoulsOnDamage(TarotCards.TarotCard card, ref int __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetBlackSoulsOnDamage(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetItemToDrop))]
    [HarmonyPrefix]
    public static bool TarotCards_GetItemToDrop(TarotCards.TarotCard card, ref InventoryItem __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetItemToDrop(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetChanceOfGainingBlueHeart))]
    [HarmonyPrefix]
    public static bool TarotCards_GetChanceOfGainingBlueHeart(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCards.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCards[card.CardType].GetChanceOfGainingBlueHeart(card);

        return false;
    }
}