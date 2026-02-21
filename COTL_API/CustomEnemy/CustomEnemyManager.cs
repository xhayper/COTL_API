using System.Collections;
using System.Reflection;
using COTL_API.Guid;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace COTL_API.CustomEnemy;

public static partial class CustomEnemyManager
{
    internal static Dictionary<Enemy, CustomEnemy> CustomEnemyList { get; } = []; //customEnemy Class list
    public static Dictionary<Enemy, GameObject> CustomEnemyPrefabList { get; } = []; //prefabs used to spawn enemies
    public static List<UnitObject> CustomSpawnedEnemies { get; } = [];

    public static Enemy Add(CustomEnemy customEnemy)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var innerType = GuidManager.GetEnumValue<Enemy>(guid, customEnemy.InternalName);
        customEnemy.enemyType = innerType;
        customEnemy.ModPrefix = guid;

        CustomEnemyList.Add(innerType, customEnemy);
        LogWarning($"Added: {innerType} {customEnemy.InternalName} {customEnemy.ModPrefix}");
        
        return innerType;
    }

    public static IEnumerator BuildEnemyPrefab(CustomEnemy customEnemy)
    {
        
        var opHandle = Addressables.LoadAssetAsync<GameObject>(customEnemy.EnemyToMimic);
        yield return opHandle;

        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            var loadedPrefab = opHandle.Result;
            LogInfo("Successfully loaded enemy prefab: " + loadedPrefab.name);
            
            //var obj = Instantiate(loadedPrefab);
            //obj.transform.position = PlayerFarming.Instance.transform.position;

            //TODO: set attributes, spine, controller, etc on the custom enemy
            CustomEnemyPrefabList.Add(customEnemy.enemyType, loadedPrefab);
        }
        else
        {
            LogError("Failed to load prefab at path: " + customEnemy.EnemyToMimic);
        }
    }

    public static UnitObject? Spawn(Enemy enemyType, Vector3 position)
    {
        if (!CustomEnemyPrefabList.ContainsKey(enemyType))
        {
            LogWarning($"No custom enemy prefab found for {enemyType}!");
            return null;
        }

        var prefab = CustomEnemyPrefabList[enemyType];
        var objInfo = CustomEnemyList[enemyType];
        var obj = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
        var unitObject = obj.GetComponent<UnitObject>();

        if (unitObject != null)
        {
            unitObject.health.totalHP = objInfo.maxHealth;
            unitObject.health.HP = objInfo.maxHealth;

            if (objInfo.SpineOverride != null)
            {
                //apply spine override here
                var spineFlash = obj.GetComponentsInChildren<SimpleSpineFlash>();
                if (spineFlash.Length <= 0)
                {
                    LogWarning("No spine found on spawned custom enemy! " + prefab);
                }
                else
                {
                    spineFlash[0].Spine.skeletonDataAsset = objInfo.SpineOverride;
                    spineFlash[0].Spine.skeleton.SetBonesToSetupPose();
                    spineFlash[0].Spine.Update(0);
                }

            }

            if (objInfo.EnemyController != null)
            {
                //apply enemy controller here, remove original controller
            }


            CustomSpawnedEnemies.Add(unitObject);
            return unitObject;
        }
        LogWarning("A spawned custom enemy does not have a UnitObject component! " + prefab);
        return null;
    }

}
