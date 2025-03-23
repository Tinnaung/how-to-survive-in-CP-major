using UnityEngine;
using UnityEngine.UI;

public class ChoosingCharacterLogic : MonoBehaviour
{

    public int currentClothIndex = 0;
    public int currentPetIndex = 0;
    public int currentHairIndex = 0;
    public GameObject[] clothes;
    public GameObject[] pets;
    public GameObject[] hairs;
    public Button previousClothButton; 
    public Button nextClothButton;
    public Button previousPetButton;
    public Button nextPetButton;
    public Button previousHairButton;
    public Button nextHairButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeCloth(currentClothIndex);
        ChangePet(currentPetIndex);
        ChangeHair(currentHairIndex);

        previousClothButton.onClick.AddListener(HandlePreviousCloth);
        nextClothButton.onClick.AddListener (HandleNextCloth);

        previousPetButton.onClick.AddListener(HandlePreviousPet);
        nextPetButton.onClick.AddListener(HandleNextPet);

        previousHairButton.onClick.AddListener(HandlePreviousHair);
        nextHairButton.onClick.AddListener(HandleNextHair);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangePet(int index) {
        foreach (var pet in pets)
        {
            pet.SetActive(false);
        }
        pets[index].SetActive(true);
        currentPetIndex = index;
    }

    private void ChangeCloth(int index) {
        foreach (var cloth in clothes)
        { 
            cloth.SetActive(false);
        }
        clothes[index].SetActive(true);
        currentClothIndex = index;
    }

    private void ChangeHair(int index) {
        foreach (var hair in hairs)
        {
            hair.SetActive(false);
        }
        hairs[index].SetActive(true);
        currentHairIndex = index;
    }

    public void HandleNextPet() { 
    
        int newPetIndex = (currentPetIndex + 1) % pets.Length;
        ChangePet(newPetIndex);
    }

    public void HandlePreviousPet()
    {
        int newPetIndex = (currentPetIndex + pets.Length - 1) % pets.Length;
        ChangePet(newPetIndex);
    }

    public void HandleNextCloth()
    {

        int newClothIndex = (currentClothIndex + 1) % clothes.Length;
        ChangeCloth(newClothIndex);
    }

    public void HandlePreviousCloth()
    {

        int newClothIndex = (currentClothIndex + clothes.Length - 1) % clothes.Length;
        ChangeCloth(newClothIndex);
    }

    public void HandleNextHair() { 

        int newHairIndex = (currentHairIndex + 1) % hairs.Length;
        ChangeHair(newHairIndex);

    }

    public void HandlePreviousHair()
    {

        int newHairIndex = (currentHairIndex + hairs.Length - 1) % hairs.Length;
        ChangeHair(newHairIndex);
    }
}
