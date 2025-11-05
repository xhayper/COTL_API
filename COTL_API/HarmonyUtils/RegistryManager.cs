namespace COTL_API.HarmonyUtils;

public class RegistryManager
{
    public static Dictionary<Type, Registry<Enum, object>> Registries = new();

    public static Registry<Enum, object> GetRegistry(Type t)
    {
        if (Registries.TryGetValue(t, out var registry))
            return registry;

        Registries[t] = new Registry<Enum, object>();
        return Registries[t];
    }
}