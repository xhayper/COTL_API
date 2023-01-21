//NOSONAR

using COTL_API.CustomSkins;
using COTL_API.Helpers;
using UnityEngine;

namespace COTL_API.Debug;

public class DebugPlayerSkin : CustomPlayerSkin
{
    public override string Name => "Debug Skin";

    public override Texture2D Texture =>
        TextureHelper.CreateTextureFromPath(PluginPaths.ResolveAssetPath("debug_lamb_sheet.png"));

    public override List<SkinOverride> Overrides => new()
    {
        new("HeadBack", new(0, 0, 128, 128)),
        new("HeadBackDown", new(128, 0, 128, 128)),
        new("HeadBackDown_RITUAL", new(0, 128, 128, 128)),
        new("HeadBackDown_SERMON", new(128, 128, 128, 128)),
        new("HeadFront", new(256, 0, 128, 128)),
        new("HeadFrontDown", new(256, 128, 128, 128))
    };
}