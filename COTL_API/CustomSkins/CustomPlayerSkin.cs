using COTL_API.Helpers;
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
                    delegate (AtlasRegion region)
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

                        var atts = from.Attachments.Where(x => x.Name == simpleName);
                        var tuples = new List<Tuple<int, string>>();
                        foreach (var att in atts)
                        {
                            region.name = att.SlotIndex + ":" + att.Name + add;
                            tuples.Add(Tuple.Create(att.SlotIndex, att.Name));
                        }
                        return tuples;
                    }, out var mat, out var atlas);
                var overrideSkin = SkinUtils.ApplyAllOverrides(from, to, overrides, mat, atlas);
                _cachedSkin = overrideSkin;
            }

            if (CustomSkinManager.PlayerSkinOverride != null)
                LogHelper.LogInfo("PlayerSkinOverride already exists. Overwriting.");
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