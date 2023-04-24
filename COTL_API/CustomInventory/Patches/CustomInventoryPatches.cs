using Lamb.UI.FollowerInteractionWheel;
using System.Reflection.Emit;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using Lamb.UI;

namespace COTL_API.CustomInventory;

[HarmonyPatch]
public static partial class CustomItemManager
{
    [HarmonyPatch(typeof(ItemInfoCard), nameof(ItemInfoCard.Configure))]
    [HarmonyPostfix]
    private static void ItemInfoCard_Configure(ItemInfoCard __instance, InventoryItem.ITEM_TYPE config)
    {
        if (!CustomItemList.ContainsKey(config)) return;

        __instance._itemHeader.text = CustomItemList[config].Name();
        __instance._itemLore.text = CustomItemList[config].Lore();
        __instance._itemDescription.text = CustomItemList[config].Description();
    }

    // Prepare for lag...
    [HarmonyPatch(typeof(Interaction_AddFuel), nameof(Interaction_AddFuel.Update))]
    [HarmonyPrefix]
    private static void Interaction_AddFuel_Update(Interaction_AddFuel __instance)
    {
        foreach (var itemType in CustomItemList.Keys)
        {
            if (!CustomItemList[itemType].IsBurnableFuel && __instance.fuel.Contains(itemType))
                __instance.fuel.Remove(itemType);
            if (CustomItemList[itemType].IsBurnableFuel && !__instance.fuel.Contains(itemType))
                __instance.fuel.Add(itemType);
        }
    }

    [HarmonyPatch(typeof(Inventory), nameof(Inventory.HasGift))]
    [HarmonyPostfix]
    private static void Inventory_HasGift(Inventory __instance, ref bool __result)
    {
        if (__result) return;

        if (Inventory.items.Where(item => CustomItemList.ContainsKey((InventoryItem.ITEM_TYPE)item.type)).Any(item =>
                CustomItemList[(InventoryItem.ITEM_TYPE)item.type].CanBeGivenToFollower))
        {
            __result = true;
        }
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.GiftCommands))]
    [HarmonyPostfix]
    private static void FollowerCommandGroups_GiftCommands(ref List<CommandItem> __result)
    {
        __result.AddRange(from item in CustomItemList.Values
                          where item.CanBeGivenToFollower
                          where 0 < Inventory.GetItemQuantity(item.ItemType)
                          select new FollowerCommandItems.GiftCommandItem(item.ItemType) { Command = item.GiftCommand });
    }

    [HarmonyPatch(typeof(FollowerCommandItems.GiftCommandItem), nameof(FollowerCommandItems.GiftCommandItem.GetTitle))]
    [HarmonyPrefix]
    private static bool FollowerCommandItems_GiftCommandItem_GetTitle(FollowerCommandItems.GiftCommandItem __instance,
        Follower follower, ref string __result)
    {
        if (!CustomItemList.ContainsKey(__instance._itemType)) return true;
        __result = CustomItemList[__instance._itemType].GiftTitle(follower);
        return false;
    }

    [HarmonyPatch(typeof(FontImageNames), nameof(FontImageNames.GetIconByType))]
    [HarmonyPrefix]
    private static bool FontImageNames_GetIconByType(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!CustomItemList.ContainsKey(Type)) return true;
        __result = CustomItemList[Type].InventoryStringIcon();
        return false;
    }

    [HarmonyPatch(typeof(Lamb.UI.Assets.InventoryIconMapping), nameof(Lamb.UI.Assets.InventoryIconMapping.GetImage),
        typeof(InventoryItem.ITEM_TYPE))]
    [HarmonyPrefix]
    private static bool InventoryIconMapping_GetImage(InventoryItem.ITEM_TYPE type, ref Sprite __result)
    {
        if (!CustomItemList.ContainsKey(type)) return true;
        __result = CustomItemList[type].InventoryIcon;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.Name))]
    [HarmonyPrefix]
    private static bool InventoryItem_Name(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!CustomItemList.ContainsKey(Type)) return true;
        __result = CustomItemList[Type].Name();
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.LocalizedName))]
    [HarmonyPrefix]
    private static bool InventoryItem_LocalizedName(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!CustomItemList.ContainsKey(Type)) return true;
        __result = CustomItemList[Type].LocalizedName();
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.Description))]
    [HarmonyPrefix]
    private static bool InventoryItem_Description(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!CustomItemList.ContainsKey(Type)) return true;
        __result = CustomItemList[Type].Description();
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.LocalizedDescription))]
    [HarmonyPrefix]
    private static bool InventoryItem_LocalizedDescription(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!CustomItemList.ContainsKey(Type)) return true;
        __result = CustomItemList[Type].LocalizedDescription();
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.Lore))]
    [HarmonyPrefix]
    private static bool InventoryItem_Lore(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!CustomItemList.ContainsKey(Type)) return true;
        __result = CustomItemList[Type].Lore();
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.GetItemCategory))]
    [HarmonyPrefix]
    private static bool InventoryItem_ItemCategory(InventoryItem.ITEM_TYPE type,
        ref InventoryItem.ITEM_CATEGORIES __result)
    {
        if (!CustomItemList.ContainsKey(type)) return true;
        __result = CustomItemList[type].ItemCategory;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.GetSeedType))]
    [HarmonyPrefix]
    private static bool InventoryItem_GetSeedType(InventoryItem.ITEM_TYPE type, ref InventoryItem.ITEM_TYPE __result)
    {
        if (!CustomItemList.ContainsKey(type)) return true;
        __result = CustomItemList[type].SeedType;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.FuelWeight), typeof(InventoryItem.ITEM_TYPE))]
    [HarmonyPrefix]
    private static bool InventoryItem_FuelWeight(InventoryItem.ITEM_TYPE type, ref int __result)
    {
        if (!CustomItemList.ContainsKey(type)) return true;
        __result = CustomItemList[type].FuelWeight;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.FoodSatitation))]
    [HarmonyPrefix]
    private static bool InventoryItem_FoodSatitation(InventoryItem.ITEM_TYPE Type, ref int __result)
    {
        if (!CustomItemList.ContainsKey(Type)) return true;
        __result = CustomItemList[Type].FoodSatitation;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.IsFish))]
    [HarmonyPrefix]
    private static bool InventoryItem_IsFish(InventoryItem.ITEM_TYPE Type, ref bool __result)
    {
        if (!CustomItemList.ContainsKey(Type)) return true;
        __result = CustomItemList[Type].IsFish;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.IsFood))]
    [HarmonyPrefix]
    private static bool InventoryItem_IsFood(InventoryItem.ITEM_TYPE Type, ref bool __result)
    {
        if (!CustomItemList.ContainsKey(Type)) return true;
        __result = CustomItemList[Type].IsFood;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.IsBigFish))]
    [HarmonyPrefix]
    private static bool InventoryItem_IsBigFish(InventoryItem.ITEM_TYPE Type, ref bool __result)
    {
        if (!CustomItemList.ContainsKey(Type)) return true;
        __result = CustomItemList[Type].IsBigFish;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.CanBeGivenToFollower))]
    [HarmonyPrefix]
    private static bool InventoryItem_CanBeGivenToFollower(InventoryItem.ITEM_TYPE Type, ref bool __result)
    {
        if (!CustomItemList.ContainsKey(Type)) return true;
        __result = CustomItemList[Type].CanBeGivenToFollower;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.CapacityString))]
    [HarmonyPrefix]
    private static bool InventoryItem_CapacityString(InventoryItem.ITEM_TYPE type, int minimum, ref string __result)
    {
        if (!CustomItemList.ContainsKey(type)) return true;
        __result = CustomItemList[type].CapacityString(minimum);
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.AllPlantables), MethodType.Getter)]
    [HarmonyPostfix]
    private static void InventoryItem_AllPlantables(ref List<InventoryItem.ITEM_TYPE> __result)
    {
        __result.AddRange(CustomItemList.Where(x => x.Value.IsPlantable).Select(x => x.Key));
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.GiveToFollowerCallbacks))]
    [HarmonyPrefix]
    private static bool InventoryItem_GiveToFollowerCallbacks(InventoryItem.ITEM_TYPE Type,
        ref Action<Follower, InventoryItem.ITEM_TYPE, Action> __result)
    {
        if (!CustomItemList.ContainsKey(Type)) return true;
        __result = (follower, type, callback) =>
        {
            if (!CustomItemList.ContainsKey(type))
                InventoryItem.GiveToFollowerCallbacks(type)(follower, type, callback);
            else CustomItemList[type].OnGiftTo(follower, callback);
        };
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.AllSeeds), MethodType.Getter)]
    [HarmonyPostfix]
    private static void InventoryItem_AllSeeds(ref List<InventoryItem.ITEM_TYPE> __result)
    {
        __result.AddRange(CustomItemList.Where(x => x.Value.IsSeed).Select(x => x.Key));
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.AllBurnableFuel), MethodType.Getter)]
    [HarmonyPostfix]
    private static void InventoryItem_AllBurnableFuel(ref List<InventoryItem.ITEM_TYPE> __result)
    {
        __result.AddRange(CustomItemList.Where(x => x.Value.IsBurnableFuel).Select(x => x.Key));
    }

    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetAllFoods))]
    [HarmonyPostfix]
    private static void CookingData_GetAllFoods(ref InventoryItem.ITEM_TYPE[] __result)
    {
        var copy = __result;
        __result = __result
            .Concat(CustomItemList.Where(i => !copy.Contains(i.Key) && i.Value.IsFood).Select(i => i.Key))
            .ToArray();
    }


    [HarmonyPatch(typeof(InventoryMenu))]
    private static class InventoryMenu_Patches
    {
        [HarmonyPatch(nameof(InventoryMenu.OnShowStarted))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> OnShowStarted(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                yield return instruction;

                if (instruction.LoadsField(typeof(InventoryMenu).GetField("_currencyFilter",
                        BindingFlags.NonPublic | BindingFlags.Instance)))
                    yield return new(OpCodes.Call,
                        SymbolExtensions.GetMethodInfo(() => AppendCustomCurrencies(null)));
                
                if (instruction.LoadsField(typeof(InventoryMenu).GetField("_foodSorter",
                        BindingFlags.NonPublic | BindingFlags.Instance)))
                    yield return new(OpCodes.Call,
                        SymbolExtensions.GetMethodInfo(() => AppendCustomFood(null)));
                
                if (instruction.LoadsField(typeof(InventoryMenu).GetField("_itemsSorter",
                        BindingFlags.NonPublic | BindingFlags.Instance)))
                    yield return new(OpCodes.Call,
                        SymbolExtensions.GetMethodInfo(() => AppendCustomItem(null)));
            }
        }

        internal static List<InventoryItem.ITEM_TYPE>? AppendCustomCurrencies(
            ICollection<InventoryItem.ITEM_TYPE>? currencyFilter)
        {
            return currencyFilter?.Concat(CustomItemList
                .Where(i => !currencyFilter.Contains(i.Key) && i.Value.InventoryItemType == CustomInventoryItemType.CURRENCY)
                .Select(i => i.Key)).ToList();
        }
        
        internal static List<InventoryItem.ITEM_TYPE>? AppendCustomFood(
            ICollection<InventoryItem.ITEM_TYPE>? foodSorter)
        {
            return foodSorter?.Concat(CustomItemList
                .Where(i => !foodSorter.Contains(i.Key) && i.Value.InventoryItemType == CustomInventoryItemType.FOOD)
                .Select(i => i.Key)).ToList();
        }
        
        internal static List<InventoryItem.ITEM_TYPE>? AppendCustomItem(
            ICollection<InventoryItem.ITEM_TYPE>? itemsSorter)
        {
            return itemsSorter?.Concat(CustomItemList
                .Where(i => !itemsSorter.Contains(i.Key) && i.Value.InventoryItemType == CustomInventoryItemType.ITEM)
                .Select(i => i.Key)).ToList();
        }
    }
}