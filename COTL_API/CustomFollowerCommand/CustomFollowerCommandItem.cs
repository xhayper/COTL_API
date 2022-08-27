using COTL_API.Helpers;
using Lamb.UI.FollowerInteractionWheel;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace COTL_API.CustomFollowerCommand
{
    public abstract class CustomFollowerCommandItem : CommandItem
    {
        public virtual string InternalName { get; set; }
        public Sprite Icon { get; internal set; }

        public virtual List<FollowerCommandCategory> GetCategories() { return new List<FollowerCommandCategory>() { FollowerCommandCategory.DEFAULT_COMMAND }; }

        public string ModPrefix;

        public virtual List<CommandItem> GetSubCommands()
        {
            return new List<CommandItem>();
        }

        public virtual bool CheckSelectionPreconditions(Follower follower)
        {
            return true;
        }

        public abstract bool Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand = FollowerCommands.None);

        public CustomFollowerCommandItem()
        {
            Icon = TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"));
        }
    }
}