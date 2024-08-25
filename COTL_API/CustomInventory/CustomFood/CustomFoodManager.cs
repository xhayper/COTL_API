using COTL_API.Guid;

namespace COTL_API.CustomInventory;

public static partial class CustomItemManager
{
    internal static Dictionary<InventoryItem.ITEM_TYPE, CustomFood> CustomFoodList { get; } = [];
    
    private static InventoryItem.ITEM_TYPE Add(CustomFood food)
    {
        var itemType = Add(food as CustomInventoryItem);
        var guid = CustomItemList[itemType].ModPrefix;

        food!.FollowerCommand = GuidManager.GetEnumValue<FollowerCommands>(guid, food.InternalName);
        var structureType = GuidManager.GetEnumValue<StructureBrain.TYPES>(guid, food.InternalName);
        food.StructureType = structureType;

        if (!StructuresData.AllStructures.Contains(structureType)) StructuresData.AllStructures.Add(structureType);

        CustomFoodList.Add(itemType, food);

        return itemType;
    }
}