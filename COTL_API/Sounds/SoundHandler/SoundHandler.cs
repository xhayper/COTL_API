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
    ChannelSafeHandle handle; // SafeHandle. Important. Please do not remove.

    Sound _sound; // Current sound.

    string _id; // Sound id
    public string Id => _id;

    public SoundHandler(Sound sound, string id)
    {
        _sound = sound;
        _id = id;
        handle = new ChannelSafeHandle();
    }

    public RESULT Play()
    {
        var system = RuntimeManager.CoreSystem;
        RESULT result = system.playSound(_sound, new ChannelGroup(), false, out Channel channel);
        if(result != RESULT.OK)
        {
            result.IfErrorPrintWith($"Play -- SoundHandler instance id: {Id}");
            return result;
        }

        handle.gCHandle = GCHandle.Alloc(channel, GCHandleType.Pinned);
        handle.SetTheHandle(handle.gCHandle.AddrOfPinnedObject());
        return result;
    }

    public void SetVolume(float a)
    {
        Channel channel = (Channel)handle.gCHandle.Target;
        RESULT result = channel.setVolume(a);
        result.IfErrorPrintWith($"SetVolume -- SoundHandler instance id: {Id}");
    }

    public void Stop()
    {
        Channel channel = (Channel)handle.gCHandle.Target;
        RESULT result = channel.stop();
        result.IfErrorPrintWith($"Stop -- SoundHandler instance id: {Id}");

        handle.Dispose();
    }

    public void Pause(bool pause)
    {
        Channel channel = (Channel)handle.gCHandle.Target;
        channel.isPlaying(out bool isPlaying);
        bool flag = pause && isPlaying;
        if (flag)
        {
            RESULT result = channel.setPaused(pause);
            result.IfErrorPrintWith($"Pause -- SoundHandler instance id: {Id}");
        }
    }
}

internal class ChannelSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    public GCHandle gCHandle;
        
    public ChannelSafeHandle()
        : base(true)
    {
    }

    public void SetTheHandle(IntPtr handle)
    {
        SetHandle(handle);
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    protected override bool ReleaseHandle()
    {
        FileLog.Log("SafeHandle called ReleaseHandle()");
        if (gCHandle.IsAllocated)
        {
            gCHandle.Free();
            FileLog.Log("SafeHandle did gCHandle.Free()");
        }
        return true;
    }
}