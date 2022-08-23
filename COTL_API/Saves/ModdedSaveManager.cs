using HarmonyLib;
using System;

namespace COTL_API.Saves;

[HarmonyPatch]
public static class ModdedSaveManager
{
    public static int SAVE_SLOT = 5;

    internal static COTLDataReadWriter<ModdedSaveData> _saveDataReadWriter = new();

    public static ModdedSaveData SaveData;

    static ModdedSaveManager()
    {
        COTLDataReadWriter<ModdedSaveData> saveFileReadWriter = _saveDataReadWriter;
        saveFileReadWriter.OnReadCompleted = (Action<ModdedSaveData>)Delegate.Combine(saveFileReadWriter.OnReadCompleted, delegate (ModdedSaveData saveData)
        {
            SaveData = saveData;
        });

        COTLDataReadWriter<ModdedSaveData> saveFileReadWriter2 = _saveDataReadWriter;
        saveFileReadWriter2.OnCreateDefault = (Action)Delegate.Combine(saveFileReadWriter2.OnCreateDefault, (Action)delegate
        {
            SaveData = new();
        });
    }

    [HarmonyPatch(typeof(SaveAndLoad), "ResetSave")]
    [HarmonyPostfix]
    public static void ResetSave(int saveSlot, bool newGame)
    {
        SAVE_SLOT = saveSlot;
        SaveData = new();
        if (!newGame) Save();
    }

    [HarmonyPatch(typeof(SaveAndLoad), "Save")]
    [HarmonyPostfix]
    public static void Save()
    {
        if (!DataManager.Instance.AllowSaving || CheatConsole.IN_DEMO) return;

        _saveDataReadWriter.Write(SaveData, MakeSaveSlot(SAVE_SLOT));
    }

    [HarmonyPatch(typeof(SaveAndLoad), "Load")]
    [HarmonyPostfix]
    public static void Load(int saveSlot)
    {
        if (CheatConsole.IN_DEMO) return;

        SAVE_SLOT = saveSlot;
        _saveDataReadWriter.Read(MakeSaveSlot(SAVE_SLOT));
    }

    [HarmonyPatch(typeof(SaveAndLoad), "DeleteSaveSlot")]
    [HarmonyPostfix]
    public static void DeleteSaveSlot(int saveSlot)
    {
        _saveDataReadWriter.Delete(MakeSaveSlot(saveSlot));
    }

    public static string MakeSaveSlot(int slot)
    {
        return $"modded_slot_{slot}.json";
    }
}