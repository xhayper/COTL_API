using System.Collections.Generic;
using System.Reflection;
using COTL_API.Guid;

namespace COTL_API.CustomRituals;

internal class CustomRitualManager
{
    public static Dictionary<UpgradeSystem.Type, CustomRitual> CustomRituals { get; } = new();

    public static UpgradeSystem.Type Add(CustomRitual ritual)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var upgradeType = GuidManager.GetEnumValue<UpgradeSystem.Type>(guid, ritual.InternalName);
        ritual.ModPrefix = guid;
        ritual.upgradeType = upgradeType;

        CustomRituals.Add(upgradeType, ritual);
        return upgradeType;
    }
}