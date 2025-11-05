namespace COTL_API.HarmonyUtils;

public class Registry<T, V> : Dictionary<T, V>
    where T : Enum
    where V : class;