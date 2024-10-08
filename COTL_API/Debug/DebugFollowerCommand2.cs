using COTL_API.CustomFollowerCommand;

namespace COTL_API.Debug;

public class DebugFollowerCommandClass2 : CustomFollowerCommand.CustomFollowerCommand
{
    public DebugFollowerCommandClass2()
    {
        SubCommands = FollowerCommandGroups.AreYouSureCommands();
    }

    public override string InternalName => "DEBUG_FOLLOWER_COMMAND_2";

    public override List<FollowerCommandCategory> Categories => [FollowerCommandCategory.MAKE_DEMAND_COMMAND];

    public override void Execute(interaction_FollowerInteraction interaction,
        FollowerCommands finalCommand = FollowerCommands.None)
    {
        interaction.follower.Brain.MakeDissenter();
        interaction.Close(true, reshowMenu: false);
    }
}