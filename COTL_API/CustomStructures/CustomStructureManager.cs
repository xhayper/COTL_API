using System.Reflection;
using COTL_API.Guid;
using HarmonyLib;

namespace COTL_API.CustomStructures;

[HarmonyPatch]
public static partial class CustomStructureManager
{
    public static Dictionary<StructureBrain.TYPES, CustomStructure> CustomStructureList { get; } = new();

    public static StructureBrain.TYPES Add(CustomStructure structure)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var structureType =
            GuidManager.GetEnumValue<StructureBrain.TYPES>(guid, structure.InternalName);
        structure.StructureType = structureType;
        structure.ModPrefix = guid;

        CustomStructureList.Add(structureType, structure);

        if (!StructuresData.AllStructures.Contains(structureType)) StructuresData.AllStructures.Add(structureType);
        return structureType;
    }

    public static StructureBrain.TYPES GetStructureByType<T>()
    {
        return CustomStructureList.FirstOrDefault(x => x.Value.GetType() == typeof(T)).Key;
    }

    public static CustomStructure GetStructureByPrefabName(string name)
    {
        return CustomStructureList
            .First(x => x.Value.PrefabPath == name || $"Assets/{x.Value.PrefabPath}.prefab" == name).Value;
    }
}