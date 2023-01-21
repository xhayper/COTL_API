//NOSONAR

using COTL_API.CustomStructures;
using Lamb.UI.BuildMenu;
using COTL_API.Helpers;
using UnityEngine;

namespace COTL_API.Debug;

public class DebugStructure : CustomStructure
{
    public override Sprite Sprite =>
        TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder_1.png"));

    public override string InternalName => "DEBUG_STRUCTURE";
    public override FollowerCategory.Category Category => FollowerCategory.Category.Misc;
    public override Vector2Int Bounds => new(1, 1);

    public override int BuildDurationMinutes => 15;

    public override List<StructuresData.ItemCost> Cost => new()
    {
        new(Plugin.Instance!.DebugItem, 1)
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