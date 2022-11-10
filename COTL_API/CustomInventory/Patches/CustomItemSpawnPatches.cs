using Object = UnityEngine.Object;
using System.Collections.Generic;
using MMBiomeGeneration;
using MMRoomGeneration;
using COTL_API.Helpers;
using UnityEngine;
using HarmonyLib;
using System;

//if it asks, choose "Does not introduce namespace"
namespace COTL_API.CustomInventory;

/// <summary>
/// This class is used for patches required for the custom items to spawn.
/// </summary>
[HarmonyPatch]
public static partial class CustomItemManager
{
    private const string PathPrefix = "CustomItem_";

    private static bool IsCustomItem(InventoryItem.ITEM_TYPE type)
    {
        return CustomItems.ContainsKey(type);
    }

    private static GameObject ReplaceSpawn(InventoryItem.ITEM_TYPE type)
    {
        CustomInventoryItem customItem = CustomItems[type];
        GameObject customItemObject = ItemPickUp.GetItemPickUpObject(customItem.ItemPickUpToImitate);
        customItemObject.GetComponentInChildren<SpriteRenderer>().sprite = customItem.Sprite;

        PickUp pickup = customItemObject.GetComponentInChildren<PickUp>();
        if (pickup != null)
            pickup.type = type;

        customItemObject.name = customItem.InternalObjectName;
        customItemObject.transform.localScale = customItem.LocalScale;
        Plugin.Logger.LogWarning($"Custom item, returning custom GameObject!");
        return customItemObject;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.Spawn), typeof(InventoryItem.ITEM_TYPE), typeof(int),
        typeof(Vector3), typeof(float), typeof(Action<PickUp>))]
    private static class InventoryItemSpawnPatches
    {
        [HarmonyPrefix]
        private static bool Prefix(InventoryItem.ITEM_TYPE type, int quantity, Vector3 position, float StartSpeed,
            ref Action<PickUp> result, ref PickUp __result)
        {
            if (!CustomItems.ContainsKey(type)) return true;
            Plugin.Logger.LogWarning($"Running custom spawn. Item type = {type}, Qty: {quantity}");

            GameObject gameObject = GameObject.FindGameObjectWithTag("Unit Layer");
            GameObject customObject;
            Transform transform = gameObject != null ? gameObject.transform : null;
            PickUp p = null;
            while (--quantity >= 0)
            {
                BiomeGenerator instance = BiomeGenerator.Instance;
                if ((instance != null ? instance.CurrentRoom : null) != null)
                    transform = BiomeGenerator.Instance.CurrentRoom.GameObject.transform;

                if (transform == null && GenerateRoom.Instance != null)
                    transform = GenerateRoom.Instance.transform;

                if (transform == null)
                    break;

                customObject = GetObject.GetCustomObject(CustomItems[type]).Spawn(transform);
                customObject.transform.position = position;
                customObject.transform.eulerAngles = Vector3.zero;
                p = customObject.GetComponent<PickUp>();

                if (p == null) continue;

                p.type = type;
                p.Speed = StartSpeed;
            }

            //whatever the user chose to imitate, all those objects get converted into the custom item without this....
            GetObject.SetInactive();
            __result = p;
            return false;
        }
    }

    [HarmonyPatch(typeof(ObjectPool), nameof(ObjectPool.Spawn), typeof(string), typeof(Vector3), typeof(Quaternion),
        typeof(Transform), typeof(Action<GameObject>))]
    private static class ObjectPoolSpawnStringPatches
    {
        [HarmonyPrefix]
        private static void Prefix(ref string path)
        {
            if (!path.StartsWith(PathPrefix)) return;

            KeyValuePair<InventoryItem.ITEM_TYPE, CustomInventoryItem> item = GetItemObjectByInternalObjectName(path);

            if (ObjectPool.instance.loadedAddressables.TryGetValue(item.Value.InternalObjectName, out GameObject _))
                return;

            ObjectPool.instance.loadedAddressables.Add(item.Value.InternalObjectName,
                GetObject.GetCustomObject(item.Value));
        }
    }

    private static class GetObject
    {
        private static GameObject _myObject;

        public static GameObject GetCustomObject(CustomInventoryItem item)
        {
            if (_myObject != null)
            {
                _myObject.SetActive(true);
                return _myObject;
            }

            _myObject = Object.Instantiate(ItemPickUp.GetItemPickUpObject(item.ItemPickUpToImitate), null,
                instantiateInWorldSpace: false) as GameObject;
            Plugin.Logger.LogWarning($"_myObject is NULL? {_myObject == null}");
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