using COTL_API.CustomSkins;
using UnityEngine;

namespace COTL_API.Debug;

public class DebugFollowerSkin2 : CustomFollowerSkin
{
    public override string Name => "Debug Skin_1";

    public override Texture2D Texture =>
        TextureHelper.CreateTextureFromPath(PluginPaths.ResolveAssetPath("debug_sheet2.png"));

    public override List<SkinOverride> Overrides =>
    [
        new SkinOverride("HEAD_SKIN_TOP_BACK", new Rect(0, 127, 111, 127)),
        new SkinOverride("HEAD_SKIN_BTM_BACK", new Rect(0, 0, 111, 127)),
        new SkinOverride("HEAD_SKIN_TOP", new Rect(111, 127, 149, 127)),
        new SkinOverride("HEAD_SKIN_BTM", new Rect(265, 0, 145, 129))
    ];
}