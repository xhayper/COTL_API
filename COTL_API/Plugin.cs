using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using COTL_API.CustomFollowerCommand;
using COTL_API.CustomInventory;
using COTL_API.CustomObjectives;
using COTL_API.CustomSettings;
using COTL_API.CustomSkins;
using COTL_API.CustomStructures;
using COTL_API.CustomTarotCard;
using COTL_API.CustomTasks;
using COTL_API.Debug;
using COTL_API.Helpers;
using COTL_API.Saves;
using COTL_API.UI;
using HarmonyLib;
using I2.Loc;
using MonoMod.Utils;
using Spine;

namespace COTL_API;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
// [BepInProcess("Cult Of The Lamb.exe")] // To be decided
[HarmonyPatch]
public class Plugin : BaseUnityPlugin
{
    private readonly Harmony _harmony = new(MyPluginInfo.PLUGIN_GUID);

    internal readonly ModdedSaveData<ApiData> APIData = new(MyPluginInfo.PLUGIN_GUID)
    {
        LoadOrder = ModdedSaveLoadOrder.LOAD_AS_SOON_AS_POSSIBLE
    };

    internal readonly ModdedSaveData<ApiSlotData> APISlotData = new($"{MyPluginInfo.PLUGIN_GUID}_slot");

    public readonly ModdedSaveData<ObjectDictionary> ModdedSettingsData = new("modded_settings")
    {
        LoadOrder = ModdedSaveLoadOrder.LOAD_AS_SOON_AS_POSSIBLE
    };

    internal bool DebugContentAdded;
    internal static Plugin? Instance { get; private set; }

    internal new ManualLogSource Logger { get; private set; } = new(MyPluginInfo.PLUGIN_NAME);

    internal string PluginPath { get; private set; } = "";

    internal InventoryItem.ITEM_TYPE DebugItem { get; private set; }
    internal InventoryItem.ITEM_TYPE DebugItem2 { get; private set; }
    internal InventoryItem.ITEM_TYPE DebugItem3 { get; private set; }
    internal InventoryItem.ITEM_TYPE DebugItem4 { get; private set; }

    internal FollowerCommands DebugGiftFollowerCommand { get; private set; }

    private ConfigEntry<bool>? _debug { get; set; }
    public bool Debug => _debug?.Value ?? false;
    internal static bool Started { get; private set; }

    internal static ObjectDictionary? SettingsData => Instance != null ? Instance.ModdedSettingsData.Data : null;

    internal static Dictionary<int, CustomObjective>? QuestData =>
        Instance != null ? Instance.APISlotData.Data?.QuestData : null;

    internal static ObjectDictionary? EnumData => Instance != null ? Instance.APIData.Data?.EnumData : null;

    private void Awake()
    {
        Instance = this;
        Logger = base.Logger;

        PluginPath = Path.GetDirectoryName(Info.Location) ?? string.Empty;
        _debug = Config.Bind("misc", "debug", false, "Should debug mode be enabled?");

        ModdedSaveManager.RegisterModdedSave(ModdedSettingsData);
        ModdedSaveManager.RegisterModdedSave(APIData);
        ModdedSaveManager.RegisterModdedSave(APISlotData);

        RunSavePatch();

        Skin S1()
        {
            return PlayerFarming.Instance.Spine.Skeleton.Data.FindSkin("Goat");
        }

        Skin S2()
        {
            return PlayerFarming.Instance.Spine.Skeleton.Data.FindSkin("Owl");
        }

        Skin S3()
        {
            return PlayerFarming.Instance.Spine.Skeleton.Data.FindSkin("Snake");
        }

        CustomSkinManager.AddPlayerSkin(new OverridingPlayerSkin("Goat", S1));
        CustomSkinManager.AddPlayerSkin(new OverridingPlayerSkin("Owl", S2));
        CustomSkinManager.AddPlayerSkin(new OverridingPlayerSkin("Snake", S3));

        var dd = CustomSettingsManager.AddSavedDropdown("API", MyPluginInfo.PLUGIN_GUID, "Lamb Skin", "Default",
            new[] { "Default" }.Concat(CustomSkinManager.CustomPlayerSkins.Keys).ToArray(), i =>
            {
                if (0 >= i)
                    CustomSkinManager.ResetPlayerSkin();
                else
                    CustomSkinManager.SetPlayerSkinOverride(
                        CustomSkinManager.CustomPlayerSkins.Values.ElementAt(i - 1));
            });

        // TODO: TEMP WORKAROUND, PATCH ON SAVE SLOT SELECTED INSTEAD! (IDK WHAT METHOD IT IS)
        if (dd?.Value != null && dd.Value != "Default")
        {
            if (CustomSkinManager.CustomPlayerSkins.TryGetValue(dd.Value, out var skin))
                CustomSkinManager.SetPlayerSkinOverride(skin);
            else
                dd.Value = "Default";
        }

        CustomSettingsManager.AddBepInExConfig("API", "Debug", _debug, delegate(bool isActivated)
        {
            if (!isActivated)
            {
                if (dd?.Value != "Debug Skin") return;
                dd.Value = "Default";
                CustomSkinManager.ResetPlayerSkin();
            }
            else
            {
                if (DebugContentAdded) return;
                AddDebugContent();
            }
        });

        if (Debug)
            AddDebugContent();

        UIManager.OnSettingsLoaded += () =>
        {
            if (dd != null) dd.Options = new[] { "Default" }.Concat(CustomSkinManager.CustomPlayerSkins.Keys).ToArray();
        };

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} loaded!");
    }

    private void Start()
    {
        OnStart.Invoke();
        Started = true;
    }

    private void OnEnable()
    {
        _harmony.PatchAll(Assembly.GetExecutingAssembly());
        Logger.LogInfo($"{Harmony.GetAllPatchedMethods().Count()} harmony patches applied!");
    }

    private void OnDisable()
    {
        _harmony.UnpatchSelf();
        Logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} unloaded!");
    }

    internal static event Action OnStart = delegate { };

    private void RunSavePatch()
    {
        // LOAD_AFTER_START handler
        SaveAndLoad.OnLoadComplete += delegate
        {
            Logger.LogWarning("Loading Modded Save Data with LoadOrder=ModdedSaveLoadOrder.LOAD_AFTER_SAVE_START.");
            foreach (var saveData in ModdedSaveManager.ModdedSaveDataList.Values.Where(save =>
                         save.LoadOrder == ModdedSaveLoadOrder.LOAD_AFTER_SAVE_START))
                saveData.Load(SaveAndLoad.SAVE_SLOT);

            Logger.LogWarning("Re-adding any custom quests from the players existing objectives.");
            Dictionary<int, CustomObjective> tempObjectives = new();

            if (QuestData == null) return;

            foreach (var objective in QuestData)
                if (DataManager.instance.Objectives.Exists(a => a.ID == objective.Key))
                    tempObjectives.Add(objective.Key, objective.Value);
                else if (Quests.QuestsAll.Exists(a => a.ID == objective.Key))
                    tempObjectives.Add(objective.Key, objective.Value);

            CustomObjectiveManager.CustomObjectiveList.AddRange(tempObjectives);

            Logger.LogWarning("Added custom quests to Plugin.Instance.APIQuestData.Data.QuestData.");
            foreach (var quest in CustomObjectiveManager.CustomObjectiveList) QuestData.TryAdd(quest.Key, quest.Value);
        };

        // This will reset the language to "English" if it can't find specified language.
        SettingsManager.Instance._readWriter.OnReadCompleted += delegate
        {
            if (LocalizationManager.HasLanguage(SettingsManager.Settings.Game.Language)) return;

            SettingsManager.Settings.Game.Language = "English";
            LocalizationManager.CurrentLanguage = "English";

            if (TwitchAuthentication.IsAuthenticated)
                TwitchRequest.SendEBSData();

            LocalizationManager.LocalizeAll(true);
        };


        // Cleans the users QuestHistory indexes to prevent issues when they remove/disable mods that add custom quests.
        // The index stored inside the QuestHistoryData is based on the static Quests.QuestAll list count, which gets changed when we add/remove quests.
        // This fix stops Index out of bound errors when accepting a new quest, and keeps the users history intact.
        SaveAndLoad.OnLoadComplete += delegate
        {
            if (DataManager.Instance == null) return;

            foreach (var quest in DataManager.Instance.CompletedQuestsHistorys.Where(a =>
                         a.QuestIndex >= Quests.QuestsAll.Count))
            {
                if (Debug)
                    Logger.LogDebug(
                        "Found quests in history with an index higher than total quests (user may have removed mods that add quests), resetting to maximum possible.");
                quest.QuestIndex = Quests.QuestsAll.Count - 1;
            }
        };
    }

    private void AddDebugContent()
    {
        if (DebugContentAdded) return;

        CustomSkinManager.AddFollowerSkin(new DebugFollowerSkin());
        CustomSkinManager.AddPlayerSkin(new DebugPlayerSkin());

        CustomFollowerCommandManager.Add(new DebugFollowerCommand());
        CustomFollowerCommandManager.Add(new DebugFollowerCommandClass2());
        CustomFollowerCommandManager.Add(new DebugFollowerCommandClass3());
        DebugGiftFollowerCommand = CustomFollowerCommandManager.Add(new DebugGiftFollowerCommand());

        DebugItem = CustomItemManager.Add(new DebugItemClass());
        DebugItem2 = CustomItemManager.Add(new DebugItemClass2());
        DebugItem3 = CustomItemManager.Add(new DebugItemClass3());
        DebugItem4 = CustomItemManager.Add(new DebugItemClass4());

        CustomStructureManager.Add(new DebugStructure());
        CustomStructureManager.Add(new DebugStructure2());
        CustomStructureManager.Add(new DebugStructure3());

        CustomTarotCardManager.Add(new DebugTarotCard());

        CustomTaskManager.Add(new DebugTask());

        var test = CustomObjectiveManager.BedRest("Test");
        test.InitialQuestText = "This is my custom quest text for this objective.";

        Logger.LogDebug("Debug mode enabled!");

        DebugContentAdded = true;
    }
}