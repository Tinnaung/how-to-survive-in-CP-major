using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarAndScore : MonoBehaviour
{
    public Text score;
    public Bar bar;
    public Image upImage;    
    public Image downImage;  

    public float animationDuration = 1.5f;

    private float currentValue;
    private Coroutine animationCoroutine;

    public void Initialize(int maxScore)
    {
        bar.SetMaxHealth(maxScore);
        float.TryParse(score.text, out currentValue);

        if (upImage != null) upImage.enabled = false;
        if (downImage != null) downImage.enabled = false;
    }

    public void SetScore(int newValue)
    {
        // display up or down image
        if (newValue > currentValue && upImage != null)
            StartCoroutine(FlashImage(upImage));
        else if (newValue < currentValue && downImage != null)
            StartCoroutine(FlashImage(downImage));

        bar.SetHealth(newValue);

        // score animation
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(AnimateScore(currentValue, newValue));
    }

    // for score
    private IEnumerator AnimateScore(float from, float to)
    {
        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);
            float val = Mathf.Lerp(from, to, t);
            score.text = Mathf.RoundToInt(val).ToString();
            yield return null;
        }

        score.text = Mathf.RoundToInt(to).ToString();
        currentValue = to;
    }

    // for up or down image
    private IEnumerator FlashImage(Image image)
    {
        image.enabled = true;
        yield return new WaitForSeconds(0.5f); 
        image.enabled = false;
    }
}
