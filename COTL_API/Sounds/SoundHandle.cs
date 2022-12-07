using FMOD;
using HarmonyLib;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;
using Unity.Audio;

namespace COTL_API.Sounds;

internal unsafe class SoundWrapper : SafeHandleZeroOrMinusOneIsInvalid
{
    internal Sound* sound;

    public SoundWrapper(Sound *sound)
        : base(true)
    {
        this.sound = sound;
        SetHandle(sound->handle);
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    protected override bool ReleaseHandle()
    {
        sound->release();
        return true;
    }
}
