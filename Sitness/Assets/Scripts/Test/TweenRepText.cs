using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TweenRepText : MonoBehaviour
{
    public float tweenTime;
    public float fadeTime;
    public float scaleSize;
    public TextMeshProUGUI textMesh;

    GameObject rainbowText;


    // Start is called before the first frame update
    void Start()
    {
        textMesh = this.gameObject.GetComponent<TextMeshProUGUI>();
        //Tween();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RepTweenOut()
    {
        LeanTween.cancel(gameObject);

        transform.localScale = Vector3.one;

        LeanTween.scale(gameObject, Vector3.one * scaleSize, tweenTime).setEase(LeanTweenType.easeInCirc);
        LeanTween.alphaCanvas(textMesh.GetComponent<CanvasGroup>(), 0f, fadeTime).setEase(LeanTweenType.easeInCirc);
    }

    public void RepTweenIn()
    {
        LeanTween.alphaCanvas(textMesh.GetComponent<CanvasGroup>(), 1f, 0f).setEase(LeanTweenType.easeInCirc);
    }
}
