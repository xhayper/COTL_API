using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using HarmonyLib;
using BepInEx;
using System;

namespace COTL_API.Guid;

// TODO: Refactor this system
[HarmonyPatch]
public static class TypeManager
{
    private static readonly Dictionary<string, Type> TypeCache = new();

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

    private static readonly Dictionary<string, string> ModIds = new();

    private static string GetModIdFromAssembly(Assembly assembly)
    {
        if (ModIds.ContainsKey(assembly.FullName))
            return ModIds[assembly.FullName];

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

    [HarmonyPatch(typeof(CustomType), nameof(CustomType.GetType), new[] { typeof(string), typeof(string) })]
    [MethodImpl(MethodImplOptions.NoInlining)]
    [HarmonyReversePatch]
    public static Type OriginalGetType(string nameSpace, string typeName)
    {
        throw new NotImplementedException();
    }

    [HarmonyPatch(typeof(CustomType), nameof(CustomType.GetType), new[] { typeof(string), typeof(string) })]
    [HarmonyPrefix]
    private static bool GetCustomType(string nameSpace, string typeName, ref Type __result)
    {
        if (TypeCache.ContainsKey(typeName))
        {
            __result = TypeCache[typeName];
            return false;
        }

        if (int.TryParse(typeName, out _))
        {
            if (Plugin.Instance != null) Plugin.Instance.Logger.LogInfo("This appears to be a custom type");
        }

        __result = AccessTools.TypeByName($"{nameSpace}.{typeName}");
        TypeCache.Add(typeName, __result);
        return false;
    }
}