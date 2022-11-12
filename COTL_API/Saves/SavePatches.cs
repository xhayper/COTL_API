using HarmonyLib;

namespace COTL_API.Saves;

public static partial class ModdedSaveManager
{
    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.ResetSave))]
    [HarmonyPrefix]
    private static void SaveAndLoad_ResetSave(int saveSlot, bool newGame)
    {
        foreach (var saveData in _moddedSaveData.Values)
        {
            saveData.ResetSave();
        }
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save))]
    [HarmonyPrefix]
    private static void SaveAndLoad_Save()
    {
        foreach (var saveData in _moddedSaveData.Values)
        {
            saveData.Save();
        }
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Load))]
    [HarmonyPrefix]
    private static void SaveAndLoad_Load(int saveSlot)
    {
        foreach (var saveData in _moddedSaveData.Values)
        {
            saveData.Load(saveData.LoadOnStart ? null : saveSlot);
        }
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.DeleteSaveSlot))]
    [HarmonyPrefix]
    private static void SaveAndLoad_DeleteSaveSlot(int saveSlot)
    {
        foreach (var saveData in _moddedSaveData.Values)
        {
            saveData.DeleteSaveSlot(saveData.LoadOnStart ? null : saveSlot);
        }
    }
}