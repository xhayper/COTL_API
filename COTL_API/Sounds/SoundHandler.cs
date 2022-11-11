using System;
using FMOD;
using FMODUnity;
using UnityEngine;

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
        FMOD.System system = RuntimeManager.CoreSystem;
        RESULT result = system.playSound(currentSound, new ChannelGroup(), false, out channel);
        if (result != RESULT.OK)
        {
            result.IfErrorPrintWith($"Play -- SoundHandler instance id: {Id}");
            return result;
        }

        return result;
    }

    public void SetVolume(float a)
    {
        RESULT result = channel.setVolume(a * volumeMultiplier);
        result.IfErrorPrintWith($"SetVolume -- SoundHandler instance id: {Id}");
    }

    public void SetMultiplier(float a)
    {
        volumeMultiplier = a;
    }

    public void Stop()
    {
        RESULT result = channel.stop();
        result.IfErrorPrintWith($"Stop -- SoundHandler instance id: {Id}");
    }

    public bool isPlaying()
    {
        RESULT result = channel.isPlaying(out bool isPlaying);
        result.IfErrorPrintWith($"isPlaying -- SoundHandler instance id: {Id}");
        return isPlaying;
    }

    public bool isPaused()
    {
        if (!isPlaying()) return false;

        RESULT result = channel.getPaused(out bool isPaused);
        result.IfErrorPrintWith($"isPaused -- SoundHandler instance id: {Id}");
        return isPaused;
    }

    public void Pause(bool pause)
    {
        if (isPlaying())
        {
            RESULT result = channel.setPaused(pause);
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
        RESULT result = channel.setReverbProperties(0, x);
        result.IfErrorPrintWith($"SetReverb -- SoundHandler instance id: {Id}");
    }
    public void SetLowPass(float a)
    {
        Mathf.Clamp(a, 0f, 1f);
        RESULT result = channel.setLowPassGain(a); // 0 to 1
        result.IfErrorPrintWith($"SetLowPass -- SoundHandler instance id: {Id}");
    }
}