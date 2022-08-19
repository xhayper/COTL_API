using System.Reflection;
using BepInEx.Logging;
using HarmonyLib;
using BepInEx;

namespace COLT_API;


[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource logger;
    internal static Harmony harmony;


    private void Awake()
    {
        logger = Logger;
        harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    private void OnEnable()
    {
        harmony.PatchAll();
    }

    private void OnDisable()
    {
        harmony.UnpatchSelf();
    }
}