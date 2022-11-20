using HarmonyLib;

namespace COTL_API.Saves;

[HarmonyPatch]
internal static class SettingsDataManager
{
    internal const string DataPath = "cotl_api_settings.json";

    internal static readonly COTLDataReadWriter<SettingsData> DataReadWriter = new();

    internal static SettingsData _settingsData;

    public static SettingsData SettingsData {
        get {
            if (_settingsData == null)
            {
                Load();
            }

            return _settingsData;
        }
    }

    static SettingsDataManager()
    {
        DataReadWriter.OnReadCompleted += delegate (SettingsData data)
        {
            _settingsData = data;
        };

        DataReadWriter.OnCreateDefault += delegate
        {
            _settingsData = new SettingsData();
        };

        Load();
    }

    internal static void Save()
    {
        DataReadWriter.Write(SettingsData, DataPath);
    }

    internal static void Load()
    {
        DataReadWriter.Read(DataPath);
    }
}