using System.Reflection;
using COTL_API.Guid;
using HarmonyLib;

namespace COTL_API.CustomTasks;

[HarmonyPatch]
public static partial class CustomTaskManager
{
    public static Dictionary<FollowerTaskType, CustomTask> CustomTaskList { get; } = new();

    public static FollowerTaskType Add(CustomTask task)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var taskType = GuidManager.GetEnumValue<FollowerTaskType>(guid, task.InternalName);
        task.TaskType = taskType;
        task.ModPrefix = guid;

        CustomTaskList.Add(taskType, task);

        return taskType;
    }
}