using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UseItemModal : MonoBehaviour
{
    public LogicScript logic;
    public Button[] itemButtons;
    public GameObject itemListPanel;
    private List<ExistingItemData> itemList = new() {};
    public Button backpackButton;
    public ModalManager _modalManager;
    public ConfirmUseItemModal confirmUseItemModal;

    public void OpenUseItemModal()
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (i >= itemList.Count) {continue;}
            bool isUsed = itemList[i].IsUsed;
            itemButtons[i].interactable = !isUsed;
            itemButtons[i].image.color = isUsed ? Color.gray : Color.white;
        }
        gameObject.SetActive(true);
    }

    private void OnCloseModal()
    {
        gameObject.SetActive(false);
        _modalManager.CloseModal();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("MainGameLogic").GetComponent<LogicScript>();
        if (StaticData.SelectedItems != null)
        {
            Debug.Log("Selected items: " + string.Join(", ", StaticData.SelectedItems.ConvertAll(i => i.Name)));
            foreach (var selectedItem in StaticData.SelectedItems)
            {
            string itemName = selectedItem.Name;
            itemList.Add(new ExistingItemData(itemName, false, itemName, selectedItem.Item)); 
            }
        }
        
        itemButtons = itemListPanel.GetComponentsInChildren<Button>();
        itemButtons = itemButtons.OrderBy(b => b.transform.GetSiblingIndex()).ToArray();

        for (int i = 0; i < itemButtons.Length; i++)
        {
            int index = i;
            var btn = itemButtons[index];
            if (index >= itemList.Count) {
                btn.interactable = false;
                btn.gameObject.SetActive(false);
                continue;
            }
            var spriteState = new SpriteState
            {
                highlightedSprite = Resources.Load<Sprite>($"components/item/hover/{itemList[index].ItemImageFile}-hover"),
                pressedSprite = Resources.Load<Sprite>($"components/item/selected/{itemList[index].ItemImageFile}-selected"),
            };
            btn.spriteState = spriteState;
            btn.image.sprite = Resources.Load<Sprite>($"components/item/default/{itemList[index].ItemImageFile}");
            btn.onClick.AddListener(() => 
            {
                confirmUseItemModal.OpenModal(index, ApplyItem, itemList[index].ItemName);
            });
        }
        backpackButton.onClick.AddListener(() => OnCloseModal());

        gameObject.SetActive(true);
    }

    private void ApplyItem(int itemIndex)
    {
        Debug.Log($"Use Item: {itemList[itemIndex].ItemName}");

        var itemBuff = itemList[itemIndex].ItemBuff;

        CurrentStateData currentState = logic.GetCurrentStatus();

        var stateData = new CurrentStateData 
        {
            Money = currentState.Money + itemBuff.Money,
            Time = currentState.Time + itemBuff.Time,
            Grade = Mathf.Min(currentState.Grade + itemBuff.Grade, 100),
            Health = Mathf.Min(currentState.Health + itemBuff.Health, 100),
            Happiness = Mathf.Min(currentState.Happiness + itemBuff.Happiness, 100),
            Social = Mathf.Min(currentState.Social + itemBuff.Social, 100),
        };

        Debug.Log($"Current State - Money: {stateData.Money}, Time: {stateData.Time}, " +
                  $"Health: {stateData.Health}, Grade: {stateData.Grade}, " +
                  $"Happiness: {stateData.Happiness}, Social: {stateData.Social}");
        logic.SetCurrentStatus(stateData);

        var item = itemList[itemIndex];
        item.IsUsed = true;
        itemList[itemIndex] = item;

        itemButtons[itemIndex].interactable = false;
        itemButtons[itemIndex].image.color = Color.gray;
    }
}

public struct ExistingItemData
{
    public string ItemName; 
    public bool IsUsed;
    public string ItemImageFile;
    public ItemBuff ItemBuff;

    public ExistingItemData(string itemName, bool isUsed, string itemImageFile, ItemBuff itemBuff)
    {
        ItemName = itemName;
        IsUsed = isUsed;
        ItemImageFile = itemImageFile;
        ItemBuff = itemBuff;
    }
}

public struct ItemBuff 
{
    public int Grade, Health, Happiness, Social, Money, Time;

    public ItemBuff(int grade = 0, int health = 0, int happiness = 0, int social = 0, int money = 0, int time = 0)
    {
        Grade = grade;
        Health = health;
        Happiness = happiness;
        Social = social;
        Money = money;
        Time = time;
    }
}