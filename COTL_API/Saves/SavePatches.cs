using System.Linq;
using HarmonyLib;

namespace COTL_API.Saves;

public static partial class ModdedSaveManager
{
    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Load))]
    [HarmonyPostfix]
    private static void SaveAndLoad_Load(int saveSlot)
    {
        foreach (var saveData in _moddedSaveData.Values.Where(save => !save.LoadOnStart))
        {
            saveData.Load(saveSlot);
        }
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save))]
    [HarmonyPostfix]
    private static void SaveAndLoad_Save()
    {
        foreach (var saveData in _moddedSaveData.Values)
        {
            saveData.Save();
        }
    }

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

    [HarmonyPatch(typeof(PlayerFarming), nameof(PlayerFarming.Awake))]
    [HarmonyPostfix]
    private static void PlayerFarming_Awake()
    {
        Plugin.Instance.Logger.LogWarning($"Loading Modded Save Data with LoadAfterMainSave=true.");
        foreach (var saveData in _moddedSaveData.Values.Where(save => save.LoadAfterMainSave))
        {
            saveData.Load(SaveAndLoad.SAVE_SLOT);
        }
    }
}