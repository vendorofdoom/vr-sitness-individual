using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class CloudPrototype : MonoBehaviour
{
    [Header("Cloud Generation")]
    public Transform cloudSpawnPoint;
    public GameObject[] cloudPrefab;
    public float spawnMinDist;
    public float spawnMaxDist;
    public float minCloudScale;
    public float initialCloudForce;

    [Header("Cloud Swooshing")]
    public float cloudForce;
    [Range(0f, 1f)]
    public float cloudLift;
    [Tooltip("0 = more in the direction of the cloud velocity, 1 = more in the direction of the spawn forward")]
    [Range(0f, 1f)]
    public float swooshDirectionBalance;
    public bool isSwooshing;
    [SerializeField]
    private float swooshStartDist;
    [Range(0f, 1f)]
    public float percentDistToSwoosh; // TODO: change to a percentage of maxRecordedHandleDist?
    public LayerMask cloudLayer;
    private float currDelta;
    private float prevDelta;
    public float swooshDistThreshold;

    [Header("Flexible Swoosh")] public bool enableFlexibleSwoosh = false;
    [Range(0f, 1f)]
    public float minimumChangeInGrowthPercentage = 0.2f;

    [Header("Haptics")]
    [Range(0f, 1f)]
    public float cloudSwooshMaxAmp;
    [Range(0f, 1f)]
    public float cloudGenAmp;

    [Header("Exercise Duration Parameters")]
    public int cloudsSpawned;
    public int cloudsRequired;
    public SceneChange sceneChange;

    [Header("Controllers")]
    public XRBaseController leftController;
    public XRBaseController rightController;
    public InputActionReference leftGrip;
    public InputActionReference rightGrip;
    [Range(0f, 1f)]
    public float gripThreshold = 0.5f;
    [SerializeField]
    private bool leftGripPressed;
    [SerializeField]
    private bool rightGripPressed;
    [SerializeField]
    private bool doubleGripEnabled;
    [SerializeField]
    private float distanceBetweenHands;
    [SerializeField]
    private float prevDistanceBetweenHands;
    [SerializeField]
    private float maxRecordedHandleDist;

    private bool gameEnded = false;

    void Start()
    {
        //RenderLine(leftController, rightController);

        distanceBetweenHands = Vector3.Distance(leftController.transform.position, rightController.transform.position);
        prevDistanceBetweenHands = distanceBetweenHands;

        isSwooshing = false;
        cloudsSpawned = 0;

        currDelta = 0;
        prevDelta = currDelta;
    }

    void Update()
    {
        RecordPlayerHandleDists();
        PositionCloudSpawnPoint();
        CheckIfSwooshing();
        CheckGripsPressed();
        CloudBrewingRumble();
        CheckEndExercise(); 
    }
    private void FixedUpdate()
    {
        if (isSwooshing)
        {
            ApplySwooshForceToCloudsInRange(Mathf.Abs(prevDelta - currDelta) / Time.fixedDeltaTime);
        }
    }


    private void CheckGripsPressed()
    {
        float leftGripVal = leftGrip.action.ReadValue<float>();
        float rightGripVal = rightGrip.action.ReadValue<float>();

        leftGripPressed = (leftGripVal >= gripThreshold);
        rightGripPressed = (rightGripVal >= gripThreshold);
     
        if (doubleGripEnabled)
        {
            if (!leftGripPressed && !rightGripPressed)
            {
                OnDoubleGripExit();
            }
        }
        else
        {
            if (leftGripPressed && rightGripPressed)
            {
                OnDoubleGripEnter();
            }
        }

    }

    private void OnDoubleGripEnter()
    {
        doubleGripEnabled = true;
    }

    private void OnDoubleGripExit()
    {
        doubleGripEnabled = false;
        ProduceCloud();
    }

    private void CloudBrewingRumble()
    {
        if (doubleGripEnabled)
        {
            leftController.SendHapticImpulse(cloudGenAmp, Time.deltaTime * 0.5f);
            rightController.SendHapticImpulse(cloudGenAmp, Time.deltaTime * 0.5f);
        }
        
    }

    void PositionCloudSpawnPoint()
    {
        // Calculate forward direction of the spawn point
        // Should be perpendicular to the vector from the left hand to the right hand and facing away from the player
        Vector3 handleDirection = (rightController.transform.position - leftController.transform.position).normalized;
        if (handleDirection != Vector3.zero)
        {
            cloudSpawnPoint.rotation = Quaternion.LookRotation(handleDirection, transform.up) * Quaternion.Euler(0, -90, 0);      
        }

        // Scale the spawn point so that the spawned cloud's size is proportional to the distance between the player's hands
        cloudSpawnPoint.localScale = Vector3.one * ((distanceBetweenHands * 0.5f) + minCloudScale);

        // Calculate the position of the spawn point
        // This should be in the centre between the two handles and forwards some distance
        // so that the cloud doesn't spawn directly on top of the player
        Vector3 handleMidPoint = Vector3.Lerp(leftController.transform.position, rightController.transform.position, 0.5f);
        float spawnDist = MapValueToNewRange(0, maxRecordedHandleDist, spawnMaxDist, spawnMinDist, distanceBetweenHands);
        cloudSpawnPoint.position = handleMidPoint + (cloudSpawnPoint.forward * spawnDist);


        // Keep the spawn point "upright" relative to the world
        cloudSpawnPoint.rotation = Quaternion.FromToRotation(cloudSpawnPoint.up, Vector3.up) * cloudSpawnPoint.rotation;
        
        
        // TODO: Edge cases to consider
        // What if left and right handles switched? Currently clouds shoot backwards :) Disable cloud generation maybe?
    }

    private void RecordPlayerHandleDists()
    {
        
        prevDistanceBetweenHands = distanceBetweenHands;
        distanceBetweenHands = Vector3.Distance(leftController.transform.position, rightController.transform.position);


        float maxPossible = 2f; // Some arbitrary limit on human reach?
        if ((distanceBetweenHands > maxRecordedHandleDist) && (distanceBetweenHands <= maxPossible))
        {
            maxRecordedHandleDist = distanceBetweenHands;
        }
        else if (enableFlexibleSwoosh && (distanceBetweenHands > prevDistanceBetweenHands * (1 + minimumChangeInGrowthPercentage)))
        {
            maxRecordedHandleDist = distanceBetweenHands;
        }

        // TODO: What if we get a weird reading?
        // TODO: Should max dist be something set by the user? Like the beach scene?

    }

    private float MapValueToNewRange(float oldMin, float oldMax, float newMin, float newMax, float value)
    {
        return Mathf.Lerp(newMin, newMax, Mathf.InverseLerp(oldMin, oldMax, value));
    }

    private bool SameSign(float a, float b)
    {
        // https://stackoverflow.com/questions/2922619/how-to-efficiently-compare-the-sign-of-two-floating-point-values-while-handling
        return a * b >= 0.0f;
    }

    private void CheckIfSwooshing()
    {
        prevDelta = currDelta;
        currDelta = prevDistanceBetweenHands - distanceBetweenHands;

        bool handsMovingTogether = currDelta > 0;
        bool handsChangedDirection = !SameSign(currDelta, prevDelta);
        bool enoughDistToInitiateSwoosh = (distanceBetweenHands >= (maxRecordedHandleDist * percentDistToSwoosh));

        if (handsMovingTogether)
        {
            if (handsChangedDirection)
            {
                if (enoughDistToInitiateSwoosh)
                {
                    swooshStartDist = distanceBetweenHands;
                    isSwooshing = true;
                    FindObjectOfType<AudioManager>().Play("Swoosh");
                }
                else
                {
                    isSwooshing = false;
                }
            }
        }
        else
        {
            isSwooshing = false;
        }

        if (isSwooshing)
        {
            float amp = Mathf.Lerp(0, cloudSwooshMaxAmp, Mathf.InverseLerp(swooshStartDist, 0, distanceBetweenHands));
            leftController.SendHapticImpulse(amp, Time.deltaTime * 0.5f);
            rightController.SendHapticImpulse(amp, Time.deltaTime * 0.5f);
        }

    }

    private void ApplySwooshForceToCloudsInRange(float speed)
    {

        Collider[] hitColliders = Physics.OverlapSphere(cloudSpawnPoint.position, swooshDistThreshold, cloudLayer);
        foreach (Collider hitCollider in hitColliders)
        {
            Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
            if (rb != null)
            {

                Vector3 maxForce = (Vector3.Lerp(rb.velocity.normalized, cloudSpawnPoint.forward, swooshDirectionBalance)
                                    + new Vector3(0, cloudLift, 0))
                                    * speed * cloudForce;

                float distance = Vector3.Distance(cloudSpawnPoint.position, hitCollider.transform.position);
                Vector3 force = Vector3.Lerp(maxForce, Vector3.zero, Mathf.InverseLerp(0, swooshDistThreshold, distance));

                rb.AddForce(force);
            }
        }
    }


    public void ProduceCloud()
    {
        GameObject bebeCloud = Instantiate(cloudPrefab[UnityEngine.Random.Range(0, cloudPrefab.Length)]);
        bebeCloud.transform.position = cloudSpawnPoint.position;
        bebeCloud.transform.localScale = cloudSpawnPoint.localScale;
        bebeCloud.transform.rotation = cloudSpawnPoint.rotation * Quaternion.Euler(0, -90, 0);
        bebeCloud.GetComponent<Rigidbody>().AddRelativeForce(cloudSpawnPoint.forward * initialCloudForce);
        cloudsSpawned++;
    }

    public void CheckEndExercise()
    {
        if ((cloudsSpawned >= cloudsRequired) && !gameEnded)
        {
            gameEnded = true;
            Debug.Log("End Game!");
            StartCoroutine(TriggerEndGame());
            // sceneChange.LoadMainMenuScene();
        }
    }
    
    private IEnumerator TriggerEndGame()
    {
        yield return new WaitForSeconds(1);
        GameEvents.current.ExerciseEnd();
    }
}
