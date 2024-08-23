using COTL_API.Guid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COTL_API.CustomInventory;

public static partial class CustomMealEffectManager 
{
    public static Dictionary<CookingData.MealEffectType, CustomMealEffect> CustomEffectList { get; } = new();

    public static CookingData.MealEffectType Add(CustomMealEffect effect)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var effectType = GuidManager.GetEnumValue<CookingData.MealEffectType>(guid, effect.InternalName);
        effect._mealEffect = effectType;
        effect._modPrefix = guid;
        effect._internalObjectName = $"CustomItem_{effect.InternalName}";

        CustomEffectList.Add(effectType, effect);

        return effectType;
    }
}

public abstract class CustomMealEffect
{
    internal string _internalObjectName = "";
    internal CookingData.MealEffectType _mealEffect;
    internal string _modPrefix = "";

    public abstract string InternalName { get; }

    /// <summary>
    /// This is the effect that will occur when the follower eats this dish.
    /// </summary>
    public abstract Action<FollowerBrain> Effect { get; }
}
