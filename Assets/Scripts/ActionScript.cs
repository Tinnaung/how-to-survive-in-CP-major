using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ActionScript : MonoBehaviour
{
    public LogicScript logic;
    public Transform mapCanvas; 

    private Button[] actions;  
    private Dictionary<string, ActionData> actionDataMap = new()
    {
        { "learn", new ActionData(health: -5, happiness: -4, grade: 5, social: -3, time: -3) },
        { "do-merit", new ActionData(health: 0, happiness: 1, grade: 3, social: 0, time: -1, money: -50) },
        { "sleep", new ActionData(health: 4, happiness: 3, grade: 0, social: -3, time: -2) },
        { "exercise", new ActionData(health: 8, happiness: -2, grade: 0, social: 0, time: -2, money: -100) },
        { "hang-out", new ActionData(health: -5, happiness: 4, grade: -3, social: 7, time: -2, money: -100) },
        { "game", new ActionData(health: -3, happiness: 4, grade: -1, social: 0, time: -1, money: -50) },
        { "movie", new ActionData(health: -2, happiness: 5, grade: -2, social: 0, time: -2, money: -100) },
        { "other", new ActionData() } // Default action
    };
    // private readonly string[] actionNames = { "Learn", "Do Merit", "Sleep", "Exercise", "Hangout", "Game", "Movie", "Other" };

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("MainGameLogic").GetComponent<LogicScript>();
        logic.OnStatusChanged += UpdateActionStates;

        actions = mapCanvas.GetComponentsInChildren<Button>();
        actions = actions.OrderBy(b => b.transform.GetSiblingIndex()).ToArray();

         foreach (var button in actions)
        {
            string actionName = button.gameObject.name; // Use button name as key
            Debug.Log($"Button Clicked: {actionName}");
            if (actionDataMap.ContainsKey(actionName))
            {
                button.onClick.AddListener(() => ApplyAction(actionName, actionDataMap[actionName]));
            }
            else
            {
                Debug.LogWarning($"No action found for button: {actionName}");
            }
        }
    }

    private void ApplyAction(string actionName, ActionData action)
    {
        Debug.Log($"Button Clicked: {actionName}"); // TODO: change to success modal
        
        CurrentStateData currentState = logic.GetCurrentStatus();

        var stateData = new CurrentStateData 
        {
            Money = currentState.Money + action.Money,
            Time = currentState.Time + action.Time,
            Grade = Mathf.Min(currentState.Grade + action.Grade, 100),
            Health = Mathf.Min(currentState.Health + action.Health, 100),
            Happiness = Mathf.Min(currentState.Happiness + action.Happiness, 100),
            Social = Mathf.Min(currentState.Social + action.Social, 100),
        };

        Debug.Log($"Current State - Money: {stateData.Money}, Time: {stateData.Time}, " +
                  $"Health: {stateData.Health}, Grade: {stateData.Grade}, " +
                  $"Happiness: {stateData.Happiness}, Social: {stateData.Social}");
        if(!logic.SetCurrentStatus(stateData))
        {
            logic.RandomMiddleEvent();
        }
    }

    private void UpdateActionStates()
    {
        foreach (var button in actions)
        {
            string actionName = button.gameObject.name;
            if (actionDataMap.ContainsKey(actionName))
            {
                var isDisabled = !CanPerformAction(actionName, actionDataMap[actionName]);
                button.interactable = !isDisabled;
                button.image.color = isDisabled ? Color.gray : Color.white;
            }
        }
    }

    private bool CanPerformAction(string actionName, ActionData action)
    {
        CurrentStateData currentState = logic.GetCurrentStatus();

        if (actionName == "sleep" || actionName == "learn") {return true;}
    
        return currentState.Money + action.Money >= 0  
            && currentState.Time + action.Time >= 0;
    }

    private void LogCurrentState()
    {
        CurrentStateData currentState = logic.GetCurrentStatus();
        Debug.Log($"Current State - Money: {currentState.Money}, Time: {currentState.Time}, " +
                  $"Health: {currentState.Health}, Grade: {currentState.Grade}, " +
                  $"Happiness: {currentState.Happiness}, Social: {currentState.Social}");
    }
}

public struct ActionData
{
    public int Grade, Health, Happiness, Social, Money, Time;

    public ActionData(int grade = 0, int health = 0, int happiness = 0, int social = 0, int money = 0, int time = 0)
    {
        Grade = grade;
        Health = health;
        Happiness = happiness;
        Social = social;
        Money = money;
        Time = time;
    }
}

public struct NamedActionData
{
    public string Name;
    public ActionData Action;

    public NamedActionData(string name, ActionData action)
    {
        Name = name;
        Action = action;
    }
}
