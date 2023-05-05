using COTL_API.CustomRelics;
using COTL_API.Helpers;

namespace COTL_API.Debug;

public class DebugRelicClass : CustomRelicData
{
    public override string InternalName { get; }

    public override string GetTitleLocalisation() => "DEBUG_RELIC";

    public override string GetDescriptionLocalisation() => RelicSubType == RelicSubType.Blessed ? "Gain 10 gold." : RelicSubType == RelicSubType.Dammed ? "Gain 5 poop." : "Gain 5 gold.";

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