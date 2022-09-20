using HarmonyLib;
using MMTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace COTL_API.CustomObjectives;

[HarmonyPatch]
public class CustomObjectivePatches
{
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.GetConversationEntry), typeof(Follower.ComplaintType), typeof(ObjectivesData))]
    [HarmonyPostfix]
    private static void GetConversationEntry(Follower.ComplaintType ComplaintForBark, ObjectivesData objective, ref List<ConversationEntry> __result)
    {
        CustomObjective customObjective = CustomObjectiveManager.CustomObjectives.Values.FirstOrDefault(x => x.ObjectiveData.ID == objective.ID);
        if (customObjective == null)
        {
            Plugin.Logger.LogWarning($"No matching quest found for {objective.ID}!");
            return;
        }

        Plugin.Logger.LogWarning($"Matching quest found for {objective.ID}! ListCount: {__result.Count}");
        __result[0].TermToSpeak = customObjective.InitialQuestText();
    }
}