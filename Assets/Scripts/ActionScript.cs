using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ActionScript : MonoBehaviour
{
    public LogicScript logic;
    public Transform mapCanvas; 

    private Button[] actions;  
    private ActionData[] actionDataList =
        {
            new(grade: 10, time: -2),               // Learn
            new(grade: 5, time: -1, money: -50),    // Do Merit
            new(health: 10, time: -2),             // Sleep
            new(health: 12, time: -2, money: -100), // Exercise
            new(social: 12, time: -2, money: -100), // Hangout
            new(happiness: 5, time: -1, money: -50), // Game
            new(happiness: 10, time: -2, money: -100), // Movie
            new() // Other (default)
        };
    private readonly string[] actionNames = { "Learn", "Do Merit", "Sleep", "Exercise", "Hangout", "Game", "Movie", "Other" };

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        logic.OnStatusChanged += UpdateActionStates;

        actions = mapCanvas.GetComponentsInChildren<Button>();
        actions = actions.OrderBy(b => b.transform.GetSiblingIndex()).ToArray();

        for (int i = 0; i < actions.Length; i++)
        {
            int index = i;

            actions[i].gameObject.name = actionNames[index];
            actions[i].onClick.AddListener(() => ApplyAction(actionNames[index], actionDataList[index]));
        }
    }

    private void ApplyAction(string actionName, ActionData action)
    {
        Debug.Log($"Button Clicked: {actionName}"); // TODO: change to success modal

        var stateData = new CurrentStateData 
        {
            Money = action.Money,
            Time = action.Time,
            Grade = action.Grade,
            Health = action.Health,
            Happiness = action.Happiness,
            Social = action.Social,
        };
        logic.SetCurrentStatus(stateData);
    }

    private void UpdateActionStates()
    {
        for (int i = 0; i < actions.Length; i++)
        {
            bool canPerformAction = CanPerformAction(actionDataList[i]);
            actions[i].interactable = canPerformAction;
        }
    }

    private bool CanPerformAction(ActionData action)
    {
        CurrentStateData currentState = logic.GetCurrentStatus();

        return currentState.Money + action.Money >= 0  
            && currentState.Time + action.Time >= 0
            && currentState.Health + action.Health >= 0 
            && currentState.Grade + action.Grade >= 0
            && currentState.Social + action.Social >= 0
            && currentState.Happiness + action.Happiness >= 0;
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
