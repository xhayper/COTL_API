using COTL_API.CustomInventory;
using HarmonyLib;
using Sirenix.Utilities;

namespace COTL_API.HarmonyUtils;

public class Patcher
{
    private List<PatchProcessor> xyz = new();

    public void PatchAll()
    {
        CreatePatchClass(typeof(InventoryItem), typeof(CustomInventoryItem));
    }
    
    public void CreatePatchClass(Type orig, Type dest)
    {
        var destMethods = dest.GetMethods();
        
        foreach (var x in orig.GetMethods())
        {
            LogDebug("Trying to patch " + x.Name + "AAAAA");
            
            var method = destMethods.FirstOrDefault(info =>
            {
                if (info.Name != x.Name) return false;
                if (info.ReturnType != dest.GetReturnType()) return false;
                return info.GetParameters().Length == x.GetParameters().Length &&
                       x.GetParameters().All(parameter => parameter.GetType() == info.GetParameters()[parameter.Position].ParameterType);
            });

            if (method is null)
                return;
            
            var processor = new PatchProcessor(Plugin.Instance!._harmony, x);
            processor.AddPrefix(new HarmonyMethod
            {
                method = method,
                methodName =  method.Name,
                argumentTypes = method.GetParameters().Select(p => p.ParameterType).ToArray(),
                
            });
            processor.Patch();
            
            xyz.Add(processor);
        }
    }
}