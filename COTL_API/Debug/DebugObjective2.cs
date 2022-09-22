using COTL_API.CustomObjectives;
using UnityEngine;

namespace COTL_API.Debug;

public sealed class DebugObjective2: CustomObjective
{

    public override string InternalName => "DEBUG_OBJECTIVE_2";

    public override string InitialQuestText => "DEBUG_OBJECTIVE_2 COMPLAINT TEXT!";

    public override ObjectivesData ObjectiveData => 
        CustomObjectiveManager.Objective.CollectItem(this.ObjectiveKey, InventoryItem.ITEM_TYPE.Necklace_1, Random.Range(15, 26), false, FollowerLocation.Dungeon1_1, 4800f);
}