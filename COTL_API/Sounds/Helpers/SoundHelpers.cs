using BepInEx;
using System.Linq;
using System.IO;
using FMOD;
using FMODUnity;

namespace COTL_API.Sounds.Helpers;
public static class SoundHelpers
{
    public static float MasterVolume => SettingsManager.Settings.Audio.MasterVolume;
    public static float MusicVolume => SettingsManager.Settings.Audio.MusicVolume * MasterVolume;
    public static float SFXVolume => SettingsManager.Settings.Audio.SFXVolume * MasterVolume;
    public static float VOVolume => SettingsManager.Settings.Audio.VOVolume * MasterVolume ;

    // Sound helpers -- all personal and not meant for users, so they're internal
    internal static Sound MakeSound(string fileName, bool loop = false)
    {
        string path = GetPath(fileName);
        if(path == null) return new Sound();

        FMOD.System system = RuntimeManager.CoreSystem;

        MODE mode = loop ? MODE.LOOP_NORMAL : MODE.LOOP_OFF;

        RESULT result = system.createSound(path, mode, out Sound sound);

        if (result != RESULT.OK)
        {
            Plugin.Logger.LogError($"Error making sound from file {fileName}!");
            result.IfErrorPrintWith($"MakeSound() -- fileName: {fileName}");

            return new Sound(); // Return empty sound in the case of an error
        }

        return sound;
    }

    internal static RESULT PlaySound(Sound sound, Volume volume = Volume.Master)
    {
        FMOD.System system = RuntimeManager.CoreSystem;
        RESULT result = system.playSound(sound, new ChannelGroup(), false, out Channel channel);
        channel.SyncVolume(volume);
        return result;
    }

    internal static RESULT SyncVolume(this Channel channel, Volume volume = Volume.Master)
    {
        float x = 0f;
        switch (volume)
        {
            case Volume.Master:
                x = MasterVolume;
                break;
            case Volume.Music:
                x = MusicVolume;
                break;
            case Volume.SFX:
                x = SFXVolume;
                break;
            case Volume.VO:
                x = VOVolume;
                break;
            default:
                goto case Volume.Master;
        }

        return channel.setVolume(x);
    }

    public static void IfErrorPrintWith(this RESULT result, string where)
    {
        if(result != RESULT.OK)
        {
            Plugin.Logger.LogError($"Sound Error: {result} caught at {where}");
        }
    }


    // Find file
    internal static string GetPath(string fileName)
    {
        string[] files = Directory.GetFiles(Paths.PluginPath, fileName, SearchOption.AllDirectories);

        if (files.Length == 0)
        {
            Plugin.Logger.LogError($"Error: Couldn't find \"{fileName}\"");
            return null;
        }
        else if (files.Length > 1)
        {
            Plugin.Logger.LogWarning($"More than one file named \"{fileName}\" found. This may lead to weird behavior.");
        }

        return files.First();
    }
}

public enum Volume
{
    Master,
    Music,
    SFX,
    VO,
}
