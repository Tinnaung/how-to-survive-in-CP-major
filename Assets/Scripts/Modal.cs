using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

public class Modal : MonoBehaviour, IPointerClickHandler
{
   public ModalManager modalManager;
   private bool isCloseWhenClickOutside;
   public BadEndModal badEndModal;
   public EventScript eventModal;
   public UseItemModal useItemModal;

   void Start()
   {
        isCloseWhenClickOutside = false;
   }

   public void OnPointerClick(PointerEventData eventData)
   {
        if (isCloseWhenClickOutside)
        {
            modalManager.CloseModal();
        }
   }

   public void OpenBadEndModal(string type)
   {
        badEndModal.OpenBadEndModal(type);
        gameObject.SetActive(true);
   }

   public void OpenEventModal()
   {
        eventModal.RandomEvent();
        gameObject.SetActive(true);
   }

   public void OpenUseItemModal()
   {
          useItemModal.OpenUseItemModal();
          gameObject.SetActive(true);
   }
}
