using System.Collections.Generic;
using COTL_API.Helpers;
using System.Linq;
using HarmonyLib;

namespace COTL_API.CustomInventory;

[HarmonyPatch]
public static partial class CustomItemManager
{
    /// <summary>
    ///    This patch adds our custom items to the list of images used for the offering shrine items
    /// </summary>
    /// <param name="__instance"></param>
    [HarmonyPatch(typeof(InventoryItemDisplay), nameof(InventoryItemDisplay.GetItemImages))]
    [HarmonyPrefix]
    private static void InventoryItemDisplay_GetItemImages(ref InventoryItemDisplay __instance)
    {
        foreach (var customItem in CustomItemList.Select(type =>
                     new InventoryItemDisplay.MyDictionaryEntry
                     {
                         key = type.Key,
                         value = type.Value.InventoryIcon
                     }))
        {
            if (!__instance.ItemImages.Contains(customItem))
                __instance.ItemImages.Add(customItem);

            if (__instance.myDictionary == null) continue;

            if (!__instance.myDictionary.ContainsKey(customItem.key))
                __instance.myDictionary.Add(customItem.key, customItem.value);
        }
    }

    /// <summary>
    /// The original method fails to spawn the custom items when qty is greater than 1 for some reason. This patch fixes that.
    /// </summary>
    /// <param name="__instance"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_OfferingShrine), nameof(Interaction_OfferingShrine.OnInteract))]
    public static bool OnInteract(ref Interaction_OfferingShrine __instance, ref StateMachine state)
    {
        if (!CustomItemList.ContainsKey(
                (InventoryItem.ITEM_TYPE)__instance.StructureInfo.Inventory[0].type)) return true;

        Helpers.OnInteractHelper.Interaction_OnInteract(__instance, state);
        if (__instance.StructureInfo.Inventory.Count <= 0) return false;

        var type = (InventoryItem.ITEM_TYPE)__instance.StructureInfo.Inventory[0].type;
        var quantity = __instance.StructureInfo.Inventory[0].quantity;

        InventoryItem.Spawn(type, quantity, __instance.Item.transform.position, 0f)
            .SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f),
                270 + UnityEngine.Random.Range(-90, 90));

        __instance.StructureInfo.Inventory.Clear();
        __instance.StructureInfo.LastPrayTime = TimeManager.TotalElapsedGameTime;
        __instance.ShowItem();
        AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", __instance.transform.position);
        MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);

        return false;
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
            foreach (var item in CustomItemList.Where(item =>
                         item.Value.AddItemToOfferingShrine))
            {
                List<InventoryItem.ITEM_TYPE> listToAdd;

                switch (item.Value.Rarity)
                {
                    case ItemRarity.COMMON:
                    {
                        listToAdd = __instance.Offerings;
                        break;
                    }
                    case ItemRarity.RARE:
                    {
                        listToAdd = __instance.RareOfferings;
                        break;
                    }
                    default:
                    {
                        listToAdd = __instance.Offerings;
                        if (Plugin.Instance != null && Plugin.Instance.Debug)
                            LogHelper.LogDebug(
                                $"Something went horribly wrong here... we should never hit this.");
                        break;
                    }
                }

                if (listToAdd != null && !listToAdd.Contains(item.Key))
                    listToAdd.Add(item.Key);
            }
        }
    }
}