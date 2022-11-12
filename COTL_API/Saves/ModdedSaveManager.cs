using System.Collections.Generic;
using HarmonyLib;

namespace COTL_API.Saves;

[HarmonyPatch]
public static partial class ModdedSaveManager
{
    internal static readonly Dictionary<string, BaseModdedSaveData> ModdedSaveData = new();

    public static void RegisterModdedSave(BaseModdedSaveData saveData)
    {
        if (saveData.LoadOnStart && saveData.LoadAfterMainSave)
        {
            throw new System.Exception("Modded save data cannot be loaded on start and loaded after main save!");
        }
        ModdedSaveData.Add(saveData.GUID, saveData);
        if (saveData.LoadOnStart) saveData.Load();
    }
}