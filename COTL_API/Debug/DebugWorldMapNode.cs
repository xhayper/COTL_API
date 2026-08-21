using UnityEngine;

namespace COTL_API.Debug;
internal class DebugWorldMapNode : CustomWorldMapNode.CustomWorldMapNode
{
    public override string InternalName => "Graveyard_Test";
    public override FollowerLocation Location => FollowerLocation.Graveyard_Location;
    public override string? SceneToLoad => "Graveyard";
    public override string LayerLocation => "Shore Front";
    public override Vector2 ParallaxPosition => new(58, 118);
    public override Vector3 Position => new (826f, -840f, 0f);
    public override Func<bool>? ShowConditions => () => true;
}
