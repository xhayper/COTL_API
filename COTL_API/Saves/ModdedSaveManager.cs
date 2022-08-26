using HarmonyLib;
using System;

namespace COTL_API.Saves;

[HarmonyPatch]
public static class ModdedSaveManager
{
    public static int SAVE_SLOT = 5;

    public static bool Loaded = false;

    internal static COTLDataReadWriter<ModdedSaveData> _saveDataReadWriter = new();

    public static ModdedSaveData Data;

    static ModdedSaveManager()
    {
        COTLDataReadWriter<ModdedSaveData> saveFileReadWriter = _saveDataReadWriter;
        saveFileReadWriter.OnReadCompleted += delegate (ModdedSaveData data)
        {
            Data = data;
            Loaded = true;
        };

        COTLDataReadWriter<ModdedSaveData> saveFileReadWriter2 = _saveDataReadWriter;
        saveFileReadWriter2.OnCreateDefault += delegate
        {
            Data = new();
            Loaded = true;
        };
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.ResetSave))]
    [HarmonyPostfix]
    public static void ResetSave(int saveSlot, bool newGame)
    {
        SAVE_SLOT = saveSlot;
        Data = new();
        if (!newGame) Save();
        Loaded = true;
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save))]
    [HarmonyPostfix]
    public static void Save()
    {
        if (!DataManager.Instance.AllowSaving || CheatConsole.IN_DEMO) return;

        _saveDataReadWriter.Write(Data, MakeSaveSlot(SAVE_SLOT));
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Load))]
    [HarmonyPostfix]
    public static void Load(int saveSlot)
    {
        if (CheatConsole.IN_DEMO) return;

        SAVE_SLOT = saveSlot;
        _saveDataReadWriter.Read(MakeSaveSlot(SAVE_SLOT));
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.DeleteSaveSlot))]
    [HarmonyPostfix]
    public static void DeleteSaveSlot(int saveSlot)
    {
        _saveDataReadWriter.Delete(MakeSaveSlot(saveSlot));
    }

    public static bool SaveExist(int saveSlot)
    {
        return _saveDataReadWriter.FileExists(MakeSaveSlot(saveSlot));
    }

    public static string MakeSaveSlot(int slot)
    {
        return $"modded_slot_{slot}.json";
    }
}