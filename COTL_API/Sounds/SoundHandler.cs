using UnityEngine;
using FMODUnity;
using System;
using FMOD;

namespace COTL_API.Sounds;

internal class SoundHandler
{
    // Channel through which all the sound is played.
    private Channel channel;

    private Sound currentSound;
    private string currentSoundId;
    public string Id => currentSoundId;

    // Volume control while still in sync with Master
    float volumeMultiplier = 1f;

    public SoundHandler(Sound sound, string id)
    {
        currentSound = sound;
        currentSoundId = id;
        channel = new Channel();
    }

    public RESULT Play()
    {
        var system = RuntimeManager.CoreSystem;
        var result = system.playSound(currentSound, new ChannelGroup(), false, out channel);

        if (result == RESULT.OK) return result;

        result.IfErrorPrintWith($"Play -- SoundHandler instance id: {Id}");
        return result;
    }

    public void SetVolume(float a)
    {
        var result = channel.setVolume(a * volumeMultiplier);
        result.IfErrorPrintWith($"SetVolume -- SoundHandler instance id: {Id}");
    }

    public void SetMultiplier(float a)
    {
        volumeMultiplier = a;
    }

    public void Stop()
    {
        var result = channel.stop();
        result.IfErrorPrintWith($"Stop -- SoundHandler instance id: {Id}");
    }

    public bool IsPlaying()
    {
        var result = channel.isPlaying(out var isPlaying);
        result.IfErrorPrintWith($"isPlaying -- SoundHandler instance id: {Id}");
        return isPlaying;
    }

    public bool IsPaused()
    {
        if (!IsPlaying()) return false;

        var result = channel.getPaused(out var isPaused);
        result.IfErrorPrintWith($"isPaused -- SoundHandler instance id: {Id}");
        return isPaused;
    }

    public void Pause(bool pause)
    {
        if (!IsPlaying()) return;

        var result = channel.setPaused(pause);
        result.IfErrorPrintWith($"Pause -- SoundHandler instance id: {Id}");
    }

    public void SetReverb(bool active, float a)
    {
        // REVERB PRESET
        // REVERB_PROPERTIES prop = active ? PRESET.HALLWAY() : default;
        // system.setReverbProperties(2, ref prop);

        var x = Mathf.Clamp(a, 0f, 1f) * Convert.ToInt32(active);

        // SET REVERB
        var result = channel.setReverbProperties(0, x);
        result.IfErrorPrintWith($"SetReverb -- SoundHandler instance id: {Id}");
    }

    public void SetLowPass(float a)
    {
        a = Mathf.Clamp(a, 0f, 1f);
        var result = channel.setLowPassGain(a); // 0 to 1
        result.IfErrorPrintWith($"SetLowPass -- SoundHandler instance id: {Id}");
    }
}