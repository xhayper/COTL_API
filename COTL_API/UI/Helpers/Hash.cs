using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;

namespace COTL_API.UI.Helpers
{
    /// <summary>
    /// Provides utility methods for hashing and logging the game version.
    /// </summary>
    public static class Hash
    {
        /// <summary>
        /// Logs the version of the game and the SHA256 hash of the current assembly.
        /// </summary>
        public static void RunGameVersionLogging()
        {
            var assemblyHashString = GetCurrentAssemblyHash();
            LogInfo($"Cult of the Lamb {Application.version} - SHA256 Hash: {assemblyHashString}");

            //these will need to be updated for each game update on each storefront, pita - log for now
            // const string steamHash = "CE387753F5F944502D1B352D9424DC0ECC314534B6AF59B1849A81831ED108DE";
            // const string gogHash = "B25E116FDB71174D1D01D542C1267D4CE3526B652FB119607851DC93A3545BF9";

            // var steam = VerifyAssemblyHash(assemblyHashString, steamHash);
            // var gog = VerifyAssemblyHash(assemblyHashString, gogHash);
            //
            // if (steam)
            // {
            //     LogInfo($"Cult of the Lamb (Steam) {Application.version} - SHA256 Hash: {assemblyHashString}");  
            // }
            //
            // if (gog)
            // {
            //     LogInfo($"Cult of the Lamb (GOG) {Application.version} - SHA256 Hash: {assemblyHashString}");      
            // }
            //
            // if(!steam && !gog)
            // {
            // LogInfo($"Cult of the Lamb {Application.version} - SHA256 Hash: {assemblyHashString}");
            // }
        }

        /// <summary>
        /// Computes the SHA256 hash of the current assembly.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents the SHA256 hash of the current assembly.
        /// </returns>
        private static string GetCurrentAssemblyHash()
        {
            // Get the current assembly
            var currentAssembly = Assembly.GetAssembly(typeof(TwitchManager));

            // Get the raw bytes of the assembly
            var assemblyBytes = File.ReadAllBytes(currentAssembly.Location);

            // Compute the hash
            byte[] assemblyHash;
            using (var sha256 = SHA256.Create())
            {
                assemblyHash = sha256.ComputeHash(assemblyBytes);
            }

            // Convert the hash to a string representation
            return BitConverter.ToString(assemblyHash).Replace("-", "");
        }

        /// <summary>
        /// Verifies if the given assembly hash matches the correct hash.
        /// </summary>
        /// <param name="assemblyHash">The hash to verify.</param>
        /// <param name="correctHash">The correct hash for comparison.</param>
        /// <returns>
        /// <see cref="bool"/> value indicating whether the given assembly hash matches the correct hash.
        /// </returns>
        private static bool VerifyAssemblyHash(string assemblyHash, string correctHash)
        {
            return assemblyHash.Equals(correctHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}