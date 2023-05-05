using System.Reflection;
using COTL_API.Guid;
using HarmonyLib;

namespace COTL_API.CustomRelics;

public static partial class CustomRelicManager
{
    public static Dictionary<RelicType, CustomRelicData> CustomRelicDataList { get; } = new();

    public static RelicType Add(CustomRelicData relicData)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var relicType = GuidManager.GetEnumValue<RelicType>(guid, relicData.InternalName);

        relicData.ModPrefix = guid;
        relicData.RelicType = relicType;
        relicData.Init();

        CustomRelicDataList.Add(relicType, relicData);

        return relicType;
    }
}