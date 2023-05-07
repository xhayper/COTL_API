using HarmonyLib;
using MMRoomGeneration;
using UnityEngine;
using Object = UnityEngine.Object;

namespace COTL_API.UI.Helpers
{
    [HarmonyPatch]
    public static class Fixes
    {
        // HarmonyPostfix patch for GenerateRoom.OnDisable method
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GenerateRoom), nameof(GenerateRoom.OnDisable))]
        private static void GenerateRoom_OnDisable(ref GenerateRoom __instance)
        {
            // Check if the tent should be removed
            if (!ShouldTentBeRemoved()) return;

            // Log the removal of mating tents
            LogWarning("MatingTentMod not found, removing any rogue tents.");

            // Destroy all found mating tent GameObjects
            RemoveMatingTentGameObjects();

            LogWarning("Correcting DataManager (SaveData)");

            // Iterate through the fields, removing mating tents from their respective lists
            RemoveMatingTentsFromDataManager();
        }

        private static void RemoveMatingTentGameObjects()
        {
            // Find all GameObjects containing "Mating" in their names
            var tentObjects = Object.FindObjectsOfType<GameObject>()
                .Where(obj => obj.name.Contains("Mating")).ToList();

            if (tentObjects.Any())
            {
                foreach (var tentObject in tentObjects)
                {
                    Object.DestroyImmediate(tentObject);
                    LogWarning("Destroyed mating tent game object.");
                }
            }
            else
            {
                LogWarning("Unable to find any tents!");
            }
        }

        private static void RemoveMatingTentsFromDataManager()
        {
            // Get all fields of type List<StructuresData> in DataManager
            var listOfStructuresDataFields = AccessTools.GetDeclaredFields(typeof(DataManager)).Where(a => a.FieldType == typeof(List<StructuresData>)).ToList();

            // Iterate through the fields, removing mating tents from their respective lists
            foreach (var field in listOfStructuresDataFields)
            {
                if (field.GetValue(DataManager.Instance) is not List<StructuresData> f) continue;

                // Remove all mating tents from the list and log the count
                var tentCount = f.RemoveAll(a => a == null || (a is {PrefabPath: not null} && a.PrefabPath.Contains("Mating")));
                field.SetValue(DataManager.Instance, f);
                if (tentCount > 0)
                {
                    LogWarning($"Removed {tentCount} orphaned tent(s) from {field.Name}.");
                }
            }
        }


        // Helper method to determine if the tent should be removed
        private static bool ShouldTentBeRemoved()
        {
            // Check if the MatingTentMod is not present
            var matingTentMod = BepInEx.Bootstrap.Chainloader.PluginInfos.FirstOrDefault(a => a.Value.Metadata.GUID.Contains("MatingTentMod")).Value;
            return matingTentMod == null;
        }
    }
}