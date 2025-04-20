using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SelectItemScript : MonoBehaviour
{
    [SerializeField] private Button[] itemButtons;
    
    private HashSet<Button> selectedButtons = new HashSet<Button>();
    private List<NamedItemData> selectedItems = new List<NamedItemData>();
    
    private const int maxSelections = 3;
    
    private Dictionary<string, Button> buttonLookup = new Dictionary<string, Button>();
    private Dictionary<string, ItemBuff> itemDataMap = new Dictionary<string, ItemBuff>
    {
        { "manga", new ItemBuff(happiness: 5) },
        { "ps5", new ItemBuff(happiness: 12) },
        { "food", new ItemBuff(happiness: 10) },
        { "phone", new ItemBuff(social: 5) },
        { "piggy-bank", new ItemBuff(time: -2, money: 500) },
        { "textbook", new ItemBuff(grade: 12) },
        { "motorcycle", new ItemBuff(time: 4) },
        { "pill", new ItemBuff(health: 12) },
        { "dumbell", new ItemBuff(health: 5) },
        { "gift", new ItemBuff(social: 7) },
        { "other", new ItemBuff() }
    };

    void Start()
    {
        foreach (var button in itemButtons)
        {
            string itemName = button.gameObject.name;
            buttonLookup[itemName] = button;

            if (itemDataMap.ContainsKey(itemName))
            {
                button.onClick.AddListener(() => SelectItem(itemName));
                AddPointerExitTrigger(button);
            }
            else
            {
                Debug.LogWarning($"No item data found for button: {itemName}");
            }
        }
    }

    private void AddPointerExitTrigger(Button button)
    {
        var trigger = button.gameObject.AddComponent<EventTrigger>();
        
        var exitEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        
        exitEntry.callback.AddListener((data) =>
        {
            if (selectedButtons.Contains(button))
            {
                var img = button.GetComponent<Image>();
                if (img != null)
                {
                    img.overrideSprite = button.spriteState.selectedSprite;
                }
            }
        });

        trigger.triggers.Add(exitEntry);
    }

    private void SelectItem(string itemName)
    {
        int index = selectedItems.FindIndex(item => item.Name == itemName);
        var button = buttonLookup[itemName];

        if (index >= 0)
        {
            // unselect
            selectedItems.RemoveAt(index);
            SetButtonState(button, false);
        }
        else if (selectedItems.Count < maxSelections)
        {
            // select
            var itemData = itemDataMap[itemName];
            selectedItems.Add(new NamedItemData(itemName, itemData));
            SetButtonState(button, true);
        }
        else
        {
            Debug.Log($"Maximum {maxSelections} items can be selected.");

            // reset to normal state
            var image = button.GetComponent<Image>();
            if (!selectedButtons.Contains(button) && image != null)
            {
                image.overrideSprite = null;
                ResetButtonState(button);
            }
        }

        StaticData.SelectedItems = selectedItems;
        LogSelectedItems();
    }

    private void LogSelectedItems()
    {
        Debug.Log("Selected items: " + string.Join(", ", selectedItems.ConvertAll(i => i.Name)));
    }

    private void SetButtonState(Button button, bool selected)
    {
        var image = button.GetComponent<Image>();
        
        if (image == null) return;
        
        if (selected)
        {
            selectedButtons.Add(button);
            image.overrideSprite = button.spriteState.selectedSprite;
            
            // Force pointer exit to reapply selected visual
            ResetButtonState(button);
            button.OnPointerExit(null);
        }
        else
        {
            selectedButtons.Remove(button);
            image.overrideSprite = null;
            
            // Clear button's state
            ResetButtonState(button);
        }
    }

    private void ResetButtonState(Button button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        button.OnDeselect(null);
        button.OnPointerExit(null);
    }
}

public struct ItemData
{
    public int Grade { get; }
    public int Health { get; }
    public int Happiness { get; }
    public int Social { get; }
    public int Money { get; }
    public int Time { get; }

    public ItemData(int grade = 0, int health = 0, int happiness = 0, int social = 0, int money = 0, int time = 0)
    {
        Grade = grade;
        Health = health;
        Happiness = happiness;
        Social = social;
        Money = money;
        Time = time;
    }
}

public struct NamedItemData
{
    public string Name { get; }
    public ItemBuff Item { get; }

    public NamedItemData(string name, ItemBuff item)
    {
        Name = name;
        Item = item;
    }
}