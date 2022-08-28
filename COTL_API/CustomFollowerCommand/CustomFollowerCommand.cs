using Lamb.UI.FollowerInteractionWheel;
using System.Collections.Generic;
using COTL_API.Helpers;
using UnityEngine;
using I2.Loc;

namespace COTL_API.CustomFollowerCommand;

public class CustomFollowerCommand : CommandItem
{
    public virtual string InternalName { get; }
    public string ModPrefix;

    public virtual List<FollowerCommandCategory> Categories { get; } =
        new() { FollowerCommandCategory.DEFAULT_COMMAND };

    public virtual Sprite CommandIcon { get; } =
        TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));

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

    public virtual void Execute(interaction_FollowerInteraction interaction,
        FollowerCommands finalCommand = FollowerCommands.None)
    {
        // return true;
    }
}