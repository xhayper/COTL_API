namespace COTL_API.Debug;

public class DebugTaskFollowerCommand : CustomFollowerCommand.CustomFollowerCommand
{
    public override string InternalName => "DEBUG_TASK_FOLLOWER_COMMAND";

    public override void Execute(interaction_FollowerInteraction interaction,
        FollowerCommands finalCommand = FollowerCommands.None)
    {
        interaction.StartCoroutine(interaction.FrameDelayCallback(delegate
        {
            interaction.eventListener.PlayFollowerVO(interaction.generalAcknowledgeVO);

            var str = TaskUtils.GetAvailableStructureOfType<DebugStructure>();
            if (str == null)
            {
                interaction.eventListener.PlayFollowerVO(interaction.negativeAcknowledgeVO);
                interaction.CloseAndSpeak("No_Debug_Structures");
                return;
            }

            interaction.follower.Brain.HardSwapToTask(new DebugTask());
        }));
        interaction.Close(true, reshowMenu: false);
    }
}