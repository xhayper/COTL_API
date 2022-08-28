using BepInEx.Configuration;
using System.Reflection;
using BepInEx.Logging;
using COTL_API.Debug;
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
    public const string PLUGIN_NAME = "COTL API";
    public const string PLUGIN_VERSION = "1.0.0";

    internal readonly static Harmony Harmony = new(PLUGIN_GUID);
    internal static new ManualLogSource Logger;

    internal static string PluginPath;

    internal static InventoryItem.ITEM_TYPE DebugItem;
    internal static InventoryItem.ITEM_TYPE DebugItem2;
    internal static InventoryItem.ITEM_TYPE DebugItem3;

    internal static FollowerCommands DebugFollowerCommand;
    internal static FollowerCommands DebugFollowerCommand2;

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

            DebugItem = CustomInventory.CustomItemManager.Add(new DebugItemClass());
            DebugItem2 = CustomInventory.CustomItemManager.Add(new DebugItemClass2());
            DebugItem3 = CustomInventory.CustomItemManager.Add(new DebugItemClass3());

            CustomTarotCard.CustomTarotCardManager.Add(new DebugTarotCard());

            DebugCode.CreateSkin();

            Logger.LogDebug("Debug mode enabled");
        }

        Logger.LogInfo("COTL API loaded");
    }

    private void OnEnable()
    {
        Harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    private void OnDisable()
    {
        Harmony.UnpatchSelf();
    }
}