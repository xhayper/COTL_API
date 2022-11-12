namespace COTL_API.Saves;

public class BaseModdedSaveData<T> : BaseModdedSaveData where T : class, new()
{
    public override int SAVE_SLOT { get; protected set; } = 5;
    public override bool LoadOnStart { get; set; }
    public override bool IsLoaded { get; protected set; }
    public sealed override string GUID { get; protected set; }

    public T Data { get; private set; }

    private readonly COTLDataReadWriter<T> _dataReadWriter = new();

    public BaseModdedSaveData(string guid)
    {
        GUID = guid;

        _dataReadWriter.OnReadCompleted += delegate(T saveData)
        {
            Data = saveData;
            IsLoaded = true;
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

    public override void CreateDefault()
    {
        Data = new T();
    }

    public override void Save(bool encrypt = true, bool backup = true)
    {
        if (!LoadOnStart && (!DataManager.Instance.AllowSaving || CheatConsole.IN_DEMO))
            return;

        _dataReadWriter.Write(Data, MakeSaveSlot(LoadOnStart ? null : SAVE_SLOT), encrypt, backup);
    }

    public override void Load(int? saveSlot = null)
    {
        if (!LoadOnStart && CheatConsole.IN_DEMO)
            return;

        if (saveSlot != null)
            SAVE_SLOT = saveSlot.Value;

        _dataReadWriter.Read(MakeSaveSlot(LoadOnStart ? null : SAVE_SLOT));
    }

    public override bool SaveExist(int? saveSlot = null) => _dataReadWriter.FileExists(MakeSaveSlot(saveSlot));

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

    public override string MakeSaveSlot(int? slot = null) =>
        slot != null ? $"ModdedSave_{GUID}_{slot}" : $"ModdedSave_{GUID}";
}