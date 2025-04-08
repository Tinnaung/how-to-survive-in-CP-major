using UnityEngine;

public class ThaiFormatter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMPro.TMP_Text[] tMP_texts;
    void Start()
    {
        tMP_texts = GetComponentsInChildren<TMPro.TMP_Text>();
        foreach (TMPro.TMP_Text tMP_text in tMP_texts)
        {
            tMP_text.text = ThaiFontAdjuster.Adjust(tMP_text.text);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (TMPro.TMP_Text tMP_text in tMP_texts) { 
            tMP_text.text = ThaiFontAdjuster.Adjust(tMP_text.text);
        }
    }
}
