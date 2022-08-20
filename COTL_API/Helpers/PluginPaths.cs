using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace COTL_API.Helpers;

public static class PluginPaths
{

    public static string ResolvePath(params string[] paths)
    {
        return Path.Combine((new List<string>() { Plugin.PLUGIN_PATH }).Concat(paths).ToArray());
    }

}