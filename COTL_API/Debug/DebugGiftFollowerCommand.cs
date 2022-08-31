using System.Collections;
using UnityEngine;

namespace COTL_API.Debug;

public class DebugGiftFollowerCommand : CustomFollowerCommand.CustomFollowerCommand
{
    public override string InternalName => "DEBUG_GIFT_FOLLOWER_COMMAND";
    public override bool ShouldAppearFor(Follower follower) => false;

    public override void Execute(interaction_FollowerInteraction interaction,
        FollowerCommands finalCommand)
    {
        interaction.StartCoroutine(RunEnumerator(interaction, Plugin.DebugItem));
    }

    private IEnumerator RunEnumerator(interaction_FollowerInteraction interaction, InventoryItem.ITEM_TYPE type)
    {
        interaction.eventListener.PlayFollowerVO(interaction.positiveAcknowledgeVO);
        yield return new WaitForSeconds(.35f);
        interaction.follower.Brain.HardSwapToTask(new FollowerTask_InstantPoop());
        interaction.Close();
    }
}