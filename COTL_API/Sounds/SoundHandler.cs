using UnityEngine;
using FMODUnity;
using System;
using FMOD;

namespace COTL_API.Sounds;

internal class SoundHandler
{
    // Channel through which all the sound is played.
    private Channel _channel;

    private readonly Sound _currentSound;
    public string ID { get; }

    // Volume control while still in sync with Master
    public float VolumeMultiplier = 1f;

    public SoundHandler(Sound sound, string id)
    {
        _channel = new Channel();
        _currentSound = sound;
        ID = id;
    }

    public RESULT Play()
    {
        var system = RuntimeManager.CoreSystem;
        var result = system.playSound(_currentSound, new ChannelGroup(), false, out _channel);

        if (result == RESULT.OK) return result;

        result.IfErrorPrintWith($"Play -- SoundHandler instance id: {ID}");
        return result;
    }

    public void SetVolume(float a)
    {
        var result = _channel.setVolume(a * VolumeMultiplier);
        result.IfErrorPrintWith($"SetVolume -- SoundHandler instance id: {ID}");
    }

    public void SetMultiplier(float a)
    {
        VolumeMultiplier = a;
    }

    public void Stop()
    {
        var result = _channel.stop();
        result.IfErrorPrintWith($"Stop -- SoundHandler instance id: {ID}");
    }

    public bool IsPlaying()
    {
        var result = _channel.isPlaying(out var isPlaying);
        result.IfErrorPrintWith($"isPlaying -- SoundHandler instance id: {ID}");
        return isPlaying;
    }

    public bool IsPaused()
    {
        if (!IsPlaying()) return false;

        var result = _channel.getPaused(out var isPaused);
        result.IfErrorPrintWith($"isPaused -- SoundHandler instance id: {ID}");
        return isPaused;
    }

    public void Pause(bool pause)
    {
        if (!IsPlaying()) return;

        var result = _channel.setPaused(pause);
        result.IfErrorPrintWith($"Pause -- SoundHandler instance id: {ID}");
    }

    public void SetReverb(bool active, float a)
    {
        var x = Mathf.Clamp(a, 0f, 1f) * Convert.ToInt32(active);

        // SET REVERB
        var result = _channel.setReverbProperties(0, x);
        result.IfErrorPrintWith($"SetReverb -- SoundHandler instance id: {ID}");
    }

    public void SetLowPass(float a)
    {
        a = Mathf.Clamp(a, 0f, 1f);
        var result = _channel.setLowPassGain(a); // 0 to 1
        result.IfErrorPrintWith($"SetLowPass -- SoundHandler instance id: {ID}");
    }
}