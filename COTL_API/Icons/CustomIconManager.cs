using TMPro;
using UnityEngine;
using UnityEngine.TextCore;
using ShaderUtilities = TMPro.ShaderUtilities;

namespace COTL_API.Icons;

internal static class CustomIconManager
{
    private static Dictionary<Sprite, TMP_SpriteAsset> Icons { get; } = new();

    internal static TMP_SpriteAsset GetIcon(Sprite icon, string name, GlyphMetrics iconMetrics)
    {
        if (Icons.TryGetValue(icon, out var iconCache)) return iconCache;

        icon.name = name;

        var asset = CreateAssetFor(icon, iconMetrics);
        Icons.Add(icon, asset);
        return asset;
    }

    private static TMP_SpriteAsset CreateAssetFor(Sprite sprite, GlyphMetrics iconMetrics)
    {
        var texture = sprite.texture;
        var spriteAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
        spriteAsset.version = "1.1.0";
        spriteAsset.name = sprite.name;
        spriteAsset.hashCode = TMP_TextUtilities.GetSimpleHashCode(spriteAsset.name);
        spriteAsset.spriteSheet = texture;

        spriteAsset.spriteGlyphTable = [];
        spriteAsset.spriteCharacterTable = [];

        TMP_SpriteGlyph spriteGlyph = new()
        {
            index = 0,
            metrics = iconMetrics,
            glyphRect = new GlyphRect(sprite.rect),
            scale = 1.25f,
            sprite = sprite
        };

        spriteAsset.spriteGlyphTable.Add(spriteGlyph);

        TMP_SpriteCharacter spriteCharacter = new(0, spriteGlyph)
        {
            name = sprite.name,
            scale = 1.25f
        };

        spriteAsset.spriteCharacterTable.Add(spriteCharacter);

        var shader = Shader.Find("TextMeshPro/Sprite");
        Material material = new(shader);
        material.SetTexture(ShaderUtilities.ID_MainTex, spriteAsset.spriteSheet);

        spriteAsset.material = material;

        spriteAsset.UpdateLookupTables();

        return spriteAsset;
    }
}