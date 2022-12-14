using COTL_API.CustomSkins;
using COTL_API.Helpers;
using UnityEngine;

namespace COTL_API.Debug;

public class DebugFollowerSkin : CustomFollowerSkin
{
    public override string Name => "Debug Skin";

    public override Texture2D Texture =>
        TextureHelper.CreateTextureFromPath(PluginPaths.ResolveAssetPath("debug_sheet.png"));

    public override List<SkinOverride> Overrides => new()
    {
        new("HEAD_SKIN_TOP_BACK", new(0, 127, 111, 127)),
        new("HEAD_SKIN_BTM_BACK", new(0, 0, 111, 127)),
        new("HEAD_SKIN_TOP", new(111, 127, 149, 127)),
        new("HEAD_SKIN_BTM", new(111, 0, 149, 127))
    };

    public override List<WorshipperData.SlotsAndColours> Colors { get; } = new()
    {
        new WorshipperData.SlotsAndColours
        {
            SlotAndColours = new()
            {
                new("ARM_LEFT_SKIN", new(1, 0, 0)),
                new("ARM_RIGHT_SKIN", new(1, 0, 0)),
                new("LEG_LEFT_SKIN", new(1, 0, 0)),
                new("LEG_RIGHT_SKIN", new(1, 0, 0)),
                new("BODY_SKIN", new(1, 0, 0)),
                new("BODY_SKIN_BOWED", new(1, 0, 0)),
                new("BODY_SKIN_UP", new(1, 0, 0)),
                new("HEAD_SKIN_BTM", new(1, 0, 0)),
                new("HEAD_SKIN_TOP", new(1, 0.5f, 0)),
            }
        },
        new WorshipperData.SlotsAndColours
        {
            SlotAndColours = new()
            {
                new("ARM_LEFT_SKIN", new(0, 1, 0)),
                new("ARM_RIGHT_SKIN", new(0, 1, 0)),
                new("LEG_LEFT_SKIN", new(0, 1, 0)),
                new("LEG_RIGHT_SKIN", new(0, 1, 0)),
                new("BODY_SKIN", new(0, 1, 0)),
                new("BODY_SKIN_BOWED", new(0, 1, 0)),
                new("BODY_SKIN_UP", new(0, 1, 0)),
                new("HEAD_SKIN_BTM", new(0, 1, 0)),
                new("HEAD_SKIN_TOP", new(0, 1, 0.5f)),
            }
        },
        new WorshipperData.SlotsAndColours
        {
            SlotAndColours = new()
            {
                new("ARM_LEFT_SKIN", new(0, 0, 1)),
                new("ARM_RIGHT_SKIN", new(0, 0, 1)),
                new("LEG_LEFT_SKIN", new(0, 0, 1)),
                new("LEG_RIGHT_SKIN", new(0, 0, 1)),
                new("BODY_SKIN", new(0, 0, 1)),
                new("BODY_SKIN_BOWED", new(0, 0, 1)),
                new("BODY_SKIN_UP", new(0, 0, 1)),
                new("HEAD_SKIN_BTM", new(0, 0, 1)),
                new("HEAD_SKIN_TOP", new(0.5f, 0, 1)),
            }
        }
    };
}