using UnityEngine;

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
            SlotAndColours = new List<WorshipperData.SlotAndColor>
            {
                new("ARM_LEFT_SKIN", new Color(1, 0, 1)),
                new("ARM_RIGHT_SKIN", new Color(0, 0, 0)),
                new("LEG_LEFT_SKIN", new Color(1, 0, 1)),
                new("LEG_RIGHT_SKIN", new Color(0, 0, 0)),
                new("Body_Naked", new Color(1, 0, 1)),
                new("Body_Naked_Up", new Color(1, 0, 1)),
                new("BODY_BTM", new Color(1, 0, 1)),
                new("BODY_BTM_UP", new Color(1, 0, 1)),
                new("BODY_TOP", new Color(0, 0, 0))
            }
        }
    };
}
