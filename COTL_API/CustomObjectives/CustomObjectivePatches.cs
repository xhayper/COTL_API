using HarmonyLib;
using MMTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace COTL_API.CustomObjectives;

/// <summary>
/// This class houses patches required for the Custom Objectives to function.
/// </summary>
[HarmonyPatch]
public class CustomObjectivePatches
{
    /// <summary>
    /// This overrides the text the follower "barks" at you when it's asking you to do something.
    /// </summary>
    /// <param name="ComplaintForBark">The type of complaint. We're only interested in the GiveQuest complaint.</param>
    /// <param name="objective">The objective of the complaint.</param>
    /// <param name="__result">The list of conversation entries. This is only ever 1 with standard quests.</param>
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.GetConversationEntry), typeof(Follower.ComplaintType), typeof(ObjectivesData))]
    [HarmonyPostfix]
    private static void GetConversationEntry(Follower.ComplaintType ComplaintForBark, ObjectivesData objective, ref List<ConversationEntry> __result)
    {
        CustomObjective customObjective = CustomObjectiveManager.CustomObjectives.Values.FirstOrDefault(x => x != null && x.ObjectiveData.ID == objective.ID);
        if (customObjective == null)
        {
            if(Plugin.Debug) Plugin.Logger.LogDebug($"No matching quest found for {objective.ID}!");
            return;
        }

        if(Plugin.Debug) Plugin.Logger.LogDebug($"Matching quest found for {objective.ID}! ListCount: {__result.Count}");
        __result[0].TermToSpeak = customObjective.InitialQuestText;
    }
}