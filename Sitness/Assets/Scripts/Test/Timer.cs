using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Timer : MonoBehaviour
{
    TweenText tweenText;
    TweenRepText tweenRepText;
    TimerTester timerTester;

    public event Action OnTimeout = delegate { };
    //public event Action<float> OnTimeChanged = delegate { };
    public event Action OnTimeChanged = delegate { };
    public event Action OnComplete = delegate { };
    //public event Action<float> OnRepComplete = delegate { };
    public event Action OnRepComplete = delegate { };

    [SerializeField] private float startTime = 1f;
    [SerializeField] private bool loop = false;
    [SerializeField] private bool autoStart = true;
    [SerializeField] private bool reverse = false;

    [SerializeField] private float maxTime = 8.0f;
    [SerializeField] private int reps = 0;
    [SerializeField] private int maxReps = 1;
    [SerializeField] private float timeSpeed = 1;
    [SerializeField] private int repTime;
    [SerializeField] private bool endByMaxReps = true;

    [CanBeNull] public string sceneName = null;


    private bool timeout = true;
    private float timer = 0.0f;

    public bool exerciseBegun = false;
    public bool exerciseComplete = false;

    private void Start()
    {
        tweenText = FindObjectOfType<TweenText>();
        tweenRepText = FindObjectOfType<TweenRepText>();
        timerTester = FindObjectOfType<TimerTester>();

        maxTime = maxTime - 0.05f;

        if (autoStart)
        {
            StartTimer();
            exerciseBegun = true;
        }
    }

    private void Update()
    {
        if (reverse == false)
        {
            if (timer > 0 && !timeout)
            {
                timer -= Time.deltaTime;
                //OnTimeChanged.Invoke(timer);
                OnTimeChanged.Invoke();
            }
            else if (timer < 0 && !timeout)
            {
                timeout = true;
                //OnTimeout.Invoke();

                if (loop)
                {
                    StartTimer();
                }
            }
        }

        if (reverse == true)
        {
            if (timer -1 < maxTime - 1 && !timeout)
            {
                timer += Time.deltaTime / timeSpeed;
                //OnTimeChanged.Invoke(timer);
                OnTimeChanged.Invoke();
            }
            else if (timer > maxTime && !timeout)
            {
                timeout = true;
                OnTimeout.Invoke();


                if (loop)
                {
                    reps += 1;
                    //OnRepComplete.Invoke(reps);
                    OnRepComplete.Invoke();
                    StartCoroutine(RepTweenIn());
                    //tweenRepText.RepTweenOut();

                    if (reps == maxReps && endByMaxReps)
                    {
                        exerciseComplete = true;
                        OnComplete.Invoke();

                        StartCoroutine(FinalTweenStop());
                        GameEvents.current.ExerciseEnd();
                    }
                    else
                    {
                        startTime = 0;
                        StartTimer();
                    }
                }
            }
        }
    }

    public void StartTimer()
    {
        exerciseBegun = true;
        timer = startTime;
        timeout = false;
        Debug.Log("Timer 0 NOW!");

        if (startTime == 0 || startTime < 0.999f)
        {
            timerTester.counterText.text = "";
        }
        
        StopCoroutine("TweenOutEverySecond");
        StartCoroutine("TweenOutEverySecond");

        StopCoroutine("TweenInEverySecond");
        StartCoroutine("TweenInEverySecond");

        tweenText.TweenIn();
    }

    public void StopTimer()
    {
        timer = startTime;
        exerciseComplete = true;
        exerciseBegun = false;
        OnComplete.Invoke();
        StartCoroutine(FinalTweenStop());
    }

    IEnumerator TweenOutEverySecond()
    {
        while (true)
        {
            //Debug.Log("Tick");
            tweenText.Tween();
            yield return new WaitForSeconds(timeSpeed);
        }
    }

    IEnumerator TweenInEverySecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeSpeed);
            //Debug.Log("Tock");
            tweenText.TweenIn();
        }
    }

    IEnumerator FinalTweenStop()
    {
        yield return new WaitForSeconds(1);
        StopAllCoroutines();
    }

    IEnumerator RepTweenIn()
    {
        while (true)
        {
            tweenRepText.RepTweenOut();
            yield return new WaitForSeconds(maxTime * repTime);
            tweenRepText.RepTweenIn();
        }
    }
}
