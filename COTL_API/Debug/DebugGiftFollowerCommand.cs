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
        interaction.eventListener.PlayFollowerVO(interaction.positiveAcknowledgeVO);
        GameManager.GetInstance().OnConversationNext(interaction.gameObject, 3f);
        GameManager.GetInstance().CameraSetOffset(Vector3.zero);
        yield return new WaitForSeconds(1f);
        DataManager.Instance.GivenFollowerGift = true;
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        PlayerFarming.Instance.simpleSpineAnimator.Animate("give-item/gift-medium", 0, false);
        JudgementMeter.ShowModify(1);
        CultFaithManager.AddThought(Thought.Cult_GaveFollowerItem, -1, 10,
            InventoryItem.LocalizedName(type));
        PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
        GameManager.GetInstance().OnConversationNext(interaction.gameObject, 3f);
        interaction.follower.Brain.GetWillLevelUp(FollowerBrain.AdorationActions.BigGift);
        int Waiting = 0;
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interaction.transform.position);
        ResourceCustomTarget.Create(interaction.follower.gameObject,
            PlayerFarming.Instance.CameraBone.transform.position, type, (() =>
            {
                interaction.follower.TimedAnimation($"Gifts/gift-medium-{Random.RandomRangeInt(1, 4)}",
                    3.6666667f, (() =>
                    {
                        AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty",
                            interaction.gameObject.transform.position);
                        interaction.follower.Brain.AddAdoration(FollowerBrain.AdorationActions.BigGift,
                            (() => ++Waiting));
                        System.Action<Follower, InventoryItem.ITEM_TYPE, System.Action> followerCallbacks =
                            InventoryItem.GiveToFollowerCallbacks(type);
                        if (followerCallbacks == null)
                            return;
                        followerCallbacks(interaction.follower, type, (() => ++Waiting));
                    }));
                Inventory.ChangeItemQuantity(type, -1);
            }), false);
        while (Waiting < 2)
            yield return null;
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GiveGift);
        if (interaction.follower.Brain.Stats.Adoration >= interaction.follower.Brain
                .Stats
                .MAX_ADORATION)
            interaction.StartCoroutine(interaction.GiveDiscipleRewardRoutine(
                interaction.previousTaskType, interaction.Close));
        else
            interaction.Close();
        
        interaction.StartCoroutine(interaction.FrameDelayCallback(delegate
        {
            interaction.follower.Brain.HardSwapToTask(new FollowerTask_InstantPoop());
        }));
    }
}