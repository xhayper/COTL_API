using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace COTL_API.UI.Helpers;

public abstract class PauseMenuBase : MonoBehaviour
{
    public static Transform Parent;

    void Start()
    {
        InitializeMenu(Parent);
    }

    public abstract void InitializeMenu(Transform parent);
};