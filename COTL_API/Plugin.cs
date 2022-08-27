using BepInEx.Configuration;
using System.Reflection;
using BepInEx.Logging;
using COTL_API.INDEV;
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

    internal readonly static Harmony harmony = new(PLUGIN_GUID);
    internal static ManualLogSource logger;

    internal static string PLUGIN_PATH;

    internal static InventoryItem.ITEM_TYPE DEBUG_ITEM;
    internal static InventoryItem.ITEM_TYPE DEBUG_ITEM_2;
    internal static InventoryItem.ITEM_TYPE DEBUG_ITEM_3;

    internal static FollowerCommands DEBUG_FOLLOWER_COMMAND;
    internal static FollowerCommands DEBUG_FOLLOWER_COMMAND_2;
    
    private static ConfigEntry<bool> _debugEnabled;
    internal static bool DebugEnabled => _debugEnabled.Value;

    private void Awake()
    {
        logger = Logger;
        PLUGIN_PATH = Path.GetDirectoryName(Info.Location);

        _debugEnabled = Config.Bind("", "debug", false, "");

        if (!DebugEnabled) return;

        DEBUG_FOLLOWER_COMMAND = CustomFollowerCommand.CustomFollowerCommandManager.Add(new INDEV.DEBUG_FOLLOWER_COMMAND_CLASS());
        DEBUG_FOLLOWER_COMMAND_2 = CustomFollowerCommand.CustomFollowerCommandManager.Add(new INDEV.DEBUG_FOLLOWER_COMMAND_CLASS_2());

        CustomTarotCard.CustomTarotCardManager.Add(new INDEV.DEBUG_TAROT_CARD());
        
        
        DEBUG_ITEM = CustomInventory.CustomItemManager.Add(new DEBUG_ITEM_CLASS());
        DEBUG_ITEM_2 = CustomInventory.CustomItemManager.Add(new DEBUG_ITEM_CLASS_2());
        DEBUG_ITEM_3 = CustomInventory.CustomItemManager.Add(new DEBUG_ITEM_CLASS_3());

        CustomTarotCard.CustomTarotCardManager.Add(new DEBUG_TAROT_CARD());

        DEBUG_CODE.CreateSkin();
    }

    private void OnEnable()
    {
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    private void OnDisable()
    {
        harmony.UnpatchSelf();
    }
}