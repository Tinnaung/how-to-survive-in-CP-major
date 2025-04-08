using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DisplayBadge : MonoBehaviour, IPointerClickHandler
{
    public Image badgeBackground;
    public Image badgeIcon;
    public Text badgeName;
    public Text badgeOwner;
    public Transform parentToCheck;
    public ModalManager modalToToggle;
    public Action onEmptyList;

    private Dictionary<string, BadgeData> goodBadgeDataMap = new(){
        {"health", new BadgeData("อายุมั่นขวัญยืน", "health-good")},
        {"happiness", new BadgeData("มีความสุขจนจะล้นออกจากปาก\nนอนตะแคงไม่ได้มันจะไหลออกทางหู", "happiness-good")},
        {"grade", new BadgeData("ปีศาจหมูเกียรตินิยม", "grade-good")},
        {"social", new BadgeData("สสประจำคณะ", "social-good")},
        {"money", new BadgeData("อยากยิ่งใหญ่อยากเป็นเสดถี\nอยากมีบ้านมีรถยนต์", "money-good")},
        {"time", new BadgeData("ทรงนี้สติงชัว", "time-good")},
    };

    private Dictionary<string, BadgeData> badBadgeDataMap = new(){
        {"health", new BadgeData("ใช้ชีวิตเหมือนจะตายตอน 30", "health-bad")},
        {"happiness", new BadgeData("Life is not daijoubu", "happiness-bad")},
        {"grade", new BadgeData("เรียนพร้อมเพื่อนจบพร้อมแพทย์", "grade-bad")},
        {"social", new BadgeData("แปลงร่างเป็นหมาป่าเดียวดาย", "social-bad")},
        {"money", new BadgeData("เป็นแฟนคนจน\nต้องทนหน่อยน้อง", "money-bad")},
        {"time", new BadgeData("วันๆไม่ทำ_อะไรเลย", "time-bad")},
    };

    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(gameObject);

        if (parentToCheck.childCount == 1) // Only this one left (before destroy)
        {
            modalToToggle.CloseModal();
            onEmptyList?.Invoke();
        }
    }

    public void OpenDisplayBadge(string badgeType, string badgeStatus, string username)
    {
        Dictionary<string, BadgeData> badgesData;
        if (badgeType == "good")
        {
            badgeBackground.sprite = Resources.Load<Sprite>($"components/result/badge/good-badge");
            badgesData = goodBadgeDataMap;
        }
        else
        {
            badgeBackground.sprite = Resources.Load<Sprite>($"components/result/badge/bad-badge");
            badgesData = badBadgeDataMap;
        }

        if (badgesData.ContainsKey(badgeStatus))
        {
           var badgeData = badgesData[badgeStatus];
           badgeName.text = $"ได้รับยศ {badgeData.BadgeName}";
           badgeIcon.sprite = Resources.Load<Sprite>($"components/result/badge/badge-icon/{badgeData.BadgeIconPath}");
        }
        badgeOwner.text = $"คุณ {username}";

        gameObject.SetActive(true);
    }
}

public struct BadgeData
{
    public string BadgeName;
    public string BadgeIconPath;

    public BadgeData(string badgeName, string badgeIconPath)
    {
        BadgeName = badgeName;
        BadgeIconPath = badgeIconPath;
    }
}
