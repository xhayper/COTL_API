using COTL_API.Guid;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace COTL_API.CustomObjectives;

/// <summary>
/// The custom objective manager class.
/// </summary>
public static class CustomObjectiveManager
{
    /// <summary>
    /// Holds the list of custom objectives.
    /// </summary>
    internal static Dictionary<Objectives.CustomQuestTypes, CustomObjective> CustomObjectives { get; } = new();

    /// <summary>
    /// Holds the list of instantiated custom objectives.
    /// </summary>
    private static Dictionary<Objectives.CustomQuestTypes, ObjectivesData> TrackedObjectives { get; } = new();

    /// <summary>
    /// The group id
    /// </summary>
    private const string GroupId = "Objectives/GroupTitles/Quest";

    /// <summary>
    /// Used to assign a unique id (ObjectiveKey) to each custom objective, and add the objective to the game.
    /// </summary>
    /// <param name="customObjective">The custom objective created by the plugins.</param>
    /// <returns>A tuple containing ObjectiveKey and ObjectiveData so they can be easily accessed by the plugin.</returns>
    public static (Objectives.CustomQuestTypes ObjectiveKey, ObjectivesData ObjectiveData) Add(CustomObjective customObjective)
    {
        string guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());
        Objectives.CustomQuestTypes objectiveKey = GuidManager.GetEnumValue<Objectives.CustomQuestTypes>(guid, customObjective.InternalName());
        customObjective.ObjectiveKey = objectiveKey;
        GuidManager.GetEnumValue<Follower.ComplaintType>(guid, customObjective.InternalName());

        CustomObjectives.Add(objectiveKey, customObjective);

        Plugin.Logger.LogWarning($"Added custom objective with TypeID {objectiveKey} and Objective ID: {customObjective.ObjectiveData.ID}. Total custom objectives: {CustomObjectives.Count}.");

        AddQuests();

        return (objectiveKey, customObjective.ObjectiveData);
    }

    /// <summary>
    /// Adds the custom objectives to the game.
    /// </summary>
    private static void AddQuests()
    {
        foreach (KeyValuePair<Objectives.CustomQuestTypes, CustomObjective> quest in CustomObjectives.Where(quest => !Quests.QuestsAll.Contains(quest.Value.ObjectiveData)))
        {
            Quests.QuestsAll.Add(quest.Value.ObjectiveData);

            Plugin.Logger.LogWarning($"Added {quest.Key} custom objectives to QuestsAll. Objective ID: {quest.Value.ObjectiveData.ID}");
        }
    }

    /// <summary>
    /// Houses the data for the custom objectives. Due to the quest ID being randomly generated by the game, and our only way to track, we need to ensure we're returning the original instance of the quest.
    /// </summary>
    public static class Objective
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="followerName">The name of the follower.</param>
        ///<returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_BedRest BedRest(Objectives.CustomQuestTypes objectiveKey, string followerName)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_BedRest;
            Objectives_BedRest q = new(GroupId, followerName);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="structureType">The type of structure to build.</param>
        /// <param name="target">The target amount to complete the objective.</param>
        /// <param name="expireTimestamp">How long the player has to complete the objective.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_BuildStructure BuildStructure(Objectives.CustomQuestTypes objectiveKey, StructureBrain.TYPES structureType, int target = 1, float expireTimestamp = -1f)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_BuildStructure;
            Objectives_BuildStructure q = new(GroupId, structureType, target, expireTimestamp);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="itemType">The target item to collect.</param>
        /// <param name="target">The target amount to complete the objective.</param>
        /// <param name="targetIsTotal">Include items you already have or start count from 0.</param>
        /// <param name="targetLocation">The location the objective takes place. i.e. Dungeon1_1</param>
        /// <param name="expireTimestamp">How long the player has to complete the objective.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_CollectItem CollectItem(Objectives.CustomQuestTypes objectiveKey, InventoryItem.ITEM_TYPE itemType, int target, bool targetIsTotal = true, FollowerLocation targetLocation = FollowerLocation.Base, float expireTimestamp = -1f)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_CollectItem;
            Objectives_CollectItem q = new(GroupId, itemType, target, targetIsTotal, targetLocation, expireTimestamp);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="mealType">The type of meal that is to be cooked to complete the objective.</param>
        /// <param name="count">How many required to complete the objective.</param>
        /// <param name="expireTimestamp">How long the player has to complete the objective.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_CookMeal CookMeal(Objectives.CustomQuestTypes objectiveKey, InventoryItem.ITEM_TYPE mealType, int count, float expireTimestamp = -1f)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_CookMeal;
            Objectives_CookMeal q = new(GroupId, mealType, count, expireTimestamp);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="customQuestType">The type of custom quest.</param>
        /// <param name="targetFollowerID">The target followers ID.</param>
        /// <param name="questExpireDuration">How long the player has to complete the objective.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_Custom Custom(Objectives.CustomQuestTypes objectiveKey, Objectives.CustomQuestTypes customQuestType, int targetFollowerID = -1, float questExpireDuration = -1f)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_Custom;
            Objectives_Custom q = new(GroupId, customQuestType, targetFollowerID, questExpireDuration);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="characterNameTerm"></param>
        /// <param name="expireTimestamp">How long the player has to complete the objective.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_DefeatKnucklebones DefeatKnucklebones(Objectives.CustomQuestTypes objectiveKey, string characterNameTerm, float expireTimestamp = -1f)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_DefeatKnucklebones;
            Objectives_DefeatKnucklebones q = new(GroupId, characterNameTerm, expireTimestamp);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_DepositFood DepositFood(Objectives.CustomQuestTypes objectiveKey)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_DepositFood;
            Objectives_DepositFood q = new(GroupId);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="mealType">The type of meal that is to be eaten to complete the objective.</param>
        /// <param name="questExpireDuration">How long the user has to complete.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_EatMeal EatMeal(Objectives.CustomQuestTypes objectiveKey, StructureBrain.TYPES mealType, float questExpireDuration = -1f)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_EatMeal;
            Objectives_EatMeal q = new(GroupId, mealType, questExpireDuration);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="targetLocation">The location that the objective takes place. i.e. Dungeon1_1</param>
        /// <param name="followerSkin">Specify what skin the follower has.</param>
        /// <param name="followerColour">Specify what colour the follower is.</param>
        /// <param name="followerVariant">Specify what variant of the follower.</param>
        /// <param name="targetFollowerName">The target followers name.</param>
        /// <param name="objectiveVariant"></param>
        /// <param name="expireTimestamp">How long the player has to complete the objective.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_FindFollower FindFollower(Objectives.CustomQuestTypes objectiveKey, FollowerLocation targetLocation, string followerSkin, int followerColour, int followerVariant, string targetFollowerName, int objectiveVariant, float expireTimestamp = -1f)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_FindFollower;
            Objectives_FindFollower q = new(GroupId, targetLocation, followerSkin, followerColour, followerVariant, targetFollowerName, objectiveVariant, expireTimestamp);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="enemyType">The target enemy.</param>
        /// <param name="killsRequired">Kills required to complete.</param>
        /// <param name="questDuration">How long the user has to complete.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_KillEnemies KillEnemies(Objectives.CustomQuestTypes objectiveKey, Enemy enemyType, int killsRequired, float questDuration)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_KillEnemies;
            Objectives_KillEnemies q = new(GroupId, enemyType, killsRequired, questDuration);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="roomsRequired">How many rooms required with no curses to complete the objective.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_NoCurses NoCurses(Objectives.CustomQuestTypes objectiveKey, int roomsRequired)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_NoCurses;
            Objectives_NoCurses q = new(GroupId, roomsRequired);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="roomsRequired">How many rooms required with no damage to complete the objective.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_NoDamage NoDamage(Objectives.CustomQuestTypes objectiveKey, int roomsRequired)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_NoDamage;
            Objectives_NoDamage q = new(GroupId, roomsRequired);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="roomsRequired">How many rooms required with no dodging to complete the objective.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_NoDodge NoDodge(Objectives.CustomQuestTypes objectiveKey, int roomsRequired)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_NoDodge;
            Objectives_NoDodge q = new(GroupId, roomsRequired);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="roomsRequired">How many rooms required with no healing to complete the objective.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_NoHealing NoHealing(Objectives.CustomQuestTypes objectiveKey, int roomsRequired)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_NoHealing;
            Objectives_NoHealing q = new(GroupId, roomsRequired);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="ritual">The type of ritual to perform.</param>
        /// <param name="targetFollowerID">The target follower ID.</param>
        /// <param name="requiredFollowers">How many followers are required.</param>
        /// <param name="questExpireDuration">How long the player has to complete the ritual.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_PerformRitual PerformRitual(Objectives.CustomQuestTypes objectiveKey, UpgradeSystem.Type ritual, int targetFollowerID = 1, int requiredFollowers = 0, float questExpireDuration = -1f)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_PerformRitual;
            Objectives_PerformRitual q = new(GroupId, ritual, targetFollowerID, requiredFollowers, questExpireDuration);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="category">The category of structure to place.</param>
        /// <param name="target">The target amount to complete the objective.</param>
        /// <param name="expireDuration">How long the player has to complete the objective.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_PlaceStructure PlaceStructure(Objectives.CustomQuestTypes objectiveKey, StructureBrain.Categories category, int target, float expireDuration)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_PlaceStructure;
            Objectives_PlaceStructure q = new(GroupId, category, target, expireDuration);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="cursedState">Set the curse the follower has.</param>
        /// <param name="target">The target amount to complete the objective.</param>
        /// <param name="expireTimestamp">How long the player has to complete the objective.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_RecruitCursedFollower RecruitCursedFollower(Objectives.CustomQuestTypes objectiveKey, Thought cursedState, int target = 1, float expireTimestamp = -1f)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_RecruitCursedFollower;
            Objectives_RecruitCursedFollower q = new(GroupId, cursedState, target, expireTimestamp);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="count">How many required to complete the objective.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_RecruitFollower RecruitFollower(Objectives.CustomQuestTypes objectiveKey, int count = 1)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_RecruitFollower;
            Objectives_RecruitFollower q = new(GroupId, count);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="structureType">The type of structure to remove.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_RemoveStructure RemoveStructure(Objectives.CustomQuestTypes objectiveKey, StructureBrain.TYPES structureType)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_RemoveStructure;
            Objectives_RemoveStructure q = new(GroupId, structureType);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_ShootDummy ShootDummy(Objectives.CustomQuestTypes objectiveKey)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_ShootDummy;
            Objectives_ShootDummy q = new(GroupId);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="term"></param>
        /// <param name="expireTimestamp">How long the player has to complete the objective.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_TalkToFollower TalkToFollower(Objectives.CustomQuestTypes objectiveKey, string term = "", float expireTimestamp = -1f)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_TalkToFollower;
            Objectives_TalkToFollower q = new(GroupId, term, expireTimestamp);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectiveKey">Objectives unique key.</param>
        /// <param name="unlockType">The type of upgrade to unlock.</param>
        /// <returns>The original instance of the objective if it exists, otherwise returns a new instance.</returns>
        public static Objectives_UnlockUpgrade UnlockUpgrade(Objectives.CustomQuestTypes objectiveKey, UpgradeSystem.Type unlockType)
        {
            if (TrackedObjectives.TryGetValue(objectiveKey, out ObjectivesData objectiveData)) return objectiveData as Objectives_UnlockUpgrade;
            Objectives_UnlockUpgrade q = new(GroupId, unlockType);
            TrackedObjectives.Add(objectiveKey, q);
            return q;
        }
    }
}