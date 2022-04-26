using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownGuidance : MonoBehaviour
{
    public Transform GuidesParent;

    public bool guidesVisible;
    public GameObject leftGuide;
    public GameObject rightGuide;

    public Transform High;
    public Transform Low;

    private float TimeInMovement = 0f;
    public float MovingUpTime;
    public float MovingDownTime;

    public Image buttonImage;
    public Sprite yButton;
    public Sprite xButton;
    public bool showYButton = true;
    public bool showXButton = true;


    [SerializeField]
    private GuidanceState guidanceState;

    public DialogueManager dm;
    public DialogueTrigger dt;
    public int displayTriggerSentenceIdx;


    private enum GuidanceState
    {
        PauseHigh,
        PauseLow,
        MovingUp,
        MovingDown
    }

    private void Update()
    {
        GuideVisibility();

        switch (guidanceState)
        {
            case GuidanceState.PauseHigh:
                //GuidesParent.rotation = High.rotation;
                DisplayYButtonIcon();
                break;
            case GuidanceState.PauseLow:
                //GuidesParent.rotation = Low.rotation;
                DisplayXButtonIcon();
                break;
            case GuidanceState.MovingUp:
                buttonImage.enabled = false;
                GuidesParent.rotation = Quaternion.Lerp(Low.rotation, High.rotation, TimeInMovement / MovingUpTime);
                break;
            case GuidanceState.MovingDown:
                buttonImage.enabled = false;
                GuidesParent.rotation = Quaternion.Lerp(High.rotation, Low.rotation, TimeInMovement / MovingDownTime);
                break;
        }

        TimeInMovement += Time.deltaTime;
    }


    public void StartMovingUp()
    {
        TimeInMovement = 0f;
        guidanceState = GuidanceState.MovingUp;
    }

    public void StartMovingDown()
    {
        TimeInMovement = 0f;
        guidanceState = GuidanceState.MovingDown;
    }

    public void PauseHigh()
    {
        TimeInMovement = 0f;
        guidanceState = GuidanceState.PauseHigh;
    }
    
    public void PauseLow()
    {
        TimeInMovement = 0f;
        guidanceState = GuidanceState.PauseLow;
    }

    private void DisplayXButtonIcon()
    {
        if (showXButton)
        {
            buttonImage.sprite = xButton;
            buttonImage.enabled = true;
        }
    }
        
    private void DisplayYButtonIcon()
    {
        if (showYButton)
        {
            buttonImage.sprite = yButton;
            buttonImage.enabled = true;
        }
    }

    private void GuideVisibility()
    {
        if (!guidesVisible)
        {
            if (dm.sentence == dt.dialoguePr.sentences[displayTriggerSentenceIdx])
            {
                guidesVisible = true;
            }
        }

        leftGuide.SetActive(guidesVisible);
        rightGuide.SetActive(guidesVisible);
    }
}
