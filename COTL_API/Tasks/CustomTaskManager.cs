using COTL_API.Guid;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace COTL_API.Tasks;

[HarmonyPatch]
public class CustomTaskManager
{
    public static readonly Dictionary<FollowerTaskType, CustomTask> CustomTasks = new();

    public static FollowerTaskType Add(CustomTask task)
    {
        string guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        FollowerTaskType taskType = GuidManager.GetEnumValue<FollowerTaskType>(guid, task.InternalName);
        task.TaskType = taskType;
        task.ModPrefix = guid;

        CustomTasks.Add(taskType, task);
        
        return taskType;
    }
}