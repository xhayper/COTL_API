using Lamb.UI.FollowerInteractionWheel;
using BepInEx.Configuration;
using System.Reflection;
using BepInEx.Logging;
using COTL_API.Debug;
using System.Linq;
using HarmonyLib;
using System.IO;
using BepInEx;
using COTL_API.CustomFollowerCommand;
using COTL_API.Structures;
using COTL_API.Tasks;
using UnityEngine.SceneManagement;

namespace COTL_API;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
// [BepInProcess("Cult Of The Lamb.exe")] // To be decided
[HarmonyPatch]
public class Plugin : BaseUnityPlugin
{
    public const string PLUGIN_GUID = "io.github.xhayper.COTL_API";
    public const string PLUGIN_NAME = "COTL API";
    public const string PLUGIN_VERSION = "0.1.2";

    internal readonly static Harmony Harmony = new(PLUGIN_GUID);
    internal static new ManualLogSource Logger;

    internal static string PluginPath;

    internal static InventoryItem.ITEM_TYPE DebugItem;
    internal static InventoryItem.ITEM_TYPE DebugItem2;
    internal static InventoryItem.ITEM_TYPE DebugItem3;
    internal static FollowerCommands DebugGiftFollowerCommand;
    internal static FollowerCommands DebugTaskFollowerCommand;

    internal static StructureBrain.TYPES DebugStructure;
    internal static StructureBrain.TYPES DebugStructure2;
    internal static StructureBrain.TYPES DebugStructure3;

    internal static FollowerTaskType DebugTask;

    private static ConfigEntry<bool> _debug;
    internal static bool Debug => _debug.Value;

    private void Awake()
    {
        Logger = base.Logger;
        PluginPath = Path.GetDirectoryName(Info.Location);

        _debug = Config.Bind("", "debug", false, "");

        if (Debug)
        {
            CustomFollowerCommand.CustomFollowerCommandManager.Add(new DebugFollowerCommand());
            CustomFollowerCommand.CustomFollowerCommandManager.Add(new DebugFollowerCommandClass2());
            CustomFollowerCommand.CustomFollowerCommandManager.Add(new DebugFollowerCommandClass3());
            DebugGiftFollowerCommand =
                CustomFollowerCommand.CustomFollowerCommandManager.Add(new DebugGiftFollowerCommand());

            DebugTaskFollowerCommand = CustomFollowerCommandManager.Add(new DebugTaskFollowerCommand());

            DebugItem = CustomInventory.CustomItemManager.Add(new DebugItemClass());
            DebugItem2 = CustomInventory.CustomItemManager.Add(new DebugItemClass2());
            DebugItem3 = CustomInventory.CustomItemManager.Add(new DebugItemClass3());

            DebugStructure = CustomStructureManager.Add(new DebugStructure());
            DebugStructure2 = CustomStructureManager.Add(new DebugStructure2());
            DebugStructure3 = CustomStructureManager.Add(new DebugStructure3());

            CustomTarotCard.CustomTarotCardManager.Add(new DebugTarotCard());

            DebugTask = CustomTaskManager.Add(new DebugTask());

            DebugCode.CreateSkin();

            Logger.LogDebug("Debug mode enabled");
        }

        Logger.LogInfo($"COTL API loaded");
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
