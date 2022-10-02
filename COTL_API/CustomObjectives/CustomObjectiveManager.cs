using COTL_API.Saves;
using MonoMod.Utils;
using System.Collections.Generic;

namespace COTL_API.CustomObjectives;

/// <summary>
/// The custom objective manager class.
/// </summary>
public static class CustomObjectiveManager
{
    /// <summary>
    /// The group id
    /// </summary>
    private const string GroupId = "Objectives/GroupTitles/Quest";

    private const string DataPath = "cotl_api_custom_quest_data.json";
    private static readonly COTLDataReadWriter<Dictionary<int, CustomObjective>> CustomQuestDataReadWriter = new();

    static CustomObjectiveManager()
    {
        ModdedSaveManager.OnLoadComplete += LoadData;
        ModdedSaveManager.OnSaveComplete += SaveData;
    }

    private static void LoadData()
    {
        CustomQuestDataReadWriter.OnReadCompleted += delegate(Dictionary<int, CustomObjective> objectives)
        {
            Dictionary<int, CustomObjective> tempObjectives = new();
            foreach (KeyValuePair<int, CustomObjective> objective in objectives)
            {
                if (DataManager.instance.Objectives.Exists(a => a.ID == objective.Key))
                {
                    tempObjectives.Add(objective.Key, objective.Value);
                }
                else if (Quests.QuestsAll.Exists(a => a.ID == objective.Key))
                {
                    tempObjectives.Add(objective.Key, objective.Value);
                }
            }

            PluginQuestTracker.AddRange(tempObjectives);
            Plugin.Logger.LogWarning(tempObjectives.Count > 0 ? $"Needed previous session custom quests loaded. Count: {tempObjectives.Count}" : "None of the previous session quests still exist in objective trackers.");
        };

        CustomQuestDataReadWriter.OnReadError += delegate
        {
            Plugin.Logger.LogWarning("Previous session custom quests failed to load!");
        };

        CustomQuestDataReadWriter.Read(DataPath);
    }


    private static void SaveData()
    {
        CustomQuestDataReadWriter.OnWriteCompleted += delegate
        {
            Plugin.Logger.LogWarning($"Backed up {PluginQuestTracker.Count} custom QuestID's!");
        };

        CustomQuestDataReadWriter.OnWriteError += delegate(MMReadWriteError error)
        {
            Plugin.Logger.LogWarning($"There was an issue backing up current QuestID's!: {error.Message}");
        };

        CustomQuestDataReadWriter.Write(PluginQuestTracker, DataPath, true, false);
    }

    private static string DefaultQuestText => "I didn't set a custom quest text for this objective!";

    /// <summary>
    /// Holds the list of instantiated custom objectives.
    /// </summary>
    internal static Dictionary<int, CustomObjective> PluginQuestTracker { get; } = new();


    ///  <param name="followerName">The name of the follower.</param>
    ///  <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
    public static CustomObjective BedRest(string followerName)
    {
        Objectives_BedRest q = new(GroupId, followerName);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }

    /// <param name="structureType">The type of structure to build.</param>
    /// <param name="target">The target amount to complete the objective.</param>
    /// <param name="expireTimestamp">How long the player has to complete the objective.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective BuildStructure(StructureBrain.TYPES structureType, int target = 1, float expireTimestamp = -1f)
    {
        Objectives_BuildStructure q = new(GroupId, structureType, target, expireTimestamp);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <param name="itemType">The target item to collect.</param>
    /// <param name="target">The target amount to complete the objective.</param>
    /// <param name="targetIsTotal">Include items you already have or start count from 0.</param>
    /// <param name="targetLocation">The location the objective takes place. i.e. Dungeon1_1</param>
    /// <param name="expireTimestamp">How long the player has to complete the objective.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective CollectItem(InventoryItem.ITEM_TYPE itemType, int target, bool targetIsTotal = true, FollowerLocation targetLocation = FollowerLocation.Base, float expireTimestamp = -1f)
    {
        Objectives_CollectItem q = new(GroupId, itemType, target, targetIsTotal, targetLocation, expireTimestamp);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <param name="mealType">The type of meal that is to be cooked to complete the objective.</param>
    /// <param name="count">How many required to complete the objective.</param>
    /// <param name="expireTimestamp">How long the player has to complete the objective.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective CookMeal(InventoryItem.ITEM_TYPE mealType, int count, float expireTimestamp = -1f)
    {
        Objectives_CookMeal q = new(GroupId, mealType, count, expireTimestamp);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <param name="customQuestType">The type of custom quest.</param>
    /// <param name="targetFollowerID">The target followers ID.</param>
    /// <param name="questExpireDuration">How long the player has to complete the objective.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective Custom(Objectives.CustomQuestTypes customQuestType, int targetFollowerID = -1, float questExpireDuration = -1f)
    {
        Objectives_Custom q = new(GroupId, customQuestType, targetFollowerID, questExpireDuration);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <param name="characterNameTerm"></param>
    /// <param name="expireTimestamp">How long the player has to complete the objective.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective DefeatKnucklebones(string characterNameTerm, float expireTimestamp = -1f)
    {
        Objectives_DefeatKnucklebones q = new(GroupId, characterNameTerm, expireTimestamp);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective DepositFood()
    {
        Objectives_DepositFood q = new(GroupId);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <param name="mealType">The type of meal that is to be eaten to complete the objective.</param>
    /// <param name="questExpireDuration">How long the user has to complete.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective EatMeal(StructureBrain.TYPES mealType, float questExpireDuration = -1f)
    {
        Objectives_EatMeal q = new(GroupId, mealType, questExpireDuration);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <param name="targetLocation">The location that the objective takes place. i.e. Dungeon1_1</param>
    /// <param name="followerSkin">Specify what skin the follower has.</param>
    /// <param name="followerColour">Specify what colour the follower is.</param>
    /// <param name="followerVariant">Specify what variant of the follower.</param>
    /// <param name="targetFollowerName">The target followers name.</param>
    /// <param name="objectiveVariant"></param>
    /// <param name="expireTimestamp">How long the player has to complete the objective.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective FindFollower(FollowerLocation targetLocation, string followerSkin, int followerColour, int followerVariant, string targetFollowerName, int objectiveVariant, float expireTimestamp = -1f)
    {
        Objectives_FindFollower q = new(GroupId, targetLocation, followerSkin, followerColour, followerVariant, targetFollowerName, objectiveVariant, expireTimestamp);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <param name="enemyType">The target enemy.</param>
    /// <param name="killsRequired">Kills required to complete.</param>
    /// <param name="questDuration">How long the user has to complete.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective KillEnemies(Enemy enemyType, int killsRequired, float questDuration)
    {
        Objectives_KillEnemies q = new(GroupId, enemyType, killsRequired, questDuration);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <param name="roomsRequired">How many rooms required with no curses to complete the objective.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective NoCurses(int roomsRequired)
    {
        Objectives_NoCurses q = new(GroupId, roomsRequired);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <param name="roomsRequired">How many rooms required with no damage to complete the objective.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective NoDamage(int roomsRequired)
    {
        Objectives_NoDamage q = new(GroupId, roomsRequired);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <param name="roomsRequired">How many rooms required with no dodging to complete the objective.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective NoDodge(int roomsRequired)
    {
        Objectives_NoDodge q = new(GroupId, roomsRequired);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <param name="roomsRequired">How many rooms required with no healing to complete the objective.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective NoHealing(int roomsRequired)
    {
        Objectives_NoHealing q = new(GroupId, roomsRequired);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <param name="ritual">The type of ritual to perform.</param>
    /// <param name="targetFollowerID">The target follower ID.</param>
    /// <param name="requiredFollowers">How many followers are required.</param>
    /// <param name="questExpireDuration">How long the player has to complete the ritual.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective PerformRitual(UpgradeSystem.Type ritual, int targetFollowerID = 1, int requiredFollowers = 0, float questExpireDuration = -1f)
    {
        Objectives_PerformRitual q = new(GroupId, ritual, targetFollowerID, requiredFollowers, questExpireDuration);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <param name="category">The category of structure to place.</param>
    /// <param name="target">The target amount to complete the objective.</param>
    /// <param name="expireDuration">How long the player has to complete the objective.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective PlaceStructure(StructureBrain.Categories category, int target, float expireDuration)
    {
        Objectives_PlaceStructure q = new(GroupId, category, target, expireDuration);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <param name="cursedState">Set the curse the follower has.</param>
    /// <param name="target">The target amount to complete the objective.</param>
    /// <param name="expireTimestamp">How long the player has to complete the objective.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective RecruitCursedFollower(Thought cursedState, int target = 1, float expireTimestamp = -1f)
    {
        Objectives_RecruitCursedFollower q = new(GroupId, cursedState, target, expireTimestamp);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <param name="count">How many required to complete the objective.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective RecruitFollower(int count = 1)
    {
        Objectives_RecruitFollower q = new(GroupId, count);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <param name="structureType">The type of structure to remove.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective RemoveStructure(StructureBrain.TYPES structureType)
    {
        Objectives_RemoveStructure q = new(GroupId, structureType);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }


    /// <summary>
    /// ShootDummy
    /// </summary>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective ShootDummy()
    {
        Objectives_ShootDummy q = new(GroupId);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }

    /// <param name="term"></param>
    /// <param name="expireTimestamp">How long the player has to complete the objective.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective TalkToFollower(string term = "", float expireTimestamp = -1f)
    {
        Objectives_TalkToFollower q = new(GroupId, term, expireTimestamp);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }

    /// <param name="unlockType">The type of upgrade to unlock.</param>
    /// <returns>Returns a CustomObjective object.</returns>
    public static CustomObjective UnlockUpgrade(UpgradeSystem.Type unlockType)
    {
        Objectives_UnlockUpgrade q = new(GroupId, unlockType);
        return WorkMagic(q.ID, DefaultQuestText, q);
    }

    private static CustomObjective WorkMagic(int id, string text, ObjectivesData objectiveData)
    {
        CustomObjective customObjective = new(id, text, objectiveData);
        PluginQuestTracker.Add(id, customObjective);
        Quests.QuestsAll.Add(objectiveData);
        return customObjective;
    }
}