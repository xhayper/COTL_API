using UnityEngine;

namespace COTL_API.Helpers;

public static class TextureHelper
{
    private static readonly Dictionary<string, Sprite> SpriteCache = [];
    private static readonly Dictionary<string, Texture2D> TextureCache = [];

    public static Texture2D CreateTextureFromPath(string path, TextureFormat textureFormat = TextureFormat.RGBA32,
        bool mipmaps = false, bool linear = false)
    {
        if (TextureCache.TryGetValue(path, out var textureCache)) return textureCache;

        Texture2D tex = new(1, 1, textureFormat, mipmaps, linear)
        {
            filterMode = FilterMode.Point
        };
        tex.LoadImage(File.ReadAllBytes(path));
        TextureCache[path] = tex;
        return tex;
    }

    public static Sprite CreateSpriteFromPath(string path)
    {
        if (SpriteCache.TryGetValue(path, out var spriteCache)) return spriteCache;

        var tex = CreateTextureFromPath(path);
        var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        SpriteCache[path] = sprite;
        return sprite;
    }
}