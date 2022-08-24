using COTL_API.CustomInventory;
using COTL_API.Guid;
using HarmonyLib;
using Lamb.UI.FollowerInteractionWheel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace COTL_API.CustomFollowerCommand;

[HarmonyPatch]
public class CustomFollowerCommandManager
{
    internal static Dictionary<FollowerCommands, CustomFollowerCommandItem> customCommands = new Dictionary<FollowerCommands, CustomFollowerCommandItem>();

    public static FollowerCommands Add(CustomFollowerCommandItem item)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var followerCommand = GuidManager.GetEnumValue<FollowerCommands>(guid, item.InternalName);
        item.Command = followerCommand;
        item.SubCommands = item.GetSubCommands();
        item.ModPrefix = guid;

        customCommands.Add(followerCommand, item);

        return followerCommand;
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), "DefaultCommands", new Type[] { typeof(Follower) })]
    [HarmonyPostfix]
    public static void FollowerCommandGroups_DefaultCommands(Follower follower, List<CommandItem> __result)
    {
        customCommands.Values.Do(c => { if (c.GetCategories().Contains(FollowerCommandCategory.DEFAULT_COMMAND) && c.CheckSelectionPreconditions(follower)) __result.Add(c); });
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), "GiveWorkerCommands", new Type[] { typeof(Follower) })]
    [HarmonyPostfix]
    public static void FollowerCommandGroups_GiveWorkerCommands(Follower follower, List<CommandItem> __result)
    {
        customCommands.Values.Do(c => { if (c.GetCategories().Contains(FollowerCommandCategory.GIVE_WORKER_COMMAND) && c.CheckSelectionPreconditions(follower)) __result.Add(c); });
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), "MakeDemandCommands", new Type[] { typeof(Follower) })]
    [HarmonyPostfix]
    public static void FollowerCommandGroups_MakeDemandCommands(Follower follower, List<CommandItem> __result)
    {
        customCommands.Values.Do(c => { if (c.GetCategories().Contains(FollowerCommandCategory.MAKE_DEMAND_COMMAND) && c.CheckSelectionPreconditions(follower)) __result.Add(c); });
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), "WakeUpCommands", new Type[] { })]
    [HarmonyPostfix]
    public static void FollowerCommandGroups_WakeUpCommands(List<CommandItem> __result)
    {
        customCommands.Values.Do(c => { if (c.GetCategories().Contains(FollowerCommandCategory.WAKE_UP_COMMAND)) __result.Add(c); });
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), "OldAgeCommands", new Type[] { typeof(Follower) })]
    [HarmonyPostfix]
    public static void FollowerCommandGroups_OldAgeCommands(Follower follower, List<CommandItem> __result)
    {
        customCommands.Values.Do(c => { if (c.GetCategories().Contains(FollowerCommandCategory.OLD_AGE_COMMAND) && c.CheckSelectionPreconditions(follower)) __result.Add(c); });
    }

    [HarmonyPatch(typeof(FollowerCommandGroups), "DissenterCommands", new Type[] { typeof(Follower) })]
    [HarmonyPostfix]
    public static void FollowerCommandGroups_DissenterCommands(Follower follower, List<CommandItem> __result)
    {
        customCommands.Values.Do(c => { if (c.GetCategories().Contains(FollowerCommandCategory.DISSENTER_COMMAND) && c.CheckSelectionPreconditions(follower)) __result.Add(c); });
    }

    [HarmonyPatch(typeof(interaction_FollowerInteraction), "OnFollowerCommandFinalized", new Type[] { typeof(FollowerCommands[]) })]
    [HarmonyPrefix]
    public static bool interaction_FollowerInteraction_OnFollowerCommandFinalized(interaction_FollowerInteraction __instance, FollowerCommands[] followerCommands)
    {
        FollowerCommands command = followerCommands[0];
        FollowerCommands preFinalCommand = ((followerCommands.Length > 1) ? followerCommands[1] : FollowerCommands.None);

        if (customCommands.ContainsKey(command) || customCommands.ContainsKey(preFinalCommand))
        {
            bool shouldClose = true;
            if (customCommands.ContainsKey(preFinalCommand))
            {
                shouldClose = customCommands[preFinalCommand].Execute(__instance, command);
            }
            else
            {
                shouldClose = customCommands[command].Execute(__instance);
            }
            __instance.Close();
            return false;
        }
        return true;
    }

    [HarmonyPatch(typeof(FontImageNames), "IconForCommand", new Type[] { typeof(FollowerCommands) })]
    [HarmonyPrefix]
    public static bool interaction_FollowerInteraction_OnFollowerCommandFinalized(FollowerCommands followerCommands, ref string __result)
    {
        if (customCommands.ContainsKey(followerCommands))
        {
            __result = $"<sprite name=\"icon_{customCommands[followerCommands].ModPrefix}.${customCommands[followerCommands].InternalName}\">";
            return false;
        }
        return true;
    }
}