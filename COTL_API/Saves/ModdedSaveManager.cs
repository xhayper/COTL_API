using HarmonyLib;

namespace COTL_API.Saves;

[HarmonyPatch]
public static class ModdedSaveManager
{
    private static int SaveSlot = 5;
    private static readonly COTLDataReadWriter<ModdedSaveData> _readWriter = new();

    public static bool Loaded;
    public static ModdedSaveData Data;

    static ModdedSaveManager()
    {
        _readWriter.OnReadCompleted += delegate(ModdedSaveData data)
        {
            Data = data;
            Loaded = true;
        };

        _readWriter.OnCreateDefault += delegate
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

        _readWriter.Write(Data, MakeSaveSlot(SaveSlot));
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Load))]
    [HarmonyPostfix]
    public static void Load(int saveSlot)
    {
        if (CheatConsole.IN_DEMO) return;

        SaveSlot = saveSlot;
        _readWriter.Read(MakeSaveSlot(SaveSlot));
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.DeleteSaveSlot))]
    [HarmonyPostfix]
    public static void DeleteSaveSlot(int saveSlot)
    {
        _readWriter.Delete(MakeSaveSlot(saveSlot));
    }

    public static bool SaveExist(int saveSlot)
    {
        return _readWriter.FileExists(MakeSaveSlot(saveSlot));
    }

    public static string MakeSaveSlot(int slot)
    {
        return $"modded_slot_{slot}.json";
    }
}