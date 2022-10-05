using UnityEngine;

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
        string text = "";
        switch (type)
        {
            case InventoryItem.ITEM_TYPE.LOG:
                text = "Log";
                break;
            case InventoryItem.ITEM_TYPE.STONE:
                text = "Rock1";
                break;
            case InventoryItem.ITEM_TYPE.ROCK2:
                text = "Rock2";
                break;
            case InventoryItem.ITEM_TYPE.ROCK3:
                text = "Rock3";
                break;
            case InventoryItem.ITEM_TYPE.SEED_SWORD:
                text = "Seed - Sword";
                break;
            case InventoryItem.ITEM_TYPE.MEAT:
                text = "Meat";
                break;
            case InventoryItem.ITEM_TYPE.WHEAT:
                text = "Wheat";
                break;
            case InventoryItem.ITEM_TYPE.SEED:
                text = "Seed";
                break;
            case InventoryItem.ITEM_TYPE.BONE:
                text = "VileBones";
                break;
            case InventoryItem.ITEM_TYPE.SOUL:
                text = "Soul";
                break;
            case InventoryItem.ITEM_TYPE.VINES:
                text = "GildedVine";
                break;
            case InventoryItem.ITEM_TYPE.RED_HEART:
                text = "Red Heart";
                break;
            case InventoryItem.ITEM_TYPE.HALF_HEART:
                text = "Half Heart";
                break;
            case InventoryItem.ITEM_TYPE.BLUE_HEART:
                text = "Blue Heart";
                break;
            case InventoryItem.ITEM_TYPE.HALF_BLUE_HEART:
                text = "Half Blue Heart";
                break;
            case InventoryItem.ITEM_TYPE.TIME_TOKEN:
                text = "Time Token";
                break;
            case InventoryItem.ITEM_TYPE.GENERIC:
                text = "Generic Pick Up";
                break;
            case InventoryItem.ITEM_TYPE.STAINED_GLASS:
                text = "StainedGlass";
                break;
            case InventoryItem.ITEM_TYPE.FLOWERS:
                text = "SacredFlower";
                break;
            case InventoryItem.ITEM_TYPE.BLACK_GOLD:
                text = "BlackGold";
                break;
            case InventoryItem.ITEM_TYPE.BERRY:
                text = "Berries";
                break;
            case InventoryItem.ITEM_TYPE.MONSTER_HEART:
                text = "Monster Heart";
                break;
            case InventoryItem.ITEM_TYPE.TRINKET_CARD:
                text = "TarotCard";
                break;
            case InventoryItem.ITEM_TYPE.SOUL_FRAGMENT:
                text = "SoulFragment";
                break;
            case InventoryItem.ITEM_TYPE.FISH:
                text = "Fish";
                break;
            case InventoryItem.ITEM_TYPE.MUSHROOM_SMALL:
                text = "Mushroom Small";
                break;
            case InventoryItem.ITEM_TYPE.BLACK_SOUL:
                text = "Black Soul";
                break;
            case InventoryItem.ITEM_TYPE.MUSHROOM_BIG:
                text = "Mushroom Big";
                break;
            case InventoryItem.ITEM_TYPE.MEAL:
                text = "Assets/Prefabs/Structures/Other/Meal.prefab";
                break;
            case InventoryItem.ITEM_TYPE.FISH_SMALL:
                text = "Fish Small";
                break;
            case InventoryItem.ITEM_TYPE.FISH_BIG:
                text = "Fish Big";
                break;
            case InventoryItem.ITEM_TYPE.GRASS:
                text = "Grass";
                break;
            case InventoryItem.ITEM_TYPE.THORNS:
                text = "Thorns";
                break;
            case InventoryItem.ITEM_TYPE.KEY_PIECE:
                text = "Key Piece";
                break;
            case InventoryItem.ITEM_TYPE.POOP:
                text = "Poop";
                break;
            case InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION:
                text = "FoundItem";
                break;
            case InventoryItem.ITEM_TYPE.FOUND_ITEM_WEAPON:
                text = "FoundItemWeapon";
                break;
            case InventoryItem.ITEM_TYPE.FOUND_ITEM_CURSE:
                text = "FoundItemCurse";
                break;
            case InventoryItem.ITEM_TYPE.GIFT_SMALL:
                text = "Gift Small";
                break;
            case InventoryItem.ITEM_TYPE.GIFT_MEDIUM:
                text = "Gift Medium";
                break;
            case InventoryItem.ITEM_TYPE.Necklace_1:
                text = "Necklace 1";
                break;
            case InventoryItem.ITEM_TYPE.Necklace_2:
                text = "Necklace 2";
                break;
            case InventoryItem.ITEM_TYPE.Necklace_3:
                text = "Necklace 3";
                break;
            case InventoryItem.ITEM_TYPE.Necklace_4:
                text = "Necklace 4";
                break;
            case InventoryItem.ITEM_TYPE.Necklace_5:
                text = "Necklace 5";
                break;
            case InventoryItem.ITEM_TYPE.PUMPKIN:
                text = "Pumpkin";
                break;
            case InventoryItem.ITEM_TYPE.SEED_PUMPKIN:
                text = "Seed Pumpkin";
                break;
            case InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN:
                text = "FoundItemSkin";
                break;
            case InventoryItem.ITEM_TYPE.BLACK_HEART:
                text = "Black Heart";
                break;
            case InventoryItem.ITEM_TYPE.PERMANENT_HALF_HEART:
                text = "Permanent Half Heart";
                break;
            case InventoryItem.ITEM_TYPE.FLOWER_RED:
                text = "Flower_red";
                break;
            case InventoryItem.ITEM_TYPE.FLOWER_WHITE:
                text = "Flower_White";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_GRASS:
                text = "Assets/Prefabs/Structures/Other/Meal Grass.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_MEAT:
                text = "Assets/Prefabs/Structures/Other/Meal Good.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_GREAT:
                text = "Assets/Prefabs/Structures/Other/Meal Great.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
                text = "Assets/Prefabs/Structures/Other/Meal Good Fish.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAT_ROTTEN:
                text = "Meat Rotten";
                break;
            case InventoryItem.ITEM_TYPE.FOLLOWER_MEAT:
                text = "Follower Meat";
                break;
            case InventoryItem.ITEM_TYPE.FOLLOWER_MEAT_ROTTEN:
                text = "Follower Meat";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT:
                text = "Assets/Prefabs/Structures/Other/Meal Follower Meat.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_POOP:
                text = "Assets/Prefabs/Structures/Other/Meal Poop.prefab";
                break;
            case InventoryItem.ITEM_TYPE.SEED_MUSHROOM:
                text = "Seed Mushroom";
                break;
            case InventoryItem.ITEM_TYPE.SEED_FLOWER_WHITE:
                text = "Seed White Flower";
                break;
            case InventoryItem.ITEM_TYPE.SEED_FLOWER_RED:
                text = "Seed Red Flower";
                break;
            case InventoryItem.ITEM_TYPE.GRASS2:
                text = "Grass 2";
                break;
            case InventoryItem.ITEM_TYPE.GRASS3:
                text = "Grass 3";
                break;
            case InventoryItem.ITEM_TYPE.GRASS4:
                text = "Grass 4";
                break;
            case InventoryItem.ITEM_TYPE.GRASS5:
                text = "Grass 5";
                break;
            case InventoryItem.ITEM_TYPE.FLOWER_PURPLE:
                text = "Flower_Purple";
                break;
            case InventoryItem.ITEM_TYPE.SEED_TREE:
                text = "Seed_tree";
                break;
            case InventoryItem.ITEM_TYPE.MAP:
                text = "FoundItemMap";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_MUSHROOMS:
                text = "Assets/Prefabs/Structures/Other/Meal Mushrooms.prefab";
                break;
            case InventoryItem.ITEM_TYPE.LOG_REFINED:
                text = "Log Refined";
                break;
            case InventoryItem.ITEM_TYPE.STONE_REFINED:
                text = "Stone Refined";
                break;
            case InventoryItem.ITEM_TYPE.GOLD_NUGGET:
                text = "Gold Nugget";
                break;
            case InventoryItem.ITEM_TYPE.ROPE:
                text = "Rope";
                break;
            case InventoryItem.ITEM_TYPE.GOLD_REFINED:
                text = "Gold Refined";
                break;
            case InventoryItem.ITEM_TYPE.BLOOD_STONE:
                text = "Bloodstone";
                break;
            case InventoryItem.ITEM_TYPE.TRINKET_CARD_UNLOCKED:
                text = "TarotCardUnlocked";
                break;
            case InventoryItem.ITEM_TYPE.CRYSTAL:
                text = "Crystal";
                break;
            case InventoryItem.ITEM_TYPE.SPIDER_WEB:
                text = "Spider Web";
                break;
            case InventoryItem.ITEM_TYPE.FISH_CRAB:
                text = "Fish Crab";
                break;
            case InventoryItem.ITEM_TYPE.FISH_LOBSTER:
                text = "Fish Lobster";
                break;
            case InventoryItem.ITEM_TYPE.FISH_OCTOPUS:
                text = "Fish Octopus";
                break;
            case InventoryItem.ITEM_TYPE.FISH_SQUID:
                text = "Fish Squid";
                break;
            case InventoryItem.ITEM_TYPE.FISH_SWORDFISH:
                text = "Fish Swordfish";
                break;
            case InventoryItem.ITEM_TYPE.FISH_BLOWFISH:
                text = "Fish Blowfish";
                break;
            case InventoryItem.ITEM_TYPE.BEETROOT:
                text = "Beetroot";
                break;
            case InventoryItem.ITEM_TYPE.SEED_BEETROOT:
                text = "Seed Beetroot";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
                text = "Assets/Prefabs/Structures/Other/Meal Great Fish.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_BAD_FISH:
                text = "Assets/Prefabs/Structures/Other/Meal Bad Fish.prefab";
                break;
            case InventoryItem.ITEM_TYPE.BEHOLDER_EYE:
                text = "Beholder Eye";
                break;
            case InventoryItem.ITEM_TYPE.CAULIFLOWER:
                text = "Cauliflower";
                break;
            case InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER:
                text = "Seed Cauliflower";
                break;
            case InventoryItem.ITEM_TYPE.MEAT_MORSEL:
                text = "Meat Morsel";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_BERRIES:
                text = "Assets/Prefabs/Structures/Other/Meal Berries.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG:
                text = "Assets/Prefabs/Structures/Other/Meal Medium Veg.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_BAD_MIXED:
                text = "Assets/Prefabs/Structures/Other/Meal Bad Mixed.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED:
                text = "Assets/Prefabs/Structures/Other/Meal Medium Mixed.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED:
                text = "Assets/Prefabs/Structures/Other/Meal Great Mixed.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_DEADLY:
                text = "Assets/Prefabs/Structures/Other/Meal Deadly.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_BAD_MEAT:
                text = "Assets/Prefabs/Structures/Other/Meal Bad Meat.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
                text = "Assets/Prefabs/Structures/Other/Meal Great Meat.prefab";
                break;
            case InventoryItem.ITEM_TYPE.MEAL_BURNED:
                text = "Assets/Prefabs/Structures/Other/Meal Burned.prefab";
                break;
            case InventoryItem.ITEM_TYPE.DOCTRINE_STONE:
                text = "Doctrine Stone Piece";
                break;
            case InventoryItem.ITEM_TYPE.SHELL:
                text = "Shell";
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
                text = "BlackGold";
                break;
        }

        return Resources.Load("Prefabs/Resources/" + text) as GameObject;
    }
}