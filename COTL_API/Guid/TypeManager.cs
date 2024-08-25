using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using BepInEx;
using HarmonyLib;

namespace COTL_API.Guid;

// TODO: Refactor this system
[HarmonyPatch]
public static class TypeManager
{
    private static readonly Dictionary<string, Type> TypeCache = [];

    private static readonly Dictionary<string, string> ModIds = [];

    internal static void Add(string key, Type value)
    {
        if (TypeCache.ContainsKey(key) && TypeCache[key] == value)
            return;

        TypeCache.Add(key, value);
    }

    internal static void Replace(string key, Type value)
    {
        if (TypeCache.ContainsKey(key))
            TypeCache.Remove(key);

        Add(key, value);
    }

    private static string GetModIdFromAssembly(Assembly assembly)
    {
        if (ModIds.TryGetValue(assembly.FullName, out var fromAssembly))
            return fromAssembly;

        foreach (var t in assembly.GetTypes())
        {
            var plugin = t.GetCustomAttribute<BepInPlugin>();
            if (plugin == null) continue;

            ModIds.Add(assembly.FullName, plugin.GUID);
            return plugin.GUID;
        }

        ModIds.Add(assembly.FullName, default!);
        return default!;
    }

    public static string GetModIdFromCallstack(Assembly callingAssembly)
    {
        var cacheVal = GetModIdFromAssembly(callingAssembly);
        if (!string.IsNullOrEmpty(cacheVal))
            return cacheVal;

        StackTrace trace = new();
        return trace.GetFrames()?.Select(frame => GetModIdFromAssembly(frame.GetMethod().DeclaringType?.Assembly!))
            .FirstOrDefault(newVal => !string.IsNullOrEmpty(newVal))!;
    }

    [HarmonyPatch(typeof(CustomType), nameof(CustomType.GetType), typeof(string), typeof(string))]
    [MethodImpl(MethodImplOptions.NoInlining)]
    [HarmonyReversePatch]
    public static Type OriginalGetType(string nameSpace, string typeName)
    {
        throw new NotImplementedException();
    }

    [HarmonyPatch(typeof(CustomType), nameof(CustomType.GetType), typeof(string), typeof(string))]
    [HarmonyPrefix]
    private static bool CustomType_GetType(string nameSpace, string typeName, ref Type __result)
    {
        if (TypeCache.TryGetValue(typeName, out var value))
        {
            __result = value;
            return false;
        }

        __result = AccessTools.TypeByName($"{nameSpace}.{typeName}");
        TypeCache.Add(typeName, __result);
        return false;
    }
}