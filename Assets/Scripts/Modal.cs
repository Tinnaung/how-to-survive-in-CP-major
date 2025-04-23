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
        modalManager.gameObject.SetActive(true);
        gameObject.SetActive(true);
        badEndModal.gameObject.SetActive(true);
        badEndModal.OpenBadEndModal(type);
   }

   public void OpenEventModal()
   {
        modalManager.gameObject.SetActive(true);
        gameObject.SetActive(true);
        eventModal.gameObject.SetActive(true);
        eventModal.RandomEvent();
   }

   public void OpenUseItemModal()
   {
          modalManager.gameObject.SetActive(true);
          gameObject.SetActive(true);
          useItemModal.gameObject.SetActive(true);
          useItemModal.OpenUseItemModal();
   }

   public void OpenDisplayBadgeModal(List<ReceivedBadge> receivedBadges, string username)
   {
          modalManager.gameObject.SetActive(true);
          gameObject.SetActive(true);
          badgeModal.gameObject.SetActive(true);
          badgeModal.OpenDisplayBadgeList(receivedBadges, username);
   }

   public void OpenEndGameModal()
   {
         modalManager.gameObject.SetActive(true);
         gameObject.SetActive(true); 
         endGameModal.gameObject.SetActive(true);
         endGameModal.OpenEndGameModal();
   }
}
