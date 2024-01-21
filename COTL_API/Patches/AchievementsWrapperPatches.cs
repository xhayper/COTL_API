using System.Reflection.Emit;
using HarmonyLib;
using Unify;

namespace COTL_API.Patches;

[HarmonyPatch]
public static class AchievementsWrapperPatches
{
    private static IEnumerable<CodeInstruction> patchAchievementPlayerPerfs(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchForward(false,
                new CodeMatch(OpCodes.Ldstr, "unlockedAchievements"),
                new CodeMatch(OpCodes.Ldsfld,
                    AccessTools.Field(typeof(AchievementsWrapper), nameof(AchievementsWrapper.unlockedAchievements))),
                new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(object), nameof(ToString))),
                new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(PlayerPrefs), nameof(PlayerPrefs.SetString)))
            )
            .SetInstructionAndAdvance(Transpilers.EmitDelegate(() =>
            {
                if (!Plugin.Instance || !Plugin.Instance!.DisableAchievement)
                    PlayerPrefs.SetString("unlockedAchievements",
                        AchievementsWrapper.unlockedAchievements.ToString());
            }))
            .SetAndAdvance(OpCodes.Nop, null)
            .SetAndAdvance(OpCodes.Nop, null)
            .SetAndAdvance(OpCodes.Nop, null)
            .MatchForward(false,
                new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(PlayerPrefs), nameof(PlayerPrefs.Save))))
            .SetInstructionAndAdvance(Transpilers.EmitDelegate(() =>
            {
                if (!Plugin.Instance || !Plugin.Instance!.DisableAchievement)
                    PlayerPrefs.Save();
            }))
            .InstructionEnumeration();
    }

    [HarmonyPatch(typeof(AchievementsWrapper), nameof(AchievementsWrapper.UnlockAchievement))]
    [HarmonyTranspiler]
    [HarmonyDebug]
    private static IEnumerable<CodeInstruction> AchievementsWrapper_UnlockAchievement(
        IEnumerable<CodeInstruction> instructions)
    {
        return patchAchievementPlayerPerfs(instructions);
    }

    [HarmonyPatch(typeof(AchievementsWrapper), nameof(AchievementsWrapper.GetAchievementProgress))]
    [HarmonyTranspiler]
    [HarmonyDebug]
    private static IEnumerable<CodeInstruction> AchievementsWrapper_GetAchievementProgress(
        IEnumerable<CodeInstruction> instructions)
    {
        return patchAchievementPlayerPerfs(instructions);
    }

    [HarmonyPatch(typeof(AchievementsWrapper), nameof(AchievementsWrapper.UnlockPlatinum))]
    [HarmonyPrefix]
    private static bool AchievementsWrapper_UnlockPlatinum()
    {
        return Plugin.Instance == null || !Plugin.Instance.DisableAchievement;
    }
}