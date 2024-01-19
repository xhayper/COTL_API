using HarmonyLib;
using Unify;

namespace COTL_API.Patches;

[HarmonyPatch]
public static class AchievementsWrapperPatches
{
    [HarmonyPatch(typeof(AchievementsWrapper), nameof(AchievementsWrapper.UnlockAchievement))]
    [HarmonyPrefix]
    private static bool AchievementsWrapper_UnlockAchievement(ref Achievement achievementId)
    {
        if (Plugin.Instance == null || !Plugin.Instance.DontSaveAchievement)
            return true;

        if (AchievementsWrapper.unlockedAchievements.Contains(achievementId.id))
            return false;

        AchievementsWrapper.unlockedAchievements.Add(achievementId.id);

        var achievementUnlocked = AchievementsWrapper.OnAchievementUnlocked;
        achievementUnlocked?.Invoke(achievementId.label);

        if (achievementId == Achievements.Instance.Lookup("platinum"))
            return false;

        AchievementsWrapper.compareAchievements();
        return false;
    }

    [HarmonyPatch(typeof(AchievementsWrapper), nameof(AchievementsWrapper.GetAchievementProgress))]
    [HarmonyPrefix]
    private static bool AchievementsWrapper_GetAchievementProgress(ref List<AchievementProgress> result)
    {
        if (Plugin.Instance == null || !Plugin.Instance.DontSaveAchievement)
            return true;

        // Expect Error
        if (result == null || result.Count == 0)
            return false;

        foreach (var achievementProgress in result.Where(achievementProgress => achievementProgress.progress >= 100 &&
                     !AchievementsWrapper.unlockedAchievements.Contains(achievementProgress.id)))
        {
            AchievementsWrapper.unlockedAchievements.Add(achievementProgress.id);
            var achievementUnlocked = AchievementsWrapper.OnAchievementUnlocked;
            achievementUnlocked?.Invoke(achievementProgress.name);
        }

        return false;
    }

    [HarmonyPatch(typeof(AchievementsWrapper), nameof(AchievementsWrapper.UnlockPlatinum))]
    [HarmonyPrefix]
    private static bool AchievementsWrapper_UnlockPlatinum()
    {
        return Plugin.Instance == null || !Plugin.Instance.DontSaveAchievement;
    }
}