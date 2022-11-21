using Spine;

namespace COTL_API.CustomSkins;

public abstract class CustomPlayerSkin : CustomSkin
{
    private Skin? _cachedSkin;

    public virtual void Apply()
    {
        void Action()
        {
            if (_cachedSkin == null)
            {
                var from = PlayerFarming.Instance.Spine.Skeleton.Data.FindSkin("Lamb");
                Skin to = new(Name);

                var overrides = SkinUtils.CreateSkinAtlas(Name, Texture, GenerateAtlasText(),
                    delegate(AtlasRegion region)
                    {
                        var simpleName = region.name;
                        var add = "";
                        if (simpleName.Contains("#"))
                        {
                            var split = simpleName.Split('#');
                            add = "#" + split[1];
                            simpleName = split[0];
                        }

                        if (from.Attachments.All(x => x.Name != simpleName)) return null;

                        var att = from.Attachments.First(x => x.Name == simpleName);
                        region.name = att.SlotIndex + ":" + att.Name + add;
                        return Tuple.Create(att.SlotIndex, att.Name);
                    }, out var mat, out var atlas);
                var overrideSkin = SkinUtils.ApplyAllOverrides(from, to, overrides, mat, atlas);
                _cachedSkin = overrideSkin;
            }

            if (CustomSkinManager.PlayerSkinOverride != null)
                Plugin.Instance!.Logger.LogInfo("PlayerSkinOverride already exists. Overwriting.");
            CustomSkinManager.SetPlayerSkinOverride(_cachedSkin);
        }

        if (!SkinUtils.SkinsLoaded)
        {
            SkinUtils.SkinToLoad = Action;
        }
        else
        {
            Action();
        }
    }
}