using UnityEngine;
using FMODUnity;
using FMOD;
using System.Runtime.InteropServices;

namespace COTL_API.Sounds;

internal unsafe class ChannelWrap
{
    // Channel through which all the sound is played.
    private Channel channel;
    private readonly SoundHandle soundHandle;
    private Sound GetSound() => *soundHandle.sound;
    public string ID { get; }

    // Volume control while still in sync with Master
    public float VolumeMultiplier = 1f;

    public ChannelWrap(string id, in SoundHandle sound, bool loop = false)
    {
        channel = new Channel();
        ID = id;
        soundHandle = sound;
    }

    public RESULT Play()
    {
        var system = RuntimeManager.CoreSystem;
        var result = system.playSound(GetSound(), new ChannelGroup(), false, out channel);

        if (result == RESULT.OK) return result;

        result.IfErrorPrintWith($"Play -- SoundHandler instance id: {ID}");
        return result;
    }

    public void SetVolume(float a)
    {
        var result = channel.setVolume(a * VolumeMultiplier);
        result.IfErrorPrintWith($"SetVolume -- SoundHandler instance id: {ID}");
    }

    public void SetMultiplier(float a)
    {
        VolumeMultiplier = a;
    }

    public void Stop()
    {
        var result = channel.stop();
        result.IfErrorPrintWith($"Stop -- SoundHandler instance id: {ID}");
    }

    public bool IsPlaying()
    {
        var result = channel.isPlaying(out var isPlaying);
        result.IfErrorPrintWith($"isPlaying -- SoundHandler instance id: {ID}");
        return isPlaying;
    }

    public bool IsPaused()
    {
        if (!IsPlaying()) return false;

        var result = channel.getPaused(out var isPaused);
        result.IfErrorPrintWith($"isPaused -- SoundHandler instance id: {ID}");
        return isPaused;
    }

    public void Pause(bool pause)
    {
        if (!IsPlaying()) return;

        var result = channel.setPaused(pause);
        result.IfErrorPrintWith($"Pause -- SoundHandler instance id: {ID}");
    }

    public void SetReverb(bool active, float a)
    {
        var x = Mathf.Clamp(a, 0f, 1f) * Convert.ToInt32(active);

        // SET REVERB
        var result = channel.setReverbProperties(0, x);
        result.IfErrorPrintWith($"SetReverb -- SoundHandler instance id: {ID}");
    }

    public void SetLowPass(float a)
    {
        a = Mathf.Clamp(a, 0f, 1f);
        var result = channel.setLowPassGain(a); // 0 to 1
        result.IfErrorPrintWith($"SetLowPass -- SoundHandler instance id: {ID}");
    }
}