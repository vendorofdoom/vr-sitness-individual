using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    DialogueInteraction dialogueInteraction;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public int dialogueRemaining;

    public bool firstDialogue;
    //public bool secondDialogue;
    public bool exerciseFinished;
    public bool exitDialogue = false;

    public Animator animator;

    private Queue<string> sentences;

    public string sentence;

    void Start()
    {
        dialogueInteraction = FindObjectOfType<DialogueInteraction>();
        GameEvents.current.SceneBegin();
        sentences = new Queue<string>();
        firstDialogue = false;
        //secondDialogue = false;
    }

    // starts dialogue...
    public void StartDialoguePre(DialoguePre dialoguePr)
    {
        if (firstDialogue == false)
        {
            dialogueRemaining = dialoguePr.sentences.Length;

            animator.SetBool("IsOpen", true);
            FindObjectOfType<AudioManager>().Play("DialogueBoxOpen");

            nameText.text = dialoguePr.name;

            sentences.Clear();

            foreach (string sentence in dialoguePr.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }
    }

    public void StartDialoguePost(DialoguePost dialoguePo)
    {
        if (exerciseFinished == true) // && secondDialogue == false
        {
            dialogueRemaining = dialoguePo.sentences.Length;

            animator.SetBool("IsOpen", true);
            FindObjectOfType<AudioManager>().Play("DialogueBoxOpen");

            nameText.text = dialoguePo.name;

            sentences.Clear();

            foreach (string sentence in dialoguePo.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }
    }

    // cycles through dialogue...
    public void DisplayNextSentence()
    {
        if (!exerciseFinished && sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (exerciseFinished && sentences.Count == 1)
        {
            exitDialogue = true;
        }

        sentence = sentences.Dequeue();
        FindObjectOfType<AudioManager>().Play("DialogueBoxNext");
        //dialogueText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        FindObjectOfType<AudioManager>().Play("DialogueBoxClose");

        firstDialogue = true;
        // triggers next phase of scene...

        if (!exerciseFinished)
        {
            if (GameEvents.current != null)
            {
                GameEvents.current.ExerciseBegin();
                firstDialogue = true;
                dialogueInteraction.dialogueStarted = false;
            }
            return;
        }
        else if (exerciseFinished)
        {
            //secondDialogue = true;
            dialogueInteraction.dialogueStarted = false;
        }
    }
}
