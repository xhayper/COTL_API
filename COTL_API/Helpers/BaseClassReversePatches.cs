using System.Runtime.CompilerServices;
using HarmonyLib;

namespace COTL_API.Helpers;

public static class OnInteractHelper
{
    [HarmonyPatch(typeof(Interaction), nameof(Interaction.OnInteract))]
    [HarmonyReversePatch]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Interaction_OnInteract(Interaction instance, StateMachine state)
    {
        if (Plugin.Instance != null && Plugin.Instance.Debug)
            LogWarning($"Interaction.OnInteract Test({instance}, {state})");
    }
}