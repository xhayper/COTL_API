using UnityEngine;

namespace COTL_API.CustomTasks;

public abstract class CustomTask : FollowerTask
{
    public abstract string InternalName { get; }
    internal string ModPrefix;

    internal FollowerTaskType TaskType;
    public override FollowerTaskType Type => TaskType;

    public override int GetSubTaskCode()
    {
        return 0;
    }

    public override void TaskTick(float deltaGameTime) { }

    public override Vector3 UpdateDestination(Follower follower)
    {
        if (follower == null)
        {
            return _brain.LastPosition;
        }

        return follower.transform.position;
    }

    public override FollowerLocation Location => FollowerLocation.Base;
}