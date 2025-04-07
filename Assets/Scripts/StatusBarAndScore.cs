

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class StatusBarAndScore : MonoBehaviour
{
    public Text score;
    public Bar bar;
   
    public void Initialize(int maxScore)
    {
        bar.SetMaxHealth(maxScore);
    }
    public void SetScore(int health)
    {
        bar.SetHealth(health);
        score.text = health.ToString();
    }
}
