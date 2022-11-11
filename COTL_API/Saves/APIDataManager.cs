using HarmonyLib;

namespace COTL_API.Saves;

[HarmonyPatch]
internal static class APIDataManager
{
    internal const string DataPath = "cotl_api_data.json";

    internal static readonly COTLDataReadWriter<APIData> DataReadWriter = new();

    internal static APIData APIData { get; private set; }

    static APIDataManager()
    {
        DataReadWriter.OnReadCompleted += delegate(APIData data) { APIData = data; };
        DataReadWriter.OnCreateDefault += delegate { APIData = new APIData(); };

        Load();
    }

    internal static void Save()
    {
        DataReadWriter.Write(APIData, DataPath);
    }

    internal static void Load()
    {
        DataReadWriter.Read(DataPath);
    }
}