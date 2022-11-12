using UnityEngine;
using System;

namespace COTL_API.Helpers;

/// <summary>
/// List of defined items from the game. Used to retrieve the game objects prefab object.
/// </summary>
public static class ItemPickUp
{
    /// <summary>
    /// Returns a GameObject of the specified item type (if it exists). Will return a gold coin if the item type is not found.
    /// </summary>
    /// <param name="type">The type of item to return.</param>
    /// <returns>The specified item as a game object.</returns>
    public static GameObject GetItemPickUpObject(InventoryItem.ITEM_TYPE type)
    {
        Plugin.Instance.Logger.LogWarning($"ImitatePickUpObject: {type}");
        var itemText = "";
        switch (type)
        {
            case InventoryItem.ITEM_TYPE.LOG:
                itemText = "Log";
                break;
            case InventoryItem.ITEM_TYPE.STONE:
                itemText = "Rock1";
                break;
            case InventoryItem.ITEM_TYPE.ROCK2:
                itemText = "Rock2";
                break;
            case InventoryItem.ITEM_TYPE.ROCK3:
                itemText = "Rock3";
                break;
            case InventoryItem.ITEM_TYPE.SEED_SWORD:
                itemText = "Seed - Sword";
                break;
            case InventoryItem.ITEM_TYPE.MEAT:
                itemText = "Meat";
                break;
            case InventoryItem.ITEM_TYPE.WHEAT:
                itemText = "Wheat";
                break;
            case InventoryItem.ITEM_TYPE.SEED:
                itemText = "Seed";
                break;
            case InventoryItem.ITEM_TYPE.BONE:
                itemText = "VileBones";
                break;
            case InventoryItem.ITEM_TYPE.SOUL:
                itemText = "Soul";
                break;
            case InventoryItem.ITEM_TYPE.VINES:
                itemText = "GildedVine";
                break;
            case InventoryItem.ITEM_TYPE.RED_HEART:
                itemText = "Red Heart";
                break;
            case InventoryItem.ITEM_TYPE.HALF_HEART:
                itemText = "Half Heart";
                break;
            case InventoryItem.ITEM_TYPE.BLUE_HEART:
                itemText = "Blue Heart";
                break;
            case InventoryItem.ITEM_TYPE.HALF_BLUE_HEART:
                itemText = "Half Blue Heart";
                break;
            case InventoryItem.ITEM_TYPE.TIME_TOKEN:
                itemText = "Time Token";
                break;
            case InventoryItem.ITEM_TYPE.GENERIC:
                itemText = "Generic Pick Up";
                break;
            case InventoryItem.ITEM_TYPE.STAINED_GLASS:
                itemText = "StainedGlass";
                break;
            case InventoryItem.ITEM_TYPE.FLOWERS:
                itemText = "SacredFlower";
                break;
            case InventoryItem.ITEM_TYPE.BLACK_GOLD:
                itemText = "BlackGold";
                break;
            case InventoryItem.ITEM_TYPE.BERRY:
                itemText = "Berries";
                break;
            case InventoryItem.ITEM_TYPE.MONSTER_HEART:
                itemText = "Monster Heart";
                break;
            case InventoryItem.ITEM_TYPE.TRINKET_CARD:
                itemText = "TarotCard";
                break;
            case InventoryItem.ITEM_TYPE.SOUL_FRAGMENT:
                itemText = "SoulFragment";
                break;
            case InventoryItem.ITEM_TYPE.FISH:
                itemText = "Fish";
                break;
            case InventoryItem.ITEM_TYPE.MUSHROOM_SMALL:
                itemText = "Mushroom Small";
                break;
            case InventoryItem.ITEM_TYPE.BLACK_SOUL:
                itemText = "Black Soul";
                break;
            case InventoryItem.ITEM_TYPE.MUSHROOM_BIG:
                itemText = "Mushroom Big";
                break;
            case InventoryItem.ITEM_TYPE.MEAL:
                itemText = "Assets/Prefabs/Structures/Other/Meal.prefab";
                break;
            case InventoryItem.ITEM_TYPE.FISH_SMALL:
                itemText = "Fish Small";
                break;
            case InventoryItem.ITEM_TYPE.FISH_BIG:
                itemText = "Fish Big";
                break;
            case InventoryItem.ITEM_TYPE.GRASS:
                itemText = "Grass";
                break;
            case InventoryItem.ITEM_TYPE.THORNS:
                itemText = "Thorns";
                break;
            case InventoryItem.ITEM_TYPE.KEY_PIECE:
                itemText = "Key Piece";
                break;
            case InventoryItem.ITEM_TYPE.POOP:
                itemText = "Poop";
                break;
            case InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION:
                itemText = "FoundItem";
                break;
            case InventoryItem.ITEM_TYPE.FOUND_ITEM_WEAPON:
                itemText = "FoundItemWeapon";
                break;
            case InventoryItem.ITEM_TYPE.FOUND_ITEM_CURSE:
                itemText = "FoundItemCurse";
                break;
            case InventoryItem.ITEM_TYPE.GIFT_SMALL:
                itemText = "Gift Small";
                break;
            case InventoryItem.ITEM_TYPE.GIFT_MEDIUM:
                itemText = "Gift Medium";
                break;
            case InventoryItem.ITEM_TYPE.Necklace_1:
                itemText = "Necklace 1";
                break;
            case InventoryItem.ITEM_TYPE.Necklace_2:
                itemText = "Necklace 2";
                break;
            case InventoryItem.ITEM_TYPE.Necklace_3:
                itemText = "Necklace 3";
                break;
            case InventoryItem.ITEM_TYPE.Necklace_4:
                itemText = "Necklace 4";
                break;
            case InventoryItem.ITEM_TYPE.Necklace_5:
                itemText = "Necklace 5";
                break;
            case InventoryItem.ITEM_TYPE.PUMPKIN:
                itemText = "Pumpkin";
                break;
            case InventoryItem.ITEM_TYPE.SEED_PUMPKIN:
                itemText = "Seed Pumpkin";
                break;
            case InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN:
                itemText = "FoundItemSkin";
                break;
            case InventoryItem.ITEM_TYPE.BLACK_HEART:
                itemText = "Black Heart";
                break;
            case InventoryItem.ITEM_TYPE.PERMANENT_HALF_HEART:
                itemText = "Permanent Half Heart";
                break;
            case InventoryItem.ITEM_TYPE.FLOWER_RED:
                itemText = "Flower_red";
                break;
            case InventoryItem.ITEM_TYPE.FLOWER_WHITE:
                itemText = "Flower_White";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_GRASS:
                itemText = "Assets/Prefabs/Structures/Other/Meal Grass.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_MEAT:
                itemText = "Assets/Prefabs/Structures/Other/Meal Good.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_GREAT:
                itemText = "Assets/Prefabs/Structures/Other/Meal Great.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
                itemText = "Assets/Prefabs/Structures/Other/Meal Good Fish.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAT_ROTTEN:
                itemText = "Meat Rotten";
                break;
            case InventoryItem.ITEM_TYPE.FOLLOWER_MEAT:
                itemText = "Follower Meat";
                break;
            case InventoryItem.ITEM_TYPE.FOLLOWER_MEAT_ROTTEN:
                itemText = "Follower Meat";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT:
                itemText = "Assets/Prefabs/Structures/Other/Meal Follower Meat.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_POOP:
                itemText = "Assets/Prefabs/Structures/Other/Meal Poop.prefab";
                break;
            case InventoryItem.ITEM_TYPE.SEED_MUSHROOM:
                itemText = "Seed Mushroom";
                break;
            case InventoryItem.ITEM_TYPE.SEED_FLOWER_WHITE:
                itemText = "Seed White Flower";
                break;
            case InventoryItem.ITEM_TYPE.SEED_FLOWER_RED:
                itemText = "Seed Red Flower";
                break;
            case InventoryItem.ITEM_TYPE.GRASS2:
                itemText = "Grass 2";
                break;
            case InventoryItem.ITEM_TYPE.GRASS3:
                itemText = "Grass 3";
                break;
            case InventoryItem.ITEM_TYPE.GRASS4:
                itemText = "Grass 4";
                break;
            case InventoryItem.ITEM_TYPE.GRASS5:
                itemText = "Grass 5";
                break;
            case InventoryItem.ITEM_TYPE.FLOWER_PURPLE:
                itemText = "Flower_Purple";
                break;
            case InventoryItem.ITEM_TYPE.SEED_TREE:
                itemText = "Seed_tree";
                break;
            case InventoryItem.ITEM_TYPE.MAP:
                itemText = "FoundItemMap";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_MUSHROOMS:
                itemText = "Assets/Prefabs/Structures/Other/Meal Mushrooms.prefab";
                break;
            case InventoryItem.ITEM_TYPE.LOG_REFINED:
                itemText = "Log Refined";
                break;
            case InventoryItem.ITEM_TYPE.STONE_REFINED:
                itemText = "Stone Refined";
                break;
            case InventoryItem.ITEM_TYPE.GOLD_NUGGET:
                itemText = "Gold Nugget";
                break;
            case InventoryItem.ITEM_TYPE.ROPE:
                itemText = "Rope";
                break;
            case InventoryItem.ITEM_TYPE.GOLD_REFINED:
                itemText = "Gold Refined";
                break;
            case InventoryItem.ITEM_TYPE.BLOOD_STONE:
                itemText = "Bloodstone";
                break;
            case InventoryItem.ITEM_TYPE.TRINKET_CARD_UNLOCKED:
                itemText = "TarotCardUnlocked";
                break;
            case InventoryItem.ITEM_TYPE.CRYSTAL:
                itemText = "Crystal";
                break;
            case InventoryItem.ITEM_TYPE.SPIDER_WEB:
                itemText = "Spider Web";
                break;
            case InventoryItem.ITEM_TYPE.FISH_CRAB:
                itemText = "Fish Crab";
                break;
            case InventoryItem.ITEM_TYPE.FISH_LOBSTER:
                itemText = "Fish Lobster";
                break;
            case InventoryItem.ITEM_TYPE.FISH_OCTOPUS:
                itemText = "Fish Octopus";
                break;
            case InventoryItem.ITEM_TYPE.FISH_SQUID:
                itemText = "Fish Squid";
                break;
            case InventoryItem.ITEM_TYPE.FISH_SWORDFISH:
                itemText = "Fish Swordfish";
                break;
            case InventoryItem.ITEM_TYPE.FISH_BLOWFISH:
                itemText = "Fish Blowfish";
                break;
            case InventoryItem.ITEM_TYPE.BEETROOT:
                itemText = "Beetroot";
                break;
            case InventoryItem.ITEM_TYPE.SEED_BEETROOT:
                itemText = "Seed Beetroot";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
                itemText = "Assets/Prefabs/Structures/Other/Meal Great Fish.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_BAD_FISH:
                itemText = "Assets/Prefabs/Structures/Other/Meal Bad Fish.prefab";
                break;
            case InventoryItem.ITEM_TYPE.BEHOLDER_EYE:
                itemText = "Beholder Eye";
                break;
            case InventoryItem.ITEM_TYPE.CAULIFLOWER:
                itemText = "Cauliflower";
                break;
            case InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER:
                itemText = "Seed Cauliflower";
                break;
            case InventoryItem.ITEM_TYPE.MEAT_MORSEL:
                itemText = "Meat Morsel";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_BERRIES:
                itemText = "Assets/Prefabs/Structures/Other/Meal Berries.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG:
                itemText = "Assets/Prefabs/Structures/Other/Meal Medium Veg.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_BAD_MIXED:
                itemText = "Assets/Prefabs/Structures/Other/Meal Bad Mixed.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED:
                itemText = "Assets/Prefabs/Structures/Other/Meal Medium Mixed.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED:
                itemText = "Assets/Prefabs/Structures/Other/Meal Great Mixed.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_DEADLY:
                itemText = "Assets/Prefabs/Structures/Other/Meal Deadly.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_BAD_MEAT:
                itemText = "Assets/Prefabs/Structures/Other/Meal Bad Meat.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
                itemText = "Assets/Prefabs/Structures/Other/Meal Great Meat.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_BURNED:
                itemText = "Assets/Prefabs/Structures/Other/Meal Burned.prefab";
                break;
            case InventoryItem.ITEM_TYPE.DOCTRINE_STONE:
                itemText = "Doctrine Stone Piece";
                break;
            case InventoryItem.ITEM_TYPE.SHELL:
                itemText = "Shell";
                break;
            case InventoryItem.ITEM_TYPE.NONE:
                break;
            case InventoryItem.ITEM_TYPE.BLUE_PRINT:
                break;
            case InventoryItem.ITEM_TYPE.WEAPON_CARD:
                break;
            case InventoryItem.ITEM_TYPE.CURSE_CARD:
                break;
            case InventoryItem.ITEM_TYPE.KEY:
                break;
            case InventoryItem.ITEM_TYPE.MEAL_ROTTEN:
                break;
            case InventoryItem.ITEM_TYPE.SEEDS:
                break;
            case InventoryItem.ITEM_TYPE.MEALS:
                break;
            case InventoryItem.ITEM_TYPE.INGREDIENTS:
                break;
            case InventoryItem.ITEM_TYPE.FOLLOWERS:
                break;
            case InventoryItem.ITEM_TYPE.DISCIPLE_POINTS:
                break;
            case InventoryItem.ITEM_TYPE.TALISMAN:
                break;
            default:
                itemText = "BlackGold";
                break;
        }

        Plugin.Instance.Logger.LogWarning($"ImitateItemPickUpText: {itemText}");

        if (!itemText.EndsWith(".prefab", StringComparison.InvariantCultureIgnoreCase))
            return Resources.Load("Prefabs/Resources/" + itemText) as GameObject;

        return Resources.Load(itemText) as GameObject;
    }
}