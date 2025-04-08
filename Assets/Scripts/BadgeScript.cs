using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BadgeScript : MonoBehaviour
{
    public GameObject badgePrefab;
    public Transform badgeParent;
    public Modal modal;
    public ModalManager _modalManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OpenDisplayBadgeList(List<ReceivedBadge> receivedBadges, string username)
    {
        Debug.Log($"Received Badge Count: {receivedBadges.Count}");

        if (receivedBadges.Count == 0)
        {
            modal.OpenEndGameModal();
            return;
        }
        
        _modalManager.gameObject.SetActive(true);
        gameObject.SetActive(true);

        for (int i = 0 ; i < receivedBadges.Count; i++)
        {
            var receivedBadge = receivedBadges[i];
            Debug.Log($"Received Badge: {receivedBadge.BadgeType} {receivedBadge.BadgeStatus}");

            GameObject badge = Instantiate(badgePrefab, badgeParent);
            badge.transform.SetAsFirstSibling();

            var displayBadge = badge.GetComponent<DisplayBadge>();
            if (displayBadge != null)
            {
                displayBadge.parentToCheck = badgeParent;
                displayBadge.modalToToggle = _modalManager;
                displayBadge.OpenDisplayBadge(receivedBadge.BadgeType, receivedBadge.BadgeStatus, username);
                displayBadge.onEmptyList = () => modal.OpenEndGameModal();
            }
            else
            {
                Debug.LogWarning("DisplayBadge component not found on badge prefab.");
            }
        }
    }
}

public struct ReceivedBadge
{
    public string BadgeType;
    public string BadgeStatus;
    
    public ReceivedBadge(string badgeType, string badgeStatus)
    {
        BadgeType = badgeType;
        BadgeStatus = badgeStatus;
    }
}
