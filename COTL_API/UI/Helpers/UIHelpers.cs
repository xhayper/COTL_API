using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using COTL_API.UI.Patches;

namespace COTL_API.UI.Helpers;
public static class UIHelpers
{
    // UI Layer
    static LayerMask _UILayer = LayerMask.NameToLayer("UI");
    public static LayerMask UILayer => _UILayer;

    // UI Helper methods:
    public static void AddToPauseMenu<T>() where T : PauseMenuBase
    {
        UIPatches.PauseMenuQueue.Add(typeof(T));
    }

    public static GameObject CreateUIObject(string name)
    {
        GameObject obj = new GameObject(name);
        obj.layer = UILayer;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        return obj;
    }

    public static GameObject CreateUIObject(string name, Transform parent)
    {
        GameObject obj = new GameObject(name);
        obj.layer = UILayer;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.SetParent(parent);
        return obj;
    }
}
