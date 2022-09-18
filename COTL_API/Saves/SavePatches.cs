using HarmonyLib;

namespace COTL_API.Saves;
public static partial class ModdedSaveManager
{
    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.ResetSave))]
    [HarmonyPostfix]
    private static void ResetSave(int saveSlot, bool newGame)
    {
        SaveSlot = saveSlot;
        Data = new ModdedSaveData();
        if (!newGame) Save();
        Loaded = true;
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save))]
    [HarmonyPostfix]
    private static void Save()
    {
        if (!DataManager.Instance.AllowSaving || CheatConsole.IN_DEMO) return;

        _readWriter.Write(Data, MakeSaveSlot(SaveSlot));
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Load))]
    [HarmonyPostfix]
    private static void Load(int saveSlot)
    {
        if (CheatConsole.IN_DEMO) return;

        SaveSlot = saveSlot;
        _readWriter.Read(MakeSaveSlot(SaveSlot));
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.DeleteSaveSlot))]
    [HarmonyPostfix]
    private static void DeleteSaveSlot(int saveSlot)
    {
        _readWriter.Delete(MakeSaveSlot(saveSlot));
    }
}