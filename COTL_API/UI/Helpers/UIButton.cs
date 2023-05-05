using UnityEngine;
using UnityEngine.EventSystems;

namespace COTL_API.UI.Helpers;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public delegate void ButtonAction();

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnCursorEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnCursorExit?.Invoke();
    }

    /// <summary>
    ///     UIButton cursor event. You can subscribe void methods with no parameters to it.
    /// </summary>
    public event ButtonAction? OnClick;

    public event ButtonAction? OnCursorEnter;
    public event ButtonAction? OnCursorExit;
}