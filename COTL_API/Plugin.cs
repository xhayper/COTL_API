using COTL_API.CustomFollowerCommand;
using System.Collections.Generic;
using COTL_API.CustomStructures;
using COTL_API.CustomObjectives;
using BepInEx.Configuration;
using COTL_API.CustomSkins;
using COTL_API.CustomTasks;
using System.Reflection;
using COTL_API.Helpers;
using BepInEx.Logging;
using COTL_API.Saves;
using COTL_API.Debug;
using MonoMod.Utils;
using System.Linq;
using HarmonyLib;
using System.IO;
using BepInEx;

namespace COTL_API;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
// [BepInProcess("Cult Of The Lamb.exe")] // To be decided
[HarmonyPatch]
public class Plugin : BaseUnityPlugin
{
    public const string PLUGIN_GUID = "io.github.xhayper.COTL_API";
    public const string PLUGIN_NAME = "COTL_API";
    public const string PLUGIN_VERSION = "0.1.7";

    internal static Plugin? Instance { get; private set; }

    internal new ManualLogSource Logger { get; private set; } = new(PLUGIN_NAME);

    private readonly Harmony _harmony = new(PLUGIN_GUID);

    internal readonly ModdedSaveData<ApiData> APIData = new(PLUGIN_GUID)
    {
        LoadOrder = ModdedSaveLoadOrder.LOAD_AS_SOON_AS_POSSIBLE
    };

    public readonly ModdedSaveData<ApiSlotData> APISlotData = new($"{PLUGIN_GUID}_slot");

    internal string PluginPath { get; private set; } = "";

    internal InventoryItem.ITEM_TYPE DebugItem { get; private set; }
    internal InventoryItem.ITEM_TYPE DebugItem2 { get; private set; }
    internal InventoryItem.ITEM_TYPE DebugItem3 { get; private set; }
    internal InventoryItem.ITEM_TYPE DebugItem4 { get; private set; }

    internal FollowerCommands DebugGiftFollowerCommand { get; private set; }
    private ConfigEntry<bool>? _debug { get; set; }
    public bool Debug => _debug?.Value ?? false;

    private bool _questCleanDone; //flag to prevent multiple calls to clean up quests

    private void Awake()
    {
        Instance = this;
        Logger = base.Logger;

        PluginPath = Path.GetDirectoryName(Info.Location) ?? string.Empty;
        _debug = Config.Bind("", "debug", false, "");

        ModdedSaveManager.RegisterModdedSave(APIData);
        ModdedSaveManager.RegisterModdedSave(APISlotData);

        BeginLoadAfterMainSave();

        if (Debug)
            AddDebugContent();

        Logger.LogInfo($"COTL_API loaded");
    }

    private void OnEnable()
    {
        _harmony.PatchAll(Assembly.GetExecutingAssembly());
        Logger.LogInfo($"{Harmony.GetAllPatchedMethods().Count()} harmony patches applied");
    }

    private void OnDisable()
    {
        _harmony.UnpatchSelf();
        Logger.LogInfo("COTL_API unloaded");
    }

    private void BeginLoadAfterMainSave()
    {
        Singleton<SaveAndLoad>.Instance._saveFileReadWriter.OnReadCompleted += delegate
        {
            Logger.LogWarning($"Loading Modded Save Data with LoadAfterMainSave=true.");
            foreach (var saveData in ModdedSaveManager.ModdedSaveDataList.Values.Where(save =>
                         save.LoadOrder == ModdedSaveLoadOrder.LOAD_AFTER_SAVE_START))
            {
                saveData.Load(SaveAndLoad.SAVE_SLOT);
            }

            Logger.LogWarning($"Re-adding any custom quests from the players existing objectives.");
            Dictionary<int, CustomObjective> tempObjectives = new();

            if (APISlotData.Data?.QuestData == null) return;

            foreach (var objective in APISlotData.Data.QuestData)
                if (DataManager.instance.Objectives.Exists(a => a.ID == objective.Key))
                    tempObjectives.Add(objective.Key, objective.Value);
                else if (Quests.QuestsAll.Exists(a => a.ID == objective.Key))
                    tempObjectives.Add(objective.Key, objective.Value);

            CustomObjectiveManager.CustomObjectiveList.AddRange(tempObjectives);

            Logger.LogWarning($"Added custom quests to Plugin.Instance.APIQuestData.Data.QuestData.");
            foreach (var quest in CustomObjectiveManager.CustomObjectiveList)
            {
                APISlotData.Data.QuestData.TryAdd(quest.Key, quest.Value);
            }
        };
    }


    /// <summary>
    /// Cleans the users QuestHistory indexes to prevent issues when they remove/disable mods that add custom quests.
    /// The index stored inside the QuestHistoryData is based on the static Quests.QuestAll list count, which gets changed when we add/remove quests.
    /// This fix stops Index out of bound errors when accepting a new quest, and keeps the users history intact.
    /// </summary>
    private void Update()
    {
        if (_questCleanDone) return;
        if (DataManager.Instance == null) return;
        foreach (var quest in DataManager.Instance.CompletedQuestsHistorys.Where(a =>
                     a.QuestIndex >= Quests.QuestsAll.Count))
        {
            if (Debug)
                Logger.LogDebug(
                    "Found quests in history with an index higher than total quests (user may have removed mods that add quests), resetting to maximum possible.");
            quest.QuestIndex = Quests.QuestsAll.Count - 1;
        }

        _questCleanDone = true;
    }

    private void AddDebugContent()
    {
        CustomFollowerCommandManager.Add(new DebugFollowerCommand());
        CustomFollowerCommandManager.Add(new DebugFollowerCommandClass2());
        CustomFollowerCommandManager.Add(new DebugFollowerCommandClass3());
        DebugGiftFollowerCommand = CustomFollowerCommandManager.Add(new DebugGiftFollowerCommand());

        CustomFollowerCommandManager.Add(new DebugTaskFollowerCommand());

        DebugItem = CustomInventory.CustomItemManager.Add(new DebugItemClass());
        DebugItem2 = CustomInventory.CustomItemManager.Add(new DebugItemClass2());
        DebugItem3 = CustomInventory.CustomItemManager.Add(new DebugItemClass3());
        DebugItem4 = CustomInventory.CustomItemManager.Add(new DebugItemClass4());

        CustomStructureManager.Add(new DebugStructure2());
        CustomStructureManager.Add(new DebugStructure3());

        CustomTarotCard.CustomTarotCardManager.Add(new DebugTarotCard());

        CustomTaskManager.Add(new DebugTask());

        var customTex =
            TextureHelper.CreateTextureFromPath(PluginPaths.ResolveAssetPath("placeholder_sheet.png"));
        var atlasText = File.ReadAllText(PluginPaths.ResolveAssetPath("basic_atlas.txt"));

        CustomSkinManager.AddCustomSkin("Test", customTex, atlasText);

        Logger.LogDebug("Debug mode enabled");
    }
}