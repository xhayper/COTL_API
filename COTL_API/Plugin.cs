using BepInEx.Configuration;
using System.Reflection;
using BepInEx.Logging;
using COTL_API.Debug;
using System.Linq;
using HarmonyLib;
using System.IO;
using BepInEx;
using COTL_API.CustomFollowerCommand;
using COTL_API.CustomObjectives;
using COTL_API.Helpers;
using COTL_API.CustomSkins;
using COTL_API.CustomStructures;
using COTL_API.CustomTasks;
using UnityEngine;

namespace COTL_API;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
// [BepInProcess("Cult Of The Lamb.exe")] // To be decided
[HarmonyPatch]
public class Plugin : BaseUnityPlugin
{
    public const string PLUGIN_GUID = "io.github.xhayper.COTL_API";
    public const string PLUGIN_NAME = "COTL API";
    public const string PLUGIN_VERSION = "0.1.4";

    internal readonly static Harmony Harmony = new(PLUGIN_GUID);
    internal static new ManualLogSource Logger;

    internal static string PluginPath;

    internal static InventoryItem.ITEM_TYPE DebugItem;
    internal static InventoryItem.ITEM_TYPE DebugItem2;
    internal static InventoryItem.ITEM_TYPE DebugItem3;
    internal static InventoryItem.ITEM_TYPE DebugItem4;
    internal static FollowerCommands DebugGiftFollowerCommand;

    internal static Objectives.CustomQuestTypes DebugQuestKey;
    internal static ObjectivesData DebugQuestData;

    internal static (Objectives.CustomQuestTypes ObjectivesKey, ObjectivesData ObjectiveData) DebugObjective1;
    internal static (Objectives.CustomQuestTypes ObjectivesKey, ObjectivesData ObjectiveData) DebugObjective2;
    internal static (Objectives.CustomQuestTypes ObjectivesKey, ObjectivesData ObjectiveData) DebugObjective3;

    private static ConfigEntry<bool> _debug;
    internal static bool Debug => _debug.Value;


    private void Awake()
    {
        Logger = base.Logger;
        PluginPath = Path.GetDirectoryName(Info.Location);

        _debug = Config.Bind("", "debug", false, "");

        if (Debug)
        {
            AddDebugContent();
        }

        Logger.LogInfo($"COTL API loaded");
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

        Texture2D customTex =
            TextureHelper.CreateTextureFromPath(PluginPaths.ResolveAssetPath("placeholder_sheet.png"));
        string atlasText = File.ReadAllText(PluginPaths.ResolveAssetPath("basic_atlas.txt"));

        CustomSkinManager.AddCustomSkin("Test", customTex, atlasText);

        //assignment not necessary unless the plugin would like easy access to the data afterwards
        DebugObjective1 = CustomObjectiveManager.Add(new DebugObjective());
        DebugObjective2 = CustomObjectiveManager.Add(new DebugObjective2());
        DebugObjective3 = CustomObjectiveManager.Add(new DebugObjective3());
        
        Logger.LogDebug("Debug mode enabled");
    }

    private void OnEnable()
    {
        Harmony.PatchAll(Assembly.GetExecutingAssembly());
        Logger.LogInfo($"{Harmony.GetAllPatchedMethods().Count()} harmony patches applied");
    }

    private void OnDisable()
    {
        Harmony.UnpatchSelf();
        Logger.LogInfo("COTL API unloaded");
    }
}