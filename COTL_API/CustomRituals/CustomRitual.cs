using COTL_API.Helpers;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace COTL_API.CustomRituals;

public abstract class CustomRitual : Ritual
{
    public abstract string InternalName { get; }
    internal string ModPrefix;
    public UpgradeSystem.Type upgradeType;
    public virtual Sprite Sprite { get; } = TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));
    public virtual List<StructuresData.ItemCost> ItemCosts { get; } = new() { new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 1) };
    public override UpgradeSystem.Type RitualType => upgradeType;
    public virtual string GetLocalizedName => $"Custom_Ritual_{InternalName}";
    public virtual string GetLocalizedDescription => $"Custom_Ritual_{InternalName}_Description";
    public virtual float FaithChange { get; } = 5;
    public virtual FollowerTrait.TraitType RitualTrait { get; } = FollowerTrait.TraitType.None;
    public virtual float Cooldown => 120f;
}
