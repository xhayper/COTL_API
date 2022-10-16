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

    public void AddClickAnimation(Sprite spriteOnClick, float waitTime)
    {
        OnClick += () => StartCoroutine(ClickAnimation(spriteOnClick, waitTime));
    }

    public void AddClickAnimation(string spriteOnClick, float waitTime, FilterMode filter = FilterMode.Point)
    {
        Sprite sprite = UITextureLoader.MakeSprite(spriteOnClick, filter);
        OnClick += () => StartCoroutine(ClickAnimation(sprite, waitTime));
    }

    IEnumerator ClickAnimation(Sprite on, float waitTime = 1f)
    {
        Image img = gameObject.GetComponent<Image>();
        if (img == null) yield break;

        Sprite off = img.sprite;

        img.sprite = on;
        yield return new WaitForSeconds(waitTime);
        img.sprite = off;
    }
}
