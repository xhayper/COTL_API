using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using COTL_API.Guid;
using UnityEngine;
using System.Linq;
using HarmonyLib;
using Lamb.UI;

namespace COTL_API.CustomInventory;

[HarmonyPatch]
public class CustomItemManager
{
    public static readonly Dictionary<InventoryItem.ITEM_TYPE, CustomInventoryItem> CustomItems = new();

    public static InventoryItem.ITEM_TYPE Add(CustomInventoryItem item)
    {
        string guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        InventoryItem.ITEM_TYPE itemType = GuidManager.GetEnumValue<InventoryItem.ITEM_TYPE>(guid, item.InternalName);
        item.ItemType = itemType;
        item.ModPrefix = guid;

        CustomItems.Add(itemType, item);

        return itemType;
    }

    // Patch `ItemInfoCard` not using `InventoryItem`'s method
    [HarmonyPatch(typeof(ItemInfoCard), nameof(ItemInfoCard.Configure))]
    [HarmonyPostfix]
    public static void ItemInfoCard_Configure(ItemInfoCard __instance, InventoryItem.ITEM_TYPE config)
    {
        if (!CustomItems.ContainsKey(config)) return;

        __instance._itemHeader.text = CustomItems[config].Name();
        __instance._itemLore.text = CustomItems[config].Lore();
        __instance._itemDescription.text = CustomItems[config].Description();
    }

    // Prepare for lag...
    [HarmonyPatch(typeof(Interaction_AddFuel), nameof(Interaction_AddFuel.Update))]
    [HarmonyPrefix]
    public static void Interaction_AddFuel_Update(Interaction_AddFuel __instance)
    {
        foreach (InventoryItem.ITEM_TYPE itemType in CustomItems.Keys)
        {
            if (!CustomItems[itemType].IsBurnableFuel && __instance.fuel.Contains(itemType))
                __instance.fuel.Remove(itemType);
            if (CustomItems[itemType].IsBurnableFuel && !__instance.fuel.Contains(itemType))
                __instance.fuel.Add(itemType);
        }
    }

    [HarmonyPatch(typeof(FontImageNames), nameof(FontImageNames.GetIconByType))]
    [HarmonyPrefix]
    public static bool FontImageNames_GetIconByType(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!CustomItems.ContainsKey(Type)) return true;
        __result = CustomItems[Type].InventoryStringIcon();
        return false;
    }

    [HarmonyPatch(typeof(Lamb.UI.Assets.InventoryIconMapping), nameof(Lamb.UI.Assets.InventoryIconMapping.GetImage),
        typeof(InventoryItem.ITEM_TYPE))]
    [HarmonyPrefix]
    public static bool InventoryIconMapping_GetImage(InventoryItem.ITEM_TYPE type, ref Sprite __result)
    {
        if (!CustomItems.ContainsKey(type)) return true;
        __result = CustomItems[type].InventoryIcon;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.Name))]
    [HarmonyPrefix]
    public static bool InventoryItem_Name(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!CustomItems.ContainsKey(Type)) return true;
        __result = CustomItems[Type].Name();
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.LocalizedName))]
    [HarmonyPrefix]
    public static bool InventoryItem_LocalizedName(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!CustomItems.ContainsKey(Type)) return true;
        __result = CustomItems[Type].LocalizedName();
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.Description))]
    [HarmonyPrefix]
    public static bool InventoryItem_Description(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!CustomItems.ContainsKey(Type)) return true;
        __result = CustomItems[Type].Description();
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.LocalizedDescription))]
    [HarmonyPrefix]
    public static bool InventoryItem_LocalizedDescription(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!CustomItems.ContainsKey(Type)) return true;
        __result = CustomItems[Type].LocalizedDescription();
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.Lore))]
    [HarmonyPrefix]
    public static bool InventoryItem_Lore(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!CustomItems.ContainsKey(Type)) return true;
        __result = CustomItems[Type].Lore();
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.GetItemCategory))]
    [HarmonyPrefix]
    public static bool InventoryItem_ItemCategory(InventoryItem.ITEM_TYPE type,
        ref InventoryItem.ITEM_CATEGORIES __result)
    {
        if (!CustomItems.ContainsKey(type)) return true;
        __result = CustomItems[type].ItemCategory;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.GetSeedType))]
    [HarmonyPrefix]
    public static bool InventoryItem_GetSeedType(InventoryItem.ITEM_TYPE type, ref InventoryItem.ITEM_TYPE __result)
    {
        if (!CustomItems.ContainsKey(type)) return true;
        __result = CustomItems[type].SeedType;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.FuelWeight), typeof(InventoryItem.ITEM_TYPE))]
    [HarmonyPrefix]
    public static bool InventoryItem_FuelWeight(InventoryItem.ITEM_TYPE type, ref int __result)
    {
        if (!CustomItems.ContainsKey(type)) return true;
        __result = CustomItems[type].FuelWeight;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.FoodSatitation))]
    [HarmonyPrefix]
    public static bool InventoryItem_FoodSatitation(InventoryItem.ITEM_TYPE Type, ref int __result)
    {
        if (!CustomItems.ContainsKey(Type)) return true;
        __result = CustomItems[Type].FoodSatitation;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.IsFish))]
    [HarmonyPrefix]
    public static bool InventoryItem_IsFish(InventoryItem.ITEM_TYPE Type, ref bool __result)
    {
        if (!CustomItems.ContainsKey(Type)) return true;
        __result = CustomItems[Type].IsFish;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.IsFood))]
    [HarmonyPrefix]
    public static bool InventoryItem_IsFood(InventoryItem.ITEM_TYPE Type, ref bool __result)
    {
        if (!CustomItems.ContainsKey(Type)) return true;
        __result = CustomItems[Type].IsFood;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.IsBigFish))]
    [HarmonyPrefix]
    public static bool InventoryItem_IsBigFish(InventoryItem.ITEM_TYPE Type, ref bool __result)
    {
        if (!CustomItems.ContainsKey(Type)) return true;
        __result = CustomItems[Type].IsBigFish;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.CanBeGivenToFollower))]
    [HarmonyPrefix]
    public static bool InventoryItem_CanBeGivenToFollower(InventoryItem.ITEM_TYPE Type, ref bool __result)
    {
        if (!CustomItems.ContainsKey(Type)) return true;
        __result = CustomItems[Type].CanBeGivenToFollower;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.CapacityString))]
    [HarmonyPrefix]
    public static bool InventoryItem_CapacityString(InventoryItem.ITEM_TYPE type, int minimum, ref string __result)
    {
        if (!CustomItems.ContainsKey(type)) return true;
        __result = CustomItems[type].CapacityString(minimum);
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.AllPlantables), MethodType.Getter)]
    [HarmonyPostfix]
    public static void InventoryItem_AllPlantables(ref List<InventoryItem.ITEM_TYPE> __result)
    {
        __result.AddRange(CustomItems.Where(x => x.Value.IsPlantable).Select(x => x.Key));
    }
    
    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.AllSeeds), MethodType.Getter)]
    [HarmonyPostfix]
    public static void InventoryItem_AllSeeds(ref List<InventoryItem.ITEM_TYPE> __result)
    {
        __result.AddRange(CustomItems.Where(x => x.Value.IsSeed).Select(x => x.Key));
    }
    
    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.AllBurnableFuel), MethodType.Getter)]
    [HarmonyPostfix]
    public static void InventoryItem_AllBurnableFuel(ref List<InventoryItem.ITEM_TYPE> __result)
    {
        __result.AddRange(CustomItems.Where(x => x.Value.IsBurnableFuel).Select(x => x.Key));
    }

    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetAllFoods))]
    [HarmonyPostfix]
    public static void CookingData_GetAllFoods(ref InventoryItem.ITEM_TYPE[] __result)
    {
        InventoryItem.ITEM_TYPE[] copy = __result;
        __result = __result.Concat((CustomItems.Where(i => !copy.Contains(i.Key) && i.Value.IsFood).Select(i => i.Key)))
            .ToArray();
    }

    [HarmonyPatch(typeof(InventoryMenu))]
    public static class InventoryMenu_Patches
    {
        [HarmonyPatch(nameof(InventoryMenu.OnShowStarted))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> OnShowStarted(IEnumerable<CodeInstruction> instructions)
        {
            foreach (CodeInstruction instruction in instructions)
            {
                yield return instruction;

                if (instruction.LoadsField(typeof(InventoryMenu).GetField("_currencyFilter",
                        BindingFlags.NonPublic | BindingFlags.Instance)))
                    yield return new CodeInstruction(OpCodes.Call,
                        SymbolExtensions.GetMethodInfo(() => AppendCustomCurrencies(null)));
            }
        }

        internal static List<InventoryItem.ITEM_TYPE> AppendCustomCurrencies(
            List<InventoryItem.ITEM_TYPE> currencyFilter)
        {
            return currencyFilter.Concat(CustomItems.Where((i) => !currencyFilter.Contains(i.Key) && i.Value.IsCurrency)
                .Select(i => i.Key)).ToList();
        }
    }
}