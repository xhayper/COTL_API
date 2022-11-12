using UnityEngine;

namespace COTL_API.UI.Helpers;

/// <summary>
/// An abstract class that serves as a base for an UI menu controller.
/// </summary>
public abstract class UIMenuBase : MonoBehaviour
{
    /// <summary>
    /// The parent GameObject you should attach your GameObject(s) to.
    /// </summary>
    public static Transform Parent { get; set; }

    public void Start()
    {
        InitializeMenu(Parent);
    }

    /// <summary>
    /// A method you must override to initialize your menu in.
    /// You should attach your GameObject(s) to the <c>Transform parent</c> parameter.
    /// </summary>
    /// <param name="parent">The parent Transform you should attach at least one GameObject to.</param>
    public abstract void InitializeMenu(Transform parent);
};