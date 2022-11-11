using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using FMOD;

namespace COTL_API.Sounds;
public class SoundLoader : MonoBehaviour
{
    /// <summary>
    /// All Sounds loaded in this SoundLoader instance, accessible through their respective string keys.
    /// </summary>
    public Dictionary<string, Sound> Sounds = new Dictionary<string, Sound>();

    // SoundHandlers -- For looping audio that requires volume-syncing.
    private List<SoundHandler> Handlers = new List<SoundHandler>();

    // All existent SoundLoader instances, for management purposes.
    internal static List<SoundLoader> AllInstances = new List<SoundLoader>();

    private void Start()
    {
    }

    private void OnDestroy()
    {
        AllInstances.Remove(this);
        StopAll();
    }


    /// <summary>
    /// Create a new Sound, which will be cached inside of SoundLoader and accessible with a string key (which will either be the file name or a name string passed as the second optional parameter).
    /// </summary>
    /// <param name="fileName">The name of your audio file.</param>
    /// <param name="name">A string key you can access the Sound with.</param>
    /// <returns>The Sound's string key.</returns>
    public string CreateSound(string fileName, string name = null)
    {
        Sound sound = SoundHelpers.MakeSound(fileName);
        name ??= fileName;
        Sounds.Add(name, sound);
        return name; // Return name of sound in the 'Sounds' dictionary!
    }

    /// <summary>
    /// Get a Sound cached in this SoundLoader instance by its string key.
    /// </summary>
    /// <param name="name">The Sound's string key.</param>
    /// <returns>The Sound that matches that string key, or an empty Sound if none is found.</returns>
    public Sound GetSound(string name)
    {
        if (!Sounds.ContainsKey(name))
        {
            Plugin.Logger.LogError($"Couldn't get sound {name}: Sound doesn't exist!");
            return new Sound(); // Return empty Sound
        }
        return Sounds[name];
    }

    /// <summary>
    /// Add a Sound to this SoundLoader instance's cache.
    /// </summary>
    /// <param name="sound"></param>
    /// <param name="name"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Play a cached Sound once as a sound effect.
    /// </summary>
    /// <param name="name">The string key for the Sound you wanna play..</param>
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
    
    /// <summary>
    /// Play a cached Sound in a loop.
    /// </summary>
    /// <param name="name">The string key of the cached Sound.</param>
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

    /// <summary>
    /// Create a Sound from an audio file and play it in a loop.
    /// </summary>
    /// <param name="fileName">The audio file's name.</param>
    /// <param name="name">The string key with which you can access this cached Sound later.</param>
    public void CreateAndPlayMusic(string fileName, string name = null)
    {
        string soundName = CreateSound(fileName, name);
        PlayMusic(soundName);
    }

    /// <summary>
    /// Create a Sound from an audio file and play it once as a sound effect.
    /// </summary>
    /// <param name="fileName">The audio file's name.</param>
    /// <param name="name">The string key with which you can access this cached Sound later.</param>
    public void CreateAndPlaySFX(string fileName, string name = null)
    {
        string soundName = CreateSound(fileName, name);
        PlaySFX(soundName);
    }


    // == MANGEMENT ==

    /// <summary>
    /// Stop all music being played by this SoundLoader instance.
    /// </summary>
    public void StopAll()
    {
        Handlers.ForEach(x => x.Stop());
        Handlers = new List<SoundHandler>(); // Empty list.
    }

    internal void SyncAllVolume()
    {
        Handlers.ForEach(x => x.SetVolume(SoundHelpers.MusicVolume));
    }

    /// <summary>
    /// Clear this SoundLoader instance's Sound cache.
    /// </summary>
    public void ClearSounds()
    {
        // Clear all Sounds from the dictionary
        Sounds = new Dictionary<string, Sound>();
    }


    // == CHANNEL INSTANCE ==
    private SoundHandler GetHandlerByID(string id)
    {
        return Handlers.Where(x => x.Id == id).FirstOrDefault();
    }

    internal void SyncVolume(string id)
    {
        SoundHandler sl = GetHandlerByID(id);
        sl?.SetVolume(SoundHelpers.MusicVolume);
    }

    /// <summary>
    /// Change the volume of a Channel independently of the game's audio settings.
    /// The ID of a channel is the same as the string key of the Sound it's currently playing.
    /// </summary>
    /// <param name="id">The ID of the Channel you wish to change the volume of.</param>
    /// <param name="mul">The volume multiplier.</param>
    public void VolumeMultiplier(string id, float mul)
    {
        SoundHandler sl = GetHandlerByID(id);
        sl?.SetMultiplier(mul);
        sl?.SetVolume(SoundHelpers.MusicVolume);
    }

    // Still being tested!
    /*
    internal void SetReverb(string id, bool active, float amount)
    {
        SoundHandler sl = GetHandlerByID(id);
        sl?.SetReverb(active, amount);
    }
    /*

    // Still being tested!
    /*
    internal void SetLowPass(string id, float amount)
    {
        SoundHandler sl = GetHandlerByID(id);
        sl?.SetLowPass(amount);
    }
    */


    // == PLAYBACK CONTROLS ==

    /// <summary>
    /// Stops all sound in a Channel by ID.
    /// The ID of a channel is the same as the string key of the Sound it's currently playing.
    /// </summary>
    /// <param name="name">The ID of the channel.</param>
    public void Stop(string name)
    {
        SoundHandler sh = GetHandlerByID(name);
        sh?.Stop();
        Handlers.Remove(sh);
    }

    /// <summary>
    /// Pause all sound in a Channel by ID.
    /// The ID of a channel is the same as the string key of the Sound it's currently playing.
    /// </summary>
    /// <param name="name">The ID of the channel.</param>
    /// <param name="pause"><c>true</c> to pause, <c>false</c> to unpause.</param>
    public void Pause(string name, bool pause)
    {
        SoundHandler sh = GetHandlerByID(name);
        sh?.Pause(pause);
    }

    /// <summary>
    /// Checks if a Sound is being played in any channel.
    /// </summary>
    /// <param name="name">The string key of the Sound.</param>
    /// <returns><c>true</c> if any channel is playing the sound; <c>false</c> if not.</returns>
    public bool IsPlaying(string name)
    {
        SoundHandler sh = GetHandlerByID(name);
        return sh?.isPlaying() ?? false;
    }

    /// <summary>
    /// Checks if a Channel is paused.
    /// The ID of a channel is the same as the string key of the Sound it is (or was) currently playing.
    /// </summary>
    /// <param name="name">The string key of the Sound.</param>
    /// <returns><c>true</c> if the Channel is paused, <c>false</c> if not.</returns>
    public bool IsPaused(string name)
    {
        SoundHandler sh = GetHandlerByID(name);
        return sh?.isPaused() ?? false;
    }
}
