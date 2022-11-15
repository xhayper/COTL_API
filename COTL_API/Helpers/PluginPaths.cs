namespace COTL_API.Helpers;

internal static class PluginPaths
{
    internal static string ResolvePath(params string[] paths)
    {
        return Path.Combine(new List<string> { Plugin.Instance != null ? Plugin.Instance.PluginPath : "" }.Concat(paths)
            .ToArray());
    }

    internal static string ResolveAssetPath(params string[] paths)
    {
        return ResolvePath("Assets", Path.Combine(paths));
    }
}