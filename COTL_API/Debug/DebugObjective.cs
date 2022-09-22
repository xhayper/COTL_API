using COTL_API.CustomObjectives;
using UnityEngine;

namespace COTL_API.Debug;

public sealed class DebugObjective: CustomObjective
{

    public override string InternalName => "DEBUG_OBJECTIVE_1";

    public override string InitialQuestText => "DEBUG_OBJECTIVE_1 COMPLAINT TEXT!";

    public override ObjectivesData ObjectiveData =>
        CustomObjectiveManager.Objective.BuildStructure(this.ObjectiveKey, StructureBrain.TYPES.PRISON, 1, 4800f);
}