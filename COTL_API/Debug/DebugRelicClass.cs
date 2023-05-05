using COTL_API.CustomRelics;

namespace COTL_API.Debug;

public class DebugRelicClass : CustomRelicData
{
    public override string InternalName => "DEBUG_RELIC";   

    public override string GetTitleLocalisation() => "DEBUG_RELIC";

    public override string GetDescriptionLocalisation() => RelicSubType switch
    {
        RelicSubType.Blessed => "Gain 10 gold.",
        RelicSubType.Dammed => "Gain 5 poop.",
        _ => "Gain 5 gold."
    };

    public override string GetLoreLocalization() => "Only those who bear the power of the Debug shall be able to use this relic.";
   
    public override bool CanBeBlessed => true;
    public override bool CanBeDamned => true;

    public override void Init()
    {
        UISprite = TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));
        UISpriteOutline = TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));
        WorldSprite = TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));
        DamageRequiredToCharge = 5.0f;
    }
    
    public override void OnUse(bool forceConsumableAnimation = false)
    {
        Inventory.AddItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, 5);
    }
    
    public override void OnUseDamned(bool forceConsumableAnimation = false)
    {
        Inventory.AddItem(InventoryItem.ITEM_TYPE.POOP, 5);
    }
    
    public override void OnUseBlessed(bool forceConsumableAnimation = false)
    {
        Inventory.AddItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10);
    }
}