using System.Text;
using UnityEngine;

namespace COTL_API.CustomSkins;

public abstract class CustomSkin
{
    public abstract string Name { get; }

    public abstract Texture2D Texture { get; }
    public abstract List<SkinOverride> Overrides { get; }

    public class SkinOverride
    {
        public SkinOverride(string name, Rect rect, Vector2 scale, Vector2 translate)
        {
            Name = name;
            Rect = rect;
            Scale = scale;
            Translate = translate;
        }

        public SkinOverride(string name, Rect rect, float scaleX = 1f, float scaleY = 1f, float translateX = 0f,
            float translateY = 0f)
        {
            Name = name;
            Rect = rect;
            Scale = new Vector2(scaleX, scaleY);
            Translate = new Vector2(translateX, translateY);
        }

        public SkinOverride(string name, Rect rect, Tuple<float, float>? scale, Tuple<float, float>? translate)
        {
            Name = name;
            Rect = rect;
            Scale = scale != null ? new Vector2(scale.Item1, scale.Item2) : Vector2.one;
            Translate = translate != null ? new Vector2(translate.Item1, translate.Item2) : Vector2.zero;
        }

        public string Name { get; }
        public Rect Rect { get; }

        public Vector2 Scale { get; private set; }
        public Vector2 Translate { get; private set; }
    }

    public string GenerateAtlasText()
    {
        StringBuilder sb = new();
        sb.AppendLine();
        sb.AppendLine($"{Name}");
        sb.AppendLine($"size: {Texture.width}, {Texture.height}");
        sb.AppendLine("format: RGBA8888");
        sb.AppendLine("filter: Linear,Linear");
        sb.AppendLine("repeat: none");
        foreach (var skinOverride in Overrides)
        {
            sb.AppendLine(
                $"{skinOverride.Name}#{skinOverride.Scale.x},{skinOverride.Scale.y},{skinOverride.Translate.x},{skinOverride.Translate.y}");
            sb.AppendLine("  rotate: false");
            sb.AppendLine($"  xy: {skinOverride.Rect.x},{skinOverride.Rect.y}");
            sb.AppendLine($"  size: {skinOverride.Rect.width},{skinOverride.Rect.height}");
            sb.AppendLine($"  orig: {skinOverride.Rect.width},{skinOverride.Rect.height}");
            sb.AppendLine("  offset: 0,0");
            sb.AppendLine("  index: -1");
        }

        return sb.ToString();
    }
}