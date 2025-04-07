using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventScript : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject costEventPrefab;
    public Transform cardParent;
    private List<EventData> eventList;
    public ModalManager _modalManager;
    public LogicScript logic;
    private readonly float[] rotationZ = { -5f, 5f, 0f };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("MainGameLogic").GetComponent<LogicScript>();
        eventList = new List<EventData>()
        {
          new EventData("go-hospital", "go-hospital", new EventCostData(health:  8, time: -4)),
          new EventData("do-project", "do-project", new EventCostData(health:  -5, happiness: -5)),
          new EventData("pay-tuition", "pay-tuition", new EventCostData(money:  -200, time: -4)),
          new EventData("do-part-time", "do-part-time", new EventCostData(money:  200, time: -8, happiness: -10)),
          new EventData("face-pm", "face-pm", new EventCostData(health:  -5)),
          new EventData("get-drunk", "get-drunk", new EventCostData(money:  -100, social: 15)),
          new EventData("bet-football", "bet-football", new EventCostData(money:  -50, social: 10)),
          new EventData("win-lottery", "win-lottery", new EventCostData(money:  500)),
          new EventData("lost-lottery", "lost-lottery", new EventCostData(money:  -300)),
          new EventData("found-summary", "found-summary", new EventCostData(grade:  10)),
          new EventData("eat-raw-pork", "eat-raw-pork", new EventCostData(health:  -10, time: -2)),
        };

        gameObject.SetActive(false);
    }

    public void RandomEvent()
    {
        int eventCount = Random.Range(0, 3);
        Debug.Log($"Random Event Count: {eventCount}");

        if (eventCount == 0)
        {
            _modalManager.gameObject.SetActive(false);
            return;
        }

        List<EventData> chosenEvents = new List<EventData>();
        List<int> usedIndices = new List<int>();

        while (chosenEvents.Count < eventCount)
        {
            int index = Random.Range(0, eventList.Count);
            if (!usedIndices.Contains(index))
            {
                usedIndices.Add(index);
                chosenEvents.Add(eventList[index]);
            }
        }
        
        _modalManager.gameObject.SetActive(true);
        gameObject.SetActive(true);

        for (int i = 0; i < chosenEvents.Count; i++)
        {
            var ev = chosenEvents[i];
            Debug.Log($"Event Happened: {ev.Name}");

            GameObject card = Instantiate(cardPrefab, cardParent);
            card.transform.SetAsFirstSibling();

            Image img = card.GetComponent<Image>();
            img.sprite = ev.CardImage;

            Transform costParent = card.transform.Find("CostList");
            EventCostData cost = ev.Cost;

            // AddCostItem(cost.Health, "health-icon", "", costParent);
            // AddCostItem(cost.Happiness, "happiness-icon", "", costParent);
            // AddCostItem(cost.Grade, "grade-icon", "", costParent);
            // AddCostItem(cost.Social, "social-icon", "", costParent);
            // AddCostItem(cost.Money, "money-icon", "บาท", costParent);
            // AddCostItem(cost.Time, "time-icon", "ชั่วโมง", costParent);
            
            var displayCardScript = card.GetComponent<DisplayCard>();
            if (displayCardScript != null)
            {
                displayCardScript.parentToCheck = cardParent;
                displayCardScript.modalToToggle = _modalManager;
                displayCardScript.onAccept = () => ApplyEvent(ev);
            }

            float zRot = rotationZ[i % rotationZ.Length];
            card.transform.rotation = Quaternion.Euler(0, 0, zRot);
        }
    }

    private void AddCostItem(int value, string icon, string unitText, Transform costParent)
    {
        if (value == 0) return;

        GameObject costItem = Instantiate(costEventPrefab, costParent);
        DisplayEventCost displayCost = costItem.GetComponent<DisplayEventCost>();

        displayCost.icon.sprite = Resources.Load<Sprite>($"components/icon/{icon}");;
        displayCost.sign.text = value < 0 ? "-" : "+";
        displayCost.amount.text = Mathf.Abs(value).ToString();
        displayCost.unit.text = unitText;
    }

    private void ApplyEvent(EventData selectedEvent)
    {
        Debug.Log($"Apply Event: {selectedEvent.Name}");

        CurrentStateData currentState = logic.GetCurrentStatus();
        var eventCost = selectedEvent.Cost;

        var stateData = new CurrentStateData 
        {
            Money = currentState.Money + eventCost.Money,
            Time = currentState.Time + eventCost.Time,
            Grade = Mathf.Min(currentState.Grade + eventCost.Grade, 100),
            Health = Mathf.Min(currentState.Health + eventCost.Health, 100),
            Happiness = Mathf.Min(currentState.Happiness + eventCost.Happiness, 100),
            Social = Mathf.Min(currentState.Social + eventCost.Social, 100),
        };

        Debug.Log($"Current State - Money: {stateData.Money}, Time: {stateData.Time}, " +
                  $"Health: {stateData.Health}, Grade: {stateData.Grade}, " +
                  $"Happiness: {stateData.Happiness}, Social: {stateData.Social}");
        logic.SetCurrentStatus(stateData);
    } 
}

public struct EventData
{
    public string Name;
    public Sprite CardImage;
    public EventCostData Cost;

    public EventData(string name, string backgroundPath, EventCostData cost)
    {
        Name = name;
        CardImage = Resources.Load<Sprite>($"components/event/{backgroundPath}");
        Cost = cost;
    }
}

public struct EventCostData
{
    public int Grade, Health, Happiness, Social, Money, Time;

    public EventCostData(int grade = 0, int health = 0, int happiness = 0, int social = 0, int money = 0, int time = 0)
    {
        Grade = grade;
        Health = health;
        Happiness = happiness;
        Social = social;
        Money = money;
        Time = time;
    }
}