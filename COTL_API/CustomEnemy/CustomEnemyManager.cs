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

            

            if (objInfo.EnemyController != null)
            {
                //apply enemy controller here, remove original controller
                LogInfo($"Adding custom enemy controller for {enemyType}...");
                try
                {
                    var newController = (CustomEnemyController)obj.AddComponent(objInfo.EnemyController);
                    var originalController = (EnemySwordsmanWolf)unitObject;

                    newController.Spine = originalController.Spine;
                    newController.SimpleSpineFlash = originalController.SimpleSpineFlash;
                    newController.ghost = originalController.ghost;
                    newController.damageColliderEvents = originalController.damageColliderEvents;
                    if (newController.damageColliderEvents != null)
                    {
                        newController.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(newController.OnDamageTriggerEnter);
                        newController.damageColliderEvents.SetActive(false);
                    }

                    if (objInfo.SpineOverride != null)
                    {
                    //apply spine override here
                        var spine = newController.Spine;
                        if (spine == null)
                        {
                            LogWarning("No spine found on spawned custom enemy! " + prefab);
                        }
                        else
                        {
                            spine.skeletonDataAsset = objInfo.SpineOverride;
                            LogInfo("Attempting to get skin named " + objInfo.SpineSkinName + " from spine override...");
                            // var skin = spine.skeleton.Data.FindSkin(objInfo.SpineSkinName);
                            spine.initialSkinName = objInfo.SpineSkinName;
                            // if (skin != null)
                            // {
                            //     LogInfo("Successfully found skin on spine override, applying to custom enemy...");
                            //     spine.skeleton.SetSkin(skin);
                            // }
                            spine.Initialize(true);
                            spine.skeleton.SetToSetupPose();
                            spine.Update(0);
                        }
                    }

                    // Copy all fields from unitObject to newController using reflection (mm yummy reflection)
                    LogInfo("Copying fields from original UnitObject to custom enemy controller...");
                    var fields = typeof(UnitObject).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (var field in fields)
                    {
                        if (!field.IsStatic)
                        {
                            try
                            {
                                field.SetValue(newController, field.GetValue(unitObject));
                            }
                            catch (Exception fieldEx)
                            {
                                LogError($"Failed to copy field '{field.Name}' to custom enemy controller: {fieldEx.Message}");
                            }
                        }
                    }

                    // Remove the original controller
                    LogInfo("Removing original UnitObject controller and returning custom enemy controller...");
                    UnityEngine.Object.Destroy(unitObject);

                    CustomSpawnedEnemies.Add(newController);
                    return newController;
                }
                catch (Exception ex)
                {
                    LogError($"Failed to add custom enemy controller for {enemyType}: {ex}");
                }
            }

            LogInfo("Returning original UnitObject for custom enemy spawn.");
            CustomSpawnedEnemies.Add(unitObject);
            return unitObject;
        }
        LogWarning("A spawned custom enemy does not have a UnitObject component! " + prefab);
        return null;
    }

}
