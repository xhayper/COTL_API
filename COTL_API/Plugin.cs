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

    internal static Plugin Instance { get; private set; }

    internal new ManualLogSource Logger { get; private set; }

    private readonly Harmony Harmony = new(PLUGIN_GUID);
    internal readonly BaseModdedSaveData<APIData> APIData = new(PLUGIN_GUID);

    internal string PluginPath { get; private set; }

    internal InventoryItem.ITEM_TYPE DebugItem { get; private set; }
    internal InventoryItem.ITEM_TYPE DebugItem2 { get; private set; }
    internal InventoryItem.ITEM_TYPE DebugItem3 { get; private set; }
    internal InventoryItem.ITEM_TYPE DebugItem4 { get; private set; }

    internal FollowerCommands DebugGiftFollowerCommand { get; private set; }

    private ConfigEntry<bool> _debug { get; set; }
    public bool Debug => _debug.Value;

    private bool _questCleanDone; //flag to prevent multiple calls to clean up quests

    private void Awake()
    {
        Instance = this;
        Logger = base.Logger;

        BindEvent();

        PluginPath = Path.GetDirectoryName(Info.Location);
        _debug = Config.Bind("", "debug", false, "");

        APIData.LoadOnStart = true;
        ModdedSaveManager.RegisterModdedSave(APIData);

        if (Debug)
            AddDebugContent();

        Logger.LogInfo($"COTL_API loaded");
    }

    private void OnEnable()
    {
        Harmony.PatchAll(Assembly.GetExecutingAssembly());
        Logger.LogInfo($"{Harmony.GetAllPatchedMethods().Count()} harmony patches applied");
    }

    private void OnDisable()
    {
        Harmony.UnpatchSelf();
        Logger.LogInfo("COTL_API unloaded");
    }

    private void BindEvent()
    {
        APIData.OnLoadComplete += delegate
        {
            Dictionary<int, CustomObjective> tempObjectives = new();

            foreach (var objective in APIData.Data.QuestData)
                if (DataManager.instance.Objectives.Exists(a => a.ID == objective.Key))
                    tempObjectives.Add(objective.Key, objective.Value);
                else if (Quests.QuestsAll.Exists(a => a.ID == objective.Key))
                    tempObjectives.Add(objective.Key, objective.Value);

            CustomObjectiveManager.PluginQuestTracker.AddRange(tempObjectives);
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

        var test = CustomObjectiveManager.BedRest("Test");
        test.InitialQuestText = "This is my custom quest text for this objective.";

        Logger.LogDebug("Debug mode enabled");
    }
}