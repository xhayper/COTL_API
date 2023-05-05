
using UnityEngine;
using System.Text;
using I2.Loc;

namespace COTL_API.CustomTarotCard;

public abstract class CustomTarotCard
{
    internal TarotCards.Card CardType;

    internal string ModPrefix = "";
    public abstract string InternalName { get; }

    public virtual TarotCards.CardCategory Category { get; }
    public virtual Sprite? CardSprite { get; internal set; }

    public abstract string Skin { get; }

    public virtual int TarotCardWeight => 150;

    public virtual int MaxTarotCardLevel => 0;

    public virtual string AnimationSuffix => $"Card {ModPrefix}.{InternalName} Animation Suffix not set";

    public virtual bool IsCursedRelated => false;

    public virtual string LocalisedName()
    {
        var upgradeIndex =
            (from playerRunTrinket in DataManager.Instance.PlayerRunTrinkets
                where playerRunTrinket.CardType == CardType
                select playerRunTrinket.UpgradeIndex).FirstOrDefault();

        return LocalisedName(upgradeIndex);
    }

    public virtual string LocalisedName(int upgradeIndex)
    {
        StringBuilder text = new("");
        for (var i = 0; i < upgradeIndex; i++) text.Append("+");

        var text2 = upgradeIndex switch
        {
            1 => "<color=green>",
            2 => "<color=purple>",
            _ => ""
        };

        return text2 + LocalizationManager.GetTranslation($"TarotCards/{ModPrefix}.{InternalName}/Name{text}</color>");
    }

    public virtual string LocalisedDescription()
    {
        var upgradeIndex =
            (from playerRunTrinket in DataManager.Instance.PlayerRunTrinkets
                where playerRunTrinket.CardType == CardType
                select playerRunTrinket.UpgradeIndex).FirstOrDefault();

        return LocalisedDescription(upgradeIndex);
    }

    public virtual string LocalisedDescription(int upgradeIndex)
    {
        var text = $"TarotCards/{ModPrefix}.{InternalName}/Description";

        if (upgradeIndex > 0) text += upgradeIndex;

        return LocalizationManager.GetTranslation(text);
    }

    public virtual string LocalisedLore()
    {
        return LocalizationManager.GetTranslation($"TarotCards/{ModPrefix}.{InternalName}/Lore");
    }

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

    public virtual InventoryItem? GetItemToDrop(TarotCards.TarotCard card)
    {
        return null;
    }

    public virtual float GetChanceOfGainingBlueHeart(TarotCards.TarotCard card)
    {
        return 0f;
    }

    public virtual void ApplyInstantEffects(TarotCards.TarotCard card)
    {
    }
}