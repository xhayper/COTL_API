using HarmonyLib;
using Lamb.UI.SettingsMenu;
using COTL_API.Sounds.Load;

namespace COTL_API.Sounds.Patches;

[HarmonyPatch]
internal class SoundSyncPatches
{
    [HarmonyPatch(typeof(AudioSettings), nameof(AudioSettings.OnMasterVolumeChanged))]
    [HarmonyPostfix]
    static void SyncMaster()
    {
        SoundLoader.AllInstances.ForEach(x => x.SyncAllVolume());
    }

    [HarmonyPatch(typeof(AudioSettings), nameof(AudioSettings.OnMusicVolumeChanged))]
    [HarmonyPostfix]
    static void SyncMusic()
    {
        SoundLoader.AllInstances.ForEach(x => x.SyncAllVolume());
    }
}
