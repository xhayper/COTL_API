using System;
using System.Linq;
using System.Collections.Generic;
using HarmonyLib;
using Lamb;
using Lamb.UI.SettingsMenu;
using COTL_API.Sounds.Helpers;
using FMOD;
using FMODUnity;

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
