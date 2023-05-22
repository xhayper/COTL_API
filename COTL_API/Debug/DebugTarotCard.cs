namespace COTL_API.Debug;

public class DebugTarotCard : CustomTarotCard.CustomTarotCard
{
    public override string InternalName => "DEBUG_TAROT_CARD";

    public override float GetMovementSpeedMultiplier(TarotCards.TarotCard card)
    {
        return 25;
    }

    public override void ApplyInstantEffects(TarotCards.TarotCard card)
    {
        PlayerFarming.Instance.GetComponent<HealthPlayer>().BlueHearts += 50f;
        var position2 = PlayerFarming.Instance.CameraBone.transform.position;
        BiomeConstants.Instance.EmitHeartPickUpVFX(position2, 0f, "blue", "burst_big");
        AudioManager.Instance.PlayOneShot("event:/player/collect_blue_heart", position2);
    }
}