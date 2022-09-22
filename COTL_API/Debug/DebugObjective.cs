using COTL_API.CustomObjectives;
using UnityEngine;

namespace COTL_API.Debug;

public sealed class DebugObjective: CustomObjective
{

    public override string InternalName => "DebugObjective";

    public override string InitialQuestText => "This is a debug quest!";

    public override ObjectivesData ObjectiveData => 
        CustomObjectiveManager.Objective.CollectItem(this.ObjectiveKey, InventoryItem.ITEM_TYPE.Necklace_1, Random.Range(15, 26), false, FollowerLocation.Dungeon1_1, 4800f);
}