using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using COTL_API.CustomStructures;
using COTL_API.Helpers;
using UnityEngine;
using HarmonyLib;

namespace COTL_API.Prefabs;

[HarmonyPatch]
public static class CustomPrefabManager
{
    private static Dictionary<string, CustomStructure> PrefabStrings { get; } = new();

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
            GetOrCreateBuildingPrefab(CustomStructureManager.GetStructureByPrefabName(name));
        }

        var sprite = PrefabStrings[name].Sprite;
        handle.Completed += delegate(AsyncOperationHandle<GameObject> obj)
        {
            var spriteRenderer = obj.Result.GetComponentInChildren<SpriteRenderer>();
            var structure = obj.Result.GetComponentInChildren<Structure>();
            structure.Type = PrefabStrings[name].StructureType;
            var scaledSprite = Sprite.Create(sprite.texture, sprite.rect, new Vector2(0.5f, 0));
            spriteRenderer.sprite = scaledSprite;
            obj.Result.name = name + " (Custom Structure)";
        };
    }

    private static string? pathOverride;

    [HarmonyPatch(typeof(AddressablesImpl), "InstantiateAsync", typeof(object), typeof(InstantiationParameters),
        typeof(bool))]
    [HarmonyPrefix]
    public static void Addressables_InstantiateAsync(ref object key)
    {
        if (key is not string path) return;

        // Run the original code with a generic structure
        if (!path.Contains("CustomBuildingPrefab_")) return;
        pathOverride = path;
        key = "Assets/Prefabs/Structures/Buildings/Decoration Wreath Stick.prefab";
    }

    [HarmonyPatch(typeof(ResourceManager), "ProvideInstance")]
    [HarmonyPostfix]
    public static void ResourceManager_ProvideInstance(ref AsyncOperationHandle<GameObject> __result)
    {
        if (pathOverride == null) return;

        CreateBuildingPrefabOverride(pathOverride, ref __result);
        pathOverride = null;
    }

    public static GameObject CreatePlacementObjectFor(CustomStructure structure)
    {
        var obj =
            Addressables.LoadAssetAsync<GameObject>(
                "Assets/Prefabs/Placement Objects/Placement Object Security Turret Lvl2.prefab");
        obj.WaitForCompletion();

        LogHelper.LogInfo(obj.Result);
        var po = obj.Result.GetComponentInChildren<PlacementObject>();
        po.ToBuildAsset = structure.PrefabPath;
        po.StructureType = structure.StructureType;
        po.Bounds = structure.Bounds;
        return obj.Result;
    }
}