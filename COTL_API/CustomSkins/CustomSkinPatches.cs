using HarmonyLib;
using Lamb.UI;
using LeTai.Asset.TranslucentImage;
using Spine;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Object = UnityEngine.Object;

namespace COTL_API.CustomSkins;

[HarmonyPatch]
public partial class CustomSkinManager
{
    internal static readonly Dictionary<string, Texture2D> CachedTextures = new();

    [HarmonyPatch(typeof(SkeletonData), nameof(SkeletonData.FindSkin), typeof(string))]
    [HarmonyPostfix]
    private static void SkeletonData_FindSkin(ref Skin? __result, SkeletonData __instance, string skinName)
    {
        if (__result != null) return;
        if (skinName.StartsWith("CustomTarotSkin_"))
        {
            __result = CreateOrGetTarotSkinFromTemplate(__instance, skinName);
        }
        if (CustomFollowerSkins.ContainsKey(skinName)) {
            if (AlwaysUnlockedSkins[skinName]) DataManager.SetFollowerSkinUnlocked(skinName);
            __result = CustomFollowerSkins[skinName];
        }
    }

    [HarmonyPatch(typeof(Graphics), nameof(Graphics.CopyTexture), typeof(Texture), typeof(int), typeof(int),
        typeof(int), typeof(int), typeof(int), typeof(int), typeof(Texture), typeof(int), typeof(int), typeof(int),
        typeof(int))]
    [HarmonyPrefix]
    private static bool Graphics_CopyTexture(ref Texture src, int srcElement, int srcMip, int srcX, int srcY,
        int srcWidth, int srcHeight, ref Texture dst, int dstElement, int dstMip, int dstX, int dstY)
    {
        if (src is not Texture2D s2d) return true;
        if (src.graphicsFormat == dst.graphicsFormat) return false;

        Texture2D orig;
        if (CachedTextures.TryGetValue(src.name, out var cached))
        {
            LogDebug($"Using cached texture {src.name} ({cached.width}x{cached.height})");
            orig = cached;
        }
        else
        {
            LogDebug(
                $"Copying texture {src.name} ({src.width}x{src.height}) to {dst.name} ({src.width}x{src.height} with different formats: {src.graphicsFormat} to {dst.graphicsFormat}");
            orig = DuplicateTexture(s2d, dst.graphicsFormat);
            CachedTextures[src.name] = orig;
        }

        var dst2d = (Texture2D)dst;
        var fullPix = orig.GetPixels32();
        var croppedPix = new Color32[srcWidth * srcHeight];
        for (var i = 0; i < srcHeight; i++)
        for (var j = 0; j < srcWidth; j++)
            croppedPix[i * srcWidth + j] = fullPix[(i + srcY) * orig.width + j + srcX];

        dst2d.SetPixels32(croppedPix);

        return false;
    }

    private static Texture2D DuplicateTexture(Texture source, GraphicsFormat format)
    {
        var renderTex = RenderTexture.GetTemporary(
            source.width,
            source.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear
        );

        Graphics.Blit(source, renderTex);
        var previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new(source.width, source.height, format, TextureCreationFlags.None);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    [HarmonyPatch(typeof(FollowerInformationBox), nameof(FollowerInformationBox.ConfigureImpl))]
    [HarmonyPostfix]
    private static void FollowerInformationBox_ConfigureImpl(FollowerInformationBox __instance)
    {
        if (SkinTextures.ContainsKey(__instance.FollowerInfo.SkinName))
            __instance.FollowerSpine.Skeleton.Skin = CustomFollowerSkins[__instance.FollowerInfo.SkinName];
    }

    // TODO: Temp fix. Destroy the transparent image used when recruiting follower. It hides custom meshes due to render order.
    // Need to find out how to fix render order.
    [HarmonyPatch(typeof(UIFollowerIndoctrinationMenuController),
        nameof(UIFollowerIndoctrinationMenuController.OnShowStarted))]
    [HarmonyPostfix]
    private static void UIFollowerIndoctrinationMenuController_OnShowStarted(
        UIFollowerIndoctrinationMenuController __instance)
    {
        var image = __instance.gameObject.GetComponentsInChildren(typeof(TranslucentImage))[0].gameObject;
        Object.Destroy(image);
    }

    [HarmonyPatch(typeof(PlayerFarming), nameof(PlayerFarming.SetSkin), typeof(bool))]
    [HarmonyPrefix]
    private static bool PlayerFarming_SetSkin(ref Skin __result, PlayerFarming __instance, bool BlackAndWhite)
    {
        SkinUtils.InvokeOnFindSkin();

        if (PlayerSkinOverride == null) return true;

        __instance.PlayerSkin = new Skin("Player Skin");
        var skin = PlayerSkinOverride[0] ??
                   __instance.Spine.Skeleton.Data.FindSkin("Lamb_" + DataManager.Instance.PlayerFleece +
                                                           (BlackAndWhite ? "_BW" : ""));
        __instance.PlayerSkin.AddSkin(skin);
        var text = WeaponData.Skins.Normal.ToString();
        if (DataManager.Instance.CurrentWeapon != EquipmentType.None)
            text = EquipmentManager.GetWeaponData(DataManager.Instance.CurrentWeapon).Skin.ToString();

        var skin2 = __instance.Spine.Skeleton.Data.FindSkin("Weapons/" + text);
        __instance.PlayerSkin.AddSkin(skin2);
        if (__instance.health.HP + __instance.health.BlackHearts + __instance.health.BlueHearts +
            __instance.health.SpiritHearts <= 1f && DataManager.Instance.PLAYER_TOTAL_HEALTH != 2f)
        {
            var skin3 = PlayerSkinOverride[2] ?? PlayerSkinOverride[1] ??
                PlayerSkinOverride[0] ?? __instance.Spine.Skeleton.Data.FindSkin("Hurt2");
            __instance.PlayerSkin.AddSkin(skin3);
        }
        else if ((__instance.health.HP + __instance.health.BlackHearts + __instance.health.BlueHearts +
                     __instance.health.SpiritHearts <= 2f && DataManager.Instance.PLAYER_TOTAL_HEALTH != 2f) ||
                 (__instance.health.HP + __instance.health.BlackHearts + __instance.health.BlueHearts +
                     __instance.health.SpiritHearts <= 1f && DataManager.Instance.PLAYER_TOTAL_HEALTH == 2f))
        {
            var skin4 = PlayerSkinOverride[1] ?? PlayerSkinOverride[2] ??
                PlayerSkinOverride[0] ?? __instance.Spine.Skeleton.Data.FindSkin("Hurt1");
            __instance.PlayerSkin.AddSkin(skin4);
        }

        __instance.Spine.Skeleton.SetSkin(__instance.PlayerSkin);
        __instance.Spine.Skeleton.SetSlotsToSetupPose();
        __result = __instance.PlayerSkin;
        return false;
    }
}