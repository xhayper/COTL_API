using System.Reflection;

namespace COTL_API;

public static class CustomType
{
    public static Type? GetType(string nameSpace, string typeName)
    {
        var text = nameSpace + "." + typeName;
        var type = Type.GetType(text);
        if (type != null) return type;

        if (text.Contains("."))
        {
            var assembly = Assembly.Load(text.Substring(0, text.IndexOf('.')));
            if (assembly == null) return null;

            type = assembly.GetType(text);
            if (type != null) return type;
        }

        var referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
        foreach (var t in referencedAssemblies)
        {
            var assembly2 = Assembly.Load(t);
            if (assembly2 == null) continue;

            type = assembly2.GetType(text);
            if (type != null) return type;
        }

        return null;
    }
}