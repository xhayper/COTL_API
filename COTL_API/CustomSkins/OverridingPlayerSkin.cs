using Spine;
using UnityEngine;

namespace COTL_API.CustomSkins;

internal class OverridingPlayerSkin(string name, Func<Skin?> overrideSkin) : CustomPlayerSkin
{
    private Skin? _cachedSkin;
    internal Func<Skin?> overrideSkin = overrideSkin;

    public override string Name { get; } = name;

    public override Texture2D Texture => null!;
    public override List<SkinOverride> Overrides => null!;

    public override void Apply()
    {
        void Action()
        {
            _cachedSkin ??= overrideSkin.Invoke();

            CustomSkinManager.SetPlayerSkinOverride(_cachedSkin);
        }

        if (!SkinUtils.SkinsLoaded)
            SkinUtils.SkinToLoad = Action;
        else
            Action();
    }
}