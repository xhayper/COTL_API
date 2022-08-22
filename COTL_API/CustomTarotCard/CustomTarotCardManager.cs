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
    public static bool TarotCards_GetCardCategory(TarotCards.Card card, ref TarotCards.CardCategory __result)
    {
        if (!customTarotCards.ContainsKey(card)) return true;

        __result = customTarotCards[card].CardCategory;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "LocalisedName", typeof(TarotCards.Card))]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedName(TarotCards.Card Type, ref string __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].LocalisedName();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "LocalisedName", typeof(TarotCards.Card), typeof(int))]
    [HarmonyPrefix]
    public static bool TarotCards_LocalisedName(TarotCards.Card Type, int upgradeIndex, ref string __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].LocalisedName(upgradeIndex);

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
    public static bool TarotCards_GetTarotCardWeight(TarotCards.Card Type, ref int __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].TarotCardWeight;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetMaxTarotCardLevel")]
    [HarmonyPrefix]
    public static bool TarotCards_GetMaxTarotCardLevel(TarotCards.Card Type, ref int __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].MaxTarotCardLevel;

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
    public static bool TarotCards_IsCurseRelatedTarotCard(TarotCards.Card Type, ref bool __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].IsCursedRelated;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetSpiritHeartCount")]
    [HarmonyPrefix]
    public static bool TarotCards_GetSpiritHeartCount(TarotCards.Card Type, ref float __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].SpiritHeartCount;

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetSpiritAmmoCount")]
    [HarmonyPrefix]
    public static bool TarotCards_GetSpiritAmmoCount(TarotCards.Card Type, ref float __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].GetSpiritAmmoCount();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetWeaponDamageMultiplerIncrease")]
    [HarmonyPrefix]
    public static bool TarotCards_GetWeaponDamageMultiplerIncrease(TarotCards.Card Type, ref float __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].GetWeaponDamageMultiplerIncrease();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetCurseDamageMultiplerIncrease")]
    [HarmonyPrefix]
    public static bool TarotCards_GetCurseDamageMultiplerIncrease(TarotCards.Card Type, ref float __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].GetCurseDamageMultiplerIncrease();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetWeaponCritChanceIncrease")]
    [HarmonyPrefix]
    public static bool TarotCards_GetWeaponCritChanceIncrease(TarotCards.Card Type, ref float __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].GetWeaponCritChanceIncrease();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetLootIncreaseModifier")]
    [HarmonyPrefix]
    public static bool TarotCards_GetLootIncreaseModifier(TarotCards.Card Type, ref int __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].GetLootIncreaseModifier();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetMovementSpeedMultiplier")]
    [HarmonyPrefix]
    public static bool TarotCards_GetMovementSpeedMultiplier(TarotCards.Card Type, ref float __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].GetMovementSpeedMultiplier();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetAttackRateMultiplier")]
    [HarmonyPrefix]
    public static bool TarotCards_GetAttackRateMultiplier(TarotCards.Card Type, ref float __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].GetAttackRateMultiplier();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetBlackSoulsMultiplier")]
    [HarmonyPrefix]
    public static bool TarotCards_GetBlackSoulsMultiplier(TarotCards.Card Type, ref float __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].GetBlackSoulsMultiplier();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetHealChance")]
    [HarmonyPrefix]
    public static bool TarotCards_GetHealChance(TarotCards.Card Type, ref float __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].GetHealChance();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetNegateDamageChance")]
    [HarmonyPrefix]
    public static bool TarotCards_GetNegateDamageChance(TarotCards.Card Type, ref float __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].GetNegateDamageChance();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetDamageAllEnemiesAmount")]
    [HarmonyPrefix]
    public static bool TarotCards_GetDamageAllEnemiesAmount(TarotCards.Card Type, ref int __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].GetDamageAllEnemiesAmount();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetHealthAmountMultiplier")]
    [HarmonyPrefix]
    public static bool TarotCards_GetHealthAmountMultiplier(TarotCards.Card Type, ref int __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].GetHealthAmountMultiplier();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetAmmoEfficiency")]
    [HarmonyPrefix]
    public static bool TarotCards_GetAmmoEfficiency(TarotCards.Card Type, ref float __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].GetAmmoEfficiency();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetBlackSoulsOnDamage")]
    [HarmonyPrefix]
    public static bool TarotCards_GetBlackSoulsOnDamage(TarotCards.Card Type, ref int __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].GetBlackSoulsOnDamage();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetItemToDrop")]
    [HarmonyPrefix]
    public static bool TarotCards_GetItemToDrop(TarotCards.Card Type, ref InventoryItem __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].GetItemToDrop();

        return false;
    }

    [HarmonyPatch(typeof(TarotCards), "GetChanceOfGainingBlueHeart")]
    [HarmonyPrefix]
    public static bool TarotCards_GetChanceOfGainingBlueHeart(TarotCards.Card Type, ref float __result)
    {
        if (!customTarotCards.ContainsKey(Type)) return true;

        __result = customTarotCards[Type].GetChanceOfGainingBlueHeart();

        return false;
    }
}