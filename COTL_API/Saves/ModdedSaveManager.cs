using System.Collections.Generic;
using HarmonyLib;

namespace COTL_API.Saves;

[HarmonyPatch]
public static partial class ModdedSaveManager
{
    private static Dictionary<string, BaseModdedSaveData> _moddedSaveData = new();

    public static void RegisterModdedSave(BaseModdedSaveData saveData)
    {
        _moddedSaveData.Add(saveData.GUID, saveData);
        if (saveData.LoadOnStart) saveData.Load();
    }
}