using COTL_API.CustomSkins;
using UnityEngine;

namespace COTL_API.Debug;

public class DebugFollowerSkin : CustomFollowerSkin
{
    public override string Name => "Debug Skin";

    public override Texture2D Texture =>
        TextureHelper.CreateTextureFromPath(PluginPaths.ResolveAssetPath("debug_sheet2.png"));

    public override List<SkinOverride> Overrides =>
    [
        new SkinOverride("HEAD_SKIN_TOP_BACK", new Rect(0, 127, 111, 127)),
        new SkinOverride("HEAD_SKIN_BTM_BACK", new Rect(0, 0, 111, 127)),
        new SkinOverride("HEAD_SKIN_TOP", new Rect(111, 127, 149, 127)),
        new SkinOverride("HEAD_SKIN_BTM", new Rect(111, 0, 149, 127))
    ];

    public override List<WorshipperData.SlotsAndColours> Colors { get; } =
    [
        new WorshipperData.SlotsAndColours
        {
            SlotAndColours =
            [
                new WorshipperData.SlotAndColor("ARM_LEFT_SKIN", new Color(1, 0, 0)),
                new WorshipperData.SlotAndColor("ARM_RIGHT_SKIN", new Color(1, 0, 0)),
                new WorshipperData.SlotAndColor("LEG_LEFT_SKIN", new Color(1, 0, 0)),
                new WorshipperData.SlotAndColor("LEG_RIGHT_SKIN", new Color(1, 0, 0)),
                new WorshipperData.SlotAndColor("Body_Naked", new Color(1, 0, 0)),
                new WorshipperData.SlotAndColor("Body_Naked_Up", new Color(1, 0, 0)),
                new WorshipperData.SlotAndColor("BODY_BTM", new Color(1, 0, 0)),
                new WorshipperData.SlotAndColor("BODY_BTM_UP", new Color(1, 0, 0)),
                new WorshipperData.SlotAndColor("BODY_TOP", new Color(1, 0.5f, 0))
            ]
        },
        new WorshipperData.SlotsAndColours
        {
            SlotAndColours =
            [
                new WorshipperData.SlotAndColor("ARM_LEFT_SKIN", new Color(0, 1, 0)),
                new WorshipperData.SlotAndColor("ARM_RIGHT_SKIN", new Color(0, 1, 0)),
                new WorshipperData.SlotAndColor("LEG_LEFT_SKIN", new Color(0, 1, 0)),
                new WorshipperData.SlotAndColor("LEG_RIGHT_SKIN", new Color(0, 1, 0)),
                new WorshipperData.SlotAndColor("Body_Naked", new Color(0, 1, 0)),
                new WorshipperData.SlotAndColor("Body_Naked_Up", new Color(0, 1, 0)),
                new WorshipperData.SlotAndColor("BODY_BTM", new Color(0, 1, 0)),
                new WorshipperData.SlotAndColor("BODY_BTM_UP", new Color(0, 1, 0)),
                new WorshipperData.SlotAndColor("BODY_TOP", new Color(0, 1, 0.5f))
            ]
        },
        new WorshipperData.SlotsAndColours
        {
            SlotAndColours =
            [
                new WorshipperData.SlotAndColor("ARM_LEFT_SKIN", new Color(0, 0, 1)),
                new WorshipperData.SlotAndColor("ARM_RIGHT_SKIN", new Color(0, 0, 1)),
                new WorshipperData.SlotAndColor("LEG_LEFT_SKIN", new Color(0, 0, 1)),
                new WorshipperData.SlotAndColor("LEG_RIGHT_SKIN", new Color(0, 0, 1)),
                new WorshipperData.SlotAndColor("Body_Naked", new Color(0, 0, 1)),
                new WorshipperData.SlotAndColor("Body_Naked_Up", new Color(0, 0, 1)),
                new WorshipperData.SlotAndColor("BODY_BTM", new Color(0, 0, 1)),
                new WorshipperData.SlotAndColor("BODY_BTM_UP", new Color(0, 0, 1)),
                new WorshipperData.SlotAndColor("BODY_TOP", new Color(0.5f, 0, 1))
            ]
        }
    ];
}