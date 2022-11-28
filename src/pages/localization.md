---
title: Localization
description: Documentation on how to add localizations using the Cult of the Lamb API
layout: ../layouts/MainLayout.astro
---

## Creating Localizations

You first need to create a localization file. A localization file is formatted as a list of `"term", "localization"` entries. (Space is required!)  
_Excerpt from the English localization:_

```
"TarotCards/EnemyBlast/Description", "Knocks back enemies within your range while also damaging them."
"TarotCards/EnemyBlast/Lore", "A curse against those unworthy of your presence."
"TarotCards/EnemyBlast/Name", "Divine Blast"
"TarotCards/Fireball/Description", "Shoot a piercing bolt of fire."
"TarotCards/Fireball/Lore", "Send forth the flames of the Underworld."
```

You can use the [Template Spreadsheet](https://docs.google.com/spreadsheets/d/1yhkYddwJ_pYPAP58uXIcC_JadWqYzjUc0NIf0Hb6ocU/edit?usp=sharing) to get started.
Use the _Your Localization Here_ column to write your localizations, and copy the **Output Localization** (without the header row) into a text file called `<YourLocalization>.language` (any extension is possible).

## Adding Localizations

```csharp
private void Awake() {
    string localizationPath = Path.Combine(Plugin.PluginPath, "Assets", "<YourLocalization>.language");
    Localization.LoadLocalization("BadlyTranslated", localizationPath);
}
```

If you plugin does not have `PluginPath` defined, you need to do so with

```csharp
PluginPath = Path.GetDirectoryName(Info.Location);
```

## Final Steps

For the localization to load, you need to put it in the appropriate location. For the example, this would be `/Assets/<YourLocalization>.language` relative to the root folder containing the .dll  
Directory structure:

```
📂plugins
 ┣📂Assets
 ┃ ┗📖<YourLocalization>.language
 ┗📜mod_name.dll
```
