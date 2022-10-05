using HarmonyLib;
using MMTools;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace COTL_API.CustomObjectives;

/// <summary>
/// Patch to ensure the correct quest text is loaded as there is no traditional GetLocalised method for the quest text.
/// </summary>
[HarmonyPatch]
public static class CustomObjectivePatches
{
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.GetConversationEntry), typeof(Follower.ComplaintType), typeof(ObjectivesData))]
    [HarmonyPostfix]
    private static void interaction_FollowerInteraction_GetConversationEntry(ObjectivesData objective, ref List<ConversationEntry> __result)
    {
        if (objective == null)
        {
            return;
        }

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

    /// <summary>
    /// Fixes the quest indexes in CompletedQuestsHistory as they're used to determine if a quest has been done recently. The index can become invalid if the user
    /// removes any mods that adds new quests.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Quests), nameof(Quests.GetQuest))]
    public static void Quests_GetQuest()
    {
        foreach (DataManager.QuestHistoryData quest in DataManager.Instance.CompletedQuestsHistorys.Where(a => a.QuestIndex >= Quests.QuestsAll.Count))
        {
            Plugin.Logger.LogWarning("Found quests in history with an index higher than total quests (user may have removed mods that add quests), resetting to maximum possible.");
            quest.QuestIndex = Quests.QuestsAll.Count - 1;
        }
    }

    /// <summary>
    /// There is a hardcoded check during the main for loop in Quests.GetQuest. The number used becomes invalid when we change quest count and could potentially return what appears to be a dud quest
    /// Why they didnt just remove the dud quest from the list is beyond me. This fixes that.
    /// </summary>
    /// <returns>int</returns>
    public static int GetAdjustedCount()
    {
        if (DataManager.Instance is null)
        {
            return 25;
        }

        //47 is the hardcoded random quest amount
        //25 is the reverse index of the dud(?) quest in the list
        int adjustedNumber = 25 + (Quests.QuestsAll.Count - 47);
       // Plugin.Logger.LogWarning($"GetAdjustedCount(): Total quests: {Quests.QuestsAll.Count}, Adjusted number: {adjustedNumber}");
        return adjustedNumber;
    }


    //[HarmonyDebug]
    /// <summary>
    /// This is a patch to fix the hardcoded quest count in the Quests.GetQuest method. This is done by replacing the hardcoded value with a call to our own method.
    /// </summary>
    /// <param name="instructions"></param>
    /// <returns></returns>
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Quests), nameof(Quests.GetQuest))]
    public static IEnumerable<CodeInstruction> Quests_GetQuest_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> instructionList = instructions.ToList();

        for (int index = 0; index < instructionList.Count; index++)
        {
            CodeInstruction instruction = instructionList[index];
            if (instruction.opcode == OpCodes.Ldc_I4_S && (sbyte)instruction.operand == 0x19)
                instructionList[index] = new CodeInstruction(OpCodes.Call, typeof(CustomObjectivePatches).GetMethod(nameof(GetAdjustedCount)));
        }

        return instructionList.AsEnumerable();
    }
}