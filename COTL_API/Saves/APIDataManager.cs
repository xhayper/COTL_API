using HarmonyLib;

namespace COTL_API.Saves;

[HarmonyPatch]
internal static class APIDataManager
{
    internal const string DataPath = "cotl_api_data.json";

    internal static readonly COTLDataReadWriter<APIData> DataReadWriter = new();

    internal static APIData APIData;

    internal static System.Action OnSaveComplete;
    internal static System.Action OnLoadComplete;

    static APIDataManager()
    {
        DataReadWriter.OnReadCompleted += delegate (APIData data)
        {
            APIData = data;

            OnLoadComplete?.Invoke();
        };

        DataReadWriter.OnCreateDefault += delegate
        {
            APIData = new APIData();

            OnLoadComplete?.Invoke();
            OnSaveComplete?.Invoke();
        };

        DataReadWriter.OnWriteCompleted += delegate
        {
            OnSaveComplete?.Invoke();
        };

        Load();
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save))]
    [HarmonyPostfix]
    internal static void Save()
    {
        DataReadWriter.Write(APIData, DataPath);
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save))]
    [HarmonyPostfix]
    internal static void Load()
    {
        DataReadWriter.Read(DataPath);
    }
}