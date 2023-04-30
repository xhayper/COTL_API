using System.Runtime.ConstrainedExecution;
using FMOD;
using Microsoft.Win32.SafeHandles;

namespace COTL_API.Sounds;

internal class SoundWrapper : SafeHandleZeroOrMinusOneIsInvalid
{
    private readonly Sound? sound;

    public SoundWrapper(Sound sound)
        : base(true)
    {
        SetHandle(sound.handle);
        this.sound = sound;
    }

    public Sound GetSound()
    {
        return sound ?? default;
    }

    public void ChangeLoopMode(MODE mode)
    {
        sound?.setMode(mode);
    }


    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    protected override bool ReleaseHandle()
    {
        sound?.release();
        return true;
    }
}