using HarmonyLib;

namespace COTL_API.Saves;

[HarmonyPatch]
public static class ModdedSaveManager
{
    public static int SaveSlot = 5;

    public static bool Loaded;

    internal static readonly COTLDataReadWriter<ModdedSaveData> SaveDataReadWriter = new();

    public static ModdedSaveData Data;

    static ModdedSaveManager()
    {
        SaveDataReadWriter.OnReadCompleted += delegate(ModdedSaveData data)
        {
            Data = data;
            Loaded = true;
        };

        SaveDataReadWriter.OnCreateDefault += delegate
        {
            Data = new ModdedSaveData();
            Loaded = true;
        };
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.ResetSave))]
    [HarmonyPostfix]
    public static void ResetSave(int saveSlot, bool newGame)
    {
        SaveSlot = saveSlot;
        Data = new ModdedSaveData();
        if (!newGame) Save();
        Loaded = true;
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save))]
    [HarmonyPostfix]
    public static void Save()
    {
        if (!DataManager.Instance.AllowSaving || CheatConsole.IN_DEMO) return;

        SaveDataReadWriter.Write(Data, MakeSaveSlot(SaveSlot));
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Load))]
    [HarmonyPostfix]
    public static void Load(int saveSlot)
    {
        if (CheatConsole.IN_DEMO) return;

        SaveSlot = saveSlot;
        SaveDataReadWriter.Read(MakeSaveSlot(SaveSlot));
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.DeleteSaveSlot))]
    [HarmonyPostfix]
    public static void DeleteSaveSlot(int saveSlot)
    {
        SaveDataReadWriter.Delete(MakeSaveSlot(saveSlot));
    }

    public static bool SaveExist(int saveSlot)
    {
        return SaveDataReadWriter.FileExists(MakeSaveSlot(saveSlot));
    }

    public static string MakeSaveSlot(int slot)
    {
        return $"modded_slot_{slot}.json";
    }
}