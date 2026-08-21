using HarmonyLib;
using Lamb.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace COTL_API.CustomWorldMapNode;
[HarmonyPatch]
public partial class CustomWorldMapNodeManager
{
    private static GameObject? NodeTemplate;
    private static readonly Dictionary<WorldMapIcon.WorldMapRegion, GameObject> CustomWorldMapNodeList = [];

    [HarmonyPatch(typeof(UIWorldMapMenuController), nameof(UIWorldMapMenuController.Start))]
    [HarmonyPostfix]
    public static void UIWorldMapMenuController_Start(UIWorldMapMenuController __instance)
    {
        var mapContainerTransform = __instance._mapContainer.transform;
        NodeTemplate = mapContainerTransform.Find("Locations/Base").gameObject;
    }

    [HarmonyPatch(typeof(UIWorldMapMenuController), nameof(UIWorldMapMenuController.OnShowStarted))]
    [HarmonyPrefix]
    public static void UIWorldMapMenuController_OnShowStarted(UIWorldMapMenuController __instance)
    {
        CustomWorldMapNodeList.Clear();
        var mapContainerTransform = __instance._mapContainer.transform;
        var locationsTransfrom = mapContainerTransform.Find("Locations");
        var layersTransform = mapContainerTransform.Find("Layers");
        if (layersTransform is null) 
        { 
            Log(BepInEx.Logging.LogLevel.Error, "Can't find Layers");
            return;
        }
        if (NodeTemplate is null)
        {
            Log(BepInEx.Logging.LogLevel.Error, "Can't find base node.");
            return;
        }

        var isOriginalObjectActive = NodeTemplate.activeSelf;
        NodeTemplate.SetActive(false);

        foreach (var customWorldMapNode in CustomWorldMapNodes.Select(x => x.Value))
        {
            var newWorldMapNode = Object.Instantiate(NodeTemplate, locationsTransfrom);
            newWorldMapNode.name = customWorldMapNode.InternalName;

            var worldMapIcon = newWorldMapNode.GetComponent<WorldMapIcon>() ?? throw new Exception("WorldMapIcon not found!");
            worldMapIcon._location = customWorldMapNode.Location;
            worldMapIcon._locationTerm = "NAMES/Places/" + customWorldMapNode.InternalName;
            worldMapIcon._mapRegion = customWorldMapNode.MapRegion;
            worldMapIcon._parallaxPosition = customWorldMapNode.ParallaxPosition;
            var newScene = new InspectorScene
            {
                SceneName = customWorldMapNode.SceneToLoad,
            };
            worldMapIcon._scene = newScene;
            var layerToPutMarker = layersTransform.Find(customWorldMapNode.LayerLocation);
            if (layerToPutMarker is null)
            {
                layerToPutMarker = layersTransform.Find("Base");
                Log(BepInEx.Logging.LogLevel.Warning, "Can't find layer to put marker, check the layer name");
            }
            if (layerToPutMarker is null) throw new Exception("Something gone horribly wrong");
            worldMapIcon._layer = layerToPutMarker.GetComponent<ParallaxLayer>();
            var nodeMarker = new GameObject(customWorldMapNode.InternalName + "_Marker");
            var nodeMarkerTransform = nodeMarker.AddComponent<RectTransform>();
            nodeMarker.transform.SetParent(layerToPutMarker);
            nodeMarkerTransform.localPosition = customWorldMapNode.Position;
            worldMapIcon._localPoint = nodeMarkerTransform;
            if (customWorldMapNode.OnLocationSelected is null)
            {
                worldMapIcon.OnLocationSelected += new Action<WorldMapIcon>(__instance.OnLocationSelected);
            }
            else
            {
                worldMapIcon.OnLocationSelected = new Action<WorldMapIcon>(worldMapIcon =>
                {
                    if (__instance.isLoadingAssets)
                    {
                        return;
                    }

                    if (worldMapIcon.Location != DataManager.Instance.CurrentLocation)
                    {
                        DataManager.Instance.CurrentLocation = worldMapIcon.Location;
                        __instance._canvasGroup.interactable = false;
                        __instance.Hide(true);
                        SaveAndLoad.Save();
                        if (!DataManager.Instance.VisitedLocations.Contains(worldMapIcon.Location))
                        {
                            DataManager.Instance.VisitedLocations.Add(worldMapIcon.Location);
                        }
                    }
                });
                worldMapIcon.OnLocationSelected += customWorldMapNode.OnLocationSelected;
            }
            if (customWorldMapNode.ShowConditions is null)
            {
                if (DataManager.Instance.DiscoveredLocations.Contains(worldMapIcon.Location) || __instance._revealLocation == worldMapIcon.Location)
                {
                    worldMapIcon.gameObject.SetActive(true);
                }
                if (__instance._revealLocation == FollowerLocation.None)
                {
                    worldMapIcon.gameObject.SetActive(true);
                }
            }
            else
            {
                if (customWorldMapNode.ShowConditions()) worldMapIcon.gameObject.SetActive(true);
            }
            CustomWorldMapNodeList.Add(customWorldMapNode.MapRegion, newWorldMapNode);
        }

        NodeTemplate.SetActive(true);
    }
}