using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerTester : MonoBehaviour
{
    [SerializeField] private Timer timer = default;
    //[SerializeField] private Text counterText = default;
    public TextMeshProUGUI counterText = default;
    public TextMeshProUGUI repText = default;
    //public TextMeshProUGUI completeExText = default;

    private void Start()
    {
        timer.OnTimeout += ReactToTimeout;
        timer.OnTimeChanged += SetCounterText;
        timer.OnComplete += OnCompleteText;
        timer.OnRepComplete += OnRepCompleteText;
        //timer.OnExComplete += OnCompleteExText;
    }

    private void OnDestroy()
    {
        timer.OnTimeout -= ReactToTimeout;
        timer.OnTimeChanged -= SetCounterText;
        timer.OnComplete -= OnCompleteText;
        timer.OnRepComplete -= OnRepCompleteText;
        //timer.OnExComplete -= OnCompleteExText;
    }

    private void ReactToTimeout()
    {
        //print("Timeout");
    }

    private void SetCounterText()//float counter
    {
        //counterText.text = Mathf.Ceil(counter) + "";
        counterText.text = ".";
    }

    private void OnCompleteText()
    {
        // repText.text = "Finished!";
        //FindObjectOfType<AudioManager>().Play("Applause");
    }

    private void OnRepCompleteText()//float counter
    {
        //repText.text = Mathf.Ceil(counter) + "";
        repText.text = ".";

    }

    /*private void OnCompleteExText()
    {

        FindObjectOfType<AudioManager>().Play("Applause");
    }*/
}
