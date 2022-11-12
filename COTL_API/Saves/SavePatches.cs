using System.Linq;
using HarmonyLib;

namespace COTL_API.Saves;

public static partial class ModdedSaveManager
{
    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.ResetSave))]
    [HarmonyPostfix]
    private static void SaveAndLoad_ResetSave(int saveSlot, bool newGame)
    {
        foreach (var saveData in _moddedSaveData.Values.Where(save => !save.LoadOnStart))
        {
            saveData.ResetSave(saveSlot, newGame);
        }
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.DeleteSaveSlot))]
    [HarmonyPostfix]
    private static void SaveAndLoad_DeleteSaveSlot(int saveSlot)
    {
        foreach (var saveData in _moddedSaveData.Values.Where(save => !save.LoadOnStart))
        {
            saveData.DeleteSaveSlot(saveSlot);
        }
    }

    [HarmonyPatch(typeof(SaveAndLoad), MethodType.Constructor)]
    [HarmonyPostfix]
    private static void SaveAndLoad_Constructor(ref SaveAndLoad __instance)
    {
        __instance._saveFileReadWriter.OnReadCompleted += delegate
        {
            var saveSlot = SaveAndLoad.SAVE_SLOT;
            foreach (var saveData in _moddedSaveData.Values.Where(save => !save.LoadOnStart))
            {
                saveData.Load(saveSlot);
            }
        };

        __instance._saveFileReadWriter.OnWriteCompleted += delegate
        {
            foreach (var saveData in _moddedSaveData.Values)
            {
                saveData.Save();
            }
        };
    }
}