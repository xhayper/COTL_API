using System;
using Spine.Unity;

namespace COTL_API.CustomEnemy;

public abstract class CustomEnemy
{
    internal string ModPrefix = "";
    public abstract string InternalName { get; }
    public Enemy enemyType = Enemy.None;

    //although EnemyToMimic is changeable, you will not be able to pass the attack events from the original enemy
    //to the custom enemy controller if you change it, soo dont change it :3
    public virtual string EnemyToMimic => "Assets/Prefabs/Enemies/DLC/Enemy Swordsman Wolf.prefab";
    public SkeletonDataAsset? SpineOverride = null;
    public string SpineSkinName = "";
    public virtual Type? EnemyController => null; //setting this will disable the original AI and replace with this
    public virtual float maxHealth => 1f;

}
