using I2.Loc;
using UnityEngine;

namespace COTL_API.CustomRelics;

public abstract class CustomRelicData : RelicData
{
    internal RelicType RelicType;

    internal string ModPrefix = "";
    public abstract string InternalName { get; }

    public virtual string GetTitleLocalisation() => LocalizationManager.GetTermTranslation($"Relics/{ModPrefix}.{InternalName}");

    public virtual string GetDescriptionLocalisation() => LocalizationManager.GetTermTranslation($"Relics/{ModPrefix}.{InternalName}/Description");

    public virtual string GetLoreLocalization() => LocalizationManager.GetTermTranslation($"Relics/{ModPrefix}.{InternalName}/Lore");

    public virtual RelicChargeCategory GetChargeCategory()
    {
        if (DamageRequiredToCharge < 50.0)
            return RelicChargeCategory.Fast;
        return DamageRequiredToCharge < 80.0 ? RelicChargeCategory.Average : RelicChargeCategory.Slow;
    }

    public virtual void OnUse(bool forceConsumableAnimation)
    {
        
    }
}