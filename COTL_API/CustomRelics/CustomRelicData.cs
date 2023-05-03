using COTL_API.Helpers;
using I2.Loc;
using UnityEngine;

namespace COTL_API.CustomRelics;

public abstract class CustomRelicData : RelicData
{
    internal string ModPrefix = "";
    public abstract string InternalName { get; }

    public virtual string GetTitleLocalisation() => LocalizationManager.GetTermTranslation($"Relics/{ModPrefix}.{InternalName}");

    public virtual string GetDescriptionLocalisation() => LocalizationManager.GetTermTranslation($"Relics/{ModPrefix}.{InternalName}/Description");

    public virtual string GetLoreLocalization() => LocalizationManager.GetTermTranslation($"Relics/{ModPrefix}.{InternalName}/Lore");

    public virtual bool CanBeBlessed => true;
    public virtual bool CanBeDamned => true;
    
    public CustomRelicData()
    {
        UISprite = TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));
        UISpriteOutline = TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));
        WorldSprite = TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));
        InteractionType = RelicInteractionType.Charging;
    }

    public virtual void Init() { }
    
    public virtual RelicChargeCategory GetChargeCategory()
    {
        if (DamageRequiredToCharge < 50.0)
            return RelicChargeCategory.Fast;
        return DamageRequiredToCharge < 80.0 ? RelicChargeCategory.Average : RelicChargeCategory.Slow;
    }

    public abstract void OnUse(bool forceConsumableAnimation = false);
    public virtual void OnUseBlessed(bool forceConsumableAnimation = false) => OnUse(forceConsumableAnimation);
    public virtual void OnUseDamned(bool forceConsumableAnimation = false) => OnUse(forceConsumableAnimation);

    public CustomRelicData ToBlessed()
    {
        var clone = (CustomRelicData)MemberwiseClone();
        clone.RelicSubType = RelicSubType.Blessed;
        return clone;
    }
    
    public CustomRelicData ToDamned()
    {
        var clone = (CustomRelicData)MemberwiseClone();
        clone.RelicSubType = RelicSubType.Dammed;
        return clone;
    }

}