using COTL_API.Helpers;
using HarmonyLib;
using Lamb.UI.BuildMenu;
using UnityEngine;

namespace COTL_API.CustomStructures;

[HarmonyPatch]
public partial class CustomStructureManager
{
    [HarmonyPatch(typeof(StructureBrain), "CreateBrain")]
    [HarmonyPrefix]
    private static bool StructureBrain_CreateBrain(ref StructureBrain __result, StructuresData data)
    {
        if (!CustomStructureList.ContainsKey(data.Type)) return true;
        var t = CustomStructureList[data.Type].GetType();
        var structureBrain = Activator.CreateInstance(t) as StructureBrain;

        StructureBrain.ApplyConfigToData(data);
        structureBrain?.Init(data);
        StructureBrain._brainsByID.Add(data.ID, structureBrain);
        StructureManager.StructuresAtLocation(data.Location).Add(structureBrain);
        __result = structureBrain!;
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "GetInfoByType")]
    [HarmonyPrefix]
    private static bool StructuresData_GetInfoByType(ref StructuresData __result, StructureBrain.TYPES Type,
        int variantIndex)
    {
        if (!CustomStructureList.ContainsKey(Type)) return true;
        __result = CustomStructureList[Type].StructuresData;

        __result.PrefabPath ??= CustomStructureList[Type].PrefabPath;

        __result.Type = Type;
        __result.VariantIndex = variantIndex;
        __result.IsUpgrade = StructuresData.IsUpgradeStructure(__result.Type);
        __result.UpgradeFromType = StructuresData.GetUpgradePrerequisite(__result.Type);

        return false;
    }

    [HarmonyPatch(typeof(FollowerCategory), "GetStructuresForCategory")]
    [HarmonyPostfix]
    private static void FollowerCategory_GetStructuresForCategory(ref List<StructureBrain.TYPES> __result,
        FollowerCategory.Category category)
    {
        __result.AddRange(from structure in CustomStructureList.Values
            where structure.Category == category
            select structure.StructureType);
    }

    [HarmonyPatch(typeof(StructuresData), "GetUnlocked")]
    [HarmonyPrefix]
    private static void StructuresData_GetUnlocked(StructureBrain.TYPES Types)
    {
        if (!CustomStructureList.ContainsKey(Types)) return;
        if (!DataManager.Instance.UnlockedStructures.Contains(Types))
            DataManager.Instance.UnlockedStructures.Add(Types);
        if (!DataManager.Instance.RevealedStructures.Contains(Types))
            DataManager.Instance.RevealedStructures.Add(Types);
    }

    [HarmonyPatch(typeof(TypeAndPlacementObjects), "GetByType")]
    [HarmonyPrefix]
    private static bool TypeAndPlacementObjects_GetByType(ref TypeAndPlacementObject __result,
        StructureBrain.TYPES Type)
    {
        if (!CustomStructureList.ContainsKey(Type)) return true;
        __result = CustomStructureList[Type].GetTypeAndPlacementObject();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "BuildDurationGameMinutes")]
    [HarmonyPrefix]
    private static bool StructuresData_BuildDurationGameMinutes(ref int __result, StructureBrain.TYPES Type)
    {
        if (!CustomStructureList.ContainsKey(Type)) return true;
        __result = CustomStructureList[Type].BuildDurationMinutes;
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "GetCost")]
    [HarmonyPrefix]
    private static bool StructuresData_GetCost(ref List<StructuresData.ItemCost> __result, StructureBrain.TYPES Type)
    {
        if (!CustomStructureList.ContainsKey(Type)) return true;
        __result = CustomStructureList[Type].Cost;
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "GetLocalizedNameStatic", typeof(StructureBrain.TYPES))]
    [HarmonyPrefix]
    public static bool StructuresData_GetLocalizedNameStatic(StructureBrain.TYPES Type, ref string __result)
    {
        if (!CustomStructureList.ContainsKey(Type)) return true;
        __result = CustomStructureList[Type].GetLocalizedNameStatic();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "LocalizedName", typeof(StructureBrain.TYPES))]
    [HarmonyPrefix]
    public static bool StructuresData_LocalizedName(StructureBrain.TYPES Type, ref string __result)
    {
        if (!CustomStructureList.ContainsKey(Type)) return true;
        __result = CustomStructureList[Type].LocalizedName();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "LocalizedDescription", typeof(StructureBrain.TYPES))]
    [HarmonyPrefix]
    public static bool StructuresData_LocalizedDescription(StructureBrain.TYPES Type, ref string __result)
    {
        if (!CustomStructureList.ContainsKey(Type)) return true;
        __result = CustomStructureList[Type].LocalizedDescription();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "LocalizedPros", typeof(StructureBrain.TYPES))]
    [HarmonyPrefix]
    public static bool StructuresData_LocalizedPros(StructureBrain.TYPES Type, ref string __result)
    {
        if (!CustomStructureList.ContainsKey(Type)) return true;
        __result = CustomStructureList[Type].LocalizedPros();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "LocalizedCons", typeof(StructureBrain.TYPES))]
    [HarmonyPrefix]
    public static bool StructuresData_LocalizedCons(StructureBrain.TYPES Type, ref string __result)
    {
        if (!CustomStructureList.ContainsKey(Type)) return true;
        __result = CustomStructureList[Type].LocalizedCons();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "GetLocalizedName", new Type[] { })]
    [HarmonyPrefix]
    public static bool StructuresData_GetLocalizedName(StructuresData __instance, ref string __result)
    {
        if (!CustomStructureList.ContainsKey(__instance.Type)) return true;
        __result = CustomStructureList[__instance.Type].GetLocalizedName();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "GetLocalizedName", typeof(bool), typeof(bool), typeof(bool))]
    [HarmonyPrefix]
    public static bool StructuresData_GetLocalizedName(StructuresData __instance, bool plural, bool withArticle,
        bool definite, ref string __result)
    {
        if (!CustomStructureList.ContainsKey(__instance.Type)) return true;
        __result = CustomStructureList[__instance.Type].GetLocalizedName(plural, withArticle, definite);
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "GetLocalizedDescription", new Type[] { })]
    [HarmonyPrefix]
    public static bool StructuresData_GetLocalizedDescription(StructuresData __instance, ref string __result)
    {
        if (!CustomStructureList.ContainsKey(__instance.Type)) return true;
        __result = CustomStructureList[__instance.Type].GetLocalizedDescription();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "GetLocalizedLore", new Type[] { })]
    [HarmonyPrefix]
    public static bool StructuresData_GetLocalizedLore(StructuresData __instance, ref string __result)
    {
        if (!CustomStructureList.ContainsKey(__instance.Type)) return true;
        __result = CustomStructureList[__instance.Type].GetLocalizedLore();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "GetResearchCost", typeof(StructureBrain.TYPES))]
    [HarmonyPrefix]
    public static bool StructuresData_GetResearchCost(StructureBrain.TYPES Type, ref int __result)
    {
        if (!CustomStructureList.ContainsKey(Type)) return true;
        __result = CustomStructureList[Type].GetResearchCost();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "RequiresTempleToBuild", typeof(StructureBrain.TYPES))]
    [HarmonyPrefix]
    public static bool StructuresData_RequiresTempleToBuild(StructureBrain.TYPES type, ref bool __result)
    {
        if (!CustomStructureList.ContainsKey(type)) return true;
        __result = CustomStructureList[type].RequiresTempleToBuild();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "GetBuildOnlyOne", typeof(StructureBrain.TYPES))]
    [HarmonyPrefix]
    public static bool StructuresData_GetBuildOnlyOne(StructureBrain.TYPES Type, ref bool __result)
    {
        if (!CustomStructureList.ContainsKey(Type)) return true;
        __result = CustomStructureList[Type].GetBuildOnlyOne();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "GetBuildSfx", typeof(StructureBrain.TYPES))]
    [HarmonyPrefix]
    public static bool StructuresData_GetBuildSfx(StructureBrain.TYPES Type, ref string __result)
    {
        if (!CustomStructureList.ContainsKey(Type)) return true;
        __result = CustomStructureList[Type].GetBuildSfx();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "HiddenUntilUnlocked", typeof(StructureBrain.TYPES))]
    [HarmonyPrefix]
    public static bool StructuresData_HiddenUntilUnlocked(StructureBrain.TYPES structure, ref bool __result)
    {
        if (!CustomStructureList.ContainsKey(structure)) return true;
        __result = CustomStructureList[structure].HiddenUntilUnlocked();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "CanBeFlipped", typeof(StructureBrain.TYPES))]
    [HarmonyPrefix]
    public static bool StructuresData_CanBeFlipped(StructureBrain.TYPES type, ref bool __result)
    {
        if (!CustomStructureList.ContainsKey(type)) return true;
        __result = CustomStructureList[type].CanBeFlipped();
        return false;
    }

    [HarmonyPatch(typeof(Structure), nameof(Structure.Start))]
    [HarmonyPostfix]
    public static void Structure_Start(Structure __instance)
    {
        if (!CustomStructureList.ContainsKey(__instance.Type)) return;
        if (CustomStructureList[__instance.Type].Interaction == null) return;

        LogHelper.LogDebug("adding structure interaction " +
                           CustomStructureList[__instance.Type].Interaction);
        var parent = __instance.GetComponentInParent<Transform>();
        parent.gameObject.AddComponent(CustomStructureList[__instance.Type].Interaction);
    }
}