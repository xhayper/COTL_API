using BepInEx;
using COTL_API.Helpers;
using FMOD;
using FMODUnity;

namespace COTL_API.Sounds;

public enum VolumeCategory
{
    MASTER,
    MUSIC,
    SFX,
    VO
}

public static class SoundUtils
{
    /// <summary>
    ///     The game's Master Volume.
    /// </summary>
    public static float MasterVolume => SettingsManager.Settings.Audio.MasterVolume;

    /// <summary>
    ///     The game's Music Volume.
    /// </summary>
    public static float MusicVolume => SettingsManager.Settings.Audio.MusicVolume * MasterVolume;

    /// <summary>
    ///     The game's SFX Volume.
    /// </summary>
    public static float SfxVolume => SettingsManager.Settings.Audio.SFXVolume * MasterVolume;

    /// <summary>
    ///     The game's VO Volume.
    /// </summary>
    public static float VoVolume => SettingsManager.Settings.Audio.VOVolume * MasterVolume;

    internal static RESULT SyncVolume(this Channel channel, VolumeCategory volumeCategory = VolumeCategory.MASTER)
    {
        return channel.setVolume(GetVolume(volumeCategory));
    }

    internal static float GetVolume(VolumeCategory x)
    {
        switch (x)
        {
            case VolumeCategory.MUSIC:
                return MusicVolume;
            case VolumeCategory.SFX:
                return SfxVolume;
            case VolumeCategory.VO:
                return VoVolume;
            default:
                return MasterVolume;
        }
    }

    internal static void IfErrorPrintWith(this RESULT result, string where)
    {
        if (result == RESULT.OK) return;

        LogHelper.LogError($"Sound Error: {result} caught at {where}");
    }

    internal static string? GetPath(string fileName)
    {
        var path = Path.IsPathRooted(fileName)
            ? fileName
            : Directory.GetFiles(Paths.PluginPath, fileName, SearchOption.AllDirectories).FirstOrDefault();

        if (path == null) LogHelper.LogError($"Error: Couldn't find \"{fileName}\"");

        return path;
    }

    internal static Sound MakeSound(string fileName, bool loop = false)
    {
        var path = GetPath(fileName);
        if (path == null) return default;

        var system = RuntimeManager.CoreSystem;
        var mode = loop ? MODE.LOOP_NORMAL : MODE.LOOP_OFF;

        var result = system.createSound(path, mode, out var sound);
        if (result == RESULT.OK) return sound;

        LogHelper.LogError($"Error making sound from file {fileName}!");
        result.IfErrorPrintWith($"MakeSound() -- fileName: {fileName}");

        return default;
    }

    internal static RESULT PlayOneShot(SoundWrapper soundHandle, VolumeCategory volume = VolumeCategory.SFX)
    {
        var system = RuntimeManager.CoreSystem;
        soundHandle.ChangeLoopMode(MODE.LOOP_OFF);
        var result = system.playSound(soundHandle.GetSound(), new ChannelGroup(), false, out var channel);
        channel.SyncVolume(volume);
        return result;
    }

    internal static RESULT PlayOneShot(string path, VolumeCategory volume = VolumeCategory.SFX)
    {
        SoundWrapper soundHandle = new(MakeSound(path));
        return PlayOneShot(soundHandle, volume);
    }
}