using System.Collections.Generic;
using HarmonyLib;
using src.UI.Menus;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace COTL_API.CustomMission;

[HarmonyPatch]
public static partial class CustomMissionManager
{
    public class MissionInstance
    {
        public int instance { get; }
        public MissionButton button { get; }
        private InventoryItem.ITEM_TYPE type { get; }
        public CustomMission mission { get; }
        public MissionInstance(int instance, MissionButton button, InventoryItem.ITEM_TYPE type, CustomMission mission)
        {
            this.instance = instance;
            this.button = button;
            this.type = type;
            this.mission = mission;
        }
    }

    private static MissionButton newMissionButton;
    private static readonly List<MissionInstance> missionInstances = new();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MissionInfoCard), nameof(MissionInfoCard.Configure))]
    public static void MissionInfoCard_Configure(ref MissionInfoCard __instance, ref FollowerInfo config)
    {
        missionInstances.RemoveAll(a => a.button == null);

        foreach (var customMission in CustomMissions)
        {
            var instance = __instance;
            var existing = missionInstances.FindAll(a => a.instance == instance.GetInstanceID() && a.mission == customMission.Value);
            if (existing.Count > 0)
            {
                foreach (var mi in existing)
                {
                    newMissionButton = mi.button;
                    mi.button.Configure(config);
                    mi.button.Start();
                }

                continue;
            }

            var mission = __instance._missionButtons.RandomElement();

            newMissionButton = Object.Instantiate(mission, mission.transform.parent);
            __instance._missionButtons.Add(newMissionButton);

            newMissionButton.name = "MISSION_BUTTON_" + customMission.Value.InternalName;
            newMissionButton._type = customMission.Value.InnerType;

            newMissionButton.Configure(config);
            newMissionButton.Start();
            var card = __instance;
            newMissionButton.OnMissionSelected += delegate(InventoryItem.ITEM_TYPE itemType)
            {
                var onMissionSelected = card.OnMissionSelected;
                if (onMissionSelected == null)
                {
                    return;
                }

                onMissionSelected(itemType);
            };
            missionInstances.Add(new MissionInstance(__instance.GetInstanceID(), newMissionButton, customMission.Value.InnerType, customMission.Value));
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(FontImageNames), nameof(FontImageNames.GetIconByType), typeof(InventoryItem.ITEM_TYPE))]
    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.LocalizedName), typeof(InventoryItem.ITEM_TYPE))]
    public static void PrefixPatchesOne(ref InventoryItem.ITEM_TYPE Type)
    {
        if (CustomMissions.ContainsKey(Type))
        {
            Type = CustomMissions[Type].RewardType;
        }
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetDurationDeterministic))]
    public static void MissionaryManager_GetDurationDeterministic(ref InventoryItem.ITEM_TYPE type)
    {
        if (CustomMissions.ContainsKey(type))
        {
            type = CustomMissions[type].RewardType;
        }
        
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.GetItemQuantity), typeof(InventoryItem.ITEM_TYPE))]
    public static void Inventory_GetItemQuantity(ref InventoryItem.ITEM_TYPE itemType)
    {
        if (CustomMissions.ContainsKey(itemType))
        {
            itemType = CustomMissions[itemType].RewardType;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetChance))]
    public static bool MissionaryManager_GetChance(ref float __result, InventoryItem.ITEM_TYPE type, FollowerInfo followerInfo, StructureBrain.TYPES missionaryType)
    {
        if (!CustomMissions.ContainsKey(type)) return true;

       

        var baseChanceMultiplier = MissionaryManager.GetBaseChanceMultiplier(CustomMissions[type].RewardType, followerInfo);
        var random = new System.Random((int) (followerInfo.ID + CustomMissions[type].RewardType));

        __result = Mathf.Clamp((CustomMissions[type].BaseChance + random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f * baseChanceMultiplier, 0f, 0.95f);
      
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetRewardRange))]
    public static bool MissionaryManager_GetRewardRange(ref IntRange __result, InventoryItem.ITEM_TYPE type)
    {
        if (!CustomMissions.ContainsKey(type)) return true;

        __result = CustomMissions[type].RewardRange;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetReward))]
    public static bool GetReward(ref InventoryItem.ITEM_TYPE type, ref float chance, ref int followerID, ref InventoryItem[] __result)
    {
        if (!CustomMissions.ContainsKey(type)) return true;
        
        var num = Random.Range(0f, 1f);
        foreach (var objective in DataManager.Instance.CompletedObjectives)
        {
            if (objective.Follower != followerID) continue;
            chance = float.MaxValue;
            break;
        }

        if (chance > num)
        {
            __result = new[] {new InventoryItem(CustomMissions[type].RewardType, CustomMissions[type].RewardRange.Random())};
        }

        return false;
    }
}