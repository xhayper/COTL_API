namespace COTL_API.CustomStructures;

public abstract class CustomStructureWithTask : CustomStructure, ITaskProvider
{
    //hewwo?
    //uwu
    //e
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

    public virtual void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> sortedTasks)
    {
        if (activity != ScheduledActivity.Work || ReservedForTask)
            return;

        sortedTasks.Add(StructureTask.Priorty, StructureTask);
    }
}