namespace COTL_API.Saves;

public enum ModdedSaveLoadOrder
{
    LOAD_AS_SOON_AS_POSSIBLE,
    LOAD_AFTER_SAVE_START
}

public abstract class BaseModdedSaveData
{
    public abstract int SAVE_SLOT { get; protected set; }
    public abstract bool IsLoaded { get; protected set; }
    public abstract string GUID { get; protected set; }

    // TODO: Make this an enum
    /**
     * If the save data should load when the game is open or when the save slot is loaded.
     * true if the save data should load when the game is open.
     * false if the save data should load when the save slot is loaded.
     */
    public abstract ModdedSaveLoadOrder LoadOrder { get; set; }

    public System.Action OnSaveCompleted { get; set; }
    public System.Action<MMReadWriteError> OnSaveError { get; set; }
    public System.Action OnLoadComplete { get; set; }
    public System.Action<int> OnSaveSlotDeleted { get; set; }

    public abstract void CreateDefault();
    public abstract void Save(bool encrypt = true, bool backup = true);
    public abstract void Load(int? saveSlot = null);
    public abstract bool SaveExist(int? saveSlot = null);
    public abstract void DeleteSaveSlot(int? saveSlot = null);
    public abstract void ResetSave(int? saveSlot = null, bool newGame = false);
    public abstract string MakeSaveSlot(int? slot = null);
}