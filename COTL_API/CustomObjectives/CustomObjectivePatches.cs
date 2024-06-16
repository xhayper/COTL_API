using HarmonyLib;
using MMTools;

namespace COTL_API.CustomObjectives;

/// <summary>
///     Patch to ensure the correct quest text is loaded as there is no traditional GetLocalised method for the quest text.
/// </summary>
[HarmonyPatch]
public static partial class CustomObjectiveManager
{
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.GetConversationEntry),
        typeof(Follower.ComplaintType), typeof(ObjectivesData), typeof(FollowerTask_GetAttention))]
    [HarmonyPostfix]
    private static void interaction_FollowerInteraction_GetConversationEntry(ObjectivesData objective,
        ref List<ConversationEntry> __result)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (objective == null) return;

        if (CustomObjectiveList.TryGetValue(objective.ID, out var customObjective))
        {
            LogWarning($"Matching quest found for {objective.ID}!");
            __result[0].TermToSpeak = customObjective.InitialQuestText;
        }
        else
        {
            LogWarning($"No matching quest found for {objective.ID}!");
        }
    }

    /// <summary>
    ///     Fixes the quest indexes in CompletedQuestsHistory as they're used to determine if a quest has been done recently.
    ///     The index can become invalid if the user
    ///     removes any mods that adds new quests.
    /// </summary>
    [HarmonyPatch(typeof(Quests), nameof(Quests.GetQuest))]
    [HarmonyPrefix]
    private static void Quests_GetQuest()
    {
        foreach (var quest in DataManager.Instance.CompletedQuestsHistorys.Where(a =>
                     a.QuestIndex >= Quests.QuestsAll.Count))
        {
            LogWarning(
                "Found quests in history with an index higher than total quests (user may have removed mods that add quests), resetting to maximum possible.");
            quest.QuestIndex = Quests.QuestsAll.Count - 1;
        }
    }
}