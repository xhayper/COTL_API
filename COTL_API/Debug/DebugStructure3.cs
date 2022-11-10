using System.Collections.Generic;
using COTL_API.CustomStructures;
using Lamb.UI.BuildMenu;
using COTL_API.Helpers;
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

    public override List<StructuresData.ItemCost> Cost => new() {
        new(Plugin.DebugItem3, 3)
    };

    public override string GetLocalizedName()
    {
        return "DEBUG_STRUCTURE_3";
    }

    public override string GetLocalizedDescription()
    {
        return "COTL_API'S 3RD DEBUG STRUCTURE";
    }

    public override string GetLocalizedLore()
    {
        return "DEBUG STRUCTURE 3 LORE";
    }
}