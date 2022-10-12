using FMOD;
using FMODUnity;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace COTL_API.Sounds.Helpers;
public static class SoundLoaderExtensions
{
    public static SoundLoader AddSoundLoader(this GameObject obj)
    {
        SoundLoader sound = obj.AddComponent<SoundLoader>();
        SoundLoader.AllInstances.Add(sound);
        return sound;
    }

    // I don't think any more extensions are needed for now.
    // SoundHandler already has CreateAndPlayMusic() and CreateAndPlaySFZ(), which are already short, and I think they're enough.
}