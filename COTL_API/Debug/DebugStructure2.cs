using COTL_API.CustomStructures;
using Lamb.UI.BuildMenu;
using COTL_API.Helpers;
using UnityEngine;

namespace COTL_API.Debug;

public class DebugStructure2 : CustomStructure
{
    public override Sprite Sprite =>
        TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder_2.png"));

    public override string InternalName => "DEBUG_STRUCTURE_2";
    public override FollowerCategory.Category Category => FollowerCategory.Category.Food;
    
    public override Vector2Int Bounds => new(2, 2);

    public override int BuildDurationMinutes => 30;

    public override List<StructuresData.ItemCost> Cost => new()
    {
        new(Plugin.Instance!.DebugItem2, 2)
    };

    public override string GetLocalizedName()
    {
        return "DEBUG_STRUCTURE_2";
    }

    public override string GetLocalizedDescription()
    {
        return "COTL_API'S 2ND DEBUG STRUCTURE";
    }

    public override string GetLocalizedLore()
    {
        return "DEBUG STRUCTURE 2 LORE";
    }
}