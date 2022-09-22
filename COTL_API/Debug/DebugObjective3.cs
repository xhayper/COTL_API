using COTL_API.CustomObjectives;
using UnityEngine;

namespace COTL_API.Debug;

public sealed class DebugObjective3: CustomObjective
{

    public override string InternalName => "DEBUG_OBJECTIVE_3";

    public override string InitialQuestText => "DEBUG_OBJECTIVE_3 COMPLAINT TEXT!";

    public override ObjectivesData ObjectiveData => 
        CustomObjectiveManager.Objective.CookMeal(this.ObjectiveKey, InventoryItem.ITEM_TYPE.MEAL_POOP, 1,4800f);
}