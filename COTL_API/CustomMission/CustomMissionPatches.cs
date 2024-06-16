using HarmonyLib;
using src.UI.Menus;
using UnityEngine;
using UnityEngine.ProBuilder;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace COTL_API.CustomMission;

[HarmonyPatch]
public static partial class CustomMissionManager
{
    private static MissionButton? _newMissionButton;
    private static readonly List<MissionInstance> MissionInstanceList = [];

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MissionInfoCard), nameof(MissionInfoCard.Configure))]
    public static void MissionInfoCard_Configure(ref MissionInfoCard __instance, ref FollowerInfo config)
    {
        MissionInstanceList.RemoveAll(a => a.Button == null);

        foreach (var customMission in CustomMissionList.Select(x => x.Value))
        {
            var instance = __instance;
            var existing = MissionInstanceList.FindAll(a =>
                a.Instance == instance.GetInstanceID() && a.Mission == customMission);
            if (existing.Count > 0)
            {
                foreach (var mi in existing.Select(x => x.Button))
                {
                    _newMissionButton = mi;

                    if (mi == null) continue;

                    mi.Configure(config);
                    mi.Start();
                }

                continue;
            }

            var mission = __instance._missionButtons.RandomElement();

            _newMissionButton = Object.Instantiate(mission, mission.transform.parent);
            __instance._missionButtons.Add(_newMissionButton);

            _newMissionButton.name = "MISSION_BUTTON_" + customMission.InternalName;
            _newMissionButton._type = customMission.InnerType;

            _newMissionButton.Configure(config);
            _newMissionButton.Start();
            var card = __instance;
            _newMissionButton.OnMissionSelected += delegate(InventoryItem.ITEM_TYPE itemType)
            {
                var onMissionSelected = card.OnMissionSelected;
                if (onMissionSelected == null) return;

                onMissionSelected(itemType);
            };
            MissionInstanceList.Add(new MissionInstance(__instance.GetInstanceID(), _newMissionButton,
                customMission.InnerType, customMission));
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(FontImageNames), nameof(FontImageNames.GetIconByType), typeof(InventoryItem.ITEM_TYPE))]
    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.LocalizedName), typeof(InventoryItem.ITEM_TYPE))]
    public static void PrefixPatchesOne(ref InventoryItem.ITEM_TYPE Type)
    {
        if (CustomMissionList.TryGetValue(Type, out var value)) Type = value.RewardType;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetDurationDeterministic))]
    public static void MissionaryManager_GetDurationDeterministic(ref InventoryItem.ITEM_TYPE type)
    {
        if (CustomMissionList.TryGetValue(type, out var value)) type = value.RewardType;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.GetItemQuantity), typeof(InventoryItem.ITEM_TYPE))]
    public static void Inventory_GetItemQuantity(ref InventoryItem.ITEM_TYPE itemType)
    {
        if (CustomMissionList.TryGetValue(itemType, out var value)) itemType = value.RewardType;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetChance))]
    public static bool MissionaryManager_GetChance(ref float __result, InventoryItem.ITEM_TYPE type,
        FollowerInfo followerInfo, StructureBrain.TYPES missionaryType)
    {
        if (!CustomMissionList.TryGetValue(type, out var value)) return true;


        var baseChanceMultiplier =
            MissionaryManager.GetBaseChanceMultiplier(value.RewardType, followerInfo);
        var random = new Random((int)(followerInfo.ID + CustomMissionList[type].RewardType));

        __result = Mathf.Clamp(
            (CustomMissionList[type].BaseChance +
             random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f *
            baseChanceMultiplier, 0f, 0.95f);

        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetRewardRange))]
    public static bool MissionaryManager_GetRewardRange(ref IntRange __result, InventoryItem.ITEM_TYPE type)
    {
        if (!CustomMissionList.TryGetValue(type, out var value)) return true;

        __result = value.RewardRange;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MissionaryManager), nameof(MissionaryManager.GetReward))]
    public static bool GetReward(ref InventoryItem.ITEM_TYPE type, ref float chance, ref int followerID,
        ref InventoryItem[] __result)
    {
        if (!CustomMissionList.ContainsKey(type)) return true;

        var num = UnityEngine.Random.Range(0f, 1f);
        foreach (var objective in DataManager.Instance.CompletedObjectives)
        {
            if (objective.Follower != followerID) continue;
            chance = float.MaxValue;
            break;
        }

        if (chance > num)
            __result =
            [
                new InventoryItem(CustomMissionList[type].RewardType, CustomMissionList[type].RewardRange.Random())
            ];

        return false;
    }

    public class MissionInstance(
        int instance,
        MissionButton? button,
        InventoryItem.ITEM_TYPE type,
        CustomMission mission)
    {
        public int Instance { get; } = instance;
        public MissionButton? Button { get; } = button;
        private InventoryItem.ITEM_TYPE Type { get; } = type;
        public CustomMission Mission { get; } = mission;
    }
}