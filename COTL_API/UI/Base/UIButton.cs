using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using COTL_API.UI.Helpers;

namespace COTL_API.UI.Base;
public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public delegate void ButtonAction();

    public event ButtonAction OnClick, OnCursorEnter, OnCursorExit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnCursorEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnCursorExit?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}
