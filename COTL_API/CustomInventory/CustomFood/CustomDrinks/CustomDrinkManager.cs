using COTL_API.Guid;

namespace COTL_API.CustomInventory;

public static partial class CustomItemManager
{
    public static Dictionary<InventoryItem.ITEM_TYPE, CustomDrink> CustomDrinkList { get; } = [];
    
    public static InventoryItem.ITEM_TYPE Add(CustomDrink drink)
    {
        if (!CookingData.GetAllDrinks().Contains((drink as CustomInventoryItem).ItemPickUpToImitate))
            throw new ArgumentException("Custom Drink Imitation Item is not a Drink!", drink.InternalName);
        
        var itemType = Add(drink as CustomFood);
        var guid = CustomItemList[itemType].ModPrefix;
        
        drink.PleasureAction = GuidManager.GetEnumValue<FollowerBrain.PleasureActions>(guid, drink.InternalName);
        FollowerBrain.PleasureAndActions.Add(drink.PleasureAction, drink.Pleasure);
        
        CustomDrinkList.Add(itemType, drink);

        return itemType;
    }
}