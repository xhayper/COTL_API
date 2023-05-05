using HarmonyLib;
using Lamb.UI;

namespace COTL_API.CustomTarotCard;

[HarmonyPatch]
public partial class CustomTarotCardManager
{
    private static void UpdateUIWeaponCard(UIWeaponCard UICard, TarotCards.TarotCard Card)
    {
        if (!CustomTarotCardList.ContainsKey(Card.CardType)) return;

        UICard.NameText.text = CustomTarotCardList[Card.CardType].LocalisedName();
        UICard.SubtitleText.text = CustomTarotCardList[Card.CardType].LocalisedLore();
        UICard.EffectText.text = CustomTarotCardList[Card.CardType].LocalisedDescription();
    }

    [HarmonyPatch(typeof(UIWeaponCard), nameof(UIWeaponCard.Play))]
    [HarmonyPostfix]
    private static void UIWeaponCard_Play(UIWeaponCard __instance, TarotCards.TarotCard Card)
    {
        UpdateUIWeaponCard(__instance, Card);
    }

    [HarmonyPatch(typeof(UIWeaponCard), nameof(UIWeaponCard.Show))]
    [HarmonyPostfix]
    private static void UIWeaponCard_Show(UIWeaponCard __instance, TarotCards.TarotCard Card)
    {
        UpdateUIWeaponCard(__instance, Card);
    }

    [HarmonyPatch(typeof(TarotInfoCard), nameof(TarotInfoCard.Configure))]
    [HarmonyPostfix]
    private static void TarotInfoCard_Configure(TarotInfoCard __instance, TarotCards.TarotCard card)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return;

        __instance._tarotCard.Configure(card);
        __instance._itemHeader.text = CustomTarotCardList[card.CardType].LocalisedName();
        __instance._itemLore.text = CustomTarotCardList[card.CardType].LocalisedLore();
        __instance._itemDescription.text = CustomTarotCardList[card.CardType].LocalisedDescription();
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetCardCategory))]
    [HarmonyPrefix]
    private static bool TarotCards_GetCardCategory(TarotCards.Card Type, ref TarotCards.CardCategory __result)
    {
        if (!CustomTarotCardList.ContainsKey(Type)) return true;

        __result = CustomTarotCardList[Type].Category;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.LocalisedName), typeof(TarotCards.Card))]
    [HarmonyPrefix]
    private static bool TarotCards_LocalisedName(TarotCards.Card type, ref string __result)
    {
        if (!CustomTarotCardList.ContainsKey(type)) return true;

        __result = CustomTarotCardList[type].LocalisedName();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.LocalisedName), typeof(TarotCards.Card), typeof(int))]
    [HarmonyPrefix]
    private static bool TarotCards_LocalisedName(TarotCards.Card Card, int upgradeIndex, ref string __result)
    {
        if (!CustomTarotCardList.ContainsKey(Card)) return true;

        __result = CustomTarotCardList[Card].LocalisedName(upgradeIndex);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.LocalisedDescription), typeof(TarotCards.Card))]
    [HarmonyPrefix]
    private static bool TarotCards_LocalisedDescription(TarotCards.Card Type, ref string __result)
    {
        if (!CustomTarotCardList.ContainsKey(Type)) return true;

        __result = CustomTarotCardList[Type].LocalisedDescription();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.LocalisedDescription), typeof(TarotCards.Card), typeof(int))]
    [HarmonyPrefix]
    private static bool TarotCards_LocalisedDescription(TarotCards.Card Type, int upgradeIndex, ref string __result)
    {
        if (!CustomTarotCardList.ContainsKey(Type)) return true;

        __result = CustomTarotCardList[Type].LocalisedDescription(upgradeIndex);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.LocalisedLore))]
    [HarmonyPrefix]
    private static bool TarotCards_LocalisedLore(TarotCards.Card Type, ref string __result)
    {
        if (!CustomTarotCardList.ContainsKey(Type)) return true;

        __result = CustomTarotCardList[Type].LocalisedLore();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.Skin))]
    [HarmonyPrefix]
    private static bool TarotCards_Skin(TarotCards.Card Type, ref string __result)
    {
        if (!CustomTarotCardList.ContainsKey(Type)) return true;

        __result = CustomTarotCardList[Type].Skin;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetTarotCardWeight))]
    [HarmonyPrefix]
    private static bool TarotCards_GetTarotCardWeight(TarotCards.Card cardType, ref int __result)
    {
        if (!CustomTarotCardList.ContainsKey(cardType)) return true;

        __result = CustomTarotCardList[cardType].TarotCardWeight;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetMaxTarotCardLevel))]
    [HarmonyPrefix]
    private static bool TarotCards_GetMaxTarotCardLevel(TarotCards.Card cardType, ref int __result)
    {
        if (!CustomTarotCardList.ContainsKey(cardType)) return true;

        __result = CustomTarotCardList[cardType].MaxTarotCardLevel;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.AnimationSuffix))]
    [HarmonyPrefix]
    private static bool TarotCards_AnimationSuffix(TarotCards.Card Type, ref string __result)
    {
        if (!CustomTarotCardList.ContainsKey(Type)) return true;

        __result = CustomTarotCardList[Type].AnimationSuffix;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.IsCurseRelatedTarotCard))]
    [HarmonyPrefix]
    private static bool TarotCards_IsCurseRelatedTarotCard(TarotCards.Card card, ref bool __result)
    {
        if (!CustomTarotCardList.ContainsKey(card)) return true;

        __result = CustomTarotCardList[card].IsCursedRelated;

        return false;
    }

    //////////////////////////////////////////////////////////////////////////////////

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.ApplyInstantEffects))]
    [HarmonyPrefix]
    private static bool TarotCards_ApplyInstantEffects(TarotCards.TarotCard card)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        CustomTarotCardList[card.CardType].ApplyInstantEffects(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetSpiritHeartCount))]
    [HarmonyPrefix]
    private static bool TarotCards_GetSpiritHeartCount(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCardList[card.CardType].GetSpiritHeartCount(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetSpiritAmmoCount))]
    [HarmonyPrefix]
    private static bool TarotCards_GetSpiritAmmoCount(TarotCards.TarotCard card, ref int __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCardList[card.CardType].GetSpiritAmmoCount(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetWeaponDamageMultiplerIncrease))]
    [HarmonyPrefix]
    private static bool TarotCards_GetWeaponDamageMultiplerIncrease(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCardList[card.CardType].GetWeaponDamageMultiplerIncrease(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetCurseDamageMultiplerIncrease))]
    [HarmonyPrefix]
    private static bool TarotCards_GetCurseDamageMultiplerIncrease(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCardList[card.CardType].GetCurseDamageMultiplerIncrease(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetWeaponCritChanceIncrease))]
    [HarmonyPrefix]
    private static bool TarotCards_GetWeaponCritChanceIncrease(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCardList[card.CardType].GetWeaponCritChanceIncrease(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetLootIncreaseModifier))]
    [HarmonyPrefix]
    private static bool TarotCards_GetLootIncreaseModifier(
        TarotCards.TarotCard card,
        InventoryItem.ITEM_TYPE itemType, ref int __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCardList[card.CardType].GetLootIncreaseModifier(card, itemType);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetMovementSpeedMultiplier))]
    [HarmonyPrefix]
    private static bool TarotCards_GetMovementSpeedMultiplier(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCardList[card.CardType].GetMovementSpeedMultiplier(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetAttackRateMultiplier))]
    [HarmonyPrefix]
    private static bool TarotCards_GetAttackRateMultiplier(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCardList[card.CardType].GetAttackRateMultiplier(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetBlackSoulsMultiplier))]
    [HarmonyPrefix]
    private static bool TarotCards_GetBlackSoulsMultiplier(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCardList[card.CardType].GetBlackSoulsMultiplier(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetHealChance))]
    [HarmonyPrefix]
    private static bool TarotCards_GetHealChance(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCardList[card.CardType].GetHealChance(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetNegateDamageChance))]
    [HarmonyPrefix]
    private static bool TarotCards_GetNegateDamageChance(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCardList[card.CardType].GetNegateDamageChance(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetDamageAllEnemiesAmount))]
    [HarmonyPrefix]
    private static bool TarotCards_GetDamageAllEnemiesAmount(TarotCards.TarotCard card, ref int __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCardList[card.CardType].GetDamageAllEnemiesAmount(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetHealthAmountMultiplier))]
    [HarmonyPrefix]
    private static bool TarotCards_GetHealthAmountMultiplier(TarotCards.TarotCard card, ref int __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCardList[card.CardType].GetHealthAmountMultiplier(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetAmmoEfficiency))]
    [HarmonyPrefix]
    private static bool TarotCards_GetAmmoEfficiency(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCardList[card.CardType].GetAmmoEfficiency(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetBlackSoulsOnDamage))]
    [HarmonyPrefix]
    private static bool TarotCards_GetBlackSoulsOnDamage(TarotCards.TarotCard card, ref int __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCardList[card.CardType].GetBlackSoulsOnDamage(card);

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetItemToDrop))]
    [HarmonyPrefix]
    private static bool TarotCards_GetItemToDrop(TarotCards.TarotCard card, ref InventoryItem __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

#pragma warning disable CS8601
        __result = CustomTarotCardList[card.CardType].GetItemToDrop(card);
#pragma warning restore CS8601

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.GetChanceOfGainingBlueHeart))]
    [HarmonyPrefix]
    private static bool TarotCards_GetChanceOfGainingBlueHeart(TarotCards.TarotCard card, ref float __result)
    {
        if (!CustomTarotCardList.ContainsKey(card.CardType)) return true;

        __result = CustomTarotCardList[card.CardType].GetChanceOfGainingBlueHeart(card);

        return false;
    }
}