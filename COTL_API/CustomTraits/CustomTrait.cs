using I2.Loc;
using UnityEngine;

namespace FoodPlus.CustomTraits;

public abstract class CustomTrait
{
    public abstract string InternalName { get; }

    internal FollowerTrait.TraitType TraitType;
    internal string ModPrefix = "";
    
    public virtual string LocalizedTitle() => LocalizationManager.GetTranslation($"Traits/{ModPrefix}.{InternalName}");
    
    public virtual string LocalizedDescription() => LocalizationManager.GetTranslation($"Traits/{InternalName}.description");

    public virtual bool IsTraitUnavailable() => false;

    public virtual TraitFlags TraitFlags => TraitFlags.NONE;
    
    public virtual List<FollowerTrait.TraitType> ExclusiveTraits => [];
    public virtual Sprite Icon => TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));
    public virtual bool Positive => true;
    
}

[Flags]
public enum TraitFlags
{
    NONE = 0,
    STARTING_TRAIT = 1 << 0,
    FAITHFUL_TRAIT = 1 << 1,
    RARE_STARTING_TRAIT = 1 << 2,
    SINGLE_TRAIT = 1 << 3,
    EXCLUDE_FROM_MATING = 1 << 4,
    SIN_TRAIT = 1 << 5,
    PURE_BLOOD_TRAIT = 1 << 6,
    REQUIRES_ONBOARDING_COMPLETE = 1 << 7,
}