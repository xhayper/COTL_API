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
        LogInfo("Regestering effect with effecType " + effectType);
        effect._mealEffect = effectType;
        effect._modPrefix = guid;
        effect._internalObjectName = $"CustomEffect_{effect.InternalName}";

        CustomEffectList.Add(effectType, effect);

        return effectType;
    }
}

