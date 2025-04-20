using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterEffect : MonoBehaviour
{
    public LogicScript logic;
    public GameObject effectPrefab;
    public Transform effectParent;
    public List<Effect> effectList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("MainGameLogic").GetComponent<LogicScript>();
        logic.OnStatusChanged += UpdateEffect;

        gameObject.SetActive(false);
    }

    public void UpdateEffect()
    {
        CurrentStateData currentState = logic.GetCurrentStatus();

        List<Effect> activatedEffect = new List<Effect>();
        string goodEffectType = "good";
        string badEffectType = "bad";

        int health = currentState.Health;
        int happiness = currentState.Happiness;
        int grade = currentState.Grade;
        int social = currentState.Social;

        var effectConditions = new List<(string statName, int value, string badgeType, bool condition)>
        {
            ("health", health,  goodEffectType, health   >= 90),
            ("happiness", happiness, goodEffectType, happiness >= 90),
            ("grade", grade,    goodEffectType, grade    >= 90),
            ("social", social,  goodEffectType, social   >= 90),
            

            ("health", health,  badEffectType, health    <= 10),
            ("happiness", happiness, badEffectType, happiness <= 10),
            ("grade", grade,    badEffectType, grade     <= 10),
            ("social", social,  badEffectType, social    <= 10),
        };

        foreach (var (statName, _, effectType, condition) in effectConditions)
        {
            if (condition)
            {
                Debug.Log($"You got the {statName} {effectType} effect");
                var isBehind = statName == "health" && effectType == "good";
                activatedEffect.Add(new Effect(effectType, statName, isBehind));
            }
        }

        effectList = activatedEffect;
        ApplyEffect();
    }

    public void ApplyEffect()
    {
        gameObject.SetActive(true);

        foreach (Transform child in effectParent)
        {
            if (child.name.StartsWith("Effect-"))
            {
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < effectList.Count; i++)
        {
            var ef = effectList[i];
            
            GameObject effect = Instantiate(effectPrefab, effectParent);
            effect.name = $"Effect-{ef.EffectFrom}-{ef.EffectType}";
            if (ef.IsBehind)
            {
                effect.transform.SetAsFirstSibling();
            }
            else 
            {
                effect.transform.SetAsLastSibling();
            }

            Image img = effect.GetComponent<Image>();

            Sprite sprite = Resources.Load<Sprite>($"components/character/effect/{ef.EffectType}-{ef.EffectFrom}");
            if (sprite == null)
            {
                Debug.LogWarning($"Sprite not found at: components/character/effect/{ef.EffectType}-{ef.EffectFrom}");
            }
            else
            {
                img.sprite = sprite;
            }
        }
    }
}

public struct Effect {
    public string EffectFrom;
    public string EffectType;
    public bool IsBehind;

    public Effect(string effectFrom = null, string effectType = null, bool isBehind = false)
    {
        EffectFrom = effectFrom;
        EffectType = effectType;
        IsBehind = isBehind;
    }
}
