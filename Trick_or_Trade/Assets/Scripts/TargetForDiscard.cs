using UnityEngine;
using UnityEngine.EventSystems;

// Temporary component added to opponent cards to allow the player to click one to force discard
public class TargetForDiscard : MonoBehaviour, IPointerDownHandler
{
    public PropBow origin;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (origin != null)
        {
            origin.OnTargetSelected(gameObject);
        }
    }
}
