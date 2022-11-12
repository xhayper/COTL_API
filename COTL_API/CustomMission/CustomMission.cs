namespace COTL_API.CustomMission;

public abstract class CustomMission
{
    public abstract string InternalName { get; }
    internal InventoryItem.ITEM_TYPE InnerType;
    internal string ModPrefix;

    public virtual InventoryItem.ITEM_TYPE RewardType => InventoryItem.ITEM_TYPE.BONE;
    public virtual int BaseChance => 75;
    public virtual IntRange RewardRange { get; } = new(15, 25);
}