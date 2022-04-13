using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlanetWalkEvents : MonoBehaviour
{
    PlanetWalkerPrototype planetWalker;
    Timer timer;
    TweenText tweenText;
    TweenRainbowText tweenRainbowText;
    private bool begun;

    public TextMeshProUGUI rainbowText;

    public GameObject avatar;
    public ParticleSystem avatarSparkles;
    public int emissionQuantity;

    // Start is called before the first frame update
    void Start()
    {
        begun = false;
        planetWalker = FindObjectOfType<PlanetWalkerPrototype>();
        tweenText = FindObjectOfType<TweenText>();
        tweenRainbowText = FindObjectOfType<TweenRainbowText>();
        timer = FindObjectOfType<Timer>();
    }

    private void OnEnable()
    {
        GameEvents.current.onExerciseExit += OnExit;
    }
    
    private void OnDisable()
    {
        GameEvents.current.onExerciseExit -= OnExit;
    }

    // Update is called once per frame
    void Update()
    {
        if (!begun && timer.exerciseBegun && !timer.exerciseComplete)
        {
            StartCoroutine(EarthWalkMusic());
            planetWalker.EnableWalker();
            begun = true;
            AvatarDisappear();
        }
        
        if (timer.exerciseComplete)
        {
            begun = false;
            planetWalker.DisableWalker();
            AvatarAppear();
            //FindObjectOfType<AudioManager>().Play("Applause");
        }

        if (planetWalker.rainbowCollected)
        {
            StartCoroutine(RainbowTextTween());
            rainbowText.text = planetWalker.rainbowCounter + "";
            planetWalker.rainbowCollected = false;
        }
    }


    void AvatarAppear()
    {
        avatar.SetActive(true);
        // avatarSparkles.Emit(emissionQuantity);
    }

    void AvatarDisappear()
    {
        avatar.SetActive(false);
        avatarSparkles.Emit(emissionQuantity);
    }

    IEnumerator EarthWalkMusic()
    {
        yield return new WaitForSeconds(1);

        FindObjectOfType<AudioManager>().Play("EarthWalkMusic");
    }

    void OnExit()
    {
        FindObjectOfType<AudioManager>().Stop("EarthWalkMusic");
    }

    IEnumerator RainbowTextTween()
    {
        tweenRainbowText.RainbowTweenIn();
        tweenRainbowText.RainbowTweenOut();
        yield return new WaitForSeconds(2);
    }
}
