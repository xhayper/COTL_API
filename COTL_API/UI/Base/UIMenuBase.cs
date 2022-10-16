using UnityEngine;

namespace COTL_API.UI.Base;

public abstract class UIMenuBase : MonoBehaviour
{
    public static Transform Parent;

    void Start()
    {
        InitializeMenu(Parent);
    }

    public abstract void InitializeMenu(Transform parent);
};