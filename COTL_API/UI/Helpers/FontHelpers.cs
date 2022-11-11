using TMPro;

namespace COTL_API.UI.Helpers;
public static class FontHelpers
{
    internal static TMP_FontAsset _startMenu, _pauseMenu;

    /// <summary>
    /// The pause menu's font as a TextMeshPro FontAsset.
    /// </summary>
    public static TMP_FontAsset PauseMenu => _pauseMenu;

    /// <summary>
    /// The start menu's font as a TextMeshPro FontAsset.
    /// </summary>
    public static TMP_FontAsset StartMenu => _startMenu;

    /// <summary>
    /// The game's UI's font as a TextMeshPro FontAsset.
    /// </summary>
    public static TMP_FontAsset UIFont => PauseMenu ?? StartMenu;

}
