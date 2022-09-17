using COTL_API.Helpers;
using COTL_API.Structures;
using Lamb.UI.BuildMenu;
using System.Collections.Generic;
using UnityEngine;

namespace COTL_API.Debug;

public class DebugStructure3 : CustomStructure
{
    public override Sprite Sprite => TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder_3.png"));
    public override string InternalName => "DEBUG_STRUCTURE_3";
    public override FollowerCategory.Category Category => FollowerCategory.Category.Items;
    public override Vector2Int Bounds => new Vector2Int(3, 3);
    
    public override int BuildDurationMinutes => 45;

    public override List<StructuresData.ItemCost> Cost => new() {
        new(Plugin.DebugItem3, 3)
    };
}