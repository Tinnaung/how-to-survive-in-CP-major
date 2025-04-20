using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadCharacter : MonoBehaviour
{
    public Image cloth;
    public Image hair;
    public Image pet;

    private Dictionary<int, string> clothDataMap = new()
    {
        {0, "girl-nisit" },
        {1, "hacker"},
        {2, "boy-nisit"},
        {3, "kengchang"}
    };

    private Dictionary<int, string> hairDataMap = new()
    {
        {0, "short" },
        {1, "ponytail"},
        {2, "long"},
        {3, "afro"}
    };
    private Dictionary<int, string> petDataMap = new()
    {
        {0, "cat" },
        {1, "dog"},
    };
    
    void Start()
    {
        string clothPath = clothDataMap[StaticData.Character.Cloth];
        string hairPath = hairDataMap[StaticData.Character.Hair];
        string petPath = petDataMap[StaticData.Character.Pet];
        Debug.Log("Character: ");
        Debug.Log($"Cloth: {clothPath}");
        Debug.Log($"Hair: {hairPath}");
        Debug.Log($"Pet: {petPath}");

        pet.sprite = Resources.Load<Sprite>($"components/character/pet/{petPath}");
        cloth.sprite = Resources.Load<Sprite>($"components/character/cloth/{clothPath}");
        hair.sprite = Resources.Load<Sprite>($"components/character/hair/{hairPath}");
    }
}
