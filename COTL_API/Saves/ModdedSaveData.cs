namespace COTL_API.Saves;

public class ModdedSaveData<T> : BaseModdedSaveData where T : class, new()
{
    private readonly COTLDataReadWriter<T> _dataReadWriter = new();

    public ModdedSaveData(string guid)
    {
        GUID = guid;

        _dataReadWriter.OnReadCompleted += delegate(T saveData)
        {
            Data = saveData;
            IsLoaded = true;
            OnLoadComplete?.Invoke();
        };

        _dataReadWriter.OnCreateDefault += delegate
        {
            CreateDefault();
            OnLoadComplete?.Invoke();
            IsLoaded = true;
            OnSaveCompleted?.Invoke();
        };

        _dataReadWriter.OnWriteCompleted += delegate { OnSaveCompleted?.Invoke(); };
        _dataReadWriter.OnWriteError += delegate(MMReadWriteError error) { OnSaveError?.Invoke(error); };
    }

    public override int SAVE_SLOT { get; protected set; } = 5;
    public override ModdedSaveLoadOrder LoadOrder { get; set; } = ModdedSaveLoadOrder.LOAD_AFTER_SAVE_START;

    public sealed override string GUID { get; protected set; }
    public override bool IsLoaded { get; protected set; }

    public T? Data { get; private set; }

    public override void CreateDefault()
    {
        Data = new T();
    }

    public override void Save(bool encrypt = true, bool backup = true)
    {
        if (LoadOrder != ModdedSaveLoadOrder.LOAD_AS_SOON_AS_POSSIBLE &&
            (!DataManager.Instance.AllowSaving || CheatConsole.IN_DEMO))
            return;

        if (Data != null)
            _dataReadWriter.Write(Data,
                MakeSaveSlot(LoadOrder == ModdedSaveLoadOrder.LOAD_AS_SOON_AS_POSSIBLE ? null : SAVE_SLOT), encrypt,
                backup);
    }

    public override void Saving()
    {
        if (Data == null)
            return;

        var deletePreviousSave = false;
        if (SAVE_SLOT >= 10 && !SaveExist(SAVE_SLOT - 10))
        {
            SAVE_SLOT -= 10;
            deletePreviousSave = true;
        }

        _dataReadWriter.Write(Data, SaveAndLoad.MakeSaveSlot(SaveAndLoad.SAVE_SLOT), !Plugin.Instance.DecryptSaveFile);
        _dataReadWriter.OnWriteCompleted += () =>
        {
            if (!deletePreviousSave)
                return;
            DeleteSaveSlot(SaveAndLoad.SAVE_SLOT + 10);
        };
    }

    public override void Load(int? saveSlot = null)
    {
        if (LoadOrder != ModdedSaveLoadOrder.LOAD_AS_SOON_AS_POSSIBLE && CheatConsole.IN_DEMO)
            return;

        if (saveSlot != null)
            SAVE_SLOT = saveSlot.Value;

        _dataReadWriter.Read(MakeSaveSlot(LoadOrder == ModdedSaveLoadOrder.LOAD_AS_SOON_AS_POSSIBLE
            ? null
            : SAVE_SLOT));
    }

    public override bool SaveExist(int? saveSlot = null)
    {
        return _dataReadWriter.FileExists(MakeSaveSlot(saveSlot));
    }

    public override void DeleteSaveSlot(int? saveSlot = null)
    {
        _dataReadWriter.Delete(MakeSaveSlot(saveSlot));
        if (saveSlot != null)
            OnSaveSlotDeleted?.Invoke(saveSlot.Value);
    }

    public override void ResetSave(int? saveSlot = null, bool newGame = false)
    {
        if (saveSlot != null)
            SAVE_SLOT = saveSlot.Value;
        CreateDefault();
        if (!newGame)
            Save();
        IsLoaded = true;
    }

    public override string MakeSaveSlot(int? slot = null)
    {
        return slot != null ? $"{GUID}_{slot}.json" : $"{GUID}.json";
    }
}