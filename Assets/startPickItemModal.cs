using UnityEngine;
using UnityEngine.UI;
using System;

public class startPickItemModal : MonoBehaviour
{
    public Button confirmButton;
    private Action onConfirm;

    void Start()
    {
        gameObject.SetActive(false);

        confirmButton.onClick.AddListener(() =>
        {
            onConfirm?.Invoke();
            CloseModal();
        });

        OpenModal(() => {
            Debug.Log("Confirmed!");
        });
    }

    public void OpenModal(Action onConfirmAction)
    {
        onConfirm = onConfirmAction;
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseModal()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
