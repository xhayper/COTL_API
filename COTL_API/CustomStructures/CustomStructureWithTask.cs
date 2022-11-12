using System.Collections.Generic;

namespace COTL_API.CustomStructures;

public abstract class CustomStructureWithTask : CustomStructure, ITaskProvider
{
    public override string InternalName => "Custom_Task_Structure";
    public abstract FollowerTask StructureTask { get; }

    public bool CheckOverrideComplete()
    {
        return true;
    }

    public FollowerTask? GetOverrideTask(FollowerBrain brain)
    {
        return null;
    }

    public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> sortedTasks)
    {
        if (activity != ScheduledActivity.Work || ReservedForTask)
            return;

        sortedTasks.Add(StructureTask.Priorty, StructureTask);
    }
}