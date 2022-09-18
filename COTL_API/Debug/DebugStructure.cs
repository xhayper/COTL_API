using COTL_API.Helpers;
using COTL_API.Structures;
using Lamb.UI.BuildMenu;
using System.Collections.Generic;
using UnityEngine;

namespace COTL_API.Debug;

public class DebugStructure : CustomStructure
{
    public override Sprite Sprite => TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder_1.png"));
    public override string InternalName => "DEBUG_STRUCTURE";
    public override FollowerCategory.Category Category => FollowerCategory.Category.Misc;
    public override Vector2Int Bounds => new Vector2Int(1, 1);

    public override int BuildDurationMinutes => 15;

    public override List<StructuresData.ItemCost> Cost => new() {
        new(Plugin.DebugItem, 1)
    };

    public override string GetLocalizedName()
    {
        return "DEBUG_STRUCTURE";
    }

    public override string GetLocalizedDescription()
    {
        return "COTL_API'S DEBUG STRUCTURE";
    }

    public override string GetLocalizedLore()
    {
        return "DEBUG STRUCTURE LORE";
    }
}