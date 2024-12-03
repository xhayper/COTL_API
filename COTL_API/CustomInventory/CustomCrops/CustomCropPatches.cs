using HarmonyLib;
using MonoMod.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace COTL_API.CustomInventory;

[HarmonyPatch]
public static partial class CustomItemManager
{
    [HarmonyPatch(typeof(CropController), nameof(CropController.CropStatesForSeedType))]
    [HarmonyPostfix]
    private static void CropController_CropStatesForSeedType(InventoryItem.ITEM_TYPE seedType, ref int __result)
    {
        if (!CustomCropList.TryGetValue(seedType, out var item)) return;
    
        __result = item.CropStatesCount;
    }
    
    [HarmonyPatch(typeof(CropController), nameof(CropController.CropGrowthTimes))]
    [HarmonyPostfix]
    private static void CropController_CropGrowthTimes(InventoryItem.ITEM_TYPE seedType, ref float __result)
    {
        if (!CustomCropList.TryGetValue(seedType, out var item)) return;
    
        __result = item.CropGrowthTime;
    }
    
    [HarmonyPatch(typeof(CropController), nameof(CropController.GetHarvestsPerSeedRange))]
    [HarmonyPostfix]
    private static void CropController_GetHarvestsPerSeedRange(InventoryItem.ITEM_TYPE seedType,
        ref Vector2Int __result)
    {
        if (!CustomCropList.TryGetValue(seedType, out var item)) return;
    
        __result = item.HarvestsPerSeedRange;
    }
    
    
    [HarmonyPatch(typeof(StructuresData), nameof(StructuresData.GetInfoByType))]
    [HarmonyPostfix]
    private static void StructureData_GetInfoByType(StructureBrain.TYPES Type, ref StructuresData __result)
    {
        if (CustomCropList.Values.All(x => x.StructureType != Type)) return;
    
        var crop = CustomCropList.Values.First(x => x.StructureType == Type);
    
        __result = new StructuresData
        {
            PrefabPath = "Prefabs/Structures/Other/Berry Bush",
            DontLoadMe = true,
            ProgressTarget = crop.ProgressTarget,
            MultipleLootToDrop = crop.HarvestResult.Select(item => item.Type).ToList(),
            MultipleLootToDropChance = crop.HarvestResult.Select(item => item.Probability).ToList(),
            LootCountToDropRange = crop.CropCountToDropRange,
            CropLootCountToDropRange = crop.CropCountToDropRange
        };
        __result.Type = crop.StructureType;
    }
    
    [HarmonyPatch(typeof(StructureBrain), nameof(StructureBrain.CreateBrain))]
    [HarmonyPostfix]
    private static void StructureBrain_CreateBrain(StructuresData data, ref StructureBrain __result)
    {
        if (CustomCropList.Values.All(x => x.StructureType != data.Type)) return;
    
        __result = new Structures_BerryBush();
    }

    [HarmonyPatch(typeof(FarmPlot), nameof(FarmPlot.Awake))]
    [HarmonyPrefix]
    private static void FarmPlot_Awake_Prefix(FarmPlot __instance)
    {
        __instance._cropPrefabsBySeedType.AddRange(CropObjectList);
    }
    // [HarmonyPatch(typeof(FarmPlot), nameof(FarmPlot.Awake))]
    // [HarmonyPostfix]
    // private static void FarmPlot_Awake_Postfix(FarmPlot __instance)
    // {
    //     __instance.CropPrefabs.AddRange(CropObjectList.Select(x => x.Value.GetComponent<CropController>()));
    // }
}