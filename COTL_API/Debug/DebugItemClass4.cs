using COTL_API.CustomInventory;
using UnityEngine;

namespace COTL_API.Debug;

public class DebugItemClass4 : CustomInventoryItem
{
    public override string InternalName => "DEBUG_ITEM_4";

    public override string LocalizedName()
    {
        return "DEBUG_ITEM_4";
    }

    public override string LocalizedDescription()
    {
        return "COTL_API'S 4TH DEBUG ITEM";
    }

    public override bool IsPlantable => true;

    public override InventoryItem.ITEM_TYPE ItemPickUpToImitate => InventoryItem.ITEM_TYPE.BLACK_GOLD;
    public override CustomItemManager.ItemRarity Rarity => CustomItemManager.ItemRarity.COMMON;

    public override bool AddItemToDungeonChests => true;
    public override int DungeonChestSpawnChance => 15;
    public override int DungeonChestMinAmount => 4;
    public override int DungeonChestMaxAmount => 7;

    public override bool AddItemToOfferingShrine => true;

    public override Vector3 LocalScale { get; } = new(0.6f, 0.6f, 0.6f);
}