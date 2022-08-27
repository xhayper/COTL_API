using System;
using System.Reflection;

namespace COTL_API;

public class CustomType
{
    public static Type GetType(string nameSpace, string typeName)
    {
        string text = nameSpace + "." + typeName;
        Type type = Type.GetType(text);
        if (type != null) return type;
        
        if (text.Contains("."))
        {
            Assembly assembly = Assembly.Load(text.Substring(0, text.IndexOf('.')));
            if (assembly == null) return null;

            type = assembly.GetType(text);
            if (type != null) return type;
        }
        
        AssemblyName[] referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
        foreach (AssemblyName t in referencedAssemblies)
        {
            Assembly assembly2 = Assembly.Load(t);
            if (assembly2 == null) continue;
            
            type = assembly2.GetType(text);
            if (type != null) return type;
        }
        
        return null;
    }
}