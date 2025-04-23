using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Modal : MonoBehaviour, IPointerClickHandler
{
   public ModalManager modalManager;
   private bool isCloseWhenClickOutside;
   public BadEndModal badEndModal;
   public EventScript eventModal;
   public UseItemModal useItemModal;
   public BadgeScript badgeModal;
   public EndGameModal endGameModal;

   void Start()
   {
        isCloseWhenClickOutside = false;
        ResetModal();
   }

   public void ResetModal()
   {
     badEndModal.gameObject.SetActive(false);
     eventModal.gameObject.SetActive(false);
     useItemModal.gameObject.SetActive(false);
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
        gameObject.SetActive(true);
        badEndModal.gameObject.SetActive(true);
        badEndModal.OpenBadEndModal(type);
   }

   public void OpenEventModal()
   {
        gameObject.SetActive(true);
        eventModal.gameObject.SetActive(true);
        eventModal.RandomEvent();
   }

   public void OpenUseItemModal()
   {
          gameObject.SetActive(true);
          useItemModal.gameObject.SetActive(true);
          useItemModal.OpenUseItemModal();
   }

   public void OpenDisplayBadgeModal(List<ReceivedBadge> receivedBadges, string username)
   {
          gameObject.SetActive(true);
          badgeModal.gameObject.SetActive(true);
          badgeModal.OpenDisplayBadgeList(receivedBadges, username);
   }

   public void OpenEndGameModal()
   {
         gameObject.SetActive(true); 
         endGameModal.gameObject.SetActive(true);
         endGameModal.OpenEndGameModal();
   }
}
