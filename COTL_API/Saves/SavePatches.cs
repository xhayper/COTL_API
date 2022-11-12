using System.Linq;
using HarmonyLib;

namespace COTL_API.Saves;

public static partial class ModdedSaveManager
{
    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Load))]
    [HarmonyPostfix]
    private static void SaveAndLoad_Load(int saveSlot)
    {
        foreach (var saveData in ModdedSaveData.Values.Where(save => !save.LoadOnStart))
        {
            saveData.Load(saveSlot);
        }
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save))]
    [HarmonyPostfix]
    private static void SaveAndLoad_Save()
    {
        foreach (var saveData in ModdedSaveData.Values)
        {
            saveData.Save();
        }
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.ResetSave))]
    [HarmonyPostfix]
    private static void SaveAndLoad_ResetSave(int saveSlot, bool newGame)
    {
        foreach (var saveData in ModdedSaveData.Values.Where(save => !save.LoadOnStart))
        {
            saveData.ResetSave(saveSlot, newGame);
        }
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.DeleteSaveSlot))]
    [HarmonyPostfix]
    private static void SaveAndLoad_DeleteSaveSlot(int saveSlot)
    {
        foreach (var saveData in ModdedSaveData.Values.Where(save => !save.LoadOnStart))
        {
            saveData.DeleteSaveSlot(saveSlot);
        }
    }
}