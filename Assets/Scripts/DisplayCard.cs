using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisplayCard : MonoBehaviour, IPointerClickHandler
{
    public Transform parentToCheck;
    public ModalManager modalToToggle;
    public Action onAccept;

    public void OnPointerClick(PointerEventData eventData)
    {
        onAccept?.Invoke();
        Destroy(gameObject);

        if (parentToCheck.childCount == 1) // Only this one left (before destroy)
        {
            modalToToggle.CloseModal();
        }
    }
}
