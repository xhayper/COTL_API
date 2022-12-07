using FMOD;
using Microsoft.Win32.SafeHandles;
using System.Runtime.ConstrainedExecution;

namespace COTL_API.Sounds;

internal unsafe class SoundHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal Sound* sound;

    public SoundHandle(Sound *sound)
        : base(true)
    {
        this.sound = sound;
        SetHandle(sound->handle);
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    protected override bool ReleaseHandle()
    {
        HarmonyLib.FileLog.Log($"About to release handle. Handle is valid: {!this.IsInvalid}");
        sound->release();
        HarmonyLib.FileLog.Log($"Have released handle. Handle is valid: {!this.IsInvalid}");
        return true;
    }
}
