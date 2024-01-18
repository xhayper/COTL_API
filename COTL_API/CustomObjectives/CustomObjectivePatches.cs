using System.Reflection.Emit;
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
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Quests), nameof(Quests.GetQuest))]
    public static void Quests_GetQuest()
    {
        foreach (var quest in DataManager.Instance.CompletedQuestsHistorys.Where(a =>
                     a.QuestIndex >= Quests.QuestsAll.Count))
        {
            LogWarning(
                "Found quests in history with an index higher than total quests (user may have removed mods that add quests), resetting to maximum possible.");
            quest.QuestIndex = Quests.QuestsAll.Count - 1;
        }
    }

    /// <summary>
    ///     There is a hardcoded check during the main for loop in Quests.GetQuest. The number used becomes invalid when we
    ///     change quest count and could potentially return what appears to be a dud quest
    ///     Why they didnt just remove the dud quest from the list is beyond me. This fixes that.
    /// </summary>
    /// <returns>int</returns>
    public static int GetAdjustedCount()
    {
        if (DataManager.Instance is null)
            return 25;

        //47 is the hardcoded random quest amount
        //25 is the reverse index of the dud(?) quest in the list
        var adjustedNumber = 25 + (Quests.QuestsAll.Count - 47);
        return adjustedNumber;
    }

    //[HarmonyDebug]
    /// <summary>
    ///     This is a patch to fix the hardcoded quest count in the Quests.GetQuest method. This is done by replacing the
    ///     hardcoded value with a call to our own method.
    /// </summary>
    /// <param name="instructions"></param>
    /// <returns></returns>
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Quests), nameof(Quests.GetQuest))]
    public static IEnumerable<CodeInstruction> Quests_GetQuest_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var instructionList = instructions.ToList();

        for (var index = 0; index < instructionList.Count; index++)
        {
            var instruction = instructionList[index];
            if (instruction.opcode == OpCodes.Ldc_I4_S && (sbyte)instruction.operand == 0x19)
                instructionList[index] = new CodeInstruction(OpCodes.Call,
                    typeof(CustomObjectiveManager).GetMethod(nameof(GetAdjustedCount)));
        }

        return instructionList.AsEnumerable();
    }
}