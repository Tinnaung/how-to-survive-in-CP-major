using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ConfirmUseItemModal : MonoBehaviour
{
    public Text title;
    public Button confirmButton;
    public Button cancelButton;
    private int currentIndex;
    private Action<int> onConfirm;
    private Dictionary<string, string> itemNameDataMap = new ()
    {
        {"manga", "หนังสือการ์ตูน"},
        {"ps5", "ps5"},
        {"food", "อาหาร"},
        {"phone", "โทรศัพท์"},
        {"piggy-bank", "กระปุกหมูทองคำ"},
        {"textbook", "หนังสือเรียนอาจารย์โต"},
        {"motorcycle", "แง๊นๆ"},
        {"pill", "ยาวิเศษตราม้า"},
        {"dumbell", "ดัมเบล"},
        {"gift", "???"},
        {"other", ""},
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);

        cancelButton.onClick.AddListener(() => CloseModal());
        confirmButton.onClick.AddListener(() =>
        {
            onConfirm?.Invoke(currentIndex); // Call back with the stored index
            CloseModal();
        });
    }

    public void OpenModal(int index, Action<int> onConfirmAction, string itemName)
    {
        currentIndex = index;
        onConfirm = onConfirmAction;
        title.text = $"คุณต้องการใช้ {itemNameDataMap[itemName]} ใช่ไหม";
        gameObject.SetActive(true);
    }

    public void CloseModal()
    {
        gameObject.SetActive(false);
    }
}
