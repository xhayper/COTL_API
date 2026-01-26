using System;
using UnityEngine;

namespace COTL_API.Helpers;

public static class KeyCodes
{
    public static string KeyCodeToString(KeyCode keyCode)
    {
        return keyCode.ToString();
    }

    public static KeyCode StringToKeyCode(string keyCodeString)
    {
        return (KeyCode)Enum.Parse(typeof(KeyCode), keyCodeString, true);
    }

    public static string[] GetKeyCodeOptions()
    {
        return Enum.GetNames(typeof(KeyCode));
    }
}