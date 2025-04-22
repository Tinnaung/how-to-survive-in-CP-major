using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class SplitTransition : MonoBehaviour
{
    public Animator transition;
    public Text yearSemText;
    public Text splitText;
    public float transitionDuration = 3f;
    private System.Action onComplete;

    public void PlaySplitTransition(int year, int sem, string split, System.Action callback = null)
    {
        yearSemText.text = $"ปีที่ {year} เทอม {sem}";
        splitText.text = split == "Midterm" ? "กลางภาค" : "ปลายภาค";
        onComplete = callback;

        gameObject.SetActive(true);
        StartCoroutine(PlayAndHide());
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator PlayAndHide()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionDuration);
        gameObject.SetActive(false);

        onComplete?.Invoke();
    }
}
