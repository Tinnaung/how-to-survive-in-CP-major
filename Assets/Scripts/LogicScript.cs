using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class LogicScript : MonoBehaviour
{
    public event System.Action OnStatusChanged;

    [Header("Game Settings")]
    public string playerName = StaticData.Username;
    public int roundTime = 24;
    public int roundMoney = 1000;
    
    [Header("Game State")]
    public int currentYear = 1;
    public int currentSemester = 1;
    public string split = "Midterm";
    
    [Header("Player Stats")]
    public int time;
    public int money;
    public int health;
    public int grade;
    public int happiness;
    public int social;
    public int allRemainingTime;

    [Header("UI Elements")]
    public Text nameText;
    public Text timeText;
    public Text moneyText;
    public Text splitText;
    public Text currentYearText;
    public Text currentSemesterText;
    public StatusBarAndScore healthBar;
    public StatusBarAndScore gradeBar;
    public StatusBarAndScore happinessBar;
    public StatusBarAndScore socialBar;
    public Modal modal;

    public void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        time = roundTime;
        money = roundMoney;
        health = grade = happiness = social = 20;
        split = "Midterm";
        currentYear = 1;
        currentSemester = 1;
        healthBar.Initialize(100);
        gradeBar.Initialize(100);
        happinessBar.Initialize(100);
        socialBar.Initialize(100);
        UpdateUI();
    }

    private void UpdateUI()
    {
        nameText.text = StaticData.Username;
        splitText.text = split == "Midterm" ? "กลางภาค" : "ปลายภาค";
        currentYearText.text = currentYear.ToString();
        currentSemesterText.text = currentSemester.ToString();
        timeText.text = time.ToString();
        moneyText.text = money.ToString();

        healthBar.SetScore(health);
        gradeBar.SetScore(grade);
        happinessBar.SetScore(happiness);
        socialBar.SetScore(social);
    }

    private void ModifyStat(ref int stat, int amount)
    {
        stat = amount;
    }

    private void SetTime(int amount) => ModifyStat(ref time, amount);
    private void SetMoney(int amount) => ModifyStat(ref money, amount);
    private void SetHealth(int amount) => ModifyStat(ref health, amount);
    private void SetGrade(int amount) => ModifyStat(ref grade, amount);
    private void SetHappiness(int amount) => ModifyStat(ref happiness, amount);
    private void SetSocial(int amount) => ModifyStat(ref social, amount);

    [ContextMenu("End Split")]
    public void EndSplit()
    {
        if (split == "Midterm")
        {
            split = "Final";
        }
        else
        {
            if (currentSemester == 2) currentYear++;
            currentSemester = (currentSemester % 2) + 1;
            split = "Midterm";
            money += roundMoney;
        }
        allRemainingTime += time;
        time = roundTime;
        UpdateUI();
        if (currentYear > 1)
        {
            EndGame();
            return;
        }
        modal.OpenEventModal();
        OnStatusChanged?.Invoke();
    }

    public CurrentStateData GetCurrentStatus()
    {
        return new CurrentStateData
        {
            PlayerName = playerName,
            CurrentYear = currentYear,
            CurrentSemester = currentSemester,
            Split = split,
            Money = money,
            Time = time,
            Health = health,
            Grade = grade,
            Happiness = happiness,
            Social = social,
        };
    }

    public void SetCurrentStatus(CurrentStateData stateData)
    {
        SetMoney(stateData.Money);
        SetTime(stateData.Time);
        SetHealth(stateData.Health);
        SetGrade(stateData.Grade);
        SetHappiness(stateData.Happiness);
        SetSocial(stateData.Social);
        UpdateUI();
        OnStatusChanged?.Invoke();

        CheckGameOver();
        if (time <= 0) {EndSplit();}
    }

    private void CheckGameOver()
    {
        if (health <= 0)
        {
            Debug.Log("Game Over! Health is 0 or less. Transitioning to Result Scene...");
            modal.OpenBadEndModal("health");
        }
        else if (happiness <= 0)
        {
            Debug.Log("Game Over! Happiness is 0 or less. Transitioning to Result Scene...");
            modal.OpenBadEndModal("happiness");
        }
        else if (grade <= 0)
        {
            Debug.Log("Game Over! Grade is 0 or less. Transitioning to Result Scene...");
            modal.OpenBadEndModal("grade");
        }
        else if (social <= 0)
        {
            Debug.Log("Game Over! Social is 0 or less. Transitioning to Result Scene...");
            modal.OpenBadEndModal("social");
        }
    }

    private void EndGame()
    {
        List<ReceivedBadge> receivedBadges = new List<ReceivedBadge>();
        string goodBadgeType = "good";
        string badBadgeType = "bad";

        var badgeConditions = new List<(string statName, int value, string badgeType, bool condition)>
        {
            ("health", health,  goodBadgeType, health   >= 90),
            ("happiness", happiness, goodBadgeType, happiness >= 90),
            ("grade", grade,    goodBadgeType, grade    >= 90),
            ("social", social,  goodBadgeType, social   >= 90),
            ("money", money,    goodBadgeType, money    >= 2000),
            ("time",  allRemainingTime,     goodBadgeType, allRemainingTime     <= 10),

            ("health", health,  badBadgeType, health    <= 10),
            ("happiness", happiness, badBadgeType, happiness <= 10),
            ("grade", grade,    badBadgeType, grade     <= 10),
            ("social", social,  badBadgeType, social    <= 10),
            ("money", money,    badBadgeType, money     <= 200),
            ("time",  allRemainingTime,     badBadgeType, allRemainingTime      >= 50),
        };

        foreach (var (statName, _, badgeType, condition) in badgeConditions)
        {
            if (condition)
            {
                Debug.Log($"Congratulations! You got the {badgeType} {statName} badge");
                receivedBadges.Add(new ReceivedBadge(badgeType, statName));
            }
        }

        modal.OpenDisplayBadgeModal(receivedBadges, playerName);
    }




    public void RestartGame()
    {
        InitializeGame();
    }
}

public struct CurrentStateData
{
   public string PlayerName { get; set; }
   public int CurrentYear { get; set; }
   public int CurrentSemester { get; set; }
   public string Split { get; set; }
   public int Money { get; set; }
   public int Time { get; set; }
   public int Health { get; set; }
   public int Grade { get; set; }
   public int Happiness { get; set; }
   public int Social { get; set; }
}
