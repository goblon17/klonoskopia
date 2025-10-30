using UnityEngine;
using UnityEngine.EventSystems;

public class CablesPoint : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    public event System.Action<CablesPoint, PointerEventData> OnPointerEnter;
    public event System.Action<CablesPoint, PointerEventData> OnPointerDown;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnPointerDown?.Invoke(this, eventData);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnter?.Invoke(this, eventData);
    }
}
