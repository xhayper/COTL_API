using COTL_API.CustomInventory;
using COTL_API.Guid;
using HarmonyLib;
using Lamb.UI.BuildMenu;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace COTL_API.Structures;

[HarmonyPatch]
public class CustomStructureManager
{
    public static readonly Dictionary<StructureBrain.TYPES, CustomStructure> CustomStructures = new();
    public static StructureBrain.TYPES Add(CustomStructure structure)
    {
        string guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        StructureBrain.TYPES structureType = GuidManager.GetEnumValue<StructureBrain.TYPES>(guid, structure.InternalName);
        structure.StructureType = structureType;
        structure.ModPrefix = guid;

        CustomStructures.Add(structureType, structure);

        if (!StructuresData.AllStructures.Contains(structureType)) StructuresData.AllStructures.Add(structureType);
        return structureType;
    }
    
    [HarmonyPatch(typeof(StructureBrain), "CreateBrain")]
    [HarmonyPrefix]
    public static bool StructureBrain_CreateBrain(ref StructureBrain __result, StructuresData data)
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
    public static bool StructuresData_GetInfoByType(ref StructuresData __result, StructureBrain.TYPES Type, int variantIndex)
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
    public static void FollowerCategory_GetInfoByType(ref List<StructureBrain.TYPES> __result, FollowerCategory.Category category)
    {
        __result.AddRange(from structure in CustomStructures.Values where structure.Category == category select structure.StructureType);
    }
    
    [HarmonyPatch(typeof(StructuresData), "GetUnlocked")]
    [HarmonyPrefix]
    public static void StructuresData_GetUnlocked(StructureBrain.TYPES Types)
    {
        if (!CustomStructures.ContainsKey(Types)) return;
        if (!DataManager.Instance.UnlockedStructures.Contains(Types)) DataManager.Instance.UnlockedStructures.Add(Types);
        if (!DataManager.Instance.RevealedStructures.Contains(Types)) DataManager.Instance.RevealedStructures.Add(Types);
    }

    [HarmonyPatch(typeof(TypeAndPlacementObjects), "GetByType")]
    [HarmonyPrefix]
    public static bool TypeAndPlacementObjects_GetByType(ref TypeAndPlacementObject __result, StructureBrain.TYPES Type)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].GetTypeAndPlacementObject();
        return false;
    }
    
    [HarmonyPatch(typeof(StructuresData), "BuildDurationGameMinutes")]
    [HarmonyPrefix]
    public static bool StructuresData_BuildDurationGameMinutes(ref int __result, StructureBrain.TYPES Type)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].BuildDurationMinutes;
        return false;
    }
    
    [HarmonyPatch(typeof(StructuresData), "GetCost")]
    [HarmonyPrefix]
    public static bool StructuresData_BuildDurationGameMinutes(ref List<StructuresData.ItemCost> __result, StructureBrain.TYPES Type)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].Cost;
        return false;
    }

    public static StructureBrain.TYPES GetStructureByType<T>()
    {
        return CustomStructures.First(x => x.Value is T).Key;
    }

    public static CustomStructure GetStructureByPrefabName(string name)
    {
        return CustomStructures.First(x => (x.Value.PrefabPath == name) || ($"Assets/{x.Value.PrefabPath}.prefab" == name)).Value;
    }
}