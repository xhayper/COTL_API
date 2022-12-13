namespace COTL_API.CustomSkins;

public abstract class CustomFollowerSkin : CustomSkin
{
    public virtual bool TwitchPremium { get; } = false;
    public virtual bool Hidden { get; } = false;
    public virtual bool Unlocked { get; } = true;
    public virtual bool Invariant { get; } = false;

    public virtual List<WorshipperData.SlotsAndColours> Colors { get; } = new()
    {
        new WorshipperData.SlotsAndColours
        {
            SlotAndColours = new()
            {
                new("ARM_LEFT_SKIN", new(1, 0, 1)),
                new("ARM_RIGHT_SKIN", new(0, 0, 0)),
                new("LEG_LEFT_SKIN", new(1, 0, 1)),
                new("LEG_RIGHT_SKIN", new(0, 0, 0)),
                new("BODY_SKIN", new(1, 0, 1)),
                new("BODY_SKIN_BOWED", new(1, 0, 1)),
                new("BODY_SKIN_UP", new(1, 0, 1)),
                new("HEAD_SKIN_BTM", new(1, 0, 1)),
                new("HEAD_SKIN_TOP", new(0, 0, 0)),
            }
        }
    };
}