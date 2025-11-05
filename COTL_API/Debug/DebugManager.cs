using COTL_API.CustomFollowerCommand;
using COTL_API.CustomInventory;
using COTL_API.CustomLocalization;
using COTL_API.CustomObjectives;
using COTL_API.CustomRelics;
using COTL_API.CustomSettings;
using COTL_API.CustomSkins;
using COTL_API.CustomStructures;
using COTL_API.CustomTarotCard;
using COTL_API.CustomTasks;
using Lamb.UI;
using UnityEngine;

namespace COTL_API.Debug;

internal class DebugManager
{
    internal static bool DebugContentAdded;

    internal static InventoryItem.ITEM_TYPE DebugItem { get; private set; }
    internal static InventoryItem.ITEM_TYPE DebugItem2 { get; private set; }
    internal static InventoryItem.ITEM_TYPE DebugItem3 { get; private set; }
    internal static InventoryItem.ITEM_TYPE DebugItem4 { get; private set; }

    internal static FollowerCommands DebugGiftFollowerCommand { get; private set; }

    internal static RelicType DebugRelic { get; private set; }

    internal static void AddDebugContent()
    {
        if (DebugContentAdded) return;

        CustomLocalizationManager.LoadLocalization("English",
            PluginPaths.ResolveAssetPath("English-Debug.language"));

        CustomSkinManager.AddFollowerSkin([new DebugFollowerSkin(), new DebugFollowerSkin2()]);
        CustomSkinManager.AddPlayerSkin(new DebugPlayerSkin());

        CustomFollowerCommandManager.Add(new DebugFollowerCommand());
        CustomFollowerCommandManager.Add(new DebugFollowerCommandClass2());
        CustomFollowerCommandManager.Add(new DebugFollowerCommandClass3());
        DebugGiftFollowerCommand = CustomFollowerCommandManager.Add(new DebugGiftFollowerCommand());

        DebugItem = CustomItemManager.Add(new DebugItemClass());
        DebugItem2 = CustomItemManager.Add(new DebugItemClass2());
        DebugItem3 = CustomItemManager.Add(new DebugItemClass3());
        DebugItem4 = CustomItemManager.Add(new DebugItemClass4());

        CustomStructureManager.Add(new DebugStructure());
        CustomStructureManager.Add(new DebugStructure2());
        CustomStructureManager.Add(new DebugStructure3());

        CustomTarotCardManager.Add(new DebugTarotCard());

        CustomTaskManager.Add(new DebugTask());

        var test = CustomObjectiveManager.BedRest("Test");
        test.InitialQuestText = "This is my custom quest text for this objective.";

        CustomSettingsManager.AddDropdown("Debug", "Dropdown", "Option 1",
            ["Option 1", "Option 2", "Option 3"], i => { LogDebug($"Dropdown selected {i}"); });

        CustomSettingsManager.AddKeyboardShortcutDropdown("Debug", "Keyboard Shortcut", KeyCode.None,
            i => { LogDebug($"Keyboard Shortcut selected {i}"); });

        CustomSettingsManager.AddHorizontalSelector("Debug", "Horizontal Selector", "Option 1",
            ["Option 1", "Option 2", "Option 3"],
            i => { LogDebug($"Horizontal Selector selected {i}"); });

        CustomSettingsManager.AddSlider("Debug", "Slider", 0, -100, 100, 1, MMSlider.ValueDisplayFormat.RawValue,
            i => { LogDebug($"Slider value: {i}"); });

        CustomSettingsManager.AddToggle("Debug", "Toggle", true,
            i => { LogDebug($"Toggled: {i}"); });

        DebugRelic = CustomRelicManager.Add(ScriptableObject.CreateInstance<DebugRelicClass>());

        LogDebug("Debug mode enabled!");

        DebugContentAdded = true;
    }
}