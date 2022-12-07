using COTL_API.Helpers;
using UnityEngine;
using FMOD;

namespace COTL_API.Sounds;

public class SoundLoader : MonoBehaviour
{
    // Sound cache
    private Dictionary<string, SoundWrapper> SoundCache = new();

    // The new sound cache.
    private List<ChannelWrap> _channelList = new();

    // All existent SoundLoader instances, for management purposes.
    internal static readonly List<SoundLoader> InstanceList = new();

    private void OnDestroy()
    {
        InstanceList.Remove(this);
        StopAll();
    }

    /// <summary>
    /// Create a new Sound, which will be cached inside of SoundLoader and accessible with a string key (which will either be the file name or a name string passed as the second optional parameter).
    /// </summary>
    /// <param name="fileName">The name of your audio file.</param>
    /// <param name="name">A string key you can access the Sound with.</param>
    /// <returns>The Sound's string key.</returns>
    public unsafe string CreateSound(string fileName, string? name = null)
    {
        SoundWrapper sound = new SoundWrapper(SoundMaker.MakeSound(fileName));
        name ??= fileName;
        SoundCache.Add(name, sound);
        return name; // Return name of sound in the 'Sounds' dictionary!
    }


    // == PLAYING AUDIO ==

    /// <summary>
    /// Play a cached Sound once as a sound effect.
    /// </summary>
    /// <param name="name">The string key for the Sound you wanna play..</param>
    public unsafe void PlaySfx(string name)
    {
        if (!SoundCache.ContainsKey(name))
        {
            LogHelper.LogError($"Error playing sound {name}: Sound doesn't exist!");
        }

        var soundHandle = SoundCache[name];
        soundHandle.sound->setMode(MODE.LOOP_OFF);

        SoundMaker.PlayOneShot(soundHandle, VolumeCategory.SFX);
    }

    /// <summary>
    /// Play a cached Sound in a loop.
    /// </summary>
    /// <param name="name">The string key of the cached Sound.</param>
    public unsafe void PlayMusic(string name)
    {
        if (!SoundCache.ContainsKey(name))
        {
            LogHelper.LogError($"Error playing sound {name}: Sound doesn't exist!");
        }

        var soundHandle = SoundCache[name];
        soundHandle.sound->setMode(MODE.LOOP_NORMAL); // Music should loop

        var sh = new ChannelWrap(name, in soundHandle);
        var result = sh.Play();
        sh.SetVolume(AudioUtils.MusicVolume);
        if (result != RESULT.OK) return; // Return before adding to the Handlers list.
        _channelList.Add(sh);
    }

    /// <summary>
    /// Create a Sound from an audio file and play it in a loop.
    /// </summary>
    /// <param name="fileName">The audio file's name.</param>
    /// <param name="name">The string key with which you can access this cached Sound later.</param>
    public void CreateAndPlayMusic(string fileName, string? name = null)
    {
        var soundName = CreateSound(fileName, name);
        PlayMusic(soundName);
    }

    /// <summary>
    /// Create a Sound from an audio file and play it once as a sound effect.
    /// </summary>
    /// <param name="fileName">The audio file's name.</param>
    /// <param name="name">The string key with which you can access this cached Sound later.</param>
    public void CreateAndPlaySfx(string fileName, string? name = null)
    {
        var soundName = CreateSound(fileName, name);
        PlaySfx(soundName);
    }


    // == MANGEMENT ==

    /// <summary>
    /// Stop all music being played by this SoundLoader instance.
    /// </summary>
    public void StopAll()
    {
        _channelList.ForEach(x => x.Stop());
        _channelList.Clear();
    }

    internal void SyncAllVolume()
    {
        _channelList.ForEach(x => x.SetVolume(AudioUtils.MusicVolume));
    }

    /// <summary>
    /// Clear this SoundLoader instance's Sound cache.
    /// </summary>
    public void ClearSounds()
    {
        SoundCache.Clear();
    }


    // == CHANNEL INSTANCE ==
    private ChannelWrap? GetHandlerByID(string id)
    {
        return _channelList.FirstOrDefault(x => x.ID == id);
    }

    internal void SyncVolume(string id)
    {
        var sl = GetHandlerByID(id);
        sl?.SetVolume(AudioUtils.MusicVolume);
    }

    /// <summary>
    /// Change the volume of a Channel independently of the game's audio settings.
    /// The ID of a channel is the same as the string key of the Sound it's currently playing.
    /// </summary>
    /// <param name="id">The ID of the Channel you wish to change the volume of.</param>
    /// <param name="mul">The volume multiplier.</param>
    public void VolumeMultiplier(string id, float mul)
    {
        var sl = GetHandlerByID(id);
        sl?.SetMultiplier(mul);
        sl?.SetVolume(AudioUtils.MusicVolume);
    }

    // == PLAYBACK CONTROLS ==

    /// <summary>
    /// Stops all sound in a Channel by ID.
    /// The ID of a channel is the same as the string key of the Sound it's currently playing.
    /// </summary>
    /// <param name="name">The ID of the channel.</param>
    public void Stop(string name)
    {
        var sh = GetHandlerByID(name);
        sh?.Stop();
        if (sh != null) _channelList.Remove(sh);
    }

    /// <summary>
    /// Pause all sound in a Channel by ID.
    /// The ID of a channel is the same as the string key of the Sound it's currently playing.
    /// </summary>
    /// <param name="name">The ID of the channel.</param>
    /// <param name="pause"><c>true</c> to pause, <c>false</c> to unpause.</param>
    public void Pause(string name, bool pause)
    {
        var sh = GetHandlerByID(name);
        sh?.Pause(pause);
    }

    /// <summary>
    /// Checks if a Sound is being played in any channel.
    /// </summary>
    /// <param name="name">The string key of the Sound.</param>
    /// <returns><c>true</c> if any channel is playing the sound; <c>false</c> if not.</returns>
    public bool IsPlaying(string name)
    {
        var sh = GetHandlerByID(name);
        return sh?.IsPlaying() ?? false;
    }

    /// <summary>
    /// Checks if a Channel is paused.
    /// The ID of a channel is the same as the string key of the Sound it is (or was) currently playing.
    /// </summary>
    /// <param name="name">The string key of the Sound.</param>
    /// <returns><c>true</c> if the Channel is paused, <c>false</c> if not.</returns>
    public bool IsPaused(string name)
    {
        var sh = GetHandlerByID(name);
        return sh?.IsPaused() ?? false;
    }
}