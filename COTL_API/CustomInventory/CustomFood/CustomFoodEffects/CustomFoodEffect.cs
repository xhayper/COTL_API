using I2.Loc;

namespace COTL_API.CustomInventory;

public abstract class CustomFoodEffect
{
    internal string InternalObjectName = "";

    internal CookingData.MealEffectType MealEffect;
    internal string ModPrefix = "";
    public abstract string InternalName { get; }

    /// <summary>
    ///     This is the effect that will occur when the follower eats this dish.
    /// </summary>
    public abstract Action<FollowerBrain> Effect { get; }

    public virtual bool EffectEnabled()
    {
        return true;
    }

    public virtual string Description()
    {
        return LocalizationManager.GetTranslation($"CookingData/{ModPrefix}.{InternalName}/Description");
    }

    /// <summary>
    ///     Used for adding a suffix to the description, normally for when the effect is disabled
    /// </summary>
    /// <returns>The suffix to the description</returns>
    public virtual string DescriptionSuffix()
    {
        return string.Empty;
    }

    /// <summary>
    ///     Whether this MealEffect is positive or negative (The arrow on the cooking screen)
    /// </summary>
    public virtual bool Positive()
    {
        return true;
    }
}