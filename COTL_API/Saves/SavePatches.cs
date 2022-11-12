using HarmonyLib;
using Socket.Newtonsoft.Json.Utilities.LinqBridge;

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

    //this is no longer needed (as we've attached to the games own OnWriteComplete action further down, but I'm leaving it here for reference
    // [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save))]
    // [HarmonyPostfix] //these should run AFTER the game has saved it's own data
    // private static void SaveAndLoad_Save()
    // {
    //     foreach (var saveData in _moddedSaveData.Values)
    //     {
    //         saveData.Save();
    //     }
    // }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Load))]
    [HarmonyPostfix] //these should run AFTER the game has loaded it's own data otherwise there will be no data for mods that rely on it
    private static void SaveAndLoad_Load(int saveSlot)
    {
        foreach (var saveData in _moddedSaveData.Values.Where(save => save.LoadOnStart))
        {
            saveData.Load(saveSlot);
        }
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.DeleteSaveSlot))]
    [HarmonyPrefix]
    private static void SaveAndLoad_DeleteSaveSlot(int saveSlot)
    {
        foreach (var saveData in _moddedSaveData.Values)
        {
            saveData.DeleteSaveSlot(saveSlot);
        }
    }

    [HarmonyPatch(typeof(SaveAndLoad), MethodType.Constructor)]
    [HarmonyPostfix]
    //these should run AFTER the game has loaded it's own data otherwise there will be no data for mods that rely on it.
    //Attaching to the games saveFileReader Actions ensures mod save data is loaded after the game has loaded it's own save data
    private static void SaveAndLoad_Constructor(ref SaveAndLoad __instance)
    {
        foreach (var saveData in _moddedSaveData.Values)
        {
            if (!saveData.LoadOnStart)
            {
                __instance._saveFileReadWriter.OnReadCompleted += delegate
                {
                    Plugin.Instance.Logger.LogWarning($"Loading modded save data in slot {SaveAndLoad.SAVE_SLOT} for {saveData.GUID}");
                    saveData.Load(SaveAndLoad.SAVE_SLOT);
                };
            }

            __instance._saveFileReadWriter.OnWriteCompleted += delegate
            {
                Plugin.Instance.Logger.LogWarning($"Saving modded save data in slot {SaveAndLoad.SAVE_SLOT} for {saveData.GUID}");
                saveData.Save();
            };
        }
    }
}