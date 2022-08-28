using COTL_API.CustomFollowerCommand;
using COTL_API.CustomInventory;
using COTL_API.Helpers;
using HarmonyLib;
using TMPro;

namespace COTL_API.Icons;

[HarmonyPatch]
public class IconPatcher
{
    [HarmonyPatch(typeof(TMP_SpriteAsset), nameof(TMP_SpriteAsset.SearchForSpriteByHashCode))]
    [HarmonyPrefix]
    public static bool TMP_SpriteAsset_SearchForSpriteByHashCode(TMP_SpriteAsset spriteAsset, int hashCode,
        bool includeFallbacks, ref int spriteIndex, ref TMP_SpriteAsset __result)
    {
        if (spriteAsset == null) return true;

        foreach (CustomInventoryItem item in CustomItemManager.CustomItems.Values)
        {
            string name = $"icon_ITEM_{item.ModPrefix}.${item.InternalName}";
            if (hashCode != HashCode.GetValueHashCode(name)) continue;
            spriteIndex = 0;
            __result = IconManager.GetIcon(item.InventoryIcon, name, spriteAsset.material.shader, hashCode);
            return false;
        }

        foreach (CustomFollowerCommand.CustomFollowerCommand item in CustomFollowerCommandManager.CustomFollowerCommands.Values)
        {
            string name = $"icon_FCOMMAND_{item.ModPrefix}.${item.InternalName}";
            if (hashCode != HashCode.GetValueHashCode(name)) continue;
            spriteIndex = 0;
            __result = IconManager.GetIcon(item.CommandIcon, name, spriteAsset.material.shader, hashCode);
            return false;
        }

        return true;
    }
}