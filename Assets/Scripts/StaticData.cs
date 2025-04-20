using UnityEngine;

public class StaticData : MonoBehaviour
{
    public static string Username;
    public static Character Character;
}


public struct Character
{
    public int Cloth;
    public int Pet;
    public int Hair;
    public int Glasses;

    public Character(int currentClothIndex = 0, int currentHairIndex = 0, int currentPetIndex = 0, int currentGlassesIndex = 0)
    {
        Cloth = currentClothIndex;
        Hair = currentHairIndex;
        Pet = currentPetIndex;
        Glasses = currentGlassesIndex;
    }
}