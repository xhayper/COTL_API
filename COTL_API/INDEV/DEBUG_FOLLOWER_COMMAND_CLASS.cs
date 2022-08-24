using COTL_API.CustomFollowerCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace COTL_API.INDEV
{
    public class DEBUG_FOLLOWER_COMMAND_CLASS : CustomFollowerCommandItem
    {
        public override string InternalName { get => "DEBUG_FOLLOWER_COMMAND"; }

        public override bool CheckSelectionPreconditions(Follower follower)
        {
            return base.CheckSelectionPreconditions(follower);
        }

        public override bool Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand = FollowerCommands.None)
        {
            interaction.StartCoroutine(interaction.FrameDelayCallback(delegate
            {
                interaction.eventListener.PlayFollowerVO(interaction.generalAcknowledgeVO);
                interaction.follower.Brain.HardSwapToTask(new FollowerTask_InstantPoop());
            }));

            return true;
        }
    }
}
