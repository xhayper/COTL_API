using HarmonyLib;
using UnityEngine;

namespace COTL_API.Debug;

[HarmonyPatch]
public static class DebugHelpers
{
    public static Interaction CurrentInteraction { get; private set; }
    
    [HarmonyPatch(typeof(Interaction), nameof(Interaction.OnInteract), typeof(StateMachine))]
    public static class InteractionPatches
    {
        [HarmonyPostfix]
        public static void Postfix(Interaction __instance)
        {
            if (!Plugin.Debug || __instance == null) return;
            CurrentInteraction = __instance;
            string message = "[Interaction]: ";
            if(__instance.gameObject != null)
            {
                GameObject gameObject = __instance.gameObject;
                message += $"GameObject Name: {gameObject.name}, ";
            }
            if (__instance.OutlineTarget != null) message += $"Outline target: {__instance.OutlineTarget.name}, ";
            message += $"Name: {__instance.name}";
            Plugin.Logger.LogWarning(message);
        }
    }
}