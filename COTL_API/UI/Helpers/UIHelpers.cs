using UnityEngine;

namespace COTL_API.UI.Helpers;

public static class UIHelpers
{
    /// <summary>
    ///     The game's UI layer.
    /// </summary>
    public static LayerMask UILayer => LayerMask.NameToLayer("UI");

    /// <summary>
    ///     Add your UI menu to the game's pause menu.
    /// </summary>
    /// <typeparam name="T">A class that inherits from UIMenuBase.</typeparam>
    public static void AddToPauseMenu<T>() where T : UIMenuBase
    {
        UIPatches.PauseMenuQueue.Add(typeof(T));
    }

    /// <summary>
    ///     Creates a GameObject in the game's UI layer.
    /// </summary>
    /// <param name="name">The name given to your GameObject.</param>
    /// <returns>The GameObject created.</returns>
    public static GameObject CreateUIObject(string name)
    {
        GameObject obj = new(name)
        {
            layer = UILayer,
            transform =
            {
                localPosition = Vector3.zero,
                localScale = Vector3.one
            }
        };
        return obj;
    }

    /// <summary>
    ///     Creates a GameObject in the game's UI layer.
    /// </summary>
    /// <param name="name">The name given to your GameObject.</param>
    /// <param name="parent">The parent Transform this GameObject should be attached to.</param>
    /// <returns>The GameObject created.</returns>
    public static GameObject CreateUIObject(string name, Transform parent)
    {
        GameObject obj = new(name)
        {
            layer = UILayer
        };
        obj.transform.SetParent(parent);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        return obj;
    }
}