using System.Reflection;
using COTL_API.Guid;

namespace COTL_API.CustomMission;

public static partial class CustomMissionManager
{
    internal static Dictionary<InventoryItem.ITEM_TYPE, CustomMission> CustomMissionList { get; } = new();

    public static InventoryItem.ITEM_TYPE Add(CustomMission mission)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var innerType = GuidManager.GetEnumValue<InventoryItem.ITEM_TYPE>(guid, mission.InternalName);
        mission.InnerType = innerType;
        mission.ModPrefix = guid;

        CustomMissionList.Add(innerType, mission);
        LogWarning($"Added: {innerType} {mission.InternalName} {mission.ModPrefix}");
        return innerType;
    }
}