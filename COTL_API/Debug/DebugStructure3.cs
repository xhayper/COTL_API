﻿using COTL_API.CustomStructures;
using Lamb.UI.BuildMenu;
using UnityEngine;

namespace COTL_API.Debug;

public class DebugStructure3 : CustomStructure
{
    public override Sprite Sprite =>
        TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder_3.png"));

    public override string InternalName => "DEBUG_STRUCTURE_3";
    public override FollowerCategory.Category Category => FollowerCategory.Category.Items;
    public override Vector2Int Bounds => new(3, 3);

    public override int BuildDurationMinutes => 45;

    public override List<StructuresData.ItemCost> Cost =>
    [
        new StructuresData.ItemCost(Plugin.Instance!.DebugItem3, 3)
    ];
}