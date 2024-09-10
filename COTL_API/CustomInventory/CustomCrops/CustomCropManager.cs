using COTL_API.Guid;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace COTL_API.CustomInventory;

public static partial class CustomItemManager
{
    public static Dictionary<InventoryItem.ITEM_TYPE, CustomSeed> CustomCropList { get; } = [];

    public static Dictionary<InventoryItem.ITEM_TYPE, CropController> CustomCropControllers { get; } = [];

    public static InventoryItem.ITEM_TYPE Add(CustomSeed seed)
    {
        var item = Add(seed as CustomInventoryItem);
        seed.StructureType =
            GuidManager.GetEnumValue<StructureBrain.TYPES>(CustomItemList[item].ModPrefix, seed.InternalName);

        CustomCropList.Add(item, seed);
        CustomCropControllers.Add(item, CreateCropController(seed));

        return item;
    }

    #region Custom CropControllers

    private static CropController ControllerPrefab;

    private static CropController CreateCropController(CustomSeed seed)
    {
        if (!ControllerPrefab)
        {
            ControllerPrefab = CreateGenericController();
        }

        var holderObject = new GameObject();
        holderObject.SetActive(false);

        var customController =
            Object.Instantiate(ControllerPrefab, null, instantiateInWorldSpace: false) as CropController;

        customController.transform.SetParent(holderObject.transform);

        customController.SeedType = seed.ItemType;

        var cropStage = customController.CropStates[0];
        var finalCrop = customController.CropStates[customController.CropStates.Count - 1];

        customController.CropStates = [];

        for (var i = 0; i < seed.CropStates.Count - 1; i++)
        {
            var sprite = seed.CropStates[i];
            var newObj = Object.Instantiate(cropStage, null, instantiateInWorldSpace: false) as GameObject;
            newObj?.transform.SetParent(customController.transform);
            Object.DontDestroyOnLoad(newObj);
            customController.CropStates.Add(newObj);
            customController.CropStates[i].GetComponent<SpriteRenderer>().sprite = sprite;
        }

        var newFinalCrop = Object.Instantiate(finalCrop, null, instantiateInWorldSpace: false) as GameObject;
        newFinalCrop?.transform.SetParent(customController.transform);
        finalCrop.GetComponent<SpriteRenderer>().sprite = seed.CropStates[seed.CropStates.Count-1];
        Object.DontDestroyOnLoad(newFinalCrop);
        customController.CropStates.Add(newFinalCrop);

        customController.CropStates[customController.CropStates.Count - 1]
            .GetComponentInChildren<SpriteRenderer>()
            .sprite = seed.CropStates[seed.CropStates.Count - 1];

        customController.CropStates[customController.CropStates.Count - 1].GetComponent<Structure>().Type =
            seed.StructureType;

        Object.DontDestroyOnLoad(customController);
        return customController;
    }

    private static CropController CreateGenericController()
    {
        var cropController = new GameObject("Custom Crop").AddComponent<CropController>();

        var cropState = new GameObject("Crop");
        cropState.AddComponent<SpriteRenderer>();

        var finalCropState = new GameObject("Harvest");
        finalCropState.AddComponent<StateMachine>();
        finalCropState.AddComponent<DropMultipleLootOnDeath>();
        //var interactionBerries = finalCropState.AddComponent<Interaction_Berries>();
        var structure = finalCropState.AddComponent<Structure>();
        finalCropState.AddComponent<Health>();

        var playerPosLeft = new GameObject("PlayerPositionLeft");
        var playerPosRight = new GameObject("PlayerPositionRight");
        playerPosLeft.transform.SetParent(finalCropState.transform);
        playerPosRight.transform.SetParent(finalCropState.transform);

        var bush = new GameObject("bush");
        bush.AddComponent<SpriteRenderer>();
        var toShake = new GameObject("ToShake");
        var gameObj = new GameObject("GamObject");
        var stump = new GameObject("Stump");
        bush.transform.SetParent(toShake.transform);
        toShake.transform.SetParent(gameObj.transform);
        stump.transform.SetParent(gameObj.transform);
        gameObj.transform.SetParent(finalCropState.transform);


        // interactionBerries.berryToShake = toShake;
        // interactionBerries.berryBush_Normal = gameObj;
        // interactionBerries.Structure = structure;
        // interactionBerries.PlayerPositionLeft = playerPosLeft;
        // interactionBerries.PlayerPositionRight = playerPosRight;
        // interactionBerries.berryToShake = stump;

        cropController.CropStates.Add(cropState);
        cropController.CropStates.Add(finalCropState);

        return cropController;
    }

    #endregion
}