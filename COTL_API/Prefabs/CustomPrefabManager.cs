using COTL_API.CustomStructures;
using HarmonyLib;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace COTL_API.Prefabs;

[HarmonyPatch]
public static class CustomPrefabManager
{
    private static string? _pathOverride;
    internal static Dictionary<string, CustomStructure> PrefabStrings { get; } = new();

    public static string GetOrCreateBuildingPrefab(CustomStructure structure)
    {
        var pstr = $"CustomBuildingPrefab_{structure.InternalName}";
        var altstr = $"Assets/{pstr}.prefab";

        if (PrefabStrings.ContainsValue(structure)) return pstr;

        PrefabStrings.Add(pstr, structure);
        PrefabStrings.Add(altstr, structure);
        return pstr;
    }

    private static void CreateBuildingPrefabOverride(string name, ref AsyncOperationHandle<GameObject> handle)
    {
        if (!PrefabStrings.ContainsKey(name))
        {
            var structure = CustomStructureManager.GetStructureByPrefabName(name);
            switch (structure)
            {
                case null:
                    LogWarning($"Structure {name} not found in CustomStructureManager");
                    return;
                default:
                    GetOrCreateBuildingPrefab(structure);
                    break;
            }
        }


        var sprite = PrefabStrings[name].Sprite;
        handle.Completed += delegate(AsyncOperationHandle<GameObject> obj)
        {
            var spriteRenderer = obj.Result.GetComponentInChildren<SpriteRenderer>();
            var structure = obj.Result.GetComponentInChildren<Structure>();
            structure.Type = PrefabStrings[name].StructureType;
            if (PrefabStrings[name].BuildingParts.Count > 0)
            {
                LogInfo($"Overriding building parts for {name}");
                CustomStructureManager.OverrideStructureBuilding(obj.Result, PrefabStrings[name].BuildingParts);
            }
            else
            {
                var scaledSprite = Sprite.Create(sprite.texture, sprite.rect, new Vector2(0.5f, 0));
                spriteRenderer.sprite = scaledSprite;
            }

            obj.Result.name = name + " (Custom Structure)";
        };
    }

    [HarmonyWrapSafe]
    [HarmonyPatch(typeof(AddressablesImpl), "InstantiateAsync", typeof(object), typeof(InstantiationParameters),
        typeof(bool))]
    [HarmonyPrefix]
    private static void Addressables_InstantiateAsync(ref object key)
    {
        if (key is not string path) return;

        // Run the original code with a generic structure
        if (!path.Contains("CustomBuildingPrefab_")) return;

        var cs = CustomStructureManager.CustomStructureExists(path);
        if (!cs)
        {
            LogWarning($"Structure attempting to be loaded no longer exists. Path: {path}");
            _pathOverride = null;

            //this is to stop invalid key exceptions from Unity (1 per custom structure in the save file)
            key = "Assets/Prefabs/Structures/Other/Rubble.prefab";

            return;
        }

        _pathOverride = path;
        key = "Assets/Prefabs/Structures/Buildings/Decoration Wreath Stick.prefab";
    }

    [HarmonyWrapSafe]
    [HarmonyPatch(typeof(ResourceManager), "ProvideInstance")]
    [HarmonyPostfix]
    private static void ResourceManager_ProvideInstance(ref AsyncOperationHandle<GameObject> __result)
    {
        if (_pathOverride == null) return;

        CreateBuildingPrefabOverride(_pathOverride, ref __result);
        _pathOverride = null;
    }

    public static GameObject CreatePlacementObjectFor(CustomStructure structure)
    {
        var obj = TypeAndPlacementObjects.GetByType(StructureBrain.TYPES.BED).PlacementObject;

        var po = obj.GetComponent<PlacementObject>();
        po.ToBuildAsset = structure.PrefabPath;
        po.StructureType = structure.StructureType;
        po.Bounds = structure.Bounds;

        return obj;
    }
}