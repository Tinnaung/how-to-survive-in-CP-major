using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TooltipManager : MonoBehaviour
{
    public static TooltipManager _instance;
    public GameObject _costPanel;
    public Text title;
    public Text description;
    public Text usedTime;
    public Text usedMoney;

    private bool isTooltipVisible = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
        _costPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTooltipVisible)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void SetAndShowTooltip(string titleMessage, string descriptionMessage = null, bool isShowCost = false, int time = 0, int money = 0)
    {
        transform.position = Input.mousePosition;
        transform.SetAsLastSibling();
        title.text = titleMessage;
        description.text = descriptionMessage == null ? "" : descriptionMessage;
        if (isShowCost)
        {
            _costPanel.SetActive(true);
        }
        usedTime.text = time.ToString();
        usedMoney.text = money.ToString();
        gameObject.SetActive(true);

        isTooltipVisible = true;

    }

    public void HideTooltip()
    {
        _costPanel.SetActive(false);
        gameObject.SetActive(false);
        title.text = string.Empty;

        isTooltipVisible = false;
    }
}
