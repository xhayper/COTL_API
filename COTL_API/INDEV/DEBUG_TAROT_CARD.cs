namespace COTL_API.INDEV;

public class DEBUG_TAROT_CARD : CustomTarotCard.CustomTarotCard
{
    public override string InternalName => "DEBUG_TAROT_CARD";
    public override string LocalisedName(int _) { return "DEBUG_TAROT_CARD"; }
    public override string LocalisedDescription(int _) { return "<sprite=7>+3"; }

    public override void OnPickup()
    {
        base.OnPickup();
        PlayerFarming.Instance.GetComponent<HealthPlayer>().BlueHearts += 6;
        PlayerFarming.Instance.health.Update();
    }
}