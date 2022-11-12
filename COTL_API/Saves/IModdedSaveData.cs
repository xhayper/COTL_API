namespace COTL_API.Saves;

public abstract class IModdedSaveData
{
    public abstract int SAVE_SLOT { get; protected set; }
    public abstract bool IsLoaded { get; protected set; }
    public abstract string GUID { get; protected set; }
    public abstract bool LoadOnStart { get; set; }

    public System.Action OnSaveCompleted;
    public System.Action<MMReadWriteError> OnSaveError;
    public System.Action OnLoadComplete;
    public System.Action<int> OnSaveSlotDeleted;

    public abstract void CreateDefault();
    public abstract void Save(bool encrypt = true, bool backup = true);
    public abstract void Load(int? saveSlot = null);
    public abstract bool SaveExist(int? saveSlot = null);
    public abstract void DeleteSaveSlot(int? saveSlot = null);
    public abstract void ResetSave(int? saveSlot = null, bool newGame = false);
    public abstract string MakeSaveSlot(int? slot = null);
}