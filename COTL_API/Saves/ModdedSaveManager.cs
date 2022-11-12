using System.Collections.Generic;
using Galaxy.Api;
using HarmonyLib;

namespace COTL_API.Saves;

[HarmonyPatch]
public static partial class ModdedSaveManager
{
    private static Dictionary<string, BaseModdedSaveData> _moddedSaveData = new();

    public static void RegisterModdedSave(BaseModdedSaveData saveData)
    {
        if (saveData.LoadOnStart && saveData.LoadAfterMainSave)
        {
            throw new System.Exception("Modded save data cannot be loaded on start and loaded after main save!");
        }
        _moddedSaveData.Add(saveData.GUID, saveData);
        if (saveData.LoadOnStart) saveData.Load();
    }
}