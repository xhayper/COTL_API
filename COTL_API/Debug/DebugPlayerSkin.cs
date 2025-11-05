using COTL_API.CustomSkins;
using UnityEngine;

namespace COTL_API.Debug;

public class DebugPlayerSkin : CustomPlayerSkin
{
    public override string Name => "Debug";

    public override Texture2D Texture =>
        TextureHelper.CreateTextureFromPath(PluginPaths.ResolveAssetPath("debug_lamb_sheet.png"));

    public override List<SkinOverride> Overrides =>
    [
        new("HeadBack", new Rect(0, 0, 128, 128)),
        new("HeadBackDown", new Rect(128, 0, 128, 128)),
        new("HeadBackDown_RITUAL", new Rect(0, 128, 128, 128)),
        new("HeadBackDown_SERMON", new Rect(128, 128, 128, 128)),
        new("HeadFront", new Rect(256, 0, 128, 128)),
        new("HeadFrontDown", new Rect(256, 128, 128, 128))
    ];
}