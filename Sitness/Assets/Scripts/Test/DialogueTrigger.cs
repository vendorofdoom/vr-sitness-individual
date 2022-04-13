using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialoguePre dialoguePr;
    public DialoguePost dialoguePo;

    public void TriggerDialoguePre()
    {
        FindObjectOfType<DialogueManager>().StartDialoguePre(dialoguePr);
    }

    public void TriggerDialoguePost()
    {
        FindObjectOfType<DialogueManager>().StartDialoguePost(dialoguePo);
    }
}
