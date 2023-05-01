using COTL_API.Helpers;
using Spine;
using UnityEngine;

namespace COTL_API.CustomSkins;

internal class OverridingPlayerSkin : CustomPlayerSkin
{
    private Skin? _cachedSkin;
    internal Func<Skin?> overrideSkin;

    public OverridingPlayerSkin(string name, Func<Skin?> overrideSkin)
    {
        this.overrideSkin = overrideSkin;
        Name = name;
    }

    public override string Name { get; }

    public override Texture2D Texture => null!;
    public override List<SkinOverride> Overrides => null!;

    public override void Apply()
    {
        void Action()
        {
            if (_cachedSkin == null) _cachedSkin = overrideSkin.Invoke();

            if (CustomSkinManager.PlayerSkinOverride != null)
                LogHelper.LogInfo("PlayerSkinOverride already exists. Overwriting.");
            CustomSkinManager.SetPlayerSkinOverride(_cachedSkin);
        }

        if (!SkinUtils.SkinsLoaded)
            SkinUtils.SkinToLoad = Action;
        else
            Action();
    }
}