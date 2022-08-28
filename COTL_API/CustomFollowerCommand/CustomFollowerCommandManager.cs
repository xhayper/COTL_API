using Lamb.UI.FollowerInteractionWheel;
using System.Collections.Generic;
using System.Reflection;
using COTL_API.Guid;
using HarmonyLib;

namespace COTL_API.CustomFollowerCommand;

[HarmonyPatch]
public class CustomFollowerCommandManager
{
    internal static readonly Dictionary<FollowerCommands, CustomFollowerCommand> CustomFollowerCommands = new();

    public static FollowerCommands Add(CustomFollowerCommand command)
    {
        string guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        FollowerCommands followerCommand = GuidManager.GetEnumValue<FollowerCommands>(guid, command.InternalName);
        command.Command = followerCommand;
        command.ModPrefix = guid;

        CustomFollowerCommands.Add(followerCommand, command);

        return followerCommand;
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.DefaultCommands))]
    [HarmonyPostfix]
    public static void FollowerCommandGroups_DefaultCommands(Follower follower, List<CommandItem> __result)
    {
        CustomFollowerCommands.Values.Do(c =>
        {
            if (c.Categories.Contains(FollowerCommandCategory.DEFAULT_COMMAND) && c.ShouldAppearFor(follower))
                __result.Add(c);
        });
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.GiveWorkerCommands))]
    [HarmonyPostfix]
    public static void FollowerCommandGroups_GiveWorkerCommands(Follower follower, List<CommandItem> __result)
    {
        CustomFollowerCommands.Values.Do(c =>
        {
            if (c.Categories.Contains(FollowerCommandCategory.GIVE_WORKER_COMMAND) && c.ShouldAppearFor(follower))
                __result.Add(c);
        });
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.MakeDemandCommands))]
    [HarmonyPostfix]
    public static void FollowerCommandGroups_MakeDemandCommands(Follower follower, List<CommandItem> __result)
    {
        CustomFollowerCommands.Values.Do(c =>
        {
            if (c.Categories.Contains(FollowerCommandCategory.MAKE_DEMAND_COMMAND) && c.ShouldAppearFor(follower))
                __result.Add(c);
        });
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.WakeUpCommands))]
    [HarmonyPostfix]
    public static void FollowerCommandGroups_WakeUpCommands(List<CommandItem> __result)
    {
        CustomFollowerCommands.Values.Do(c =>
        {
            if (c.Categories.Contains(FollowerCommandCategory.WAKE_UP_COMMAND)) __result.Add(c);
        });
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.OldAgeCommands))]
    [HarmonyPostfix]
    public static void FollowerCommandGroups_OldAgeCommands(Follower follower, List<CommandItem> __result)
    {
        CustomFollowerCommands.Values.Do(c =>
        {
            if (c.Categories.Contains(FollowerCommandCategory.OLD_AGE_COMMAND) && c.ShouldAppearFor(follower))
                __result.Add(c);
        });
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.DissenterCommands))]
    [HarmonyPostfix]
    public static void FollowerCommandGroups_DissenterCommands(Follower follower, List<CommandItem> __result)
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
    public static bool interaction_FollowerInteraction_OnFollowerCommandFinalized(
        interaction_FollowerInteraction __instance, FollowerCommands[] followerCommands)
    {
        FollowerCommands command = followerCommands[0];
        FollowerCommands preFinalCommand = followerCommands.Length > 1 ? followerCommands[1] : FollowerCommands.None;

        if (!CustomFollowerCommands.ContainsKey(command) &&
            !CustomFollowerCommands.ContainsKey(preFinalCommand)) return true;

        bool shouldClose = CustomFollowerCommands.ContainsKey(preFinalCommand)
            ? CustomFollowerCommands[preFinalCommand].Execute(__instance, command)
            : CustomFollowerCommands[command].Execute(__instance);

        if (shouldClose) __instance.Close();

        return false;
    }

    [HarmonyPatch(typeof(FontImageNames), nameof(FontImageNames.IconForCommand))]
    [HarmonyPrefix]
    public static bool FontImageNames_IconForCommand(FollowerCommands followerCommands,
        ref string __result)
    {
        if (!CustomFollowerCommands.ContainsKey(followerCommands)) return true;

        __result =
            $"<sprite name=\"icon_FCOMMAND_{CustomFollowerCommands[followerCommands].ModPrefix}.${CustomFollowerCommands[followerCommands].InternalName}\">";

        return false;
    }
}