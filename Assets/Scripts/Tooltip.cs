using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string title;
    public string description;
    public bool isShowCost;
    public int usedTime;
    public int usedMoney;


    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager._instance.SetAndShowTooltip(title, description, isShowCost, usedTime, usedMoney);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager._instance.HideTooltip();
    }
}
