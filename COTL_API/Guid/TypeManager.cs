using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using HarmonyLib;
using BepInEx;
using System;

namespace COTL_API.Guid;

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

        foreach (Type t in assembly.GetTypes())
        {
            foreach (BepInPlugin d in t.GetCustomAttributes<BepInPlugin>())
            {
                ModIds.Add(assembly.FullName, d.GUID);
                return d.GUID;
            }
        }

        ModIds.Add(assembly.FullName, default);
        return default;
    }

    public static string GetModIdFromCallstack(Assembly callingAssembly)
    {
        string cacheVal = GetModIdFromAssembly(callingAssembly);
        if (!string.IsNullOrEmpty(cacheVal))
            return cacheVal;

        StackTrace trace = new();
        foreach (StackFrame frame in trace.GetFrames())
        {
            string newVal = GetModIdFromAssembly(frame.GetMethod().DeclaringType.Assembly);
            if (!string.IsNullOrEmpty(newVal))
                return newVal;
        }

        return default;
    }

    [HarmonyPatch(typeof(CustomType), nameof(CustomType.GetType), new[] { typeof(string), typeof(string) })]
    [MethodImpl(MethodImplOptions.NoInlining)]
    [HarmonyReversePatch]
    public static Type OriginalGetType(string nameSpace, string typeName) { throw new NotImplementedException(); }

    [HarmonyPatch(typeof(CustomType), nameof(CustomType.GetType), new[] { typeof(string), typeof(string) })]
    [HarmonyPrefix]
    private static bool GetCustomType(string nameSpace, string typeName, ref Type __result)
    {
        if (TypeCache.ContainsKey(typeName))
        {
            __result = TypeCache[typeName];
            return false;
        }

        if (int.TryParse(typeName, out _)) Plugin.Logger.LogInfo("This appears to be a custom type");

        __result = AccessTools.TypeByName($"{nameSpace}.{typeName}");
        TypeCache.Add(typeName, __result);
        return false;
    }
}