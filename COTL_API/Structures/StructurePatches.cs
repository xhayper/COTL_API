using HarmonyLib;
using Lamb.UI.BuildMenu;
using System.Collections.Generic;
using System.Linq;

namespace COTL_API.Structures;

public partial class CustomStructureManager
{
    [HarmonyPatch(typeof(StructureBrain), "CreateBrain")]
    [HarmonyPrefix]
    private static bool StructureBrain_CreateBrain(ref StructureBrain __result, StructuresData data)
    {
        if (!CustomStructures.ContainsKey(data.Type)) return true;
        StructureBrain structureBrain = new StructureBrain();
        StructureBrain.ApplyConfigToData(data);
        structureBrain.Init(data);
        StructureBrain._brainsByID.Add(data.ID, structureBrain);
        StructureManager.StructuresAtLocation(data.Location).Add(structureBrain);
        __result = structureBrain;
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "GetInfoByType")]
    [HarmonyPrefix]
    private static bool StructuresData_GetInfoByType(ref StructuresData __result, StructureBrain.TYPES Type, int variantIndex)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].StructuresData;
        
        if (__result.PrefabPath == null) __result.PrefabPath = CustomStructures[Type].PrefabPath;
        
        __result.Type = Type;
        __result.VariantIndex = variantIndex;
        __result.IsUpgrade = StructuresData.IsUpgradeStructure(__result.Type);
        __result.UpgradeFromType = StructuresData.GetUpgradePrerequisite(__result.Type);
        
        return false;
    }

    [HarmonyPatch(typeof(FollowerCategory), "GetStructuresForCategory")]
    [HarmonyPostfix]
    private static void FollowerCategory_GetInfoByType(ref List<StructureBrain.TYPES> __result, FollowerCategory.Category category)
    {
        __result.AddRange(from structure in CustomStructures.Values where structure.Category == category select structure.StructureType);
    }

    [HarmonyPatch(typeof(StructuresData), "GetUnlocked")]
    [HarmonyPrefix]
    private static void StructuresData_GetUnlocked(StructureBrain.TYPES Types)
    {
        if (!CustomStructures.ContainsKey(Types)) return;
        if (!DataManager.Instance.UnlockedStructures.Contains(Types)) DataManager.Instance.UnlockedStructures.Add(Types);
        if (!DataManager.Instance.RevealedStructures.Contains(Types)) DataManager.Instance.RevealedStructures.Add(Types);
    }

    [HarmonyPatch(typeof(TypeAndPlacementObjects), "GetByType")]
    [HarmonyPrefix]
    private static bool TypeAndPlacementObjects_GetByType(ref TypeAndPlacementObject __result, StructureBrain.TYPES Type)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].GetTypeAndPlacementObject();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "BuildDurationGameMinutes")]
    [HarmonyPrefix]
    private static bool StructuresData_BuildDurationGameMinutes(ref int __result, StructureBrain.TYPES Type)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].BuildDurationMinutes;
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "GetCost")]
    [HarmonyPrefix]
    private static bool StructuresData_BuildDurationGameMinutes(ref List<StructuresData.ItemCost> __result, StructureBrain.TYPES Type)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].Cost;
        return false;
    }
}