using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using COTL_API.Sounds.Helpers;

namespace COTL_API.Sounds.Load;
public class SoundLoader : MonoBehaviour
{
    // Sound cache.
    Dictionary<string, Sound> Sounds = new Dictionary<string, Sound>();

    // SoundHandlers -- For looping audio that requires volume-syncing.
    List<SoundHandler> Handlers = new List<SoundHandler>();

    // All existent SoundLoader instances, for management purposes.
    internal static List<SoundLoader> AllInstances = new List<SoundLoader>();

    void Start()
    {
    }

    void OnDestroy()
    {
        AllInstances.Remove(this);
        StopAll();
    }


    // fileName e.g. "Duck.mp3" // name e.g. "Duck"
    public string CreateSound(string fileName, string name = null)
    {
        Sound sound = SoundHelpers.MakeSound(fileName);
        name ??= fileName;
        Sounds.Add(name, sound);
        return name; // Return name of sound in the 'Sounds' dictionary!
    }

    public Sound GetSound(string name)
    {
        if (!Sounds.ContainsKey(name))
        {
            Plugin.Logger.LogError($"Couldn't get sound {name}: Sound doesn't exist!");
            return new Sound(); // Return empty Sound
        }
        return Sounds[name];
    }

    public string AddExistingSound(Sound sound, string name)
    {
        if (!Sounds.ContainsValue(sound))
        {
            Sounds.Add(name, sound);
            return name;
        }
        else
        {
            // If sound is already in dictionary, return key.
            return Sounds.Where(x => x.Value.Equals(sound)).First().Key;
        }
    }


    // == PLAYING AUDIO ==

    public void PlaySFX(string name)
    {
        if (!Sounds.ContainsKey(name))
        {
            Plugin.Logger.LogError($"Error playing sound {name}: Sound doesn't exist!");
        }
        Sound sound = Sounds[name];
        sound.setMode(MODE.LOOP_OFF);

        SoundHelpers.PlaySound(sound, Volume.SFX);
    }

    public void PlayMusic(string name)
    {
        if (!Sounds.ContainsKey(name))
        {
            Plugin.Logger.LogError($"Error playing sound {name}: Sound doesn't exist!");
        }

        Sound sound = Sounds[name];
        sound.setMode(MODE.LOOP_NORMAL); // Music should loop

        SoundHandler sh = new SoundHandler(sound, name);
        RESULT result = sh.Play();
        sh.SetVolume(SoundHelpers.MusicVolume);
        if (result != RESULT.OK) return; // Return before adding to the Handlers list.
        Handlers.Add(sh);
    }

    public void CreateAndPlayMusic(string fileName, string name = null)
    {
        string soundName = CreateSound(fileName, name);
        PlayMusic(soundName);
    }

    public void CreateAndPlaySFX(string fileName, string name = null)
    {
        string soundName = CreateSound(fileName, name);
        PlaySFX(soundName);
    }


    // == MANGEMENT ==

    // Stop all SoundHandlers instances. (aka the music!)
    public void StopAll()
    {
        Handlers.ForEach(x => x.Stop());
        Handlers = new List<SoundHandler>(); // Empty list.
    }

    public void SyncAllVolume()
    {
        Handlers.ForEach(x => x.SetVolume(SoundHelpers.MusicVolume));
    }

    public void ClearSounds()
    {
        // Clear all Sounds from the dictionary
        Sounds = new Dictionary<string, Sound>();
    }


    // == CHANNEL INSTANCE ==
    SoundHandler GetHandlerByID(string id)
    {
        return Handlers.Where(x => x.Id == id).FirstOrDefault();
    }

    public void SyncVolume(string id)
    {
        SoundHandler sl = GetHandlerByID(id);
        sl?.SetVolume(SoundHelpers.MusicVolume);
    }

    public void VolumeMultiplier(string id, float mul)
    {
        SoundHandler sl = GetHandlerByID(id);
        sl?.SetMultiplier(mul);
        sl?.SetVolume(SoundHelpers.MusicVolume);
    }

    public void SetReverb(string id, bool active, float amount)
    {
        SoundHandler sl = GetHandlerByID(id);
        sl?.SetReverb(active, amount);
    }

    public void SetLowPass(string id, float amount)
    {
        SoundHandler sl = GetHandlerByID(id);
        sl?.SetLowPass(amount);
    }


    // == PLAYBACK CONTROLS ==

    public void Stop(string name)
    {
        SoundHandler sh = GetHandlerByID(name);
        sh?.Stop();
        Handlers.Remove(sh);
    }

    public void Pause(string name, bool pause)
    {
        SoundHandler sh = GetHandlerByID(name);
        sh?.Pause(pause);
    }

    public bool IsPlaying(string name)
    {
        SoundHandler sh = GetHandlerByID(name);
        return sh?.isPlaying() ?? false;
    }

    public bool IsPaused(string name)
    {
        SoundHandler sh = GetHandlerByID(name);
        return sh?.isPaused() ?? false;
    }
}
