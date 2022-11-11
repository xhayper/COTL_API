using COTL_API.CustomFollowerCommand;
using System.Collections.Generic;

namespace COTL_API.Debug;

public class DebugFollowerCommandClass2 : CustomFollowerCommand.CustomFollowerCommand
{
    public override string InternalName => "DEBUG_FOLLOWER_COMMAND_2";

    public override string GetTitle(Follower follower)
    {
        return "Dissent";
    }

    public override string GetDescription(Follower follower)
    {
        return "Make this follower turns into a dissenter instantly";
    }

    public override List<FollowerCommandCategory> Categories => new() { FollowerCommandCategory.MAKE_DEMAND_COMMAND };

    public override void Execute(interaction_FollowerInteraction interaction,
        FollowerCommands finalCommand = FollowerCommands.None)
    {
        interaction.follower.Brain.MakeDissenter();
        interaction.Close();
    }

    public DebugFollowerCommandClass2()
    {
        SubCommands = FollowerCommandGroups.AreYouSureCommands();
    }
}