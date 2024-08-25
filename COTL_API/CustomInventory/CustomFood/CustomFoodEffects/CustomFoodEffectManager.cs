using System.Reflection;
using COTL_API.Guid;

namespace COTL_API.CustomInventory;

public static partial class CustomFoodEffectManager
{
    public static Dictionary<CookingData.MealEffectType, CustomFoodEffect> CustomEffectList { get; } = [];

    public static CookingData.MealEffectType Add(CustomFoodEffect effect)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var effectType = GuidManager.GetEnumValue<CookingData.MealEffectType>(guid, effect.InternalName);
        effect.MealEffect = effectType;
        effect.ModPrefix = guid;
        effect.InternalObjectName = $"CustomMealEffect_{effect.InternalName}";

        CustomEffectList.Add(effectType, effect);

        return effectType;
    }
}