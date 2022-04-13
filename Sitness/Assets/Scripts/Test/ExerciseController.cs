using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ExerciseController : MonoBehaviour
{
    DialogueManager dialogueManager;
    Timer timer;
    public TweenFinished finishedTween;
    private GameObject finishedText;

    private void Start()
    {
        GameEvents.current.onSceneBegin += OnSceneBegin;
        GameEvents.current.onExerciseBegin += OnExerciseBegin;
        GameEvents.current.onExerciseEnd += OnExerciseEnd;
        GameEvents.current.onDebriefEnd += OnDebriefEnd;

        dialogueManager = FindObjectOfType<DialogueManager>();
        finishedTween = FindObjectOfType<TweenFinished>();
        finishedText = GameObject.FindGameObjectWithTag("FinishedText");
        timer = FindObjectOfType<Timer>();

        finishedText.SetActive(false);
    }

    private void OnSceneBegin()
    {
        Debug.Log("Scene loaded!");
        // Begin welcome dialogue
    }

    private void OnExerciseBegin()
    {
        Debug.Log("Exercise begun!");
        //dialogueManager.secondDialogue = false;
        // Begin countdown to exercise

        // Begin exercise
        timer.StartTimer();
    }

    private void OnExerciseEnd()
    {
        Debug.Log("Exercise finished!!!! Wooop!");
        dialogueManager.exerciseFinished = true;
        FindObjectOfType<AudioManager>().Play("Applause");
        finishedText.SetActive(true);
        finishedTween.TweenFinish();

        if (timer.sceneName == SceneChange.SceneNames.PlanetWalking || timer.sceneName == SceneChange.SceneNames.CloudCreation)
        {
            timer.StopTimer();
        }
        
        // Begin debrief
    }

    private void OnDebriefEnd()
    {
        // play again or 
    }

    private void OnDestroy()
    {
        GameEvents.current.onSceneBegin -= OnSceneBegin;
        GameEvents.current.onExerciseBegin -= OnExerciseBegin;
        GameEvents.current.onExerciseEnd -= OnExerciseEnd;
        GameEvents.current.onDebriefEnd -= OnDebriefEnd;
    }

    IEnumerator TweenFinished()
    {
        yield return new WaitForSeconds(4);

    }
}