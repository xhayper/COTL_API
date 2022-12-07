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

public static class AudioUtils
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
}