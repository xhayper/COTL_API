using COTL_API.CustomTasks;
using COTL_API.Helpers;
using UnityEngine;

namespace COTL_API.Debug;

public class DebugTask : CustomTask
{
    private Follower? _follower;

    private float _progress;

    private Structure? _structure;
    public override string InternalName => "DEBUG_TASK";

    public override bool BlockReactTasks => true;
    public override bool BlockTaskChanges => true;

    public override FollowerLocation Location => GetStructure().Brain.Data.Location;

    private Structure GetStructure()
    {
        if (_structure != null) return _structure;
        return _structure = TaskUtils.GetAvailableStructureOfType<DebugStructure>()!;
    }

    public override void TaskTick(float deltaGameTime)
    {
        if (_follower == null || State != FollowerTaskState.Doing) return;

        _progress += deltaGameTime;
        if (_progress >= 50)
        {
            _follower.Brain.AddAdoration(FollowerBrain.AdorationActions.PetDog, null);
            _progress -= 50;
        }

        if (_follower.Brain.Stats.Adoration >= _follower.Brain.Stats.MAX_ADORATION) Complete();
    }

    public override void ClaimReservations()
    {
        var structureByID = GetStructure().Brain;
        structureByID.ReservedForTask = true;
    }

    public override void ReleaseReservations()
    {
        var structureByID = GetStructure().Brain;
        structureByID.ReservedForTask = false;
    }

    public override void Setup(Follower follower)
    {
        base.Setup(follower);
        _follower = follower;
        follower.SetHat(HatType.Miner);
    }

    public override void Cleanup(Follower follower)
    {
        follower.SetHat(HatType.None);
        base.Cleanup(follower);
    }

    public override Vector3 UpdateDestination(Follower follower)
    {
        var structureByID = GetStructure().Brain;
        var pos = structureByID.Data.Position;
        return new Vector3(pos.x, pos.y, pos.z);
    }

    public override void OnStart()
    {
        SetState(FollowerTaskState.GoingTo);
    }
}