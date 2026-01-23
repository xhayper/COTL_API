using COTL_API.Guid;
using HarmonyLib;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Object;

namespace COTL_API.CustomInventory;

public static partial class CustomItemManager
{
    private const string AssetPath = "Prefabs/Structures/Crops/Berry Crop";
    internal static GameObject CropPrefab = null!;

    public static Dictionary<InventoryItem.ITEM_TYPE, CustomCrop> CustomCropList { get; } = [];
    private static Dictionary<InventoryItem.ITEM_TYPE, CropController> CropObjectList { get; } = [];

    public static InventoryItem.ITEM_TYPE Add(CustomCrop crop)
    {
        var item = Add(crop as CustomInventoryItem);
        crop.ItemType = item;
        crop.StructureType =
            GuidManager.GetEnumValue<StructureBrain.TYPES>(CustomItemList[item].ModPrefix, crop.InternalName);

        CustomCropList.Add(item, crop);

        return item;
    }

    private static void CreateCropObject(CustomCrop crop)
    {
        if (CropPrefab == null)
            throw new NullReferenceException("This REALLY shouldn't happen, send a bug report!");

        var duplicate = Instantiate(CropPrefab);

        if (duplicate == null)
            throw new NullReferenceException("Somehow, the Crop Prefab could not be instantiated, send a bug report");

        duplicate.transform.name = $"{crop.Name()} Crop";

        var cropController = duplicate.GetComponent<CropController>();

        cropController.CropStates = [];
        cropController.SeedType = crop.ItemType;

        // remove extra growth stages in case there are less than 4
        DestroyImmediate(duplicate.transform.GetChild(1).gameObject); // Stage 2
        DestroyImmediate(duplicate.transform.GetChild(1).gameObject); // Stage 3
        DestroyImmediate(duplicate.transform.GetChild(1).gameObject); // Stage 4

        var harvest = duplicate.transform.GetChild(1);
        var bush = harvest.GetChild(2).GetChild(0).GetChild(0);

        var bumperHarvest = duplicate.transform.GetChild(2);
        var bumperBush = bumperHarvest.GetChild(3).GetChild(0).GetChild(0);

        var cropState = duplicate.transform.GetChild(0);
        cropController.CropStates.Add(cropState.gameObject);

        if (crop.CropStates.Count > 0)
        {
            bush.GetComponent<SpriteRenderer>().sprite = crop.CropStates.Last();
            bumperBush.GetComponent<SpriteRenderer>().sprite = crop.CropStates.Last();
            cropState.GetComponent<SpriteRenderer>().sprite = crop.CropStates[0];
        }

        for (var i = 1; i < crop.CropStates.Count - 1; i++)
        {
            var newState = Instantiate(cropState, duplicate.transform);
            newState.name = $"Crop {i + 1}";
            newState.SetSiblingIndex(i);
            newState.GetComponent<SpriteRenderer>().sprite = crop.CropStates[i];
            cropController.CropStates.Add(newState.gameObject);
        }

        cropController.CropStates.Add(harvest.gameObject);

        // Ensures that the object doesn't get deleted between scene loads
        duplicate.hideFlags = HideFlags.HideAndDontSave;

        CropObjectList.Add(crop.ItemType, duplicate.GetComponent<CropController>());
    }

    public static void InitiateCustomCrops()
    {
        LogInfo("Getting Crop Asset");
        var op = Addressables.Instance.LoadAssetAsync<GameObject>(AssetPath);
        op.Completed += handle =>
        {
            if (op.Status != AsyncOperationStatus.Succeeded)
                throw new NullReferenceException("Couldn't Find Berry Crop Object, Send a bug report!");

            CropPrefab = handle.Result;
            CropPrefab.SetActive(false);
            CustomCropList.Do(x => CreateCropObject(x.Value));
        };
    }
}