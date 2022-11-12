using System;
using System.Collections.Generic;
using HarmonyLib;

namespace COTL_API.Saves;

[HarmonyPatch]
public static partial class ModdedSaveManager
{
    internal static readonly Dictionary<string, BaseModdedSaveData> ModdedSaveDataList = new();

    // i am sorry but i have to get rid of the code smell
    public class FurryNotFound : Exception
    {
        public FurryNotFound(string message) : base(message)
        {
        }
    }

    public static void RegisterModdedSave(BaseModdedSaveData saveData)
    {
        if (saveData.LoadOnStart && saveData.LoadAfterMainSave)
        {
            throw new FurryNotFound("Modded save data cannot be loaded on start and loaded after main save!");
        }

        ModdedSaveDataList.Add(saveData.GUID, saveData);
        if (saveData.LoadOnStart) saveData.Load();
    }
}