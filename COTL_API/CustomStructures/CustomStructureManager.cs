using System.Reflection;
using COTL_API.Guid;
using HarmonyLib;
using UnityEngine;

namespace COTL_API.CustomStructures;

[HarmonyPatch]
public static partial class CustomStructureManager
{
    public static Dictionary<StructureBrain.TYPES, CustomStructure> CustomStructureList { get; } = [];

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
        //changed to FirstOrDefault instead of First so that it returns Null if not found, avoiding System.InvalidOperationException exceptions
        return CustomStructureList
            .FirstOrDefault(x => x.Value.PrefabPath == name || $"Assets/{x.Value.PrefabPath}.prefab" == name).Value;
    }


    public static bool CustomStructureExists(string name)
    {
        var exists = CustomStructureList.ToList()
            .Exists(x => x.Value.PrefabPath == name || $"Assets/{x.Value.PrefabPath}.prefab" == name);
        return exists;
    }

    internal static GameObject CreateBuildingPart(CustomStructureBuildingData data)
    {
        var go = new GameObject("Part");
        go.transform.localPosition = data.Offset;
        go.transform.localScale = new Vector3(data.Scale.x, data.Scale.y, data.Scale.z);
        go.transform.localRotation = Quaternion.Euler(data.Rotation);

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = data.Sprite;

        return go;
    }

    public static void OverrideStructureBuilding(GameObject existingStructure, List<CustomStructureBuildingData> buildingParts)
    {
        var buildingParent = new GameObject("CustomBuilding");
        buildingParent.transform.SetParent(existingStructure.transform);
        buildingParent.transform.localPosition = Vector3.zero;
        buildingParent.transform.localRotation = Quaternion.identity;
        buildingParent.transform.localScale = Vector3.one;

        //remove old building sprites
        LogInfo("Removing old building parts");
        var oldBuilding = existingStructure.GetComponentsInChildren<SpriteRenderer>();
        foreach (var sr in oldBuilding)
        {
            if (sr.gameObject == existingStructure) continue;
            sr.gameObject.SetActive(false); //or destroy?
        }
        
        foreach (var part in buildingParts)
        {
            var partGO = CreateBuildingPart(part);
            partGO.transform.SetParent(buildingParent.transform);
        }
    }
}

public class CustomStructureBuildingData
{
    public Vector3 Offset = Vector3.zero;
    public Vector3 Scale = Vector3.one;
    public Vector3 Rotation = Vector3.zero;
    public Sprite? Sprite;
}