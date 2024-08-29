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

    public override void Apply(PlayerType who = PlayerType.LAMB)
    {
        void Action()
        {
            if (who == PlayerType.LAMB && Name == "Lamb") {
                CustomSkinManager.ResetPlayerSkin(who); 
                return;
            }

            _cachedSkin ??= overrideSkin.Invoke();

            CustomSkinManager.SetPlayerSkinOverride(who, _cachedSkin);
        }

        if (who == PlayerType.LAMB)
        {
            if (!SkinUtils.LambFleeceSkinLoaded)
                SkinUtils.LambFleeceSkinToLoad = Action;
            else
                Action();
        }
        else
        {
            if (!SkinUtils.GoatFleeceSkinLoaded)
                SkinUtils.GoatFleeceToLoad = Action;
            else
                Action();
        }
    }
}