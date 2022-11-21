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
        new SkinOverride("HEAD_SKIN_TOP_BACK", new Rect(0, 127, 111, 127)),
        new SkinOverride("HEAD_SKIN_BTM_BACK", new Rect(0, 0, 111, 127)),
        new SkinOverride("HEAD_SKIN_TOP", new Rect(111, 127, 149, 127)),
        new SkinOverride("HEAD_SKIN_BTM", new Rect(111, 0, 149, 127))
    };

    public override List<WorshipperData.SlotsAndColours> Colors { get; } = new()
    {
        new WorshipperData.SlotsAndColours
        {
            SlotAndColours = new List<WorshipperData.SlotAndColor>
            {
                new("ARM_LEFT_SKIN", new Color(1, 0, 0)),
                new("ARM_RIGHT_SKIN", new Color(1, 0, 0)),
                new("LEG_LEFT_SKIN", new Color(1, 0, 0)),
                new("LEG_RIGHT_SKIN", new Color(1, 0, 0)),
                new("BODY_SKIN", new Color(1, 0, 0)),
                new("BODY_SKIN_BOWED", new Color(1, 0, 0)),
                new("BODY_SKIN_UP", new Color(1, 0, 0)),
                new("HEAD_SKIN_BTM", new Color(1, 0, 0)),
                new("HEAD_SKIN_TOP", new Color(1, 0.5f, 0)),
            }
        },
        new WorshipperData.SlotsAndColours
        {
            SlotAndColours = new List<WorshipperData.SlotAndColor>
            {
                new("ARM_LEFT_SKIN", new Color(0, 1, 0)),
                new("ARM_RIGHT_SKIN", new Color(0, 1, 0)),
                new("LEG_LEFT_SKIN", new Color(0, 1, 0)),
                new("LEG_RIGHT_SKIN", new Color(0, 1, 0)),
                new("BODY_SKIN", new Color(0, 1, 0)),
                new("BODY_SKIN_BOWED", new Color(0, 1, 0)),
                new("BODY_SKIN_UP", new Color(0, 1, 0)),
                new("HEAD_SKIN_BTM", new Color(0, 1, 0)),
                new("HEAD_SKIN_TOP", new Color(0, 1, 0.5f)),
            }
        },
        new WorshipperData.SlotsAndColours
        {
            SlotAndColours = new List<WorshipperData.SlotAndColor>
            {
                new("ARM_LEFT_SKIN", new Color(0, 0, 1)),
                new("ARM_RIGHT_SKIN", new Color(0, 0, 1)),
                new("LEG_LEFT_SKIN", new Color(0, 0, 1)),
                new("LEG_RIGHT_SKIN", new Color(0, 0, 1)),
                new("BODY_SKIN", new Color(0, 0, 1)),
                new("BODY_SKIN_BOWED", new Color(0, 0, 1)),
                new("BODY_SKIN_UP", new Color(0, 0, 1)),
                new("HEAD_SKIN_BTM", new Color(0, 0, 1)),
                new("HEAD_SKIN_TOP", new Color(0.5f, 0, 1)),
            }
        }
    };
}