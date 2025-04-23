using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BadEndModal : MonoBehaviour
{
    public Image background;
    public Text titleText;
    public Button quitButton;
    public Button restartButton;
    public ModalManager _modalManager;
    public LogicScript _logicScript;
    private Dictionary<string, BadEndData> badEndDataMap;
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

    public void OpenBadEndModal(string type)
    {
        badEndDataMap = new Dictionary<string, BadEndData>
        {
            {"health", new BadEndData("ไม่น้า คุณตุยก่อนอายุ 30 ซะอีก", "health-badend")},
            {"social", new BadEndData("สังคมไม่ต้อนรับคุณอีกต่อไป ฮือ ๆ", "social-badend")},
            {"happiness", new BadEndData("นี่เราเป็นบ้าอะไรกันนี่", "happiness-badend")},
            {"grade", new BadEndData("ดูทรงจะได้จบหลังแพทย์", "grade-badend")},
        };
        Debug.Log(badEndDataMap[type]);

        if (badEndDataMap.ContainsKey(type))
        {
            var badEndData = badEndDataMap[type];
            titleText.text = badEndData.Title;
            background.sprite = badEndData.BackgroundImage;

            gameObject.SetActive(true);
        }
    }

    void Start()
    {
        badEndDataMap = new Dictionary<string, BadEndData>
        {
            {"health", new BadEndData("ไม่น้า คุณตุยก่อนอายุ 30 ซะอีก", "health-badend")},
            {"social", new BadEndData("สังคมไม่ต้อนรับคุณอีกต่อไป ฮือ ๆ", "social-badend")},
            {"happiness", new BadEndData("นี่เราเป็นบ้าอะไรกันนี่", "happiness-badend")},
            {"grade", new BadEndData("ดูทรงจะได้จบหลังแพทย์", "grade-badend")},
        };
        quitButton.onClick.AddListener(() => OnQuit());
        restartButton.onClick.AddListener(() => OnRestart());

        gameObject.SetActive(true);
    }
}

public struct BadEndData
{
    public string Title;
    public Sprite BackgroundImage;

    public BadEndData(string title, string backgroundPath)
    {
        Title = title;
        BackgroundImage = Resources.Load<Sprite>($"components/result/badend/{backgroundPath}");
    }
}
