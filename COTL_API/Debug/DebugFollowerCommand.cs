namespace COTL_API.Debug;

public class DebugFollowerCommand : CustomFollowerCommand.CustomFollowerCommand
{
    public override string InternalName => "DEBUG_FOLLOWER_COMMAND";

    public override string CommandStringIcon()
    {
        return "<sprite name=\"icon_Poop\">";
    }

    public override string GetTitle(Follower follower)
    {
        return "Poop";
    }

    public override string GetDescription(Follower follower)
    {
        return "Make this follower poop instantly";
    }

    public override void Execute(interaction_FollowerInteraction interaction,
        FollowerCommands finalCommand = FollowerCommands.None)
    {
        interaction.StartCoroutine(interaction.FrameDelayCallback(delegate
        {
            interaction.eventListener.PlayFollowerVO(interaction.generalAcknowledgeVO);
            interaction.follower.Brain.HardSwapToTask(new FollowerTask_InstantPoop());
        }));
        interaction.Close(true);
    }
}