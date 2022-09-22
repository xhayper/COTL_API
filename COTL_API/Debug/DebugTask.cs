using COTL_API.Helpers;
using COTL_API.CustomStructures;
using COTL_API.CustomTasks;
using DG.Tweening;
using Socket.Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace COTL_API.Debug;

public class DebugTask : CustomTask
{
    public override string InternalName => "DEBUG_TASK";
    
    public override bool BlockReactTasks => true;
    public override bool BlockTaskChanges => true;
    
    private Structure _structure;

    private Follower _follower;

    public override FollowerLocation Location => GetStructure().Brain.Data.Location;

    private Structure GetStructure()
    {
        if (_structure != null) return _structure;
        return _structure = TaskUtils.GetAvailableStructureOfType<DebugStructure>();
    }

    private float _progress;

    public override void TaskTick(float deltaGameTime)
    {
        if (State == FollowerTaskState.Doing)
        {
            _progress += deltaGameTime;
            if (_progress >= 50)
            {
                _follower.Brain.AddAdoration(FollowerBrain.AdorationActions.PetDog, null);
                _progress -= 50;
            }

            if (_follower.Brain.Stats.Adoration >= _follower.Brain.Stats.MAX_ADORATION)
            {
                Complete();
            }
        }
    }

    public override void ClaimReservations()
    {
        StructureBrain structureByID = GetStructure().Brain;
        structureByID.ReservedForTask = true;
    }
    
    public override void ReleaseReservations()
    {
        StructureBrain structureByID = GetStructure().Brain;
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
        StructureBrain structureByID = GetStructure().Brain;
        Vector3 pos = structureByID.Data.Position;
        return new Vector3(pos.x, pos.y, pos.z);
    }


    public override void OnStart()
    {
        SetState(FollowerTaskState.GoingTo);
    }

}