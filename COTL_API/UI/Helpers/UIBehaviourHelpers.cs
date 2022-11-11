using UnityEngine;
using UnityEngine.EventSystems;

namespace COTL_API.UI;
internal class UIBehaviourHelpers
{
    public class DraggableUIObject : MonoBehaviour, IDragHandler
    {
        public RectTransform dragRectTransform;
        public Canvas canvas;

        public void Start()
        {
            dragRectTransform ??= GetComponent<RectTransform>();
            canvas ??= GetComponentInParent<Canvas>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }
}
