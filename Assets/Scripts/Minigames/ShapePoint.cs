using UnityEngine;
using UnityEngine.EventSystems;

public class ShapePoint : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    public event System.Action<ShapePoint, PointerEventData> OnPointerEnter;
    public event System.Action<ShapePoint, PointerEventData> OnPointerDown;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnPointerDown?.Invoke(this, eventData);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnter?.Invoke(this, eventData);
    }
}
