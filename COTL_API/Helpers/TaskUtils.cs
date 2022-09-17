using COTL_API.Debug;
using COTL_API.Structures;
using System.Linq;

namespace COTL_API.Helpers;

public class TaskUtils
{
    public static Structure GetAvailableStructureOfType<T>()
    {
        return Structure.Structures.FirstOrDefault(str => str.Structure_Info != null && str.Structure_Info.Type == CustomStructureManager.GetStructureByType<T>() && str.Brain != null && !str.Brain.ReservedForTask);
    }
}