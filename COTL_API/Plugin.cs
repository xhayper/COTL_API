using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using COTL_API.CustomInventory;
using COTL_API.CustomObjectives;
using COTL_API.CustomSettings;
using COTL_API.CustomSettings.Elements;
using COTL_API.CustomSkins;
using COTL_API.Debug;
using COTL_API.Saves;
using HarmonyLib;
using I2.Loc;
using MonoMod.Utils;
using UnityEngine;

namespace COTL_API;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
// [BepInProcess("Cult Of The Lamb.exe")] // To be decided
[HarmonyPatch]
public class Plugin : BaseUnityPlugin
{
    internal static Dropdown? LambFleeceSkinSettings;
    internal static Dropdown? GoatFleeceSkinSettings;
    internal static Dropdown? LambFleeceBleatSettings;
    internal static Dropdown? GoatFleeceBleatSettings;

    internal static Dropdown? CustomPlayerSpineSettings;
    internal readonly Harmony _harmony = new(MyPluginInfo.PLUGIN_GUID);

    internal readonly ModdedSaveData<ApiData> APIData = new(MyPluginInfo.PLUGIN_GUID)
    {
        LoadOrder = ModdedSaveLoadOrder.LOAD_AS_SOON_AS_POSSIBLE
    };

    internal readonly ModdedSaveData<ApiSlotData> APISlotData = new($"{MyPluginInfo.PLUGIN_GUID}_slot");

    public readonly ModdedSaveData<ObjectDictionary> ModdedSettingsData = new("modded_settings")
    {
        LoadOrder = ModdedSaveLoadOrder.LOAD_AS_SOON_AS_POSSIBLE
    };

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

    private ConfigEntry<bool>? _decryptSaveFile { get; set; }
    public bool DecryptSaveFile => _decryptSaveFile?.Value ?? false;

    internal static bool Started { get; private set; }

    internal static ObjectDictionary? SettingsData => Instance?.ModdedSettingsData.Data;

    internal static Dictionary<int, CustomObjective>? QuestData =>
        Instance != null ? Instance.APISlotData.Data?.QuestData : null;

    internal static ObjectDictionary? EnumData => Instance?.APIData.Data?.EnumData;

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

        _skipSplashScreen = Config.Bind("Miscellaneous", "Skip splash screen", false,
            "Should we skip the splash screen or not?");
        _disableAchievement = Config.Bind("Miscellaneous", "Disable new achievement", true,
            "Should we disable saving new achievements? (You will still be able to get achievement but it won't save)");
        _decryptSaveFile = Config.Bind("Miscellaneous", "Decrypt save file", false);

        _debug = Config.Bind("Debug", "API debug", false,
            "API debug mode. Will add debug content to your game for testing. Not recommended for normal play.");
        UnityDebug = Config.Bind("Debug", "Unity debug logging", false,
            "Unity debug logging. Helpful to filter out unrelated entries during testing.");

        UnityDebug.SettingChanged += (_, _) => { UnityEngine.Debug.unityLogger.logEnabled = UnityDebug.Value; };

        CustomSkinManager.AddPlayerSkin(new OverridingPlayerSkin("Lamb",
            () => PlayerFarming.Instance.Spine.Skeleton.Data.FindSkin("Lamb")));
        CustomSkinManager.AddPlayerSkin(new OverridingPlayerSkin("Goat",
            () => PlayerFarming.Instance.Spine.Skeleton.Data.FindSkin("Goat")));
        CustomSkinManager.AddPlayerSkin(new OverridingPlayerSkin("Owl",
            () => PlayerFarming.Instance.Spine.Skeleton.Data.FindSkin("Owl")));
        CustomSkinManager.AddPlayerSkin(new OverridingPlayerSkin("Snake",
            () => PlayerFarming.Instance.Spine.Skeleton.Data.FindSkin("Snake")));

        //This is to load the dropdown as it needs at least 1 entry
        CustomSkinManager.AddPlayerSpine("Placeholder", null, ["Placeholder selection"]);

        LambFleeceSkinSettings = CustomSettingsManager.AddSavedDropdown("API", MyPluginInfo.PLUGIN_GUID,
            "Lamb fleece skin",
            "Lamb",
            [.. CustomSkinManager.CustomPlayerSkins.Keys], i =>
            {
                if (0 >= i)
                    CustomSkinManager.ResetPlayerSkin(PlayerType.LAMB);
                else
                    CustomSkinManager.SetPlayerSkinOverride(
                        PlayerType.LAMB,
                        CustomSkinManager.CustomPlayerSkins.Values.ElementAt(i));
            });

        GoatFleeceSkinSettings = CustomSettingsManager.AddSavedDropdown("API", MyPluginInfo.PLUGIN_GUID,
            "Goat fleece skin",
            "Goat",
            [.. CustomSkinManager.CustomPlayerSkins.Keys], i =>
            {
                if (i is < 0 or 1)
                    CustomSkinManager.ResetPlayerSkin(PlayerType.GOAT);
                else
                    CustomSkinManager.SetPlayerSkinOverride(
                        PlayerType.GOAT,
                        CustomSkinManager.CustomPlayerSkins.Values.ElementAt(i));
            });

        LambFleeceBleatSettings = CustomSettingsManager.AddSavedDropdown("API", MyPluginInfo.PLUGIN_GUID,
            "Lamb fleece bleat",
            "Lamb",
            Enum.GetNames(typeof(PlayerBleat)),
            i => { CustomSkinManager.SetPlayerBleatOverride(PlayerType.LAMB, (PlayerBleat)i); });

        GoatFleeceBleatSettings = CustomSettingsManager.AddSavedDropdown("API", MyPluginInfo.PLUGIN_GUID,
            "Goat fleece bleat",
            "Goat",
            Enum.GetNames(typeof(PlayerBleat)),
            i => { CustomSkinManager.SetPlayerBleatOverride(PlayerType.GOAT, (PlayerBleat)i); });

        CustomPlayerSpineSettings = CustomSettingsManager.AddSavedDropdown("API", MyPluginInfo.PLUGIN_GUID,
            "Custom player spine",
            "Lamb Spine",
            [.. CustomSkinManager.CustomPlayerSpines.Keys], i =>
            {
                CustomSkinManager.ChangeSelectedPlayerSpine(
                    CustomSkinManager.CustomPlayerSpines.Keys.ElementAt(i));
            });

        CustomSettingsManager.AddBepInExConfig("API", "Skip splash screen", _skipSplashScreen);
        CustomSettingsManager.AddBepInExConfig("API", "Disable new achievement", _disableAchievement,
            delegate(bool isActivated)
            {
                if (isActivated) return;

                AchievementsWrapper.LoadAchievementData();
                AchievementsWrapper.compareAchievements();
            });
        CustomSettingsManager.AddBepInExConfig("API", "Decrypt save file", _decryptSaveFile, delegate(bool isActivated)
        {
            if (!isActivated) return;

            foreach (var save in ModdedSaveManager.ModdedSaveDataList.Values) save.Save(!_decryptSaveFile.Value);
        });

        CustomSettingsManager.AddBepInExConfig("API", "Debug", _debug, delegate(bool isActivated)
        {
            if (!isActivated)
            {
                // ReSharper disable once InvertIf
                if (LambFleeceSkinSettings?.Value is "Debug" or "Debug_1")
                {
                    LambFleeceSkinSettings.Value = "Lamb";
                    CustomSkinManager.ResetPlayerSkin(PlayerType.LAMB);
                }

                // ReSharper disable once InvertIf
                if (GoatFleeceSkinSettings?.Value is "Debug" or "Debug_1")
                {
                    GoatFleeceSkinSettings.Value = "Goat";
                    CustomSkinManager.ResetPlayerSkin(PlayerType.GOAT);
                }
            }
            else
            {
                DebugManager.AddDebugContent();
            }
        });

        CustomSettingsManager.AddBepInExConfig("API", "Unity debug logging", UnityDebug);

        if (Debug) DebugManager.AddDebugContent();

        LogInfo($"{MyPluginInfo.PLUGIN_NAME} loaded!");
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
        {
            var slots = PlayerFarming.Instance.Spine.Skeleton.Skin.Attachments;
            var sortedSlots = slots.OrderBy(x => x.SlotIndex).ToList();
            var jsonOutput = sortedSlots.Select(x => $"{{ \"{x.Name}\", Tuple.Create({x.SlotIndex}, \"{x.Name}\") }}");
            LogDebug(string.Join(",\n", jsonOutput));
        }

        // ReSharper disable once InvertIf
        if (Input.GetKeyDown(KeyCode.F3))
            foreach (var x in PlayerFarming.Instance.Spine.Skeleton.Skin.Attachments)
                LogDebug($"{{ \"{x.Name}\", Tuple.Create({x.SlotIndex}, \"{x.Name}\") }}");
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

    internal static event Action OnStart = delegate { CustomItemManager.InitiateCustomCrops(); };

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
            Dictionary<int, CustomObjective> tempObjectives = [];

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
}