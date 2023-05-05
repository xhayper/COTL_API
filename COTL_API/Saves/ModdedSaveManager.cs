using HarmonyLib;

namespace COTL_API.Saves;

[HarmonyPatch]
public static partial class ModdedSaveManager
{
    internal static readonly Dictionary<string, BaseModdedSaveData> ModdedSaveDataList = new();

    public static void RegisterModdedSave(BaseModdedSaveData saveData)
    {
        ModdedSaveDataList.Add(saveData.GUID, saveData);
        if (saveData.LoadOrder == ModdedSaveLoadOrder.LOAD_AS_SOON_AS_POSSIBLE) saveData.Load();
    }
}