using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;

namespace COTL_API.UI.Helpers;

/// <summary>
///     Provides utility methods for hashing and logging the game version.
/// </summary>
public static class GameHash
{
    /// <summary>
    ///     Logs the version of the game and the SHA256 hash of the game assembly.
    /// </summary>
    public static void LogGameInfo()
    {
        var assemblyHashString = GetGameAssemblyHash();
        LogInfo($"Cult of the Lamb {Application.version} - SHA256 Hash: {assemblyHashString}");
    }

    /// <summary>
    ///     Computes the SHA256 hash of the game assembly.
    /// </summary>
    /// <returns>
    ///     A <see cref="string" /> that represents the SHA256 hash of the game assembly.
    /// </returns>
    private static string GetGameAssemblyHash()
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
    ///     Verifies if the given assembly hash matches the correct hash.
    /// </summary>
    /// <param name="assemblyHash">The hash to verify.</param>
    /// <param name="correctHash">The correct hash for comparison.</param>
    /// <returns>
    ///     <see cref="bool" /> value indicating whether the given assembly hash matches the correct hash.
    /// </returns>
    private static bool VerifyAssemblyHash(string assemblyHash, string correctHash)
    {
        return assemblyHash.Equals(correctHash, StringComparison.OrdinalIgnoreCase);
    }
}