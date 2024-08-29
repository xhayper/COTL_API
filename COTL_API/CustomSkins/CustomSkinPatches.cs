using System.Collections;
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
    internal static readonly Dictionary<string, Texture2D> CachedTextures = [];

    [HarmonyPatch(typeof(SkeletonData), nameof(SkeletonData.FindSkin), typeof(string))]
    [HarmonyPostfix]
    private static void SkeletonData_FindSkin(ref Skin? __result, SkeletonData __instance, string skinName)
    {
        if (__result != null) return;

        if (skinName.StartsWith("CustomTarotSkin/"))
        {
            __result = CreateOrGetTarotSkinFromTemplate(__instance, skinName);
            return;
        }

        if (CustomFollowerSkins.TryGetValue(skinName, out var skin)) __result = skin;
        if (AlwaysUnlockedSkins.TryGetValue(skinName, out var alwaysUnlocked) && alwaysUnlocked)
            DataManager.SetFollowerSkinUnlocked(skinName);
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
        var playerType = !__instance.isLamb || __instance.IsGoat ? PlayerType.GOAT : PlayerType.LAMB;

        SkinUtils.InvokeOnFindSkin(playerType);

        if (!PlayerSkinOverride.ContainsKey(playerType)) return true;

        var skinToUse = PlayerSkinOverride[playerType];
        if (skinToUse == null) return true;

        __instance.IsGoat = DataManager.Instance.PlayerVisualFleece == 1003;
        __instance.PlayerSkin = new Skin("Player Skin");

        var skin = skinToUse[0];

        __instance.PlayerSkin.AddSkin(skin);
        var text = WeaponData.Skins.Normal.ToString();

        if (__instance.currentWeapon != EquipmentType.None)
            text = EquipmentManager.GetWeaponData(__instance.currentWeapon).Skin.ToString();

        var skin2 = __instance.Spine.Skeleton.Data.FindSkin("Weapons/" + text);
        __instance.PlayerSkin.AddSkin(skin2);

        if (__instance.health.HP + __instance.health.BlackHearts + __instance.health.BlueHearts +
            __instance.health.SpiritHearts <= 1f && !Mathf.Approximately(DataManager.Instance.PLAYER_TOTAL_HEALTH, 2f))
        {
            var skin3 = __instance.Spine.Skeleton.Data.FindSkin("Hurt2");

            if (skinToUse[2] != null) skin3 = skinToUse[2];
            else if (skinToUse[1] != null) skin3 = skinToUse[1];
            else if (skinToUse[0] != null) skin3 = skinToUse[0];

            __instance.PlayerSkin.AddSkin(skin3);
        }
        else if ((__instance.health.HP + __instance.health.BlackHearts + __instance.health.BlueHearts +
                  __instance.health.SpiritHearts <= 2f &&
                  !Mathf.Approximately(DataManager.Instance.PLAYER_TOTAL_HEALTH, 2f)) ||
                 (__instance.health.HP + __instance.health.BlackHearts + __instance.health.BlueHearts +
                  __instance.health.SpiritHearts <= 1f &&
                  Mathf.Approximately(DataManager.Instance.PLAYER_TOTAL_HEALTH, 2f)))
        {
            var skin4 = __instance.Spine.Skeleton.Data.FindSkin("Hurt1");
            if (skinToUse != null)
            {
                if (skinToUse[1] != null) skin4 = skinToUse[1];
                else if (skinToUse[0] != null) skin4 = skinToUse[0];
            }

            __instance.PlayerSkin.AddSkin(skin4);
        }

        __instance.PlayerSkin.AddSkin(__instance.Spine.Skeleton.Data.FindSkin("Mops/" +
                                                                              Mathf.Clamp(
                                                                                  __instance.isLamb
                                                                                      ? DataManager.Instance
                                                                                          .ChoreXPLevel + 1
                                                                                      : DataManager.Instance
                                                                                          .ChoreXPLevel_Coop + 1, 0,
                                                                                  9)));
        __instance.Spine.Skeleton.SetSkin(__instance.PlayerSkin);
        __instance.Spine.Skeleton.SetSlotsToSetupPose();
        __result = __instance.PlayerSkin;
        return false;
    }

    [HarmonyPatch(typeof(PlayerFarming), nameof(PlayerFarming.BleatRoutine), MethodType.Enumerator)]
    [HarmonyPrefix]
    private static bool PlayerFarming_BleatRoutine(PlayerFarming __instance)
    {
        var playerType = !__instance.isLamb || __instance.IsGoat ? PlayerType.GOAT : PlayerType.LAMB;
        var playerInstance = !__instance.isLamb || __instance.IsGoat ? PlayerFarming.players[1] : PlayerFarming.players[0];

        if (!PlayerBleatOverride.ContainsKey(playerType)) return true;

        var bleatOverride = PlayerBleatOverride[playerType];
        if (bleatOverride == null) return true;

        PlayerFarming.Instance.StartCoroutine(BleatOverrideRoutine(playerInstance, bleatOverride));
        return false;
        
    }

    private static IEnumerator BleatOverrideRoutine(PlayerFarming instance, PlayerBleat? bleatOverride) {
        instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        
        var anim = bleatOverride switch
        {
            PlayerBleat.LAMB => "bleat",
            PlayerBleat.GOAT => "bleat-goat3",
            PlayerBleat.COWBOY => "Cowboy/yeehaw-bleat",
            _ => "bleat"
        };

        var audio = bleatOverride switch
        {
            PlayerBleat.LAMB => "event:/player/speak_to_follower_noBookPage",
            PlayerBleat.GOAT => "event:/player/goat_player/goat_bleat",
            PlayerBleat.COWBOY => "event:/player/yeehaa",
            _ => "event:/player/speak_to_follower_noBookPage"
        };

        instance.simpleSpineAnimator.Animate(anim, 0, false);
        AudioManager.Instance.PlayOneShot(audio, instance.gameObject);
        yield return new WaitForSeconds(bleatOverride == PlayerBleat.LAMB ? 0.4f : 1.25f);

        if (instance.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
            instance.state.CURRENT_STATE = StateMachine.State.Idle;
        yield return null;

    }

}