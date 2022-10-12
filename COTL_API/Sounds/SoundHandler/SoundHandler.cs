using FMOD;
using FMODUnity;
using HarmonyLib;
using Microsoft.Win32.SafeHandles;
using COTL_API.Sounds.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Unity.Audio;

namespace COTL_API.Sounds.Handler;
internal class SoundHandler
{
    // Channel through which all the sound is played.
    Channel handle;

    Sound _sound; // Current sound.

    string _id; // Sound id
    public string Id => _id;

    public SoundHandler(Sound sound, string id)
    {
        _sound = sound;
        _id = id;
        handle = new Channel();
    }

    public RESULT Play()
    {
        var system = RuntimeManager.CoreSystem;
        RESULT result = system.playSound(_sound, new ChannelGroup(), false, out handle);
        if (result != RESULT.OK)
        {
            result.IfErrorPrintWith($"Play -- SoundHandler instance id: {Id}");
            return result;
        }

        return result;
    }

    public void SetVolume(float a)
    {
        RESULT result = handle.setVolume(a);
        result.IfErrorPrintWith($"SetVolume -- SoundHandler instance id: {Id}");
    }

    public void Stop()
    {
        RESULT result = handle.stop();
        result.IfErrorPrintWith($"Stop -- SoundHandler instance id: {Id}");
    }

    public void Pause(bool pause)
    {
        handle.isPlaying(out bool isPlaying);
        bool flag = pause && isPlaying;
        if (flag)
        {
            RESULT result = handle.setPaused(pause);
            result.IfErrorPrintWith($"Pause -- SoundHandler instance id: {Id}");
        }
    }
}