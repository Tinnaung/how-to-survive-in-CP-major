using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameModal : MonoBehaviour
{
    public Button quitButton;
    public Button restartButton;
    public ModalManager _modalManager;
    public LogicScript _logicScript;

    private void OnQuit()
    {
        _modalManager.CloseModal();
        NewMonoBehaviourScript.QuitGame();
    }

    private void OnRestart()
    {
        _logicScript.RestartGame();
        _modalManager.CloseModal();
        SceneManager.LoadScene("ChoosingCharacter");
    }

    public void OpenEndGameModal()
    {
        gameObject.SetActive(true);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        quitButton.onClick.AddListener(() => OnQuit());
        restartButton.onClick.AddListener(() => OnRestart());

        gameObject.SetActive(false);
    }
}
