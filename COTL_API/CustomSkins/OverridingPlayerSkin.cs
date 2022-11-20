using Spine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace COTL_API.CustomSkins;

internal class OverridingPlayerSkin : CustomPlayerSkin
{
    internal Func<Skin> overrideSkin;
    private Skin _cachedSkin;
    
    public OverridingPlayerSkin(string name, Func<Skin> overrideSkin)
    { 
        this.overrideSkin = overrideSkin;
        this.Name = name;
    }
    
    public override void Apply()
    {
        Action a = delegate()
        {
            if (_cachedSkin == null)
            {
                _cachedSkin = overrideSkin.Invoke();
            }

            if (CustomSkinManager.PlayerSkinOverride != null)
                Plugin.Instance!.Logger.LogInfo("PlayerSkinOverride already exists. Overwriting.");
            CustomSkinManager.SetPlayerSkinOverride(_cachedSkin);
        };
        if (!SkinUtils.SkinsLoaded)
        {
            SkinUtils.SkinToLoad = a;
        }
        else
        {
            a();
        }
    }

    public override string Name { get; }
    public override Texture2D Texture => null;
    public override List<SkinOverride> Overrides => null;
}