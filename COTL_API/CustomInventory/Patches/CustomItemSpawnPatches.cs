using COTL_API.CustomStructures;
using HarmonyLib;
using MMBiomeGeneration;
using MMRoomGeneration;
using MMTools;
using Sirenix.Serialization.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace COTL_API.CustomInventory;

/// <summary>
///     This class is used for patches required for the custom items to spawn.
/// </summary>
[HarmonyPatch]
public static partial class CustomItemManager
{
    private const string PathPrefix = "CustomItem_";

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.Spawn), typeof(InventoryItem.ITEM_TYPE), typeof(int),
        typeof(Vector3), typeof(float), typeof(Action<PickUp>))]
    private static class InventoryItemSpawnPatches
    {
        [HarmonyPrefix]
        private static bool Prefix(InventoryItem.ITEM_TYPE type, int quantity, Vector3 position, float StartSpeed,
            ref Action<PickUp> result, ref PickUp __result)
        {
            if (!CustomItemList.ContainsKey(type)) return true;
            LogWarning($"Running custom spawn. Item type = {type}, Qty: {quantity}");

            var gameObject = GameObject.FindGameObjectWithTag("Unit Layer");
            var transform = gameObject != null ? gameObject.transform : null;
            PickUp? pickUp = null;
            while (--quantity >= 0)
            {
                var instance = BiomeGenerator.Instance;
                if ((instance != null ? instance.CurrentRoom : null) != null)
                    transform = BiomeGenerator.Instance.CurrentRoom.GameObject.transform;

                if (transform == null && GenerateRoom.Instance != null)
                    transform = GenerateRoom.Instance.transform;

                if (transform == null)
                    break;

                if (ItemPickUp.ShouldUseObjectPool(CustomItemList[type].ItemPickUpToImitate))
                {
                    ObjectPool.Spawn(ItemPickUp.GetItemPickUpPath(CustomItemList[type].ItemPickUpToImitate), position,
                        Quaternion.identity, transform,
                        o =>
                        {
                            var item = CustomItemList[type];

                            o.name = item.InternalName;
                            o.transform.localScale = item.LocalScale;
                            var sprite = o.GetComponentInChildren<SpriteRenderer>();
                            sprite.sprite = item.Sprite;

                            var meal = o.GetComponent<Meal>();
                            if (meal != null)
                            {
                                meal.CreateStructureOnStop = true;
                                // not sure these are nessacery? not gonna waste my time testing it.
                                //meal.CreateStructureLocation = FollowerLocation.Base;
                                //meal.spriteRenderer.sprite = (item as CustomMeal)!.Sprite;
                                //meal.structure.Type = (item as CustomMeal)!.StructureType;
                            }

                            var pickUp = o.GetComponent<PickUp>();
                            if (pickUp == null) return;

                            pickUp.type = type;
                            pickUp.Speed = StartSpeed;

                        });
                }
                else
                {
                    var customObject = GetObject.GetCustomObject(CustomItemList[type]).Spawn(transform);
                    customObject.transform.position = position;
                    customObject.transform.eulerAngles = Vector3.zero;
                    pickUp = customObject.GetComponent<PickUp>();

                    if (pickUp == null) continue;

                    pickUp.type = type;
                    pickUp.Speed = StartSpeed;
                }
            }

            //whatever the user chose to imitate, all those objects get converted into the custom item without this....
            GetObject.SetInactive();
            __result = pickUp!;
            return false;
        }
    }

    [HarmonyPatch(typeof(ObjectPool), nameof(ObjectPool.Spawn), typeof(string), typeof(Vector3), typeof(Quaternion),
        typeof(Transform), typeof(Action<GameObject>), typeof(bool))]
    private static class ObjectPoolSpawnStringPatches
    {
        [HarmonyPrefix]
        private static void Prefix(ref string path)
        {
            if (!path.StartsWith(PathPrefix)) return;

            var item = GetItemObjectByInternalObjectName(path);

            if (ObjectPool.instance.loadedAddressables.TryGetValue(item.Value.InternalObjectName, out _))
                return;

            var asyncOperationHandle =
                Addressables.LoadAssetAsync<GameObject>(item.Value.InternalObjectName);
            asyncOperationHandle.Completed += obj =>
            {
                var myObject = obj.Result;
                if (myObject != null) myObject.SetActive(true);
                myObject = Object.Instantiate(ItemPickUp.GetItemPickUpObject(item.Value.ItemPickUpToImitate), null,
                    instantiateInWorldSpace: false) as GameObject;
                LogWarning($"myObject is NULL? {myObject == null}");
                myObject!.GetComponentInChildren<SpriteRenderer>().sprite = item.Value.Sprite;
                myObject.name = item.Value.InternalObjectName;
                myObject.transform.localScale = item.Value.LocalScale;
            };

            ObjectPool.instance.loadedAddressables.Add(item.Value.InternalObjectName, asyncOperationHandle);
        }
    }

    private static class GetObject
    {
        private static GameObject? _myObject;

        public static GameObject GetCustomObject(CustomInventoryItem item)
        {
            if (_myObject != null)
            {
                _myObject.SetActive(true);
                return _myObject;
            }

            _myObject = Object.Instantiate(ItemPickUp.GetItemPickUpObject(item.ItemPickUpToImitate), null,
                instantiateInWorldSpace: false) as GameObject;
            LogWarning($"_myObject is NULL? {_myObject == null}");
            _myObject!.GetComponentInChildren<SpriteRenderer>().sprite = item.Sprite;
            _myObject.name = item.InternalObjectName;
            _myObject.transform.localScale = item.LocalScale;

            return _myObject;
        }

        public static void SetInactive()
        {
            if (_myObject == null) return;
            _myObject.SetActive(false);
        }
    }
}