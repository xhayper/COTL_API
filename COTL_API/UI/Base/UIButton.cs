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

    // This coroutine doesn't work for some reason. I don't know why. I'll look into it some other time.
    /*
    public void AddClickAnimation(Sprite spriteOn, float waitTime)
    {
        OnClick += () => this.StartCoroutine(ClickAnimation(spriteOn, waitTime));
    }

    public void AddClickAnimation(string spriteOn, float waitTime, FilterMode filter = FilterMode.Point)
    {
        Sprite sprite1 = UITextureLoader.MakeSprite(spriteOn, filter);
        OnClick += () => StartCoroutine(ClickAnimation(sprite1, waitTime));
    }

    IEnumerator ClickAnimation(Sprite on, float waitTime = 1f)
    {
        Image img = gameObject.GetComponent<Image>();
        if (img == null) yield break;

        Sprite off = img.sprite;

        img.sprite = on;
        yield return new WaitForSeconds(waitTime);
        img.sprite = off;
    }*/
}
