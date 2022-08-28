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

    public virtual string Skin { get; } = TarotCards.Skin(TarotCards.Card.Hearts1);

    public virtual int TarotCardWeight { get; } = 100;

    public virtual int MaxTarotCardLevel { get; } = 0;

    public virtual string AnimationSuffix { get; } = "sword";

    public virtual bool IsCursedRelated { get; } = false;

    public virtual float SpiritHeartCount { get; } = 0;

    public virtual float GetSpiritAmmoCount()
    {
        return 0f;
    }

    public virtual float GetWeaponDamageMultiplerIncrease()
    {
        return 0f;
    }

    public virtual float GetCurseDamageMultiplerIncrease()
    {
        return 0f;
    }

    public virtual float GetWeaponCritChanceIncrease()
    {
        return 0f;
    }

    public virtual int GetLootIncreaseModifier()
    {
        return 0;
    }

    public virtual float GetMovementSpeedMultiplier()
    {
        return 0f;
    }

    public virtual float GetAttackRateMultiplier()
    {
        return 0f;
    }

    public virtual float GetBlackSoulsMultiplier()
    {
        return 0f;
    }

    public virtual float GetHealChance()
    {
        return 0f;
    }

    public virtual float GetNegateDamageChance()
    {
        return 0f;
    }

    public virtual int GetDamageAllEnemiesAmount()
    {
        return 0;
    }

    public virtual int GetHealthAmountMultiplier()
    {
        return 0;
    }

    public virtual float GetAmmoEfficiency()
    {
        return 0f;
    }

    public virtual int GetBlackSoulsOnDamage()
    {
        return 0;
    }

    public virtual InventoryItem GetItemToDrop()
    {
        return null;
    }

    public virtual float GetChanceOfGainingBlueHeart()
    {
        return 0f;
    }

    public virtual void OnPickup() { }
}