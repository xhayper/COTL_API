using COTL_API.CustomFollowerCommand;
using Lamb.UI.FollowerInteractionWheel;
using System;
using System.Collections.Generic;
using System.Text;

namespace COTL_API.INDEV
{
    public class DEBUG_FOLLOWER_COMMAND_CLASS_2 : CustomFollowerCommandItem
    {
        public override string InternalName { get => "DEBUG_FOLLOWER_COMMAND_2"; }

        public override bool CheckSelectionPreconditions(Follower follower)
        {
            return base.CheckSelectionPreconditions(follower);
        }

        public override bool Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand = FollowerCommands.None)
        {
            interaction.follower.Brain.MakeDissenter();
            return true;
        }

        public override List<CommandItem> GetSubCommands()
        {
            return FollowerCommandGroups.AreYouSureCommands();
        }
    }
}
