using System.Reflection;
using COTL_API.Guid;
using Sirenix.Serialization.Utilities;
using Random = UnityEngine.Random;

namespace COTL_API.CustomInventory;

public static partial class CustomItemManager
{
    /// <summary>
    ///     Used to define an items rarity for the purpose of adding them to offering shrines.
    /// </summary>
    public enum ItemRarity
    {
        COMMON,
        RARE
    }

    public static Dictionary<InventoryItem.ITEM_TYPE, CustomInventoryItem> CustomItemList { get; } = new();
    public static Dictionary<InventoryItem.ITEM_TYPE, CustomMeal> CustomMealList { get; } = [];

    public static InventoryItem.ITEM_TYPE Add(CustomInventoryItem item)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var itemType = GuidManager.GetEnumValue<InventoryItem.ITEM_TYPE>(guid, item.InternalName);
        item.ItemType = itemType;
        item.ModPrefix = guid;
        item.InternalObjectName = $"CustomItem_{item.InternalName}";

        if (item.GetType().InheritsFrom(typeof(CustomMeal)))
        {
            var meal = item as CustomMeal;

            if (!IsMeal(meal!.ItemType))
                throw new ArgumentException("Custom Meal Imitation Item is not a meal!", item.InternalName);

            meal!.FollowerCommand = GuidManager.GetEnumValue<FollowerCommands>(guid, meal.InternalName);
            var structureType = GuidManager.GetEnumValue<StructureBrain.TYPES>(guid, meal.InternalName);
            meal.StructureType = structureType;

            if (!StructuresData.AllStructures.Contains(structureType)) StructuresData.AllStructures.Add(structureType);

            CustomMealList.Add(itemType,meal);
        }

        CustomItemList.Add(itemType, item);

        return itemType;
    }

    private static bool IsMeal(InventoryItem.ITEM_TYPE itemType)
    {
        List<InventoryItem.ITEM_TYPE> meals = [
            InventoryItem.ITEM_TYPE.MEAL , InventoryItem.ITEM_TYPE.MEALS,
            InventoryItem.ITEM_TYPE.MEAL_BAD_FISH , InventoryItem.ITEM_TYPE.MEAL_BAD_MEAT,
            InventoryItem.ITEM_TYPE.MEAL_BAD_MIXED , InventoryItem.ITEM_TYPE.MEAL_BERRIES,
            InventoryItem.ITEM_TYPE.MEAL_DEADLY , InventoryItem.ITEM_TYPE.MEAL_EGG,
            InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT , InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH,
            InventoryItem.ITEM_TYPE.MEAL_GRASS, InventoryItem.ITEM_TYPE.MEAL_GREAT,
            InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH, InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH,
            InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT, InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED,
            InventoryItem.ITEM_TYPE.MEAL_MEAT, InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED,
            InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED, InventoryItem.ITEM_TYPE.MEAL_MUSHROOMS,
            InventoryItem.ITEM_TYPE.MEAL_POOP, InventoryItem.ITEM_TYPE.MEAL_SPICY
        ];

        foreach (var item in meals)
        {
            if(itemType == item)
                return true;
        }
        return false;
    }

    /// <summary>
    ///     A method to return whether to drop loot or not based on the custom items chances to drop. Defaults to the items
    ///     DungeonChestSpawnChance unless a custom chance is provided.
    /// </summary>
    /// <param name="customInventoryItem">The item to get chance for.</param>
    /// <param name="customChance">Override the items chance with a customerChance.</param>
    /// <returns>Returns true/false based on the roll.</returns>
    public static bool DropLoot(CustomInventoryItem customInventoryItem, float customChance = 0f)
    {
        float roll = Random.Range(0, 101);
        var chance = customChance > 0
            ? customChance
            : customInventoryItem.DungeonChestSpawnChance +
              customInventoryItem.DungeonChestSpawnChance * DataManager.Instance.GetLuckMultiplier();
        if (Plugin.Instance != null && Plugin.Instance.Debug)
            LogDebug(
                $"{customInventoryItem.InternalObjectName} Roll/Chance: {roll} / {chance}: Win? {roll <= chance}");
        return roll <= chance;
    }

    /// <summary>
    ///     Used to retrieve the custom item from the custom item dictionary based on it's internal object name.
    /// </summary>
    /// <param name="name">Name of the items internal object to search for.</param>
    /// <returns>If found, returns the CustomInventoryItem object.</returns>
    public static KeyValuePair<InventoryItem.ITEM_TYPE, CustomInventoryItem>
        GetItemObjectByInternalObjectName(string name)
    {
        return (from item in CustomItemList where item.Value.InternalObjectName == name select item).FirstOrDefault();
    }

    public static void AddGift(InventoryItem.ITEM_TYPE item)
    {
        if (!DataManager.AllGifts.Contains(item)) DataManager.AllGifts.Add(item);
    }

    public static void RemoveGift(InventoryItem.ITEM_TYPE item)
    {
        if (DataManager.AllGifts.Contains(item)) DataManager.AllGifts.Remove(item);
    }

    public static void AddNecklace(InventoryItem.ITEM_TYPE item)
    {
        if (!DataManager.AllNecklaces.Contains(item)) DataManager.AllNecklaces.Add(item);
    }

    public static void RemoveNecklace(InventoryItem.ITEM_TYPE item)
    {
        if (DataManager.AllNecklaces.Contains(item)) DataManager.AllNecklaces.Remove(item);
    }
}