using UnityEngine;

namespace COTL_API.Tasks;

public class CustomTask : FollowerTask
{
    public virtual string InternalName { get; }

    public FollowerTaskType TaskType;
    public override FollowerTaskType Type => TaskType;

    public string ModPrefix;
    
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