using COTL_API.CustomFollowerCommand;
using COTL_API.CustomInventory;
using COTL_API.Helpers;
using COTL_API.Icons;
using HarmonyLib;
using TMPro;
using UnityEngine.TextCore;

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

        foreach (CustomInventoryItem item in CustomItemManager.CustomItems.Values)
        {
            string name = $"icon_ITEM_{item.ModPrefix}.{item.InternalName}";
            if (hashCode != HashCode.GetValueHashCode(name)) continue;
            spriteIndex = 0;
            var sprite = item.InventoryIcon;
            var metrics = new GlyphMetrics(sprite.rect.width, sprite.rect.height, 0, sprite.rect.height * 0.75f, sprite.rect.width);
            __result = CustomIconManager.GetIcon(sprite, name, metrics);
            return false;
        }

        foreach (CustomFollowerCommand.CustomFollowerCommand item in CustomFollowerCommandManager.CustomFollowerCommands.Values)
        {
            string name = $"icon_FCOMMAND_{item.ModPrefix}.{item.InternalName}";
            if (hashCode != HashCode.GetValueHashCode(name)) continue;
            spriteIndex = 0;
            var sprite = item.CommandIcon;
            var metrics = new GlyphMetrics(sprite.rect.width, sprite.rect.height, 0, sprite.rect.height, sprite.rect.width);
            __result = CustomIconManager.GetIcon(sprite, name, metrics);
            return false;
        }

        return true;
    }
}