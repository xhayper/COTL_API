using System.Reflection;
using HarmonyLib;
using UnityEngine.UIElements.Collections;

namespace COTL_API.HarmonyUtils;

public class Patcher
{
    private readonly HashSet<MethodBase> _patchedMethods = new();

    public void PatchAll(Type targetType)
    {
        if (targetType == null)
            throw new ArgumentNullException(nameof(targetType));

        var candidateMethods = targetType
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
            .Where(IsEnumMethod)
            .Where(m => !_patchedMethods.Contains(m));

        foreach (var method in candidateMethods)
            try
            {
                HarmonyMethod patchMethod;
                if (method.ReturnType == typeof(void))
                    patchMethod =
                        new HarmonyMethod(typeof(GenericEnumPatch).GetMethod(nameof(GenericEnumPatch.PrefixVoid)));
                else
                    patchMethod =
                        new HarmonyMethod(
                            typeof(GenericEnumPatch).GetMethod(nameof(GenericEnumPatch.PrefixWithReturn)));

                Plugin.Instance!._harmony.Patch(method, patchMethod);
                _patchedMethods.Add(method);

                LogDebug($"Successfully patched method: {method.DeclaringType?.Name}.{method.Name} " +
                         $"(Enum: {method.GetParameters()[0].ParameterType.Name})");
            }
            catch (Exception ex)
            {
                LogError($"Failed to patch method {method.Name}: {ex.Message}");
            }
    }

    public void UnpatchAll(Type targetType)
    {
        if (targetType == null)
            throw new ArgumentNullException(nameof(targetType));

        var candidateMethods = targetType
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
            .Where(IsEnumMethod)
            .Where(m => _patchedMethods.Contains(m));

        foreach (var method in candidateMethods)
        {
            HarmonyMethod patchMethod;
            if (method.ReturnType == typeof(void))
                patchMethod =
                    new HarmonyMethod(typeof(GenericEnumPatch).GetMethod(nameof(GenericEnumPatch.PrefixVoid)));
            else
                patchMethod =
                    new HarmonyMethod(
                        typeof(GenericEnumPatch).GetMethod(nameof(GenericEnumPatch.PrefixWithReturn))
                    );

            Plugin.Instance!._harmony.Unpatch(method, patchMethod.method);
        }
    }

    private static bool IsEnumMethod(MethodInfo method)
    {
        var parameters = method.GetParameters();
        return parameters.Length > 0 && parameters[0].ParameterType.IsEnum;
    }
}

public static class GenericEnumPatch
{
    [HarmonyPriority(Priority.High)]
    public static bool PrefixVoid(
        MethodBase __originalMethod,
        object __instance,
        object[] __args)
    {
        try
        {
            if (__args == null || __args.Length == 0 || !(__args[0] is Enum enumValue))
                return true;

            var registry = RegistryManager.GetRegistry(enumValue.GetType());

            var customClass = registry.Get(enumValue);
            if (customClass == null)
                return true;

            var customType = customClass.GetType();
            var customMethods = customType.GetMethods(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);

            var originalParams = __originalMethod.GetParameters().Skip(1).ToArray();

            var customMethod = customMethods.FirstOrDefault(m =>
                m.Name == __originalMethod.Name &&
                IsParameterCompatible(originalParams, m.GetParameters()));

            if (customMethod == null)
            {
                LogDebug($"No matching custom method found for {__originalMethod.Name} in {customType.Name}");
                return true;
            }

            LogDebug($"Invoking custom method: {customMethod.Name}");

            var customArgs = __args.Skip(1).ToArray();
            customMethod.Invoke(customClass, customArgs);

            return false;
        }
        catch (Exception ex)
        {
            LogError($"Error in PrefixVoid: {ex.Message}");
            return true;
        }
    }

    [HarmonyPriority(Priority.High)]
    public static bool PrefixWithReturn(
        MethodBase __originalMethod,
        object __instance,
        ref object? __result,
        object[] __args)
    {
        try
        {
            if (__args == null || __args.Length == 0 || __args[0] is not Enum enumValue)
                return true;

            var registry = RegistryManager.GetRegistry(enumValue.GetType());

            var customClass = registry.Get(enumValue);
            if (customClass == null)
                return true;

            var customType = customClass.GetType();
            var customMethods = customType.GetMethods(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);

            var originalParams = __originalMethod.GetParameters().Skip(1).ToArray();

            var customMethod = customMethods.FirstOrDefault(m =>
                m.Name == __originalMethod.Name &&
                IsParameterCompatible(originalParams, m.GetParameters()));

            if (customMethod == null)
            {
                LogDebug($"No matching custom method found for {__originalMethod.Name} in {customType.Name}");
                return true;
            }

            LogDebug($"Invoking custom method: {customMethod.Name}");

            var customArgs = __args.Skip(1).ToArray();
            __result = customMethod.Invoke(customClass, customArgs);

            return false;
        }
        catch (Exception ex)
        {
            LogError($"Error in PrefixWithReturn: {ex.Message}");
            return true;
        }
    }

    private static bool IsCompatible(ParameterInfo info1, ParameterInfo info2)
    {
        if (info1 == null || info2 == null)
            return false;

        return info1.ParameterType.IsAssignableFrom(info2.ParameterType) ||
               info2.ParameterType.IsAssignableFrom(info1.ParameterType);
    }

    private static bool IsParameterCompatible(ParameterInfo[] originalParams, ParameterInfo[] customParams)
    {
        if (originalParams == null || customParams == null)
            return false;

        if (originalParams.Length != customParams.Length)
            return false;

        return !originalParams.Where((t, i) => !IsCompatible(t, customParams[i])).Any();
    }
}