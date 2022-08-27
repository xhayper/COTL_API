using BepInEx.Configuration;
using System.Reflection;
using BepInEx.Logging;
using COTL_API.INDEV;
using HarmonyLib;
using System.IO;
using BepInEx;

namespace COTL_API;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
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

    internal static ConfigEntry<bool> debugConfigEntry;

    internal static bool debug {
        get => debugConfigEntry.Value;
    }

    private void Awake()
    {
        logger = Logger;
        PLUGIN_PATH = Path.GetDirectoryName(Info.Location);

        debugConfigEntry = Config.Bind("Debug", "Enable Debug Mode", false, "Debug mode used for COTL API dev to test stuff");

        if (debug)
        {
            DEBUG_ITEM = CustomInventory.CustomItemManager.Add(new INDEV.DEBUG_ITEM_CLASS());
            DEBUG_ITEM_2 = CustomInventory.CustomItemManager.Add(new INDEV.DEBUG_ITEM_CLASS_2());
            DEBUG_ITEM_3 = CustomInventory.CustomItemManager.Add(new INDEV.DEBUG_ITEM_CLASS_3());

            CustomTarotCard.CustomTarotCardManager.Add(new INDEV.DEBUG_TAROT_CARD());

            DEBUG_CODE.CreateSkin();
        }
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