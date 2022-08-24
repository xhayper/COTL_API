using COTL_API.CustomInventory;
using COTL_API.Helpers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using static TMPro.TMP_Text;

namespace COTL_API.Icons;

[HarmonyPatch]
public class IconPatcher
{
    [HarmonyPatch(typeof(TMP_SpriteAsset), "SearchForSpriteByHashCode")]
    [HarmonyPrefix]
    public static bool TMP_SpriteAsset_SearchForSpriteByHashCode(TMP_SpriteAsset spriteAsset, int hashCode, bool includeFallbacks, ref int spriteIndex, ref TMP_SpriteAsset __result)
    {
        if (spriteAsset == null)
        {
            return true;
        }
        foreach (CustomInventoryItem item in CustomItemManager.customItems.Values)
        {
            string name = $"icon_{item.ModPrefix}.${item.InternalName}";
            if (hashCode == HashCode.GetValueHashCode(name)) {
                spriteIndex = 0;
                __result = IconManager.GetIcon(item.InventoryIcon, name, spriteAsset.material.shader, hashCode);
                return false;
            }
        }
        return true;
    }
}
