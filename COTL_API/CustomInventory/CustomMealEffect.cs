namespace COTL_API.CustomInventory;

public abstract class CustomMealEffect
{
    internal string _internalObjectName = "";
    internal CookingData.MealEffectType _mealEffect;
    internal string _modPrefix = "";

    public abstract string InternalName { get; }
    public virtual bool EffectEnabled() => true;
    public virtual string Description()
    {
        return $"CookingData/{InternalName}/Description";
    }

    /// <summary>
    /// Used for adding a suffix to the description, normally for when the effect is disabled
    /// </summary>
    /// <returns>The suffix to the description</returns>
    public virtual string DescriptionSuffix()
    {
        return "";
    }

    /// <summary>
    /// Whether this MealEffect is positive or negative (The arrow on the cooking screen)
    /// </summary>
    public virtual bool Positive() => true;
    
    /// <summary>
    /// This is the effect that will occur when the follower eats this dish.
    /// </summary>
    public abstract Action<FollowerBrain> Effect { get; }
}