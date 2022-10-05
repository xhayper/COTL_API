using System.Collections.Generic;
using HarmonyLib;
using MMTools;

namespace COTL_API.CustomObjectives;

/// <summary>
/// Patch to ensure the correct quest text is loaded as there is no traditional GetLocalised method for the quest text.
/// </summary>
[HarmonyPatch]
public class CustomObjectivePatches
{
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.GetConversationEntry), typeof(Follower.ComplaintType), typeof(ObjectivesData))]
    [HarmonyPostfix]
    private static void GetConversationEntry(ObjectivesData objective, ref List<ConversationEntry> __result)
    {
        if (CustomObjectiveManager.PluginQuestTracker.TryGetValue(objective.ID, out CustomObjective customObjective))
        {
            Plugin.Logger.LogWarning($"Matching quest found for {objective.ID}!");
            __result[0].TermToSpeak = customObjective.InitialQuestText;
        }
        else
        {
            Plugin.Logger.LogWarning($"No matching quest found for {objective.ID}!");
        }
    }
}