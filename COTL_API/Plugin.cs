using System.Runtime.CompilerServices;
using System.Reflection;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using BepInEx;
using Lamb.UI;

[assembly: InternalsVisibleTo("Assembly-CSharp")]

namespace COTL_API;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static string PLUGIN_PATH;

    public const string PLUGIN_GUID = "io.github.xhayper.COTL_API";
    public const string PLUGIN_NAME = "COTL API";
    public const string PLUGIN_VERSION = "1.0.0";

    internal readonly static Harmony harmony = new(PLUGIN_GUID);
    internal static ManualLogSource logger;

    internal static InventoryItem.ITEM_TYPE DEBUG_ITEM;
    internal static InventoryItem.ITEM_TYPE DEBUG_ITEM_2;
    internal static InventoryItem.ITEM_TYPE DEBUG_ITEM_3;

    private void Awake()
    {
        logger = Logger;
        PLUGIN_PATH = Path.GetDirectoryName(Info.Location);

        DEBUG_ITEM = CustomInventory.CustomItemManager.Add(new INDEV.DEBUG_ITEM_CLASS());
        DEBUG_ITEM_2 = CustomInventory.CustomItemManager.Add(new INDEV.DEBUG_ITEM_CLASS_2());
        DEBUG_ITEM_3 = CustomInventory.CustomItemManager.Add(new INDEV.DEBUG_ITEM_CLASS_3());
    
        CustomTarotCard.CustomTarotCardManager.Add(new INDEV.DEBUG_TAROT_CARD());
    }

    private void OnEnable()
    {
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    private void OnDisable()
    {
        harmony.UnpatchSelf();
    }

    // DEBUG CODE

    [HarmonyPatch(typeof(InventoryMenu), nameof(InventoryMenu.OnShowStarted))]
    [HarmonyPrefix]
    public static void InventoryMenu_OnShowStarted()
    {
        Inventory.AddItem(DEBUG_ITEM, 1, true);
        Inventory.AddItem(DEBUG_ITEM_2, 1, true);
        Inventory.AddItem(DEBUG_ITEM_3, 1, true);
    }
}