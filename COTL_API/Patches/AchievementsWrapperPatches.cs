using HarmonyLib;

namespace COTL_API.Patches;

[HarmonyPatch]
public static class AchievementsWrapperPatches
{
    [HarmonyPatch(typeof(AchievementsWrapper), nameof(AchievementsWrapper.UnlockAchievement))]
    [HarmonyPrefix]
    private static bool AchievementsWrapper_UnlockAchievement()
    {
        return Plugin.Instance == null || !Plugin.Instance.DontSaveAchievement;
    }
    
    [HarmonyPatch(typeof(AchievementsWrapper), nameof(AchievementsWrapper.GetAchievementProgress))]
    [HarmonyPrefix]
    private static bool AchievementsWrapper_GetAchievementProgress()
    {
        return Plugin.Instance == null || !Plugin.Instance.DontSaveAchievement;
    }

    [HarmonyPatch(typeof(AchievementsWrapper), nameof(AchievementsWrapper.UnlockPlatinum))]
    [HarmonyPrefix]
    private static bool AchievementsWrapper_UnlockPlatinum()
    {
        return Plugin.Instance == null || !Plugin.Instance.DontSaveAchievement;
    }
}