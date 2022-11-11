using HarmonyLib;

namespace COTL_API.Saves;

[HarmonyPatch]
public static partial class ModdedSaveManager
{
    private static int SaveSlot = 5;
    private static readonly COTLDataReadWriter<ModdedSaveData> _readWriter = new();

    public static bool Loaded { get; internal set; }
    public static ModdedSaveData Data { get; internal set; }

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

    public static bool SaveExists(int saveSlot)
    {
        return _readWriter.FileExists(MakeSaveSlot(saveSlot));
    }

    public static string MakeSaveSlot(int slot)
    {
        return $"modded_slot_{slot}.json";
    }
}