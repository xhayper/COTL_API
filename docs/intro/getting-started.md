# Creating the Project

The first step to creating any mod is to create a project for it. It is recommended to use the [Template Mod](https://github.com/IngoHHacks/CotLTemplateMod) the first time you start making mods with the API.  
After downloading the template mod or otherwise creating the project, you should fill in the plugin info in your main class (`Plugin.cs` in the template mod). The plugin info should usually look something like this:

```csharp
public const string PluginGuid = "IngoH.cotl.CotLTemplateMod";
public const string PluginName = "CotLTemplateMod";
public const string PluginVer = "1.0.0";
```

`PluginGuid` is the GUID of your mod. It should be unique and never change between versions. Using the format `<Creator>.cotl.<ModName>` or `io.github.<GitHubUsername>.<GitHubModName>` (only if uploading to GitHub) is encouraged.  
`PluginName` is the name of your mod. This can be anything.  
`PluginVer` is the version of your mod in [SemVer](https://semver.org/) format.
After filling in the info, you're all set to get started with modding. Refer to the FEATURES section in the left sidebar for information on the specific parts of the API.

## Building the Mod

To actually use the mod, you have to build it. Most IDEs have a `Build` option in the top menu bar.  
From this menu, select `Build Project / Build <ModName>` (Ctrl+B) to build the mod, then navigate to `bin/Debug/<ModName>` to find the DLL file and copy it to the `BepInEx/plugins` folder.

## Publishing the Mod

If you want to publish your mod to mod-hosting sites, such as Thunderstore and Nexus Mods, you need to package your mod in the appropriate format.  
Thunderstore uses the following format:

```
📁ModName.zip
 ┣📂plugins
 ┃  ┣📂Assets (Optional*)
 ┃  ┃ ┣🖼️custom_icon.png (Example)
 ┃  ┃ ┗❓...
 ┃  ┗📜mod_name.dll
 ┣🖼️icon.png
 ┣📃manifest.json
 ┗📃README.md
 * You can have any number of subfolders and files with any name in /plugins. Just ensure they can be found by your mod.
```

**Important note**: Ensure that the required files are in the **root** of the zip file.  
For more information on how to publish mods, refer to the documentation of the mod-hosting sites.
