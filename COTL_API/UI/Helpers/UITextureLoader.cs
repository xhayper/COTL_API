using BepInEx;
using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

namespace COTL_API.UI.Helpers;
internal class UITextureLoader
{
    // Yes, I know there's already a TextureHelper class.
    // I wanted a bit more flexibility. I'll switch if you tell me to. This is just an internal helper method I wanna use.

    public static Sprite MakeSprite(string path, FilterMode filter = FilterMode.Bilinear)
    {
        string[] imgs = Directory.GetFiles(Paths.PluginPath, path, SearchOption.AllDirectories);

        if (imgs.Length == 0)
        {
            Plugin.Logger.LogError($"Couldn't find image \"{path}\"!");
            return new Sprite(); // Empty sprite if image not found
        }

        byte[] array = File.ReadAllBytes(imgs[0]);

        Texture2D tex = new Texture2D(1, 1);
        tex.filterMode = filter;
        ImageConversion.LoadImage(tex, array);

        Rect texRect = new Rect(0, 0, tex.width, tex.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        return Sprite.Create(tex, texRect, pivot);
    }
}
