using System.Collections.Generic;
using COTL_API.CustomObjectives;
using MonoMod.Utils;

namespace COTL_API.Saves;

public static class CustomQuestData
{
    private const string DataPath = "cotl_api_custom_quest_data.json";
    private static readonly COTLDataReadWriter<Dictionary<int, CustomObjective>> CustomQuestDataReadWriter = new();

    internal static void LoadData()
    {
        CustomQuestDataReadWriter.OnReadCompleted += delegate(Dictionary<int, CustomObjective> objectives)
        {
            Dictionary<int, CustomObjective> tempObjectives = new();
            tempObjectives.Clear(); //needed if the user goes back to the mainmenu and loads a new save
            
            foreach (KeyValuePair<int, CustomObjective> objective in objectives)
                if (DataManager.instance.Objectives.Exists(a => a.ID == objective.Key))
                    tempObjectives.Add(objective.Key, objective.Value);
                else if (Quests.QuestsAll.Exists(a => a.ID == objective.Key))
                    tempObjectives.Add(objective.Key, objective.Value);

            CustomObjectiveManager.PluginQuestTracker.AddRange(tempObjectives);
            Plugin.Logger.LogWarning(tempObjectives.Count > 0
                ? $"Needed previous session custom quests loaded. Count: {tempObjectives.Count}"
                : "None of the previous session quests still exist in objective trackers.");
        };

        CustomQuestDataReadWriter.OnReadError += delegate
        {
            Plugin.Logger.LogWarning("Previous session custom quests failed to load!");
        };

        CustomQuestDataReadWriter.Read(DataPath);
    }


    internal static void SaveData()
    {
        CustomQuestDataReadWriter.OnWriteCompleted += delegate
        {
            Plugin.Logger.LogWarning($"Backed up {CustomObjectiveManager.PluginQuestTracker.Count} custom QuestID's!");
        };

        CustomQuestDataReadWriter.OnWriteError += delegate(MMReadWriteError error)
        {
            Plugin.Logger.LogWarning($"There was an issue backing up current QuestID's!: {error.Message}");
        };

        CustomQuestDataReadWriter.Write(CustomObjectiveManager.PluginQuestTracker, DataPath, true, false);
    }
}