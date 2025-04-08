using UnityEngine;

public class ModalManager : MonoBehaviour
{
    public GameObject modalBackground;

    void Start()
    {
        modalBackground.SetActive(false);
    }

    public void OpenModal()
    {
        modalBackground.SetActive(true);
    }

    public void CloseModal()
    {
        modalBackground.SetActive(false);
    }
}
