# Commands

## Command Creation

To create a command, you first need to make a class overriding `CustomFollowerCommand`.  
Example:

```csharp
using COTL_API.CustomFollowerCommand;
using COTL_API.Helpers;
using System.IO;
using UnityEngine;
```

```csharp
internal class ExampleFollowerCommand : CustomFollowerCommand
{
    public override string InternalName => "Example_Follower_Command";
    public override string GetTitle(Follower follower) { return "Example Follower Command"; }
    public override string GetDescription(Follower follower) { return "This is an example follower command"; }
    public override Sprite CommandIcon => TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("Assets", "example_follower_command.png"));

    public override void Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand)
    {
        interaction.follower.Brain.MakeOld();
    }
}
```

`CustomFollowerCommand` supports the following overrides:<br>
| Type   | Name                                                                                                        | Default                                                                                             |
|--------|-------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------|
| string | InternalName                                                                                                | \[REQUIRED\]                                                                                        |
| Sprite | InventoryIcon                                                                                               | TextureHelper.CreateSpriteFromPath(PluginPaths.ResolveAssetPath("placeholder.png"))                 |
| List\  | Categories                                                                                                  | new() { FollowerCommandCategory.DEFAULT_COMMAND }                                                   |
| string | GetTitle(Follower follower)                                                                                 | LocalizationManager.GetTranslation($"FollowerInteractions/{ModPrefix}.{InternalName}")              |
| string | GetDescription(Follower follower)                                                                           | LocalizationManager.GetTranslation($"FollowerInteractions/{ModPrefix}.{InternalName}/Description")  |
| string | GetLockedDescription(Follower follower)                                                                     | LocalizationManager.GetTranslation($"FollowerInteractions/{ModPrefix}.{InternalName}/NotAvailable") |
| bool   | ShouldAppearFor(Follower follower)                                                                          | true                                                                                                |
| bool   | IsAvailable(Follower follower)                                                                              | true                                                                                                |
| void   | Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand = FollowerCommands.None) | interaction.Close()                                                                                 |

## Coroutines

Some actions, such as changing tasks, require starting a coroutine using `interaction.StartCoroutine(interaction.FrameDelayCallback())`.  
Example:

```csharp
public override void Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand)
{
    interaction.StartCoroutine(interaction.FrameDelayCallback(delegate
    {
        interaction.eventListener.PlayFollowerVO(interaction.generalAcknowledgeVO);
        interaction.follower.Brain.HardSwapToTask(new FollowerTask_Vomit());
    }));
}
```

## Adding Commands

To add a command to the game, simply use `CustomFollowerCommandManager.Add()`.  
Example:

```csharp
using COTL_API.CustomFollowerCommand;
```

```csharp
CustomFollowerCommandManager.Add(new ExampleFollowerCommand());
```

## Final Steps

For the icon to load, you need to put it in the appropriate location. For the example, this would be `/Assets/example_follower_command.png` relative to the root folder containing the .dll  
Directory structure:

```
📂plugins
 ┣📂Assets
 ┃ ┗🖼️example_follower_command.png
 ┗📜mod_name.dll
```