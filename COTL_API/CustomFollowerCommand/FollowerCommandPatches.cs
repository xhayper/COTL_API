using HarmonyLib;
using Lamb.UI.FollowerInteractionWheel;
using System.Collections.Generic;

namespace COTL_API.CustomFollowerCommand;

[HarmonyPatch]
public partial class CustomFollowerCommandManager
{
    [HarmonyPatch(typeof(CommandItem), nameof(CommandItem.GetTitle))]
    [HarmonyPrefix]
    private static bool CommandItem_GetTitle(CommandItem __instance, Follower follower, ref string __result)
    {
        if (!CustomFollowerCommands.ContainsKey(__instance.Command)) return true;
        __result = CustomFollowerCommands[__instance.Command].GetTitle(follower);
        return false;
    }
    
    [HarmonyPatch(typeof(CommandItem), nameof(CommandItem.IsAvailable))]
    [HarmonyPrefix]
    private static bool CommandItem_IsAvailable(CommandItem __instance, Follower follower, ref bool __result)
    {
        if (!CustomFollowerCommands.ContainsKey(__instance.Command)) return true;
        __result = CustomFollowerCommands[__instance.Command].IsAvailable(follower);
        return false;
    }
    
    [HarmonyPatch(typeof(CommandItem), nameof(CommandItem.GetDescription))]
    [HarmonyPrefix]
    private static bool CommandItem_GetDescription(CommandItem __instance, Follower follower, ref string __result)
    {
        if (!CustomFollowerCommands.ContainsKey(__instance.Command)) return true;
        __result = CustomFollowerCommands[__instance.Command].GetDescription(follower);
        return false;
    }
    
    [HarmonyPatch(typeof(CommandItem), nameof(CommandItem.GetLockedDescription))]
    [HarmonyPrefix]
    private static bool CommandItem_GetLockedDescription(CommandItem __instance, Follower follower, ref string __result)
    {
        if (!CustomFollowerCommands.ContainsKey(__instance.Command)) return true;
        __result = CustomFollowerCommands[__instance.Command].GetLockedDescription(follower);
        return false;
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.DefaultCommands))]
    [HarmonyPostfix]
    private static void FollowerCommandGroups_DefaultCommands(Follower follower, List<CommandItem> __result)
    {
        CustomFollowerCommands.Values.Do(c =>
        {
            if (c.Categories.Contains(FollowerCommandCategory.DEFAULT_COMMAND) && c.ShouldAppearFor(follower))
                __result.Add(c);
        });
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.GiveWorkerCommands))]
    [HarmonyPostfix]
    private static void FollowerCommandGroups_GiveWorkerCommands(Follower follower, List<CommandItem> __result)
    {
        CustomFollowerCommands.Values.Do(c =>
        {
            if (c.Categories.Contains(FollowerCommandCategory.GIVE_WORKER_COMMAND) && c.ShouldAppearFor(follower))
                __result.Add(c);
        });
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.MakeDemandCommands))]
    [HarmonyPostfix]
    private static void FollowerCommandGroups_MakeDemandCommands(Follower follower, List<CommandItem> __result)
    {
        CustomFollowerCommands.Values.Do(c =>
        {
            if (c.Categories.Contains(FollowerCommandCategory.MAKE_DEMAND_COMMAND) && c.ShouldAppearFor(follower))
                __result.Add(c);
        });
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.WakeUpCommands))]
    [HarmonyPostfix]
    private static void FollowerCommandGroups_WakeUpCommands(List<CommandItem> __result)
    {
        CustomFollowerCommands.Values.Do(c =>
        {
            if (c.Categories.Contains(FollowerCommandCategory.WAKE_UP_COMMAND)) __result.Add(c);
        });
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.OldAgeCommands))]
    [HarmonyPostfix]
    private static void FollowerCommandGroups_OldAgeCommands(Follower follower, List<CommandItem> __result)
    {
        CustomFollowerCommands.Values.Do(c =>
        {
            if (c.Categories.Contains(FollowerCommandCategory.OLD_AGE_COMMAND) && c.ShouldAppearFor(follower))
                __result.Add(c);
        });
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.DissenterCommands))]
    [HarmonyPostfix]
    private static void FollowerCommandGroups_DissenterCommands(Follower follower, List<CommandItem> __result)
    {
        CustomFollowerCommands.Values.Do(c =>
        {
            if (c.Categories.Contains(FollowerCommandCategory.DISSENTER_COMMAND) && c.ShouldAppearFor(follower))
                __result.Add(c);
        });
    }

    [HarmonyPatch(typeof(interaction_FollowerInteraction),
        nameof(interaction_FollowerInteraction.OnFollowerCommandFinalized))]
    [HarmonyPrefix]
    private static bool interaction_FollowerInteraction_OnFollowerCommandFinalized(
        interaction_FollowerInteraction __instance, FollowerCommands[] followerCommands)
    {
        FollowerCommands command = followerCommands[0];
        FollowerCommands preFinalCommand = followerCommands.Length > 1 ? followerCommands[1] : FollowerCommands.None;

        if (!CustomFollowerCommands.ContainsKey(command) &&
            !CustomFollowerCommands.ContainsKey(preFinalCommand)) return true;

        if (CustomFollowerCommands.ContainsKey(preFinalCommand))
            CustomFollowerCommands[preFinalCommand].Execute(__instance, command);
        else CustomFollowerCommands[command].Execute(__instance);
        
        return false;
    }

    [HarmonyPatch(typeof(FontImageNames), nameof(FontImageNames.IconForCommand))]
    [HarmonyPrefix]
    private static bool FontImageNames_IconForCommand(FollowerCommands followerCommands,
        ref string __result)
    {
        if (!CustomFollowerCommands.ContainsKey(followerCommands)) return true;

        __result = CustomFollowerCommands[followerCommands].CommandStringIcon();
        
        return false;
    }
}