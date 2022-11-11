using UnityEngine;
using TMPro;
using System.Linq;
using Image = UnityEngine.UI.Image;
using COTL_API.Helpers;
using System.IO;
using BepInEx;

namespace COTL_API.UI.Helpers;
public static class GameObjectExtensions
{
    // Extension methods for GameObjects.

    /// <summary>
    /// Attach a GameObject to a parent Transform.
    /// </summary>
    /// <param name="obj">The GameObject to be attached.</param>
    /// <param name="parent">The parent Transform.</param>
    /// <returns>The GameObject with all changes applied.</returns>
    public static GameObject AttachToParent(this GameObject obj, Transform parent)
    {
        obj.transform.SetParent(parent);
        return obj;
    }

    /// <summary>
    /// Change the local position of a GameObject.
    /// </summary>
    /// <param name="obj">The GameObject you want to change the local position of.</param>
    /// <param name="x">The GameObject's local position in the X axis.</param>
    /// <param name="y">The GameObject's local position in the Y axis.</param>
    /// <param name="z">The GameObject's local position in the Z axis.</param>
    /// <returns>The GameObject with all the changes to its local position.</returns>
    public static GameObject ChangePosition(this GameObject obj,
        float? x = null, float? y = null, float? z = null)
    {
        x ??= obj.transform.localRotation.x;
        y ??= obj.transform.localRotation.y;
        z ??= obj.transform.localRotation.z;

        obj.transform.localPosition = new Vector3((float)x, (float)y, (float)z);
        return obj;
    }

    /// <summary>
    /// Change the local scale of a GameObject.
    /// </summary>
    /// <param name="obj">The GameObject you want to change the local scale of.</param>
    /// <param name="x">The GameObject's local scale in the X axis.</param>
    /// <param name="y">The GameObject's local scale in the Y axis.</param>
    /// <param name="z">The GameObject's local scale in the Z axis.</param>
    /// <returns>The GameObject with all the changes to its local scale.</returns>
    public static GameObject ChangeScale(this GameObject obj,
        float? x = null, float? y = null, float? z = null)
    {
        x ??= obj.transform.localScale.x;
        y ??= obj.transform.localScale.y;
        z ??= obj.transform.localScale.z;

        obj.transform.localScale = new Vector3((float)x, (float)y, (float)z);
        return obj;
    }

    /// <summary>
    /// Change the local rotation of a GameObject.
    /// </summary>
    /// <param name="obj">The GameObject you want to change the rotation of.</param>
    /// <param name="x">The GameObject's local rotation in the X axis.</param>
    /// <param name="y">The GameObject's local rotation in the Y axis.</param>
    /// <param name="z">The GameObject's local rotation in the Z axis.</param>
    /// <returns>The GameObject with all the changes to its local rotation.</returns>
    public static GameObject ChangeRotation(this GameObject obj,
        float? x = null, float? y = null, float? z = null)
    {
        x ??= obj.transform.eulerAngles.x;
        y ??= obj.transform.eulerAngles.y;
        z ??= obj.transform.eulerAngles.z;

        obj.transform.Rotate(new Vector3((float)x, (float)y, (float)z));
        return obj;
    }

    /// <summary>
    /// Create a new GameObject and attach it to a parent GameObject.
    /// </summary>
    /// <param name="obj">The parent GameObject you wish to attach the child to.</param>
    /// <param name="name">The child GameObject's name.</param>
    /// <returns>The child GameObject.</returns>
    public static GameObject CreateChild(this GameObject obj, string name)
    {
        Transform parent = obj.transform;
        return UIHelpers.CreateUIObject(name, parent);
    }

    /// <summary>
    /// Make a GameObject draggable when clicked.
    /// </summary>
    /// <param name="obj">The GameObject you want to make draggable.</param>
    /// <returns>The GameObject with all changes applied.</returns>
    public static GameObject MakeDraggable(this GameObject obj)
    {
        var script = obj.AddComponent<UIBehaviourHelpers.DraggableUIObject>();
        return obj;
    }

    /// <summary>
    /// Make a GameObject draggable when another GameObject is clicked.
    /// </summary>
    /// <param name="obj">The GameObject that can be interacted with using the mouse.</param>
    /// <param name="dragRect">The RectTransform of the GameObject you wanna drag.</param>
    /// <returns>The GameObject with all changes applied.</returns>
    public static GameObject MakeDraggable(this GameObject obj, in RectTransform dragRect)
    {
        var script = obj.AddComponent<UIBehaviourHelpers.DraggableUIObject>();
        script.dragRectTransform = dragRect;
        return obj;
    }

    /// <summary>
    /// Make a GameObject draggable when another GameObject is clicked.
    /// </summary>
    /// <param name="obj">The GameObject that can be interacted with using the mouse.</param>
    /// <param name="dragObj">The GameObject you wanna drag.</param>
    /// <returns>The GameObject with all changes applied.</returns>
    public static GameObject MakeDraggable(this GameObject obj, in GameObject dragObj)
    {
        var script = obj.AddComponent<UIBehaviourHelpers.DraggableUIObject>();
        var dragRect = dragObj.GetComponent<RectTransform>();
        script.dragRectTransform = dragRect;
        return obj;
    }

    /// <summary>
    /// Make a GameObject function like a clickable UI button by adding the UIButton component to it.
    /// </summary>
    /// <param name="obj">The GameObject you wish to add the UIButton component to.</param>
    /// <returns>The UIButton component added to the GameObject.</returns>
    public static UIButton MakeButton(this GameObject obj)
    {
        var button = obj.AddComponent<UIButton>();
        return button;
    }

    /// <summary>
    /// Add the TextMeshProUGUI component to a GameObject, making it display text in the game's UI's font.
    /// </summary>
    /// <param name="obj">The GameObject you wish to attach the text to.</param>
    /// <param name="message">The text to be displayed.</param>
    /// <param name="fontSize">The size of the font.</param>
    /// <param name="alignment">The alignment of the text (i.e. center, left, right).-</param>
    /// <returns>The GameObject with all changes applied.</returns>
    public static GameObject AddText(this GameObject obj, string message, float fontSize = 10f, TextAlignmentOptions alignment = TextAlignmentOptions.Center)
    {
        TextMeshProUGUI textMesh = obj.AddComponent<TextMeshProUGUI>();
        textMesh.font = FontHelpers.UIFont;
        textMesh.fontSize = fontSize;
        textMesh.text = message;
        textMesh.alignment = alignment;
        return obj;
    }

    /// <summary>
    /// Edits the text contents of the TextMeshProUGUI component attached to a GameObject. If the GameObject does not have a TextMeshProUGUI component attached, this method does nothing.
    /// </summary>
    /// <param name="obj">The GameObject that holds the TextMeshProUGUI component.</param>
    /// <param name="message">The text to be displayed.</param>
    /// <returns>The GameObject with all changes applied.</returns>
    public static GameObject EditText(this GameObject obj, string message)
    {
        TextMeshProUGUI textMesh = obj.GetComponent<TextMeshProUGUI>();
        if(textMesh == null)
        {
            Plugin.Logger.LogWarning("EditText: TextMeshProUGUI component not found.");
            return obj;
        }
        textMesh.text = message;
        return obj;
    }


    // -- IMAGE-RELATED --
    // I wanted opacity to be an int from 0-100 because I think most people are more used to terms like "100% opacity" and "50% opacity" than "1f opacity" and "0.5f opacity".
    // I can change it if necessary (or make a variation of this method that takes a float).

    /// <summary>
    /// Display an image by attaching an Image component to a GameObject.
    /// </summary>
    /// <param name="obj">The GameObject you wish to attach the Image component to.</param>
    /// <param name="sprite">The sprite to be displayed.</param>
    /// <param name="opacity">The opacity of the image (from 0 to 100).</param>
    /// <returns>The GameObject with all changes applied.</returns>
    public static GameObject AttachImage(this GameObject obj, Sprite sprite, int opacity = 100)
    {
        Image img = obj.AddComponent<Image>();
        img.sprite = sprite;
        img.SetNativeSize();
        img.preserveAspect = true;

        if (opacity < 100 && opacity >= 0)
        {
            Color color = img.color;
            color.a = (float)opacity / 100f;
            img.color = color;
        }
        return obj;
    }

    private static string GetPathToImage(string filename)
    {
        if (Path.IsPathRooted(filename)) return filename;
        return Directory.GetFiles(Paths.PluginPath, filename, SearchOption.AllDirectories).FirstOrDefault();
    }

    /// <summary>
    /// Display an image by attaching an Image component to a GameObject.
    /// </summary>
    /// <param name="obj">The GameObject you wish to attach the Image component to.</param>
    /// <param name="imagePath">The path for the image you wish to add.</param>
    /// <param name="opacity">The opacity of the image (from 0 to 100).</param>
    /// <returns>The GameObject with all changes applied.</returns>
    public static GameObject AttachImage(this GameObject obj, string imagePath, int opacity = 100)
    {
        string path = GetPathToImage(imagePath);
        if(path == null)
        {
            Plugin.Logger.LogError($"File {imagePath ?? "(null)"} not found.");
            return obj;
        };
        Sprite sprite = TextureHelper.CreateSpriteFromPath(path);

        Image img = obj.AddComponent<Image>();
        img.sprite = sprite;
        img.SetNativeSize();
        img.preserveAspect = true;

        Mathf.Clamp(opacity, 0, 100);
        Color color = img.color;
        color.a = (float)opacity / 100f;
        img.color = color;

        return obj;
    }

    /// <summary>
    /// Edit the Image component attached to this GameObject to change what it displays. If no Image component is found, this method does nothing.
    /// </summary>
    /// <param name="obj">The GameObject that holds the Image component.</param>
    /// <param name="sprite">The sprite you wish to attach to the Image component.</param>
    /// <returns>The GameObject with all changes applied.</returns>
    public static GameObject EditImage(this GameObject obj, Sprite sprite)
    {
        Image img = obj.GetComponent<Image>();

        if(img == null)
        {
            Plugin.Logger.LogError("EditImage: Image component not found.");
            return obj;
        }

        img.sprite = sprite;
        return obj;
    }

    /// <summary>
    /// Edit the Image component attached to this GameObject to change what it displays. If no Image component is found, this method does nothing.
    /// </summary>
    /// <param name="obj">The GameObject that holds the Image component.</param>
    /// <param name="imagePath">The path to the image you wish to attach to the Image component.</param>
    /// <returns>The GameObject with all changes applied.</returns>
    public static GameObject EditImage(this GameObject obj, string imagePath)
    {
        string path = GetPathToImage(imagePath);
        if (path == null)
        {
            Plugin.Logger.LogError($"File {imagePath ?? "(null)"} not found.");
            return obj;
        };
        Sprite sprite = TextureHelper.CreateSpriteFromPath(path);

        Image img = obj.GetComponent<Image>();
        if (img == null)
        {
            Plugin.Logger.LogError("EditImage: Image component not found.");
            return obj;
        }
        img.sprite = sprite;

        return obj;
    }

    /// <summary>
    /// Change the opacity of the image displayed by the Image component attached to this GameObject. If no Image component is attached, this method does nothing.
    /// </summary>
    /// <param name="obj">The GameObject that holds the Image component.</param>
    /// <param name="opacity">The new opacity for the Image component's contents (from 0 to 100).</param>
    /// <returns>The GameObject with all changes applied.</returns>
    public static GameObject ChangeImageOpacity(this GameObject obj, int opacity = 100)
    {
        Image img = obj.GetComponent<Image>();
        if (img == null)
        {
            Plugin.Logger.LogError("ChangeOpacity: Image component not found.");
            return obj;
        }

        Mathf.Clamp(opacity, 0, 100);
        Color color = img.color;
        color.a = (float)opacity / 100f;
        img.color = color;

        return obj;
    }

}
