using System.Reflection;
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
        var flagActions = new Dictionary<TraitFlags, Action>
        {
            { TraitFlags.STARTING_TRAIT, () => FollowerTrait.StartingTraits.Add(item.TraitType) },
            { TraitFlags.FAITHFUL_TRAIT, () => FollowerTrait.FaithfulTraits.Add(item.TraitType) },
            { TraitFlags.RARE_STARTING_TRAIT, () => FollowerTrait.RareStartingTraits.Add(item.TraitType) },
            { TraitFlags.SINGLE_TRAIT, () => FollowerTrait.SingleTraits.Add(item.TraitType) },
            { TraitFlags.SIN_TRAIT, () => FollowerTrait.SinTraits.Add(item.TraitType) },
            { TraitFlags.EXCLUDE_FROM_MATING, () => FollowerTrait.ExcludedFromMating.Add(item.TraitType) },
            { TraitFlags.PURE_BLOOD_TRAIT, () => FollowerTrait.PureBloodTraits.Add(item.TraitType) },
            { TraitFlags.REQUIRES_ONBOARDING_COMPLETE, () => FollowerTrait.RequiresOnboardingCompleted.Add(item.TraitType) }
        };
        
        foreach (var flagAction in flagActions)
        {
            if (item.TraitFlags.HasFlag(flagAction.Key))
            {
                flagAction.Value();
            }
        }
    }
}
