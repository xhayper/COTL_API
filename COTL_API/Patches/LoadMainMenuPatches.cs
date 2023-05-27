using System.Collections;
using HarmonyLib;
using MMTools;

namespace COTL_API.Patches;

[HarmonyPatch]
public static class LoadMainMenuPatches
{
    private static IEnumerator GetEmptyEnumerator()
    {
        yield break;
    }

    [HarmonyPatch(typeof(LoadMainMenu), nameof(LoadMainMenu.RunSplashScreens))]
    [HarmonyPrefix]
    private static bool LoadMainMenu_RunSplashScreens(ref IEnumerator __result)
    {
        if (Plugin.Instance != null && !Plugin.Instance.SkipSplashScreen) return true;

        __result = GetEmptyEnumerator();
        MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu",
            1f, "", null);

        return false;
    }
}