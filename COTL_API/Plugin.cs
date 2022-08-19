using System.Runtime.CompilerServices;
using System.Reflection;
using BepInEx.Logging;
using HarmonyLib;
using BepInEx;

[assembly: InternalsVisibleTo("Assembly-CSharp")]

namespace COLT_API;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public const string PLUGIN_GUID = "io.github.xhayper.COTL_API";
    public const string PLUGIN_NAME = "COTL API";
    public const string PLUGIN_VERSION = "1.0.0";

    internal readonly static Harmony harmony = new Harmony(PLUGIN_GUID);
    internal static ManualLogSource logger;

    private void OnEnable()
    {
        logger = base.Logger;
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    private void OnDisable()
    {
        harmony.UnpatchSelf();
    }
}