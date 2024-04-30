using NTC.MonoCache;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoCache, IPointerDownHandler, IPointerUpHandler
{
    public bool isPressed = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
