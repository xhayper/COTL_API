using COTL_API.CustomFollowerCommand;
using COTL_API.CustomInventory;
using COTL_API.CustomMission;
using COTL_API.CustomObjectives;
using COTL_API.CustomRelics;
using COTL_API.CustomStructures;
using COTL_API.CustomTasks;
using COTL_API.Prefabs;

namespace COTL_API.Helpers;

internal static class Debugging
{
    public static void PrintAllContent(bool printNames = false)
    {
        var items = CustomItemManager.CustomItemList;
        LogInfo($"Items: {items.Count}");
        if (printNames)
            foreach (var item in items)
                LogInfo($"{item.Key}: {item.Value.InternalName}");

        var structures = CustomStructureManager.CustomStructureList;
        LogInfo($"Structures: {structures.Count}");
        if (printNames)
            foreach (var structure in structures)
                LogInfo($"{structure.Key}: {structure.Value.InternalName}");

        var tasks = CustomTaskManager.CustomTaskList;
        LogInfo($"Tasks: {tasks.Count}");
        if (printNames)
            foreach (var task in tasks)
                LogInfo($"{task.Key}: {task.Value.InternalName}");

        var commands = CustomFollowerCommandManager.CustomFollowerCommandList;
        LogInfo($"Commands: {commands.Count}");
        if (printNames)
            foreach (var command in commands)
                LogInfo($"{command.Key}: {command.Value.InternalName}");

        var missions = CustomMissionManager.CustomMissionList;
        LogInfo($"Missions: {missions.Count}");
        if (printNames)
            foreach (var mission in missions)
                LogInfo($"{mission.Key}: {mission.Value.InternalName}");

        var objectives = CustomObjectiveManager.CustomObjectiveList;
        LogInfo($"Objectives: {objectives.Count}");
        if (printNames)
            foreach (var objective in objectives)
                LogInfo($"{objective.Key}: {objective.Value.InitialQuestText}");

        var relics = CustomRelicManager.CustomRelicDataList;
        LogInfo($"Relics: {relics.Count}");
        if (printNames)
            foreach (var relic in relics)
                LogInfo($"{relic.Key}: {relic.Value.InternalName}");

        var prefabs = PrefabsPatches.PrefabStrings;
        LogInfo($"Prefabs: {prefabs.Count}");
        if (printNames)
            foreach (var prefab in prefabs)
                LogInfo($"{prefab.Key}: {prefab.Value.InternalName}");
    }
}