﻿using System.Collections.Generic;
using System.Reflection;
using COTL_API.Guid;

namespace COTL_API.CustomFollowerCommand;

public static partial class CustomFollowerCommandManager
{
    public static Dictionary<FollowerCommands, CustomFollowerCommand> CustomFollowerCommands { get; } = new();

    public static FollowerCommands Add(CustomFollowerCommand command)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var followerCommand = GuidManager.GetEnumValue<FollowerCommands>(guid, command.InternalName);
        command.Command = followerCommand;
        command.ModPrefix = guid;

        CustomFollowerCommands.Add(followerCommand, command);

        return followerCommand;
    }
}