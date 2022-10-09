using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace COTL_API.UI.Helpers;
internal class UIBehaviourHelpers
{
    public class DraggableUIObject : MonoBehaviour, IDragHandler
    {
        public RectTransform dragRectTransform;
        public Canvas canvas;

        void Start()
        {
            dragRectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }
}
