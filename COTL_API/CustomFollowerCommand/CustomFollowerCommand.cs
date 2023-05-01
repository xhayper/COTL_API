using COTL_API.Helpers;
using I2.Loc;
using Lamb.UI.FollowerInteractionWheel;
using UnityEngine;

namespace COTL_API.CustomFollowerCommand;

public abstract class CustomFollowerCommand : CommandItem
{
    internal string ModPrefix = "";
    public abstract string InternalName { get; }

    public virtual List<FollowerCommandCategory> Categories { get; } =
        new() { FollowerCommandCategory.DEFAULT_COMMAND };

    public virtual Sprite CommandIcon { get; } =
        TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));

    public virtual string CommandStringIcon()
    {
        return $"<sprite name=\"icon_FCOMMAND_{ModPrefix}.{InternalName}\">";
    }

    public override string GetTitle(Follower follower)
    {
        return LocalizationManager.GetTranslation($"FollowerInteractions/{ModPrefix}.{InternalName}");
    }

    public override string GetDescription(Follower follower)
    {
        return LocalizationManager.GetTranslation($"FollowerInteractions/{ModPrefix}.{InternalName}/Description");
    }

    public override string GetLockedDescription(Follower follower)
    {
        return LocalizationManager.GetTranslation($"FollowerInteractions/{ModPrefix}.{InternalName}/NotAvailable");
    }

    public virtual bool ShouldAppearFor(Follower follower)
    {
        return true;
    }

    public override bool IsAvailable(Follower follower)
    {
        return true;
    }

    public virtual void Execute(interaction_FollowerInteraction interaction,
        FollowerCommands finalCommand = FollowerCommands.None)
    {
        interaction.Close(true, reshowMenu: false);
    }
}