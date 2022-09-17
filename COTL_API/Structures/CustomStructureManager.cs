using COTL_API.Guid;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace COTL_API.Structures;

[HarmonyPatch]
public static partial class CustomStructureManager
{
    public static Dictionary<StructureBrain.TYPES, CustomStructure> CustomStructures { get; } = new();

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

    public static StructureBrain.TYPES GetStructureByType<T>()
    {
        return CustomStructures.First(x => x.Value is T).Key;
    }

    public static CustomStructure GetStructureByPrefabName(string name)
    {
        return CustomStructures.First(x => (x.Value.PrefabPath == name) || ($"Assets/{x.Value.PrefabPath}.prefab" == name)).Value;
    }
}