using Lamb.UI.SettingsMenu;
using HarmonyLib;

namespace COTL_API.Sounds.Patches;

[HarmonyPatch]
internal class SoundSyncPatches
{
    [HarmonyPatch(typeof(AudioSettings), nameof(AudioSettings.OnMasterVolumeChanged))]
    [HarmonyPostfix]
    private static void SyncMaster()
    {
        SoundLoader.InstanceList.ForEach(x => x.SyncAllVolume());
    }

    [HarmonyPatch(typeof(AudioSettings), nameof(AudioSettings.OnMusicVolumeChanged))]
    [HarmonyPostfix]
    private static void SyncMusic()
    {
        SoundLoader.InstanceList.ForEach(x => x.SyncAllVolume());
    }
}
