using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class CoolDown : MonoBehaviour
{



    public Transform water;

    public float minWaterLevel;
    public float maxWaterLevel;

    public Transform leftHand;
    public Transform rightHand;

    //public XRController leftController;
    //public XRController rightController;


    public float prevY;
    public float currY;


    public bool downComplete = false;
    public bool upComplete = false;

    public float resetPosition;
    public AudioSource waterIn;
    public AudioSource waterOut;

    private float handsUp;
    private bool handsUpSet = false;
    private float handsDown;
    private bool handsDownSet = false;

    public AudioSource highConfirm;
    public AudioSource lowConfirm;
    private bool soundPlayed = false;


    public InputActionReference leftY;
    public InputActionReference leftX;

    private void OnEnable()
    {
        leftY.action.performed += ConfigureHighWaterLevel;
        leftX.action.performed += ConfigureLowWaterLevel;
    }

    private void OnDisable()
    {
        leftY.action.performed -= ConfigureHighWaterLevel;
        leftX.action.performed -= ConfigureLowWaterLevel;
    }

    // Set the highest and lowest hand positions offset by -0.02 and +0.02 (to allow space for resetPosition).
    // Raise the left controller and press Y to set your highest position
    void ConfigureHighWaterLevel(InputAction.CallbackContext callbackContext)
    {
        if (!handsDownSet || (handsDownSet && (currY > handsDown)))
        {
            highConfirm.Play();
            handsUp = prevY - 0.02f;
            handsUpSet = true;
            Debug.Log("Y pressed");
        }
    }

    // Set the highest and lowest hand positions offset by -0.02 and +0.02 (to allow space for resetPosition).
    // Lower the left controller and press X to set your lowest position
    void ConfigureLowWaterLevel(InputAction.CallbackContext callbackContext)
    {
        if (!handsUpSet || (handsUpSet && (currY < handsUp)))
        {
            lowConfirm.Play();
            handsDown = prevY + 0.02f;
            handsDownSet = true;
            Debug.Log("X pressed");

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currY = (leftHand.position.y + rightHand.position.y) / 2;
        prevY = currY;
    }

    // Update is called once per frame
    void Update()
    {
        MoveWater();
        PlaySounds();

    }

    void MoveWater()
    {

        currY = (leftHand.position.y + rightHand.position.y) / 2;
        prevY = currY;

        if (handsUpSet && handsDownSet)
        {
            float waterY = Mathf.Lerp(minWaterLevel, maxWaterLevel, Mathf.InverseLerp(handsDown, handsUp, currY));
            water.position = new Vector3(water.position.x, waterY, water.position.z);
        }


    }

    void PlaySounds()
    {

        // Once both lowest and highest positions are set, the sounds will become active
        if (handsUpSet && handsDownSet)
        {
            if (prevY <= handsDown)
            {
                downComplete = true;
                resetPosition = prevY;
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

            if (prevY >= handsUp)
            {
                upComplete = true;
                resetPosition = prevY;
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
            if (downComplete == false && prevY > resetPosition && prevY < resetPosition + 0.01)
            {
                waterIn.Play();
            }

            if (upComplete == false && prevY < resetPosition && prevY > resetPosition - 0.01)
            {
                waterOut.Play();
            }
        }
    }
}
