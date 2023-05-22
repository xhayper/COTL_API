using COTL_API.CustomStructures;
using UnityEngine;

namespace COTL_API.Debug;

public class DebugStructure : CustomStructure
{
    public override Sprite Sprite =>
        TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder_1.png"));

    public override string InternalName => "DEBUG_STRUCTURE";

    public override Vector2Int Bounds => new(1, 1);

    public override int BuildDurationMinutes => 15;

    public override List<StructuresData.ItemCost> Cost => new()
    {
        new StructuresData.ItemCost(Plugin.Instance!.DebugItem, 1)
    };
}