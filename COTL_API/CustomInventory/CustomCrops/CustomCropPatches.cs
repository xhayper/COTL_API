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
    
    
    [HarmonyPatch(typeof(StructuresData), nameof(StructuresData.GetInfoByType))]
    [HarmonyPostfix]
    private static void StructureData_GetInfoByType(StructureBrain.TYPES Type, ref StructuresData __result)
    {
        if (CustomCropList.Values.All(x => x.StructureType != Type)) return;
    
        var crop = CustomCropList.Values.First(x => x.StructureType == Type);
        // Not sure that this is necessary but just in case?
        __result = new StructuresData
        {
            PrefabPath = "Prefabs/Structures/Other/Berry Bush",
            DontLoadMe = true,
            ProgressTarget = crop.PickingTime,
            MultipleLootToDrop = crop.HarvestResult,
            LootCountToDropRange = crop.CropCountToDropRange,
            Type = crop.StructureType
        };
    }
    
    [HarmonyPatch(typeof(StructureBrain), nameof(StructureBrain.CreateBrain))]
    [HarmonyPostfix]
    private static void StructureBrain_CreateBrain(StructuresData data, ref StructureBrain __result)
    {
        if (CustomCropList.Values.All(x => x.StructureType != data.Type)) return;
    
        __result = new Structures_BerryBush();
    }
    
    [HarmonyPatch(typeof(FarmPlot), nameof(FarmPlot.Awake))]
    [HarmonyPostfix]
    private static void FarmPlot_Postfix(FarmPlot __instance)
    {
        foreach(var kvp in CropObjectList)
        {
            __instance._cropPrefabsBySeedType.Add(kvp.Key, kvp.Value);
        }
    }

    [HarmonyPatch(typeof(Interaction_Berries), nameof(Interaction_Berries.OnBrainAssigned))]
    [HarmonyPostfix]
    private static void Interaction_Berries_OnBrainAssigned(Interaction_Berries __instance)
    {
        var cropController = __instance.GetComponentInParent<CropController>();
        
        if (cropController == null) return;
        if (!CustomCropList.TryGetValue(cropController.SeedType, out var crop)) return;

        __instance.StructureBrain.Data.MultipleLootToDrop = crop.HarvestResult;
        __instance.StructureBrain.Data.LootCountToDropRange = crop.CropCountToDropRange;
        __instance.BerryPickingIncrements = 1.25f / crop.PickingTime;
    }
    
    [HarmonyPatch(typeof(Interaction_Berries), nameof(Interaction_Berries.UpdateLocalisation))]
    [HarmonyPostfix]
    private static void Interaction_Berries_UpdateLocalisation(Interaction_Berries __instance)
    {
        var cropController = __instance.GetComponentInParent<CropController>();
        if (cropController == null) return;
        if (!CustomCropList.TryGetValue(cropController.SeedType, out var crop)) return;
        
        __instance.sLabelName = crop.HarvestText;
    }
}   