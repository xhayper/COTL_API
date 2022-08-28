using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace COTL_API.Helpers;

public class TextureHelper
{
    public static Dictionary<string, Sprite> spriteCache = new();
    public static Dictionary<string, Texture2D> textureCache = new();

    public static Texture2D CreateTextureFromPath(string path, TextureFormat textureFormat = TextureFormat.RGBA32,
        bool mipmaps = false, bool linear = false)
    {
        if (textureCache.ContainsKey(path)) return textureCache[path];
        Texture2D tex = new(1, 1, textureFormat, mipmaps, linear);
        tex.filterMode = FilterMode.Point;
        tex.LoadImage(File.ReadAllBytes(path));
        textureCache[path] = tex;
        return tex;
    }

    public static Sprite CreateSpriteFromPath(string path)
    {
        if (spriteCache.ContainsKey(path)) return spriteCache[path];
        Texture2D tex = CreateTextureFromPath(path);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
        spriteCache[path] = sprite;
        return sprite;
    }
}