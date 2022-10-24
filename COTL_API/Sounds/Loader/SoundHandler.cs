using FMOD;
using FMODUnity;
using COTL_API.Sounds.Helpers;
using System;
using UnityEngine;

namespace COTL_API.Sounds.Load;
internal class SoundHandler
{
    // Channel through which all the sound is played.
    Channel handle;

    Sound _sound; // Current sound.

    string _id; // Sound id
    public string Id => _id;

    float volumeMultiplier = 1f; // Volume control while still in sync with Master

    public SoundHandler(Sound sound, string id)
    {
        _sound = sound;
        _id = id;
        handle = new Channel();
    }

    public RESULT Play()
    {
        FMOD.System system = RuntimeManager.CoreSystem;
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
        RESULT result = handle.setVolume(a * volumeMultiplier);
        result.IfErrorPrintWith($"SetVolume -- SoundHandler instance id: {Id}");
    }

    public void SetMultiplier(float a)
    {
        volumeMultiplier = a;
    }

    public void Stop()
    {
        RESULT result = handle.stop();
        result.IfErrorPrintWith($"Stop -- SoundHandler instance id: {Id}");
    }

    public bool isPlaying()
    {
        RESULT result = handle.isPlaying(out bool isPlaying);
        result.IfErrorPrintWith($"isPlaying -- SoundHandler instance id: {Id}");
        return isPlaying;
    }

    public bool isPaused()
    {
        if (!isPlaying()) return false;

        RESULT result = handle.getPaused(out bool isPaused);
        result.IfErrorPrintWith($"isPaused -- SoundHandler instance id: {Id}");
        return isPaused;
    }

    public void Pause(bool pause)
    {
        if (isPlaying())
        {
            RESULT result = handle.setPaused(pause);
            result.IfErrorPrintWith($"Pause -- SoundHandler instance id: {Id}");
        }
    }
    public void SetReverb(bool active, float a)
    {
        // REVERB PRESET
        // REVERB_PROPERTIES prop = active ? PRESET.HALLWAY() : default;
        // system.setReverbProperties(2, ref prop);

        float x = Mathf.Clamp(a, 0f, 1f) * Convert.ToInt32(active);

        // SET REVERB
        RESULT result = handle.setReverbProperties(0, x);
        result.IfErrorPrintWith($"SetReverb -- SoundHandler instance id: {Id}");
    }
    public void SetLowPass(float a)
    {
        Mathf.Clamp(a, 0f, 1f);
        RESULT result = handle.setLowPassGain(a); // 0 to 1
        result.IfErrorPrintWith($"SetLowPass -- SoundHandler instance id: {Id}");
    }
}