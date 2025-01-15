using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine.ProBuilder.Shapes;

namespace FoodPlus.CustomTraits;

public abstract class CustomTrait
{
    public abstract string InternalName { get; }

    internal FollowerTrait.TraitType TraitType;
    internal string ModPrefix = "";
    
    public virtual string LocalizedTitle() => LocalizationManager.GetTranslation($"Traits/{ModPrefix}.{InternalName}");
    
    public virtual string LocalizedDescription() => LocalizationManager.GetTranslation($"Traits/{InternalName}.description");

    public virtual bool IsTraitUnavailable() => false;

    public virtual TraitFlags TraitFlags => TraitFlags.None;
    
    public virtual List<FollowerTrait.TraitType> ExclusiveTraits => [];
    public virtual Sprite Icon => null;
    public virtual bool Positive => true;
    
}

[Flags]
public enum TraitFlags
{
    None = 0,
    StartingTrait = 1 << 0,
    FaithfulTrait = 1 << 1,
    RareStartingTrait = 1 << 2,
    SingleTrait = 1 << 3,
    ExcludeFromMating = 1 << 4,
    SinTrait = 1 << 5,
    PureBloodTrait = 1 << 6,
    RequiresOnboardingComplete = 1 << 7,
}