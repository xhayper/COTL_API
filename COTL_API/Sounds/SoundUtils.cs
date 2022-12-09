using COTL_API.Helpers;
using FMODUnity;
using BepInEx;
using FMOD;

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
    /// The game's Master Volume.
    /// </summary>
    public static float MasterVolume => SettingsManager.Settings.Audio.MasterVolume;

    /// <summary>
    /// The game's Music Volume.
    /// </summary>
    public static float MusicVolume => SettingsManager.Settings.Audio.MusicVolume * MasterVolume;

    /// <summary>
    /// The game's SFX Volume.
    /// </summary>
    public static float SfxVolume => SettingsManager.Settings.Audio.SFXVolume * MasterVolume;

    /// <summary>
    /// The game's VO Volume.
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
        if (result != RESULT.OK)
        {
            LogHelper.LogError($"Sound Error: {result} caught at {where}");
        }
    }

    internal static string? GetPath(string fileName)
    {
        var files = Directory.GetFiles(Paths.PluginPath, fileName, SearchOption.AllDirectories);

        switch (files.Length)
        {
            case 0:
                LogHelper.LogError($"Error: Couldn't find \"{fileName}\"");
                return null;
            case > 1:
                LogHelper.LogWarning(
                        $"More than one file named \"{fileName}\" found. This may lead to weird behavior.");
                break;
        }

        return files.First();
    }

    internal static Sound MakeSound(string fileName, bool loop = false)
    {
        string? path = SoundUtils.GetPath(fileName);
        if (path == null) return default;

        var system = RuntimeManager.CoreSystem;
        var mode = loop ? MODE.LOOP_NORMAL : MODE.LOOP_OFF;

        var result = system.createSound(path, mode, out var sound);
        if (result == RESULT.OK)
        {
            return sound;
        }

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
        var soundHandle = new SoundWrapper(MakeSound(path));
        return PlayOneShot(soundHandle, volume);
    }
}