using System.Collections.Generic;
using System.Reflection;
using COTL_API.CustomInventory;
using COTL_API.Guid;

namespace FoodPlus.CustomTraits;

public static partial class CustomTraitManager
{
    private static Dictionary<FollowerTrait.TraitType, CustomTrait> CustomTraitList { get; } = [];
    
    public static FollowerTrait.TraitType Add(CustomTrait trait)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var traitType = GuidManager.GetEnumValue<FollowerTrait.TraitType>(guid, trait.InternalName);
        trait.TraitType = traitType;
        trait.ModPrefix = guid;
        
        HandleTraitFlags(trait);
        foreach (var exclusive in trait.ExclusiveTraits)
        {
            FollowerTrait.ExclusiveTraits.Add(traitType, exclusive);
        }

        CustomTraitList.Add(traitType, trait);

        return traitType;
    }

    private static void HandleTraitFlags(CustomTrait item)
    {
        if (item.TraitFlags.HasFlag(TraitFlags.StartingTrait))
            FollowerTrait.StartingTraits.Add(item.TraitType);
        
        if (item.TraitFlags.HasFlag(TraitFlags.FaithfulTrait))
            FollowerTrait.FaithfulTraits.Add(item.TraitType);
        
        if (item.TraitFlags.HasFlag(TraitFlags.RareStartingTrait))
            FollowerTrait.RareStartingTraits.Add(item.TraitType);
        
        if (item.TraitFlags.HasFlag(TraitFlags.SingleTrait))
            FollowerTrait.SingleTraits.Add(item.TraitType);
        
        if (item.TraitFlags.HasFlag(TraitFlags.ExcludeFromMating))
            FollowerTrait.ExcludedFromMating.Add(item.TraitType);
        
        if (item.TraitFlags.HasFlag(TraitFlags.PureBloodTrait))
            FollowerTrait.PureBloodTraits.Add(item.TraitType);
        
        if (item.TraitFlags.HasFlag(TraitFlags.RequiresOnboardingComplete))
            FollowerTrait.RequiresOnboardingCompleted.Add(item.TraitType);
    }
}
