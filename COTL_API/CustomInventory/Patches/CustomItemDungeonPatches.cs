using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace COTL_API.CustomInventory;

[HarmonyPatch]
public static partial class CustomItemManager
{
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
            foreach (KeyValuePair<InventoryItem.ITEM_TYPE, CustomInventoryItem> item in CustomItems.Where(item => item.Value.AddItemToDungeonChests && DropLoot(item.Value)))
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
            foreach (KeyValuePair<InventoryItem.ITEM_TYPE, CustomInventoryItem> item in CustomItems.Where(item => item.Value.AddItemToDungeonChests && DropLoot(item.Value)))
            {
                InventoryItem.Spawn(item.Key, Random.Range(item.Value.DungeonChestMinAmount, item.Value.DungeonChestMaxAmount + 1), __instance.transform.position);
            }
        }
    } 
}