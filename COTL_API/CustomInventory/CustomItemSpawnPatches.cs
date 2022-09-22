using COTL_API.Helpers;
using HarmonyLib;
using MMBiomeGeneration;
using MMRoomGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace COTL_API.CustomInventory;

/// <summary>
/// This class is used for patches required for the custom items to spawn.
/// </summary>
[HarmonyPatch]
internal static class CustomItemSpawnPatches
{
    private const string PathPrefix = "CustomItem_";
    
    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.Spawn), typeof(InventoryItem.ITEM_TYPE), typeof(int), typeof(Vector3), typeof(float), typeof(Action<PickUp>))]
    private static class InventoryItemSpawnPatches
    {
        [HarmonyPrefix]
        private static bool Prefix(InventoryItem.ITEM_TYPE type, int quantity, Vector3 position, float StartSpeed, ref Action<PickUp> result, ref PickUp __result)
        {
            if (!CustomItemManager.CustomItems.ContainsKey(type)) return true;
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
            return false;
        }
    }

    [HarmonyPatch(typeof(ObjectPool), nameof(ObjectPool.Spawn), typeof(string), typeof(Vector3), typeof(Quaternion), typeof(Transform), typeof(Action<GameObject>))]
    private static class ObjectPoolSpawnStringPatches
    {
        [HarmonyPrefix]
        private static void Prefix(ref string path)
        {
            if (!path.StartsWith(PathPrefix)) return;

            KeyValuePair<InventoryItem.ITEM_TYPE, CustomInventoryItem> item = CustomItemManager.GetItemObjectByInternalObjectName(path);

            if (ObjectPool.instance.loadedAddressables.TryGetValue(item.Value.InternalObjectName, out GameObject _)) return;

            ObjectPool.instance.loadedAddressables.Add(item.Value.InternalObjectName, GetObject.GetCustomObject(item.Value));
        }
    }

    /// <summary>
    ///    This patch adds our custom items to the list of images used for the offering shrine items
    /// </summary>
    /// <param name="__instance"></param>
    [HarmonyPatch(typeof(InventoryItemDisplay), nameof(InventoryItemDisplay.GetItemImages))]
    [HarmonyPrefix]
    private static void InventoryItemDisplay_GetItemImages(ref InventoryItemDisplay __instance)
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

            if (__instance.myDictionary == null) continue;
            if (!__instance.myDictionary.ContainsKey(customItem.key))
            {
                __instance.myDictionary.Add(customItem.key, customItem.value);
            }
        }
    }

    /// <summary>
    /// This is a patch to add our custom items to the list of items that can be offered to the shrine.
    /// </summary>
    [HarmonyPatch(typeof(Structures_OfferingShrine), nameof(Structures_OfferingShrine.Complete))]
    private static class StructuresOfferingShrineCompletePatches
    {
        /// <summary>
        ///    This is a patch to allow custom items to be used in the offering shrine based on the items rarity. If not overriden, default is common.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPrefix]
        private static void Prefix(ref Structures_OfferingShrine __instance)
        {
            foreach (KeyValuePair<InventoryItem.ITEM_TYPE, CustomInventoryItem> item in CustomItemManager.CustomItems.Where(item => item.Value.AddItemToOfferingShrine))
            {
                switch (item.Value.Rarity)
                {
                    case CustomItemManager.ItemRarity.COMMON:
                    {
                        if (!__instance.Offerings.Contains(item.Key))
                        {
                            __instance.Offerings.Add(item.Key);
                            if (Plugin.Debug) Plugin.Logger.LogDebug($"Added {item.Key} to common offering shrine");
                        }

                        break;
                    }
                    case CustomItemManager.ItemRarity.RARE:
                    {
                        if (!__instance.RareOfferings.Contains(item.Key))
                        {
                            __instance.RareOfferings.Add(item.Key);
                            if (Plugin.Debug) Plugin.Logger.LogDebug($"Added {item.Key} to rare offering shrine");
                        }

                        break;
                    }
                    default:
                        if (!__instance.Offerings.Contains(item.Key))
                        {
                            __instance.Offerings.Add(item.Key);
                            if (Plugin.Debug) Plugin.Logger.LogDebug($"Somethings up, we should never hit this.");
                        }

                        break;
                }
            }
        }
    }


    /// <summary>
    /// Patches for the custom items to spawn in dungeon chests, if they are set to do so.
    /// </summary>
    [HarmonyPatch(typeof(Interaction_Chest))]
    private static class DungeonChestPatches
    {
        /// <summary>
        /// This is used to spawn the custom items in regular dungeon chests.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(Interaction_Chest.Reveal))]
        [HarmonyPostfix]
        private static void RevealPostfix(Interaction_Chest __instance)
        {
            foreach (KeyValuePair<InventoryItem.ITEM_TYPE, CustomInventoryItem> item in CustomItemManager.CustomItems.Where(item => item.Value.AddItemToDungeonChests && CustomItemManager.DropLoot(item.Value)))
            {
                InventoryItem.Spawn(item.Key, Random.Range(item.Value.DungeonChestMinAmount, item.Value.DungeonChestMaxAmount + 1), __instance.transform.position);
            }
        }

        /// <summary>
        /// This is used to spawn the custom items in the boss room dungeon chest.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(Interaction_Chest.RevealBossReward))]
        [HarmonyPostfix]
        private static void RevealBossRewardPostfix(Interaction_Chest __instance)
        {
            foreach (KeyValuePair<InventoryItem.ITEM_TYPE, CustomInventoryItem> item in CustomItemManager.CustomItems.Where(item => item.Value.AddItemToDungeonChests && CustomItemManager.DropLoot(item.Value)))
            {
                InventoryItem.Spawn(item.Key, Random.Range(item.Value.DungeonChestMinAmount, item.Value.DungeonChestMaxAmount + 1), __instance.transform.position);
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