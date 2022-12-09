using FMOD;
using Microsoft.Win32.SafeHandles;
using System.Runtime.ConstrainedExecution;

namespace COTL_API.Sounds;

internal class SoundWrapper : SafeHandleZeroOrMinusOneIsInvalid
{
    private Sound? sound;
    public SoundWrapper(Sound sound)
        : base(true)
    {
        SetHandle(sound.handle);
        this.sound = sound;
        HarmonyLib.FileLog.Log($"Is handle valid: {!IsInvalid}");
    }
    public Sound GetSound() => sound ?? default;
    public void ChangeLoopMode(MODE mode) => sound?.setMode(mode); 

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    protected override bool ReleaseHandle()
    {
        sound?.release();
        HarmonyLib.FileLog.Log("Released.");
        return true;
    }
}