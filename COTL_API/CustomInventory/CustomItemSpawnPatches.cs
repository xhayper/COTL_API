using COTL_API.Helpers;
using HarmonyLib;
using MMBiomeGeneration;
using MMRoomGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace COTL_API.CustomInventory;

[HarmonyPatch]
public static class CustomItemSpawnPatches
{
    private const string PathPrefix = "CustomItem_";

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.Spawn), typeof(InventoryItem.ITEM_TYPE), typeof(int), typeof(Vector3), typeof(float), typeof(Action<PickUp>))]
    public static class InventoryItemSpawnPatches
    {
        [HarmonyPrefix]
        public static bool Prefix(InventoryItem.ITEM_TYPE type)
        {
            if (!CustomItemManager.CustomItems.ContainsKey(type)) return true;
            return false;
        }

        [HarmonyPostfix]
        public static void Postfix(InventoryItem.ITEM_TYPE type, int quantity, Vector3 position, float StartSpeed, ref Action<PickUp> result, ref PickUp __result)
        {
            if (!CustomItemManager.CustomItems.ContainsKey(type)) return;
            GameObject gameObject = GameObject.FindGameObjectWithTag("Unit Layer");
            Transform transform = (gameObject != null ? gameObject.transform : null);
            PickUp p = null;
            while (--quantity >= 0)
            {
                BiomeGenerator instance = BiomeGenerator.Instance;
                if (((instance != null) ? instance.CurrentRoom : null) != null)
                {
                    transform = BiomeGenerator.Instance.CurrentRoom.GameObject.transform;
                }

                if (transform == null && GenerateRoom.Instance != null)
                {
                    transform = GenerateRoom.Instance.transform;
                }

                if (transform == null)
                {
                    break;
                }

                InventoryItem.ITEM_TYPE pType = type;
                Action<PickUp> action = result;

                ObjectPool.Spawn(CustomItemManager.CustomItems[type].InternalObjectName, position, Quaternion.identity, transform, delegate(GameObject obj)
                {
                    p = obj.GetComponent<PickUp>();
                    if (p != null)
                    {
                        p.type = pType;
                        p.Speed = StartSpeed;
                    }

                    if (action == null)
                    {
                        return;
                    }

                    action(p);
                });
            }

            //whatever the user chose to imitate, all those objects get converted into the custom item without this....
            GetObject.SetInactive();
            __result = p;
        }
    }

    [HarmonyPatch(typeof(ObjectPool), nameof(ObjectPool.Spawn), typeof(string), typeof(Vector3), typeof(Quaternion), typeof(Transform), typeof(Action<GameObject>))]
    public static class ObjectPoolSpawnStringPatches
    {
        [HarmonyPrefix]
        public static void Prefix(ref string path)
        {
            Plugin.Logger.LogWarning($"ObjectPool.Spawn: {path}");
            if (!path.StartsWith(PathPrefix)) return;

            KeyValuePair<InventoryItem.ITEM_TYPE, CustomInventoryItem> item = CustomItemManager.GetItemObjectByInternalObjectName(path);

            if (ObjectPool.instance.loadedAddressables.TryGetValue(item.Value.InternalObjectName, out GameObject _)) return;
            
            ObjectPool.instance.loadedAddressables.Add(item.Value.InternalObjectName, GetObject.GetCustomObject(item.Value));
        }
    }


    //add our custom items to the list of images used for the offering shrine items
    [HarmonyPatch(typeof(InventoryItemDisplay), nameof(InventoryItemDisplay.GetItemImages))]
    [HarmonyPrefix]
    public static void InventoryItemDisplay_GetItemImages(ref InventoryItemDisplay __instance)
    {
        foreach (InventoryItemDisplay.MyDictionaryEntry customItem in CustomItemManager.CustomItems.Select(type => new InventoryItemDisplay.MyDictionaryEntry {
                     key = type.Key,
                     value = type.Value.InventoryIcon
                 }))
        {
            if (!__instance.ItemImages.Contains(customItem))
            {
                __instance.ItemImages.Add(customItem);
            }
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
            _myObject = Object.Instantiate(ItemPickUp.GetItemPickUpObject(item.ItemPickUpToImitate), null, instantiateInWorldSpace: false) as GameObject;
            Sprite customSprite = item.GameObject.GetComponent<SpriteRenderer>().sprite;
            _myObject!.GetComponentInChildren<SpriteRenderer>().sprite = customSprite;
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