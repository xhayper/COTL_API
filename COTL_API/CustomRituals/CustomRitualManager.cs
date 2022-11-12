using System.Collections.Generic;
using System.Reflection;
using COTL_API.Guid;

namespace COTL_API.CustomRituals;

public static partial class CustomRitualManager
{
    public static Dictionary<UpgradeSystem.Type, CustomRitual> CustomRitualList { get; } = new();

    public static UpgradeSystem.Type Add(CustomRitual ritual)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var upgradeType = GuidManager.GetEnumValue<UpgradeSystem.Type>(guid, ritual.InternalName);
        ritual.ModPrefix = guid;
        ritual.UpgradeType = upgradeType;

        CustomRitualList.Add(upgradeType, ritual);
        return upgradeType;
    }
}