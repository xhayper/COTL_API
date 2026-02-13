using System;
using Spine.Unity;

namespace COTL_API.CustomEnemy;

public abstract class CustomEnemy
{
    internal string ModPrefix = "";
    public abstract string InternalName { get; }
    public Enemy enemyType = Enemy.None;
    public virtual string EnemyToMimic => "Assets/Prefabs/Enemies/All/Enemy Forest Archer Tennis.prefab";
    public virtual SkeletonDataAsset? SpineOverride => null;
    public virtual CustomEnemyController? EnemyController => null; //setting this will disable the original AI and replace with this
    public virtual float maxHealth => 1f;

}
