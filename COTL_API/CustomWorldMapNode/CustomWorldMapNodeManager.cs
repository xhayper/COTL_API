using COTL_API.Guid;
using HarmonyLib;
using Lamb.UI;
using System.Reflection;

namespace COTL_API.CustomWorldMapNode;
[HarmonyPatch]
public partial class CustomWorldMapNodeManager
{
    internal static Dictionary<WorldMapIcon.WorldMapRegion, CustomWorldMapNode> CustomWorldMapNodes { get; } = [];
    public static WorldMapIcon.WorldMapRegion Add(CustomWorldMapNode node)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var mapRegion = GuidManager.GetEnumValue<WorldMapIcon.WorldMapRegion>(guid, node.InternalName);
        node.MapRegion = mapRegion;
        node.ModPrefix = guid;

        CustomWorldMapNodes.Add(node.MapRegion, node);

        return mapRegion;
    }
}