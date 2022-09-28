using HarmonyLib;

namespace COTL_API.Saves;

[HarmonyPatch]
public static partial class ModdedSaveManager
{
    private static int SaveSlot = 5;
    private static readonly COTLDataReadWriter<ModdedSaveData> _readWriter = new();

    public static bool Loaded;
    public static ModdedSaveData Data;
    
    internal static System.Action OnSaveComplete;
    internal static System.Action OnLoadComplete;

    static ModdedSaveManager()
    {
        _readWriter.OnReadCompleted += delegate(ModdedSaveData data)
        {
            Data = data;
            Loaded = true;
            OnLoadComplete?.Invoke();
        };

        _readWriter.OnCreateDefault += delegate
        {
            Data = new ModdedSaveData();
            Loaded = true;
            
            OnLoadComplete?.Invoke();
            OnSaveComplete?.Invoke();    
        };
        
        _readWriter.OnWriteCompleted += delegate
        {
            OnSaveComplete?.Invoke();
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