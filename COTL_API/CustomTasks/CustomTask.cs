using UnityEngine;

namespace COTL_API.CustomTasks;

public abstract class CustomTask : FollowerTask
{
    internal string ModPrefix = "";
    internal FollowerTaskType TaskType;
    public abstract string InternalName { get; }
    public override FollowerTaskType Type => TaskType;

    public override FollowerLocation Location => FollowerLocation.Base;

    public override int GetSubTaskCode()
    {
        return 0;
    }

    public override void TaskTick(float deltaGameTime)
    {
    }

    public override Vector3 UpdateDestination(Follower follower)
    {
        return follower == null ? _brain.LastPosition : follower.transform.position;
    }
}