using UnityEngine;

namespace COTL_API.UI;

public abstract class UIMenuBase : MonoBehaviour
{
    public static Transform Parent;

    public void Start()
    {
        InitializeMenu(Parent);
    }

    public abstract void InitializeMenu(Transform parent);
};