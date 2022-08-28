using UnityEngine;
using I2.Loc;

namespace COTL_API.CustomTarotCard;

public class CustomTarotCard
{
    public virtual string InternalName { get; }
    public TarotCards.CardCategory Category;
    public TarotCards.Card CardType;
    public string ModPrefix;

    public virtual Sprite CardSprite { get; internal set; }

    public virtual string LocalisedName()
    {
        int upgradeIndex = 0;
        foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
        {
            if (playerRunTrinket.CardType != CardType) continue;

            upgradeIndex = playerRunTrinket.UpgradeIndex;

            break;
        }

        return LocalisedName(upgradeIndex);
    }

    public virtual string LocalisedName(int upgradeIndex)
    {
        string text = "";
        for (int i = 0; i < upgradeIndex; i++) text += "+";

        string text2 = upgradeIndex switch {
            1 => "<color=green>",
            2 => "<color=purple>",
            _ => ""
        };
        return text2 + LocalizationManager.GetTranslation($"TarotCards/{ModPrefix}.{InternalName}/Name") + text +
               "</color>";
    }

    public virtual string LocalisedDescription()
    {
        int upgradeIndex = 0;
        foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
        {
            if (playerRunTrinket.CardType != CardType) continue;
            upgradeIndex = playerRunTrinket.UpgradeIndex;
            break;
        }

        return LocalisedDescription(upgradeIndex);
    }

    public virtual string LocalisedDescription(int upgradeIndex)
    {
        string text = $"TarotCards/{ModPrefix}.{InternalName}/Description";

        if (upgradeIndex > 0) text += upgradeIndex;

        return LocalizationManager.GetTranslation(text);
    }

    public virtual string LocalisedLore()
    {
        return LocalizationManager.GetTranslation($"TarotCards/{ModPrefix}.{InternalName}/Lore");
    }

    public virtual string Skin { get; } = "";

    public virtual int TarotCardWeight { get; } = 150;

    public virtual int MaxTarotCardLevel { get; } = 0;

    public virtual string AnimationSuffix => $"Card {ModPrefix}.{InternalName} Animation Suffix not set";

    public virtual bool IsCursedRelated { get; } = false;

    public virtual float GetSpiritHeartCount(TarotCards.TarotCard card)
    {
        return 0f;
    }

    public virtual int GetSpiritAmmoCount(TarotCards.TarotCard card)
    {
        return 0;
    }

    public virtual float GetWeaponDamageMultiplerIncrease(TarotCards.TarotCard card)
    {
        return 0f;
    }

    public virtual float GetCurseDamageMultiplerIncrease(TarotCards.TarotCard card)
    {
        return 0f;
    }

    public virtual float GetWeaponCritChanceIncrease(TarotCards.TarotCard card)
    {
        return 0f;
    }

    public virtual int GetLootIncreaseModifier(TarotCards.TarotCard card, InventoryItem.ITEM_TYPE itemType)
    {
        return 0;
    }

    public virtual float GetMovementSpeedMultiplier(TarotCards.TarotCard card)
    {
        return 0f;
    }

    public virtual float GetAttackRateMultiplier(TarotCards.TarotCard card)
    {
        return 0f;
    }

    public virtual float GetBlackSoulsMultiplier(TarotCards.TarotCard card)
    {
        return 0f;
    }

    public virtual float GetHealChance(TarotCards.TarotCard card)
    {
        return 0f;
    }

    public virtual float GetNegateDamageChance(TarotCards.TarotCard card)
    {
        return 0f;
    }

    public virtual int GetDamageAllEnemiesAmount(TarotCards.TarotCard card)
    {
        return 0;
    }

    public virtual int GetHealthAmountMultiplier(TarotCards.TarotCard card)
    {
        return 0;
    }

    public virtual float GetAmmoEfficiency(TarotCards.TarotCard card)
    {
        return 0f;
    }

    public virtual int GetBlackSoulsOnDamage(TarotCards.TarotCard card)
    {
        return 0;
    }

    public virtual InventoryItem GetItemToDrop(TarotCards.TarotCard card)
    {
        return null;
    }

    public virtual float GetChanceOfGainingBlueHeart(TarotCards.TarotCard card)
    {
        return 0f;
    }

    public virtual void ApplyInstantEffects(TarotCards.TarotCard card) { }
}