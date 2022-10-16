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

    public void AddClickAnimation(Sprite spriteOn, Sprite spriteOff, float waitTime)
    {
        OnClick += () => StartCoroutine(ClickAnimation(spriteOn, spriteOff, waitTime));
    }

    public void AddClickAnimation(string spriteOn, string spriteOff, float waitTime, FilterMode filter = FilterMode.Point)
    {
        Sprite sprite1 = UITextureLoader.MakeSprite(spriteOn, filter);
        Sprite sprite2 = UITextureLoader.MakeSprite(spriteOff, filter);
        OnClick += () => StartCoroutine(ClickAnimation(sprite1, sprite2, waitTime));
    }

    IEnumerator ClickAnimation(Sprite on, Sprite off, float waitTime = 1f)
    {
        Image img = gameObject.GetComponent<Image>();
        if (img == null) yield break;

        img.sprite = on;
        yield return new WaitForSeconds(waitTime);
        img.sprite = off;
    }
}
