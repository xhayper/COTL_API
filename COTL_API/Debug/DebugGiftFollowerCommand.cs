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
        if (0 >= Inventory.GetItemQuantity(Plugin.DebugItem))
        {
            return;
        }
        
        interaction.StartCoroutine(RunEnumerator(interaction, Plugin.DebugItem));
    }

    private IEnumerator RunEnumerator(interaction_FollowerInteraction interaction, InventoryItem.ITEM_TYPE type)
    {
        yield return interaction.GiveItemRoutine(type);
        interaction.follower.Brain.HardSwapToTask(new FollowerTask_InstantPoop());
    }
}