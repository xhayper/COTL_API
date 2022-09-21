using COTL_API.CustomStructures;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
namespace COTL_API.Prefabs;

[HarmonyPatch]
public static class CustomPrefabManager
{
    public static Dictionary<string, CustomStructure> PrefabStrings { get; } = new();


    public static string GetOrCreateBuildingPrefab(CustomStructure structure)
    {
        string pstr = $"CustomBuildingPrefab_{structure.InternalName}";
        string altstr = $"Assets/{pstr}.prefab";
        if (!PrefabStrings.ContainsValue(structure))
        {
            PrefabStrings.Add(pstr, structure);
            PrefabStrings.Add(altstr, structure);
        }
        return pstr;
    }

    private static void CreateBuildingPrefabOverride(string name, ref AsyncOperationHandle<GameObject> handle)
    {
        if (!PrefabStrings.ContainsKey(name))
        {
            GetOrCreateBuildingPrefab(CustomStructureManager.GetStructureByPrefabName(name));
        }
        Sprite sprite = PrefabStrings[name].Sprite;
        handle.Completed += delegate(AsyncOperationHandle<GameObject> obj)
        {
            SpriteRenderer spriteRenderer = obj.Result.GetComponentInChildren<SpriteRenderer>();
            Structure structure = obj.Result.GetComponentInChildren<Structure>();
            structure.Type = PrefabStrings[name].StructureType;
            Sprite scaledSprite = Sprite.Create(sprite.texture, sprite.rect, new Vector2(0.5f, 0));
            spriteRenderer.sprite = scaledSprite;
            obj.Result.name = name + " (Custom Structure)";
        };
    }

    private static string pathOverride;
    private static bool getOverride;
    
    [HarmonyPatch(typeof(AddressablesImpl), "InstantiateAsync", typeof(object), typeof(InstantiationParameters), typeof(bool))]
    [HarmonyPrefix]
    public static void Addressables_InstantiateAsync(ref object key)
    {

        if (key is string path)
        {
            // Run the original code with a generic structure
            if (!path.Contains("CustomBuildingPrefab_")) return;
            pathOverride = path;
            getOverride = true;
            key = "Assets/Prefabs/Structures/Buildings/Decoration Wreath Stick.prefab";
        }
    }

    [HarmonyPatch(typeof(ResourceManager), "ProvideInstance")]
    [HarmonyPostfix]
    public static void ResourceManager_ProvideInstance(ref AsyncOperationHandle<GameObject> __result)
    {
        if (pathOverride != null)
        {
            CreateBuildingPrefabOverride(pathOverride, ref __result);
            pathOverride = null;
        }
    }

    public static GameObject CreatePlacementObjectFor(CustomStructure structure)
    {
        AsyncOperationHandle<GameObject> obj = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Placement Objects/Placement Object Security Turret Lvl2.prefab");
        obj.WaitForCompletion();
        
        Plugin.Logger.LogInfo(obj.Result);
        PlacementObject po = obj.Result.GetComponentInChildren<PlacementObject>();
        po.ToBuildAsset = structure.PrefabPath;
        po.StructureType = structure.StructureType;
        po.Bounds = structure.Bounds;
        return obj.Result;
    }
}
