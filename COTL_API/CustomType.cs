using System.Reflection;
using System;

public class CustomType
{
    public static Type GetType(string nameSpace, string typeName)
    {
        string text = nameSpace + "." + typeName;
        Type type = Type.GetType(text);
        if (type != null)
        {
            return type;
        }
        if (text.Contains("."))
        {
            Assembly assembly = Assembly.Load(text.Substring(0, text.IndexOf('.')));
            if (assembly == null)
            {
                return null;
            }
            type = assembly.GetType(text);
            if (type != null)
            {
                return type;
            }
        }
        AssemblyName[] referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
        for (int i = 0; i < referencedAssemblies.Length; i++)
        {
            Assembly assembly2 = Assembly.Load(referencedAssemblies[i]);
            if (assembly2 != null)
            {
                type = assembly2.GetType(text);
                if (type != null)
                {
                    return type;
                }
            }
        }
        return null;
    }
}
