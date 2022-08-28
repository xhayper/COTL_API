using UnityEngine;

namespace COTL_API.Debug;

public class DebugTarotCard : CustomTarotCard.CustomTarotCard
{
    public override string InternalName => "DEBUG_TAROT_CARD";
    public override string LocalisedName(int _) { return "DEBUG_TAROT_CARD"; }
    public override string LocalisedLore() { return "<color=purple>NYOOOOOOOOOOOOOOOOOOOOOOOOOOM</color>"; }
    public override string LocalisedDescription(int _) { return "<sprite name=\"icon_BlueHeart\"> +3, 10x walk speed"; }

    public override string Skin => "Trinkets/TheLovers2";

    public override float GetMovementSpeedMultiplier(TarotCards.TarotCard card)
    {
        return 10;
    }

    public override void ApplyInstantEffects(TarotCards.TarotCard card)
    {
        PlayerFarming.Instance.GetComponent<HealthPlayer>().BlueHearts += 6f;
        Vector3 position2 = PlayerFarming.Instance.CameraBone.transform.position;
        BiomeConstants.Instance.EmitHeartPickUpVFX(position2, 0f, "blue", "burst_big");
        AudioManager.Instance.PlayOneShot("event:/player/collect_blue_heart", position2);
    }
}