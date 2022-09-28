using COTL_API.CustomObjectives;
using HarmonyLib;
using MonoMod.Utils;
using System.Collections.Generic;

namespace COTL_API.Saves;

[HarmonyPatch]
internal static class APIDataManager
{
    internal const string DataPath = "cotl_api_data.json";

    internal static readonly COTLDataReadWriter<APIData> DataReadWriter = new();

    internal static APIData APIData;

    static APIDataManager()
    {
        DataReadWriter.OnReadCompleted += delegate(APIData data)
        {
            APIData = data;
            Dictionary<int, CustomObjective> savedObjectives = data.GetValue<Dictionary<int, CustomObjective>>("QuestID");
            if (savedObjectives != null) CustomObjectiveManager.PluginQuestTracker.AddRange(savedObjectives);
        };

        DataReadWriter.OnCreateDefault += delegate
        {
            APIData = new APIData();
        };

        Load();
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save))]
    [HarmonyPostfix]
    internal static void Save()
    {
        if (CustomObjectiveManager.PluginQuestTracker.Count > 0) APIData.SetValue("QuestID", CustomObjectiveManager.PluginQuestTracker);
        DataReadWriter.Write(APIData, DataPath);
    }

    internal static void Load()
    {
        DataReadWriter.Read(DataPath);
    }
}