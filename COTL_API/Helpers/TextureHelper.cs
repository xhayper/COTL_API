using UnityEngine;
using System.IO;

namespace COTL_API.Helpers;

public class TextureHelper
{
    public static Texture2D CreateTextureFromPath(string path, TextureFormat textureFormat = TextureFormat.RGBA32,
        bool mipmaps = true)
    {
        Texture2D tex = new(1, 1, textureFormat, mipmaps);
        tex.LoadImage(File.ReadAllBytes(path));
        return tex;
    }

    public static Sprite CreateSpriteFromPath(string path)
    {
        Texture2D tex = CreateTextureFromPath(path);
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
    }
}