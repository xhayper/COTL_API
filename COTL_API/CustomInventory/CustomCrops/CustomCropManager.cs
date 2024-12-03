using COTL_API.Guid;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Object;

namespace COTL_API.CustomInventory;

public static partial class CustomItemManager
{
    private static GameObject CropPrefab;

    public static Dictionary<InventoryItem.ITEM_TYPE, CustomCrop> CustomCropList { get; } = [];
    internal static Dictionary<InventoryItem.ITEM_TYPE, GameObject> CropObjectList { get; } = [];

    private static readonly List<CustomCrop> s_toRegister = [];

    public static InventoryItem.ITEM_TYPE Add(CustomCrop crop)
    {
        var item = Add(crop as CustomInventoryItem);
        crop.ItemType = item;
        crop.StructureType =
            GuidManager.GetEnumValue<StructureBrain.TYPES>(CustomItemList[item].ModPrefix, crop.InternalName);

        CustomCropList.Add(item, crop);
        CropObjectList.Add(item, CreateCropObject(crop));

        return item;
    }

    private static GameObject CreateCropObject(CustomCrop crop)
    {
        if (CropPrefab == null)
            GetCropAsset();

        if (CropPrefab == null)
            throw new NullReferenceException(":/ Send a bug report");

        var duplicate = Instantiate(CropPrefab);

        if (duplicate == null)
            throw new NullReferenceException("Somehow, the Crop Prefab could not be instantiated, send a bug report");

        duplicate.hideFlags = HideFlags.HideAndDontSave;
        duplicate.name = $"{crop.Name()} Crop";

        var cropController = duplicate.GetComponent<CropController>();

        cropController.CropStates = [];
        cropController.SeedType = crop.ItemType;

        Destroy(duplicate.transform.GetChild(1)); // Stage 2
        Destroy(duplicate.transform.GetChild(1)); // Stage 3
        Destroy(duplicate.transform.GetChild(1)); // Stage 4

        /*
         * DEV NOTE: this is insane, why are you like this game?  why the fuck are there two of them? why do they have
         * identical scripts? Harvest is what happens if a crop grows out without fertilizer, and BumperHarvest is with
         * fertilizer. the only difference seems to be a tiny sign...
         * it also skips stage 2 of the crop states, it goes 1,3,4, (Bumper)Harvest. WHY? why is stage 2 there?
         */

        // "Harvest" object 
        var harvest = duplicate.transform.GetChild(1);
        // "bush1" -> sprite of the final harvest crop bush
        //                 "Harvest"/"GameObject (2)/"To Shake(2)"/"Bush1"/
        var bush1 = harvest.GetChild(2).GetChild(0).GetChild(0);

        // "Bumper Harvest" object
        var bumperHarvest = duplicate.transform.GetChild(2);
        // "bumperBush1" -> the final stage that gets activated with fertilizer
        //                       "BumperHarvest"/"GameObject (2)/"To Shake(2)"/"Bush1"/
        var bumperBush1 = bumperHarvest.GetChild(3).GetChild(0).GetChild(0);

        bush1.GetComponent<SpriteRenderer>().sprite = crop.CropStates.Last();
        bumperBush1.GetComponent<SpriteRenderer>().sprite = crop.CropStates.Last();

        var cropState = duplicate.transform.GetChild(0);
        cropState.GetComponent<SpriteRenderer>().sprite = crop.CropStates[0];
        cropController.CropStates.Add(cropState.gameObject);
        foreach (var sprite in crop.CropStates)
        {
            var newState = Instantiate(cropState, duplicate.transform);
            newState.GetComponent<SpriteRenderer>().sprite = sprite;
            cropController.CropStates.Add(newState.gameObject);
        }
        
        cropController.CropStates.Add(harvest.gameObject);
        
        return duplicate;
    }

    private static void GetCropAsset()
    {
        LogInfo("GETTING CROP ASSET");
        var op = Addressables.Instance.LoadAssetAsync<GameObject>("Prefabs/Structures/Crops/Berry Crop");

        op.Completed += (handle) =>
        {
            LogInfo("OPERATION COMPLETED");
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                LogInfo("ASYNC OPERATION WE HAVE THE CROP PREFAB RESULT");
                LogInfo("ASYNC OPERATION WE HAVE THE CROP PREFAB RESULT");
                LogInfo("ASYNC OPERATION WE HAVE THE CROP PREFAB RESULT");
                LogInfo("ASYNC OPERATION WE HAVE THE CROP PREFAB RESULT");
                CropPrefab = handle.Result;
                CropPrefab.hideFlags = HideFlags.HideAndDontSave;
                DontDestroyOnLoad(CropPrefab);
            }
            else
            {
                LogInfo("ASYNC OPERATION FAILED WE DONT HAVE IT WE SUCK");
                LogInfo("ASYNC OPERATION FAILED WE DONT HAVE IT WE SUCK");
                LogInfo("ASYNC OPERATION FAILED WE DONT HAVE IT WE SUCK");
                LogInfo("ASYNC OPERATION FAILED WE DONT HAVE IT WE SUCK");
                //throw new NullReferenceException("Couldn't Find Berry Crop Object, Send a bug report!");
            }
        };
        LogInfo("FINSIHED GETTING CROP ASSET");
    }
}