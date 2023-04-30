using UnityEngine;

namespace COTL_API.Sounds;

public static class SoundLoaderExtensions
{
    /// <summary>
    ///     Add the SoundLoader component to a GameObject, making it able to play sound in the scene.
    /// </summary>
    /// <param name="obj">The GameObject you wish to attach the SoundLoader to.</param>
    /// <returns>The SoundLoader component attached to the GameObject.</returns>
    public static SoundLoader AddSoundLoader(this GameObject obj)
    {
        var sound = obj.AddComponent<SoundLoader>();
        SoundLoader.InstanceList.Add(sound);
        return sound;
    }

    // I don't think any more extensions are needed for now.
    // SoundHandler already has CreateAndPlayMusic() and CreateAndPlaySFZ(), which are already short, and I think they're enough.
}