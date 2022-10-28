using System;
using System.Collections.Generic;
using System.Text;

namespace COTL_API.CustomStructures;
internal abstract class CustomStructureWithTask : CustomStructure, ITaskProvider
{
    public override string InternalName => "Custom_Task_Structure";
    public abstract FollowerTask StructureTask { get; set; }

    public bool CheckOverrideComplete() => true;
    public FollowerTask GetOverrideTask(FollowerBrain brain) => null;


    public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> sortedTasks)
    {
        if (activity != ScheduledActivity.Work || this.ReservedForTask)
            return;

        sortedTasks.Add(StructureTask.Priorty, StructureTask);
    }

}
