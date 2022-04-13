using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownSound : MonoBehaviour
{
    public bool downComplete = false;
    public bool upComplete = false;

    public float resetPosition;
    public CoolDownPrototype handPosition;
    public AudioSource waterIn;
    public AudioSource waterOut;

    private float handsUp;
    private bool handsUpSet = false;
    private float handsDown;
    private bool handsDownSet = false;

    public AudioSource highConfirm;
    public AudioSource lowConfirm;
    private bool soundPlayed = false;

    void Start()
    {
        handPosition = GetComponent<CoolDownPrototype>();
    }

    void Update()
    {
        // Set the highest and lowest hand positions offset by -0.02 and +0.02 (to allow space for resetPosition).

        // Lower the left controller and press X to set your lowest position
        if (Input.GetButtonDown("Fire3"))
        {
            lowConfirm.Play();
            handsDown = handPosition.prevY + 0.02f;
            handsDownSet = true;
            Debug.Log("X pressed");
        }

        // Raise the left controller and press Y to set your highest position
        if (Input.GetButtonDown("Fire4"))
        {
            highConfirm.Play();
            handsUp = handPosition.prevY - 0.02f;
            handsUpSet = true;
            Debug.Log("Y pressed");
        }

        // Once both lowest and highest positions are set, the sounds will become active
        if (handsUpSet && handsDownSet)
        {
            if (handPosition.prevY <= handsDown)
            {
                downComplete = true;
                resetPosition = handPosition.prevY;
                if (!soundPlayed)
                {
                    lowConfirm.Play();
                    soundPlayed = true;   
                }
            }
            else
            {
                downComplete = false;
            }

            if (handPosition.prevY >= handsUp)
            {
                upComplete = true;
                resetPosition = handPosition.prevY;
                if (soundPlayed)
                {
                    highConfirm.Play();
                    soundPlayed = false;
                }
            }
            else
            {
                upComplete = false;
            }

            // Will only play water in and out sounds once upward and downward motions are complete and reverse movement commences past resetPosition
            if (downComplete == false && handPosition.prevY > resetPosition && handPosition.prevY < resetPosition + 0.01)
            {
                waterIn.Play();
            }

            if (upComplete == false && handPosition.prevY < resetPosition && handPosition.prevY > resetPosition - 0.01)
            {
                waterOut.Play();
            }
        }
    }
}
