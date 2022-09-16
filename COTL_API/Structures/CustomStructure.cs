using COTL_API.Helpers;
using COTL_API.Prefabs;
using Lamb.UI.BuildMenu;
using System.Collections.Generic;
using UnityEngine;

namespace COTL_API.Structures;

public class CustomStructure : StructureBrain
{
    public virtual string InternalName { get; }
    public StructureBrain.TYPES StructureType;
    public string ModPrefix;
    
    public virtual Sprite Sprite { get; } =
        TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));

    public virtual FollowerCategory.Category Category { get; } =  FollowerCategory.Category.Misc;

    internal string PrefabPath => PrefabManager.GetOrCreateBuildingPrefab(this);
    public virtual int BuildDurationMinutes { get; } = 30;

    public virtual TypeAndPlacementObject GetTypeAndPlacementObject() {
        var orig = TypeAndPlacementObjects.GetByType(StructureBrain.TYPES.DECORATION_WREATH_STICK);
        var tpo = new TypeAndPlacementObject() {
            IconImage = Sprite,
            Category = Categories,
            PlacementObject = PrefabManager.CreatePlacementObjectFor(this),
            Type = StructureType,
            Tier = Tier,
        };
        return tpo;
    }

    public virtual Categories Categories => Categories.CULT;
    public virtual TypeAndPlacementObjects.Tier Tier => TypeAndPlacementObjects.Tier.Zero;

    internal StructuresData StructuresData => new StructuresData() {
        PrefabPath = PrefabPath,
        Bounds = Bounds,
        TILE_WIDTH = Bounds.x,
        TILE_HEIGHT = Bounds.y
    };

    public virtual Vector2Int Bounds => new Vector2Int(1, 1);
    
    public virtual List<StructuresData.ItemCost> Cost => new();
}