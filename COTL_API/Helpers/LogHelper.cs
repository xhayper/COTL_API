using BepInEx.Logging;

namespace COTL_API.Helpers;

internal static class LogHelper
{
    internal static void Log(LogLevel level, object? data)
    {
        if (Plugin.Instance == null) return;
        Plugin.Instance.Logger.Log(level, data);
    }

    internal static void LogFatal(object? data)
    {
        Log(LogLevel.Fatal, data);
    }

    internal static void LogError(object? data)
    {
        Log(LogLevel.Error, data);
    }

    internal static void LogWarning(object? data)
    {
        Log(LogLevel.Warning, data);
    }

    internal static void LogMessage(object? data)
    {
        Log(LogLevel.Message, data);
    }

    internal static void LogInfo(object? data)
    {
        Log(LogLevel.Info, data);
    }

    internal static void LogDebug(object data)
    {
        Log(LogLevel.Debug, data);
    }
}