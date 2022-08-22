using UnityEngine;
using I2.Loc;

namespace COTL_API.CustomTarotCard;

public class CustomTarotCard
{
    public virtual string InternalName { get; set; }
    public TarotCards.Card CardType;
    public TarotCards.CardCategory CardCategory;
    public string ModPrefix;

    public virtual Sprite CardSprite { get; set; }

    public virtual string LocalisedName()
    {
        int upgradeIndex = 0;
        foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
        {
            if (playerRunTrinket.CardType == this.CardType)
            {
                upgradeIndex = playerRunTrinket.UpgradeIndex;
                break;
            }
        }
        return LocalisedName(upgradeIndex);
    }

    public virtual string LocalisedName(int upgradeIndex)
    {
        string text = "";
        for (int i = 0; i < upgradeIndex; i++)
        {
            text += "+";
        }
        string text2 = "";
        switch (upgradeIndex)
        {
            case 1:
                text2 = "<color=green>";
                break;
            case 2:
                text2 = "<color=purple>";
                break;
        }
        return text2 + LocalizationManager.GetTranslation($"TarotCards/{this.ModPrefix}.{this.InternalName}/Name") + text + "</color>";
    }

    public virtual string LocalisedDescription()
    {
        int upgradeIndex = 0;
        foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
        {
            if (playerRunTrinket.CardType == this.CardType)
            {
                upgradeIndex = playerRunTrinket.UpgradeIndex;
                break;
            }
        }
        return LocalisedDescription(upgradeIndex);
    }

    public virtual string LocalisedDescription(int upgradeIndex)
    {
        string text = $"TarotCards/{this.ModPrefix}.{this.InternalName}/Description";

        if (upgradeIndex > 0) text += upgradeIndex;

        return LocalizationManager.GetTranslation(text);
    }

    public virtual string LocalisedLore()
    {
        return LocalizationManager.GetTranslation($"TarotCards/{this.ModPrefix}.{this.InternalName}/Lore");
    }

    public virtual string Skin { get; set; }

    public virtual int TarotCardWeight { get; set; }

    public virtual int MaxTarotCardLevel { get; set; }

    public virtual string AnimationSuffix { get; set; }

    public virtual bool IsCursedRelated { get; set; }

    public virtual float SpiritHeartCount { get; set; }

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

    public CustomTarotCard()
    {
        this.Skin = "Template/Default";
        this.TarotCardWeight = 100;
        this.MaxTarotCardLevel = 0;
        this.AnimationSuffix = "sword";
        this.IsCursedRelated = false;
        this.SpiritHeartCount = 0;
    }
}