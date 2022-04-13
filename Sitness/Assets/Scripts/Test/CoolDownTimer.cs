using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoolDownTimer : MonoBehaviour
{
    public float currentTime;
    public float startingTime;
    public float countUpTo;

    public bool secondCounted = false;

    [SerializeField] TextMeshProUGUI countdownText;

    public float fadeTime;

    void Start()
    {
        currentTime = startingTime;
    }

    void Update()
    {
            currentTime += 1 * Time.deltaTime;
            countdownText.text = currentTime.ToString("0.00");

            if (currentTime >= countUpTo + 1)
            {
                currentTime = 1;
            }

            if (currentTime >= countUpTo + 0.5)
            {
                countdownText.text = ("and");
            }
            else
            {
                countdownText.text = ((int)currentTime).ToString("0");
            }
            StartCoroutine(FadeFrequency());
        
    }

    IEnumerator FadeCoroutine()
    {
        float waitTime = 0;
        while (waitTime < 1)
        {
            countdownText.fontMaterial.SetColor("_FaceColor", Color.Lerp(Color.white, Color.clear, waitTime));
            yield return null;
            waitTime += Time.deltaTime / fadeTime;
        }
    }
    
    IEnumerator FadeFrequency(float waitTime = 1)
    {
        while (true)
        {
            yield return FadeCoroutine();
        }
    }
}
