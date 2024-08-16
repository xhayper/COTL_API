using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using COTL_API.CustomFollowerCommand;
using COTL_API.CustomInventory;
using COTL_API.CustomLocalization;
using COTL_API.CustomObjectives;
using COTL_API.CustomRelics;
using COTL_API.CustomSettings;
using COTL_API.CustomSettings.Elements;
using COTL_API.CustomSkins;
using COTL_API.CustomStructures;
using COTL_API.CustomTarotCard;
using COTL_API.CustomTasks;
using COTL_API.Debug;
using COTL_API.Saves;
using HarmonyLib;
using I2.Loc;
using Lamb.UI;
using MonoMod.Utils;
using Spine;
using UnityEngine;

namespace COTL_API;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
// [BepInProcess("Cult Of The Lamb.exe")] // To be decided
[HarmonyPatch]
public class Plugin : BaseUnityPlugin
{
    internal static Dropdown? SkinP1Settings;
    internal static Dropdown? SkinP2Settings;

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
    internal static ConfigEntry<bool> UnityDebug { get; private set; } = null!;
    internal static Plugin? Instance { get; private set; }

    internal new ManualLogSource Logger { get; private set; } = new(MyPluginInfo.PLUGIN_NAME);

    internal string PluginPath { get; private set; } = "";

    private ConfigEntry<bool>? _debug { get; set; }
    public bool Debug => _debug?.Value ?? false;

    private ConfigEntry<bool>? _skipSplashScreen { get; set; }
    public bool SkipSplashScreen => _skipSplashScreen?.Value ?? false;

    private ConfigEntry<bool>? _disableAchievement { get; set; }
    public bool DisableAchievement => _disableAchievement?.Value ?? true;

    internal static bool Started { get; private set; }

    internal static ObjectDictionary? SettingsData => Instance != null ? Instance.ModdedSettingsData.Data : null;

    internal static Dictionary<int, CustomObjective>? QuestData =>
        Instance != null ? Instance.APISlotData.Data?.QuestData : null;

    internal static ObjectDictionary? EnumData => Instance != null ? Instance.APIData.Data?.EnumData : null;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    internal InventoryItem.ITEM_TYPE DebugItem { get; private set; }
    internal InventoryItem.ITEM_TYPE DebugItem2 { get; private set; }
    internal InventoryItem.ITEM_TYPE DebugItem3 { get; private set; }
    internal InventoryItem.ITEM_TYPE DebugItem4 { get; private set; }

    internal FollowerCommands DebugGiftFollowerCommand { get; private set; }

    internal RelicType DebugRelic { get; private set; }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Awake()
    {
        Instance = this;
        Logger = base.Logger;

        PluginPath = Path.GetDirectoryName(Info.Location) ?? string.Empty;

        ModdedSaveManager.RegisterModdedSave(ModdedSettingsData);
        ModdedSaveManager.RegisterModdedSave(APIData);
        ModdedSaveManager.RegisterModdedSave(APISlotData);

        RunSavePatch();

        _skipSplashScreen = Config.Bind("Miscellaneous", "Skip Splash Screen", false,
            "Should we skip the splash screen or not?");
        _disableAchievement = Config.Bind("Miscellaneous", "Disable Achievement", true,
            "Should we disable the achievement system? (You will still be able to get achievement but it won't save)");

        _debug = Config.Bind("Debug", "API Debug", false,
            "API debug mode. Will add debug content to your game for testing. Not recommended for normal play.");
        UnityDebug = Config.Bind("Debug", "Unity Debug Logging", true,
            "Unity debug logging. Helpful to filter out unrelated entries during testing.");

        UnityDebug.SettingChanged += (_, _) => { UnityEngine.Debug.unityLogger.logEnabled = UnityDebug.Value; };

        Skin S1()
        {
            return PlayerFarming.Instance.Spine.Skeleton.Data.FindSkin("Lamb");
        }

        Skin S2()
        {
            return PlayerFarming.Instance.Spine.Skeleton.Data.FindSkin("Goat");
        }

        Skin S3()
        {
            return PlayerFarming.Instance.Spine.Skeleton.Data.FindSkin("Owl");
        }

        Skin S4()
        {
            return PlayerFarming.Instance.Spine.Skeleton.Data.FindSkin("Snake");
        }

        CustomSkinManager.AddPlayerSkin(new OverridingPlayerSkin("Lamb", S1));
        CustomSkinManager.AddPlayerSkin(new OverridingPlayerSkin("Goat", S2));
        CustomSkinManager.AddPlayerSkin(new OverridingPlayerSkin("Owl", S3));
        CustomSkinManager.AddPlayerSkin(new OverridingPlayerSkin("Snake", S4));

        SkinP1Settings = CustomSettingsManager.AddSavedDropdown("API", MyPluginInfo.PLUGIN_GUID, "Player 1 Skin",
            "Lamb",
            [.. CustomSkinManager.CustomPlayerSkins.Keys], i =>
            {
                if (0 >= i)
                    CustomSkinManager.ResetPlayerSkin(PlayerType.P1);
                else
                    CustomSkinManager.SetPlayerSkinOverride(
                        PlayerType.P1,
                        CustomSkinManager.CustomPlayerSkins.Values.ElementAt(i - 1));
            });

        SkinP2Settings = CustomSettingsManager.AddSavedDropdown("API", MyPluginInfo.PLUGIN_GUID, "Player 2 Skin",
            "Goat",
            [.. CustomSkinManager.CustomPlayerSkins.Keys], i =>
            {
                if (0 >= i)
                    CustomSkinManager.ResetPlayerSkin(PlayerType.P2);
                else
                    CustomSkinManager.SetPlayerSkinOverride(
                        PlayerType.P2,
                        CustomSkinManager.CustomPlayerSkins.Values.ElementAt(i - 1));
            });

        CustomSettingsManager.AddBepInExConfig("API", "Skip Splash Screen", _skipSplashScreen);
        CustomSettingsManager.AddBepInExConfig("API", "Disable Achievement", _disableAchievement,
            delegate(bool isActivated)
            {
                if (isActivated) return;

                AchievementsWrapper.LoadAchievementData();
                AchievementsWrapper.compareAchievements();
            });

        CustomSettingsManager.AddBepInExConfig("API", "Debug Mode", _debug, delegate(bool isActivated)
        {
            if (!isActivated)
            {
                // ReSharper disable once InvertIf
                if (SkinP1Settings?.Value == "Debug Skin")
                {
                    SkinP1Settings.Value = "Lamb";
                    CustomSkinManager.ResetPlayerSkin(PlayerType.P1);
                }

                // ReSharper disable once InvertIf
                if (SkinP2Settings?.Value == "Debug Skin")
                {
                    SkinP2Settings.Value = "Goat";
                    CustomSkinManager.ResetPlayerSkin(PlayerType.P2);
                }
            }
            else
            {
                if (DebugContentAdded) return;
                AddDebugContent();
            }
        });

        CustomSettingsManager.AddBepInExConfig("API", "Unity Debug Logging", UnityDebug);

        if (Debug) AddDebugContent();

        LogInfo($"{MyPluginInfo.PLUGIN_NAME} loaded!");

        // GameHash.LogGameInfo();
    }

    private void Start()
    {
        OnStart.Invoke();
        Started = true;
    }

    // Debug cheats
    public void Update()
    {
        if (!Debug) return;

        // Kill all enemies
        // ReSharper disable once InvertIf
        if (Input.GetKeyDown(KeyCode.F1))
        {
            var targets = new List<Health>(Health.team2);
            var entityObject = GameObject.FindWithTag("Player");
            targets.DoIf(x => x, x => x.DealDamage(999999, null, entityObject.transform.position));
        }

        // ReSharper disable once InvertIf
        if (Input.GetKeyDown(KeyCode.F2))
            foreach (var x in PlayerFarming.Instance.Spine.Skeleton.Skin.Attachments)
                LogInfo($"{{ \"{x.Name}\", Tuple.Create({x.SlotIndex}, \"{x.Name}\") }}");

        // ReSharper disable once InvertIf
        if (Input.GetKeyDown(KeyCode.F3))
            foreach (var x in PlayerFarming.Instance.Spine.Skeleton.Skin.Attachments)
                LogInfo($"{{ \"{x.Name}\", Tuple.Create({x.SlotIndex}, \"{x.Name}\") }}");
    }

    private void OnEnable()
    {
        _harmony.PatchAll(Assembly.GetExecutingAssembly());
        LogInfo($"{_harmony.GetPatchedMethods().Count()} harmony patches applied!");
    }

    private void OnDisable()
    {
        _harmony.UnpatchSelf();
        LogInfo($"{MyPluginInfo.PLUGIN_NAME} unloaded!");
    }

    internal static event Action OnStart = delegate { };

    private void RunSavePatch()
    {
        // LOAD_AFTER_START handler
        SaveAndLoad.OnLoadComplete += delegate
        {
            LogDebug("Loading Modded Save Data with LoadOrder=ModdedSaveLoadOrder.LOAD_AFTER_SAVE_START.");
            foreach (var saveData in ModdedSaveManager.ModdedSaveDataList.Values.Where(save =>
                         save.LoadOrder == ModdedSaveLoadOrder.LOAD_AFTER_SAVE_START))
                saveData.Load(SaveAndLoad.SAVE_SLOT);

            LogDebug("Re-adding any custom quests from the players existing objectives.");
            Dictionary<int, CustomObjective> tempObjectives = new();

            if (QuestData == null) return;

            foreach (var objective in QuestData)
                if (DataManager.instance.Objectives.Exists(a => a.ID == objective.Key))
                    tempObjectives.Add(objective.Key, objective.Value);
                else if (Quests.QuestsAll.Exists(a => a.ID == objective.Key))
                    tempObjectives.Add(objective.Key, objective.Value);

            CustomObjectiveManager.CustomObjectiveList.AddRange(tempObjectives);

            LogDebug("Added custom quests to Plugin.Instance.APIQuestData.Data.QuestData.");
            foreach (var quest in CustomObjectiveManager.CustomObjectiveList) QuestData.TryAdd(quest.Key, quest.Value);
        };

        // This will reset the language to "English" if it can't find specified language.
        SettingsManager.Instance._readWriter.OnReadCompleted += delegate
        {
            if (LocalizationManager.HasLanguage(SettingsManager.Settings.Game.Language)) return;

            SettingsManager.Settings.Game.Language = "English";
            LocalizationManager.CurrentLanguage = "English";

            if (TwitchAuthentication.IsAuthenticated)
                TwitchManager.SetLanguage(LocalizationManager.CurrentLanguageCode);

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
                    LogDebug(
                        "Found quests in history with an index higher than total quests (user may have removed mods that add quests), resetting to maximum possible.");
                quest.QuestIndex = Quests.QuestsAll.Count - 1;
            }
        };
    }

    private void AddDebugContent()
    {
        if (DebugContentAdded) return;

        CustomLocalizationManager.LoadLocalization("English",
            Path.Combine(PluginPath, "Assets", "English-Debug.language"));

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

        CustomSettingsManager.AddDropdown("Debug", "Dropdown", "Option 1",
            ["Option 1", "Option 2", "Option 3"], i => { LogDebug($"Dropdown selected {i}"); });

        CustomSettingsManager.AddKeyboardShortcutDropdown("Debug", "Keyboard Shortcut", KeyCode.None,
            i => { LogDebug($"Keyboard Shortcut selected {i}"); });

        CustomSettingsManager.AddHorizontalSelector("Debug", "Horizontal Selector", "Option 1",
            ["Option 1", "Option 2", "Option 3"],
            i => { LogDebug($"Horizontal Selector selected {i}"); });

        CustomSettingsManager.AddSlider("Debug", "Slider", 0, -100, 100, 1, MMSlider.ValueDisplayFormat.RawValue,
            i => { LogDebug($"Slider value: {i}"); });

        CustomSettingsManager.AddToggle("Debug", "Toggle", true,
            i => { LogDebug($"Toggled: {i}"); });

        DebugRelic = CustomRelicManager.Add(ScriptableObject.CreateInstance<DebugRelicClass>());

        LogDebug("Debug mode enabled!");

        DebugContentAdded = true;
    }
}