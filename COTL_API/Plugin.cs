using COTL_API.CustomFollowerCommand;
using COTL_API.CustomStructures;
using COTL_API.CustomObjectives;
using BepInEx.Configuration;
using COTL_API.CustomSkins;
using COTL_API.CustomTasks;
using System.Reflection;
using BepInEx.Logging;
using COTL_API.Debug;
using System.Linq;
using HarmonyLib;
using System.IO;
using BepInEx;
using System;

namespace COTL_API;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
// [BepInProcess("Cult Of The Lamb.exe")] // To be decided
[HarmonyPatch]
public class Plugin : BaseUnityPlugin
{
    public const string PLUGIN_GUID = "io.github.xhayper.COTL_API";
    public const string PLUGIN_NAME = "COTL API";
    public const string PLUGIN_VERSION = "0.1.7";

    internal readonly static Harmony Harmony = new(PLUGIN_GUID);
    internal static new ManualLogSource Logger;

    internal static string PluginPath;

    internal static InventoryItem.ITEM_TYPE DebugItem;
    internal static InventoryItem.ITEM_TYPE DebugItem2;
    internal static InventoryItem.ITEM_TYPE DebugItem3;
    internal static InventoryItem.ITEM_TYPE DebugItem4;

    internal static FollowerCommands DebugGiftFollowerCommand;

    private static bool _questCleanDone; //flag to prevent multiple calls to clean up quests

    internal static ConfigEntry<bool> _debug;
    internal static bool Debug => _debug.Value;

    internal static event Action OnStart = delegate { };
    internal static bool Started { get; private set; }

    private void Awake()
    {
        Logger = base.Logger;
        PluginPath = Path.GetDirectoryName(Info.Location);

        _debug = Config.Bind("", "debug", false, "");

        Logger.LogInfo($"COTL API loaded");
    }

    private void Start()
    {
        OnStart.Invoke();
        Started = true;
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
        
        CustomSkinManager.Add(new DebugFollowerSkin());
        CustomSkinManager.SetPlayerSkinOverride(new DebugPlayerSkin());

        CustomObjective test = CustomObjectiveManager.BedRest("Test");
        test.InitialQuestText = "This is my custom quest text for this objective.";

        Logger.LogDebug("Debug mode enabled");
    }

    private void OnEnable()
    {
        Harmony.PatchAll(Assembly.GetExecutingAssembly());
        Logger.LogInfo($"{Harmony.GetAllPatchedMethods().Count()} harmony patches applied");
        
        if (Debug)
        {
            AddDebugContent();
        }
    }

    private void OnDisable()
    {
        Harmony.UnpatchSelf();
        Logger.LogInfo("COTL API unloaded");
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
        foreach (DataManager.QuestHistoryData quest in DataManager.Instance.CompletedQuestsHistorys.Where(a => a.QuestIndex >= Quests.QuestsAll.Count))
        {
            if (Debug) Logger.LogDebug("Found quests in history with an index higher than total quests (user may have removed mods that add quests), resetting to maximum possible.");
            quest.QuestIndex = Quests.QuestsAll.Count - 1;
        }
        _questCleanDone = true;
    }
}