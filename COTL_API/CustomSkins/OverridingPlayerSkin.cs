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

    public override void Apply(PlayerType who = PlayerType.P1)
    {
        void Action()
        {
            _cachedSkin ??= overrideSkin.Invoke();

            CustomSkinManager.SetPlayerSkinOverride(who, _cachedSkin);
        }

        if (who == PlayerType.P1)
        {
            if (!SkinUtils.SkinP1Loaded)
                SkinUtils.SkinP1ToLoad = Action;
            else
                Action();
        }
        else
        {
            if (!SkinUtils.SkinP2Loaded)
                SkinUtils.SkinP2ToLoad = Action;
            else
                Action();
        }
    }
}