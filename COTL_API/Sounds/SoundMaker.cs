using COTL_API.Helpers;
using FMOD;
using FMODUnity;
using System;
using System.Collections.Generic;
using System.Text;

namespace COTL_API.Sounds;

internal unsafe class SoundMaker
{
    internal static Sound* MakeSound(string fileName, bool loop = false)
    {
        string? path = AudioUtils.GetPath(fileName);
        if (path == null) return default;

        var system = RuntimeManager.CoreSystem;
        var mode = loop ? MODE.LOOP_NORMAL : MODE.LOOP_OFF;

        var result = system.createSound(path, mode, out var sound);
        if (result == RESULT.OK) return &sound;

        LogHelper.LogError($"Error making sound from file {fileName}!");
        result.IfErrorPrintWith($"MakeSound() -- fileName: {fileName}");

        return default; // Return empty sound in the case of an error
    }

    internal static RESULT PlayOneShot(SoundWrapper soundHandle, VolumeCategory volume = VolumeCategory.SFX)
    {
        var system = RuntimeManager.CoreSystem;
        var result = system.playSound(*soundHandle.sound, new ChannelGroup(), false, out var channel);
        channel.SyncVolume(volume);
        return result;
    }

    internal static RESULT PlayOneShot(string path, VolumeCategory volume = VolumeCategory.SFX)
    {
        var soundHandle = new SoundWrapper(MakeSound(path));
        return PlayOneShot(soundHandle, volume);
    }
}
