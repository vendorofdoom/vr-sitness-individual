using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteraction : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;
    public DialogueManager dialogueManager;
    public SceneChange sceneChange;
    public Timer timer;

    public bool dialogueStarted;
    public bool exerciseFinished;
    public bool exitChoice = false;
    public bool loadScene = false;

    public bool welcome;
    public bool complete;

    void Start()
    {
        timer = FindObjectOfType<Timer>();
        sceneChange = FindObjectOfType<SceneChange>();
        dialogueStarted = false;
        exerciseFinished = false;
        welcome = false;
        complete = false;
        //set dialogue to begin a few seconds after loading scene...


    }

    private void Update()
    {
        if (welcome == false)
            StartCoroutine(WelcomeText());

        /*if (Input.GetButtonDown("Fire2") && dialogueManager.firstDialogue == false)
        {
            dialogueTrigger.TriggerDialoguePre();
            dialogueStarted = true;
        }*/

        if (Input.GetButtonDown("Fire1") && dialogueStarted == true && dialogueManager.firstDialogue == false)
        {
            dialogueManager.DisplayNextSentence();
        }

        if (complete == false && timer.exerciseComplete && !exitChoice)
            StartCoroutine(PostExcerciseText());

        /*if (Input.GetButtonDown("Fire2") && dialogueManager.exerciseFinished == true && !exitChoice)
        {
            dialogueTrigger.TriggerDialoguePost();
            dialogueStarted = true;
        }*/

        if (Input.GetButtonDown("Fire1") && dialogueStarted && dialogueManager.exerciseFinished && !exitChoice)
        {
            dialogueManager.DisplayNextSentence();

        }

        if (dialogueManager.exitDialogue == true)
        {
            StartCoroutine(WaitForExit());
            if (exitChoice == true)
            {
                if (Input.GetButtonDown("Fire1") && !loadScene)
                {
                    loadScene = true;
                    sceneChange.ReloadCurrentScene();
                }

                if (Input.GetButtonDown("Fire2") && !loadScene)
                {
                    loadScene = true;
                    GameEvents.current.ExerciseExit();
                    sceneChange.LoadMainMenuScene();
                }
            }
        }
        
        IEnumerator WaitForExit()
        {
            yield return new WaitForSeconds(0.5f);
            exitChoice = true;
        }

        IEnumerator WelcomeText()
        {
            welcome = true;
            yield return new WaitForSeconds(1.4f);
            dialogueTrigger.TriggerDialoguePre();
            dialogueStarted = true;
        }

        IEnumerator PostExcerciseText()
        {
            complete = true;
            yield return new WaitForSeconds(1);
            dialogueTrigger.TriggerDialoguePost();
            dialogueStarted = true;
        }

    }
}
