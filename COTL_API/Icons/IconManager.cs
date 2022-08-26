using System.Collections.Generic;
using UnityEngine.TextCore;
using UnityEngine;
using TMPro;

namespace COTL_API.Icons
{
    public class IconManager
    {
        public static Dictionary<Sprite, TMP_SpriteAsset> icons = new Dictionary<Sprite, TMP_SpriteAsset>();

        public static TMP_SpriteAsset GetIcon(Sprite icon, string name, Shader shader, int hashCode)
        {
            if (!icons.ContainsKey(icon))
            {
                icon.name = name;
                TMP_SpriteAsset asset = CreateAssetFor(icon, hashCode);
                icons.Add(icon, asset);
                return asset;
            }
            else
            {
                return icons[icon];
            }
        }

        private static TMP_SpriteAsset CreateAssetFor(Sprite sprite, int hashCode)
        {
            Texture2D texture = sprite.texture;
            TMP_SpriteAsset spriteAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
            spriteAsset.version = "1.1.0";
            spriteAsset.name = sprite.name;
            spriteAsset.hashCode = TMP_TextUtilities.GetSimpleHashCode(spriteAsset.name);
            spriteAsset.spriteSheet = texture;

            spriteAsset.spriteGlyphTable = new List<TMP_SpriteGlyph>();
            spriteAsset.spriteCharacterTable = new List<TMP_SpriteCharacter>();

            TMP_SpriteGlyph spriteGlyph = new TMP_SpriteGlyph();
            spriteGlyph.index = 0;
            spriteGlyph.metrics = new GlyphMetrics(sprite.rect.width, sprite.rect.height, 0, sprite.rect.height, sprite.rect.width);
            spriteGlyph.glyphRect = new GlyphRect(sprite.rect);
            spriteGlyph.scale = 1.0f;
            spriteGlyph.sprite = sprite;

            spriteAsset.spriteGlyphTable.Add(spriteGlyph);

            TMP_SpriteCharacter spriteCharacter = new TMP_SpriteCharacter(0, spriteGlyph);
            spriteCharacter.name = sprite.name;
            spriteCharacter.scale = 1.0f;

            spriteAsset.spriteCharacterTable.Add(spriteCharacter);


            Shader shader = Shader.Find("TextMeshPro/Sprite");
            Material material = new Material(shader);
            material.SetTexture(TMPro.ShaderUtilities.ID_MainTex, spriteAsset.spriteSheet);

            spriteAsset.material = material;

            spriteAsset.UpdateLookupTables();

            return spriteAsset;
        }
    }
}