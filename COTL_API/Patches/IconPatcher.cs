using COTL_API.CustomFollowerCommand;
using COTL_API.CustomInventory;
using UnityEngine.TextCore;
using COTL_API.Helpers;
using COTL_API.Icons;
using HarmonyLib;
using TMPro;

namespace COTL_API.Patches;

[HarmonyPatch]
public static class IconPatcher
{
    [HarmonyPatch(typeof(TMP_SpriteAsset), nameof(TMP_SpriteAsset.SearchForSpriteByHashCode))]
    [HarmonyPrefix]
    public static bool TMP_SpriteAsset_SearchForSpriteByHashCode(TMP_SpriteAsset spriteAsset, int hashCode,
        bool includeFallbacks, ref int spriteIndex, ref TMP_SpriteAsset __result)
    {
        if (spriteAsset == null) return true;

        foreach (var item in CustomItemManager.CustomItemList.Values)
        {
            var name = $"icon_ITEM_{item.ModPrefix}.{item.InternalName}";
            if (hashCode != HashCode.GetValueHashCode(name)) continue;
            spriteIndex = 0;
            var sprite = item.InventoryIcon;
            GlyphMetrics metrics = new(sprite.rect.width, sprite.rect.height, 0, sprite.rect.height * 0.75f,
                sprite.rect.width);
            __result = CustomIconManager.GetIcon(sprite, name, metrics);
            return false;
        }

        foreach (var item in CustomFollowerCommandManager.CustomFollowerCommandList
                     .Values)
        {
            var name = $"icon_FCOMMAND_{item.ModPrefix}.{item.InternalName}";
            if (hashCode != HashCode.GetValueHashCode(name)) continue;
            spriteIndex = 0;
            var sprite = item.CommandIcon;
            GlyphMetrics metrics = new(sprite.rect.width, sprite.rect.height, 0, sprite.rect.height, sprite.rect.width);
            __result = CustomIconManager.GetIcon(sprite, name, metrics);
            return false;
        }

        return true;
    }
}