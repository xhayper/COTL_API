using Lamb.UI;
using UnityEngine;

namespace COTL_API.CustomWorldMapNode;
public abstract class CustomWorldMapNode
{
    internal string ModPrefix = "";
    internal WorldMapIcon.WorldMapRegion MapRegion { get; set; }

    public abstract string InternalName { get; }
    public virtual FollowerLocation Location => FollowerLocation.None;
    public virtual string? SceneToLoad => "Base Biome 1"; // unused if change OnLocationSelected

    public virtual string LayerLocation => "Base";
    public virtual Vector3 Position => new(0, 0, 0);
    public virtual Vector2 ParallaxPosition => new(0, 0);

    public virtual Action<WorldMapIcon>? OnLocationSelected => null; // use default if null
    public virtual Func<bool>? ShowConditions => null; // use default if null
}