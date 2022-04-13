using System;
using UnityEngine;
using System.Collections;
using Unity.Collections;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class PlanetWalkerPrototype : MonoBehaviour
{
    [SerializeField] private Transform
        XRHead,
        XRBody,
        planet,
        leftHand,
        rightHand,
        placeholder,
        planetCenter;
    
    public SceneChange sceneManager;
    public AudioManager audioManager;
    public bool planetWalkerEnabled = true;
    
    [Header("Walking Related")]
    [Tooltip("Base Walk Speed")]
    public float speedBase = 100;
    
    [Tooltip("Walk Speed Multiplier")]
    public float speedOffset = 8;
    
    [Tooltip("How fast the hands must move to be considered walking.")]
    public float handSpeedThreshold = 0.05f;
   
    [Tooltip("Maximum distance one can travel from swinging arms at once.")]
    public float maxArmSwingDistanceAngles = 3f;
    
    [Tooltip("The slerp ratio between the previous and next rotation of the planet.")]
    [Range(0f, 1f)]
    public float planetRotationSlerp = 1f;
    
    [Header("Moon Related")]
    [Tooltip("The range of how close one is to the target, used for detecting when the target is on the opposite side of the sphere.")]
    [Range(0f, 1f)]
    public float polarRange = 0.3f;
    
    [Tooltip("The dot product viewing angle of the player, used for clamping the moon guide to a viewable part of the screen.")]
    [Range(0f, 1f)]
    public float viewingAngle = 0.7f;
   
    [Tooltip("The smooth time used for the moon guide 'smooth camera damp'")]
    public float smoothTime = 0.3f;
    
    [Tooltip("The placeholder distance")]
    public float placeholderMinDistance = 45f;
    
    public GameObject moonGuide;
    
    [Header("Rainbow Related")]
    public float maxRainbowCount = 10;
    public bool bonusRainbowMode = false;
    private bool moonGuidePinned = false;
    public bool bonusStarShower = false;
    
    [Tooltip("The size of the initial collider around the rainbow (in local units / smaller units)")]
    public float rainbowSphereColliderSize = 1.5f;
    public GameObject rainbowPrefab;
    
    private Transform targetObj;

    private bool isNearTarget = false;
    public bool rainbowCollected = false;
    private bool reachedGoalRainbowCount = false;

    private float
        planetRadius,
        forwardAngle = 120;
    
    public float rainbowCounter = 0;

    private Vector3
        targetPos,
        planetPolePos,
        previousPosLeft,
        previousPosRight,
        placeholderPos,
        placeholderVelocity = Vector3.zero;

    private Quaternion
        targetRot,
        originPlanetRotation,
        targetPlanetRotation;

    void Start()
    {
        SetPlanetRadius();
        SetPreviousHandPos();
        SetPlanetPoles();
        NewTarget();
        SetPlaceholder();
    }

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {

        if (rainbowCounter >= maxRainbowCount && !reachedGoalRainbowCount)
        {
            // Handle reaching the max count of rainbows and displaying the mini-game bonus
            
            TriggerMiniGameBonus();
        }

        if (planetWalkerEnabled)
        {
            // Calculate hand movement and planet rotations

            SetPlanetRotations();
            SetPreviousHandPos();


            // Calculate planet pole, target, and placeholder positions

            SetPlanetPoles();
            SetTarget();
            SetPlaceholder();
        }
    }
    
    private void FixedUpdate()
    {
        RotatePlanet();
        MoveTarget();
        MovePlaceholder();
    }
    
    private void NewTarget()
    {
        SetTarget();
        SpawnTarget();
    }
    
    void SpawnTarget()
    {
        var gameObj = Instantiate(rainbowPrefab, targetPos, targetRot);
        var collider = gameObj.GetComponentInChildren<SphereCollider>();
        collider.radius = rainbowSphereColliderSize;
        targetObj = gameObj.transform;
    }

    void RotatePlanet()
    {
        planet.rotation = Quaternion.Slerp(originPlanetRotation, targetPlanetRotation, planetRotationSlerp);
    }

    void MovePlaceholder()
    {
        placeholder.position = Vector3.SmoothDamp(
            placeholder.position, 
            placeholderPos,
            ref placeholderVelocity,
            smoothTime);
    }

    void MoveTarget()
    {
        targetObj.position = targetPos;
        targetObj.rotation = targetRot;
    }
    
    void SetPreviousHandPos()
    {
        previousPosLeft = leftHand.transform.position;
        previousPosRight = rightHand.transform.position;
    }
    
    void SetPlanetRadius()
    {
        var player = XRBody;
        planetRadius = Vector3.Distance(planetCenter.position, player.position);
    }
    
    void SetPlanetPoles()
    {
        var planetUpAxis = planet.TransformDirection(Vector3.up);
        planetPolePos = planetCenter.position + planetUpAxis * planetRadius;
    }

    void SetPlanetRotations()
    {
        var distance =
            (Vector3.Distance(previousPosLeft, leftHand.position)
            + Vector3.Distance(previousPosRight, rightHand.position)) / 2;

        var speed = distance / Time.deltaTime;

        // If true Player has swung their hand
        if (speed >= handSpeedThreshold)
        {
            var desiredAngles = distance * speed * Time.deltaTime * speedOffset * speedBase;
            var clampedAngles = Mathf.Clamp(desiredAngles, 0, maxArmSwingDistanceAngles) * -1;
            
            var crossAxis = XRHead.TransformDirection(Vector3.right);
            var newPlanetAxis = planet.transform.InverseTransformDirection(crossAxis);
            var rotationAxis = newPlanetAxis.normalized;
            
            originPlanetRotation = planet.rotation;
            targetPlanetRotation = planet.rotation * Quaternion.AngleAxis(clampedAngles, rotationAxis);
        }
    }
    
    void SetTarget()
    {
        var player = XRBody;
        var polePos = planetPolePos;
        var nextPos = Quaternion.AngleAxis(forwardAngle, Vector3.Cross(planet.up, planet.forward)) * polePos;
        
        targetPos = nextPos;
        
        if (!isNearTarget)
        {
            targetRot = Quaternion.LookRotation(
                (player.position - targetPos).normalized, 
                (targetPos - planetCenter.position).normalized);
        }
    }

    void PinPlaceholderToPlayer()
    {
        var playerDir = (XRBody.position - planetCenter.position).normalized;
        var poleDir = (planetPolePos - planetCenter.position).normalized;
        placeholderPos = Quaternion.FromToRotation(poleDir, playerDir) * planetPolePos;
    }

    void StartStarShower()
    {
        var particleSystems = moonGuide.GetComponents<ParticleSystem>();
        foreach (var particleSystem in particleSystems)
        {
            particleSystem.Play();
        }
    }

    void SetPlaceholder()
    {
        if (moonGuidePinned)
        {
            PinPlaceholderToPlayer();
            
            if (bonusStarShower)
            {
               StartStarShower(); 
            }
            
            return;
        }
        
        // Calculate target direction and position

        var player = XRBody;
        var originPos = player.position;
        var distanceDegrees = placeholderMinDistance;
        var playerUpAxis = (player.position - planetCenter.position).normalized;
        
        var targetDirection = (targetPos - originPos).normalized;
        var targetCrossAxis = Vector3.Cross(playerUpAxis, targetDirection);

        var fromDirection = (originPos - planetCenter.position).normalized;
        var toDirection = (targetPos - planetCenter.position).normalized;
        var toTargetRot = Quaternion.FromToRotation(fromDirection, toDirection);
        
        var minTargetPos = Quaternion.AngleAxis(distanceDegrees, targetCrossAxis) * originPos;
        var minTargetDirection = (minTargetPos - originPos).normalized;

        
        // Calculate distance between origin and target
        
        var distanceRadians = Mathf.Atan2(
            Vector3.Cross(fromDirection, toDirection).magnitude, 
            Vector3.Dot(fromDirection, toDirection));
        
        var computedDistanceDegrees = distanceRadians * Mathf.Rad2Deg;
        
        
        // Calculate placeholder position

        var targetDot = Vector3.Dot(targetDirection, player.forward);
        var guideDot = Vector3.Dot(minTargetDirection, player.forward);
        var rightDot = Vector3.Dot(minTargetDirection, player.right);
        
        if (guideDot < viewingAngle && guideDot > -viewingAngle)
        {
            var angles = Mathf.Lerp(0f, 90f, 1 - viewingAngle);
            var leftOrRightDegrees = rightDot < 0 ? angles : -angles;
            var toDirRot = Quaternion.AngleAxis(leftOrRightDegrees, playerUpAxis);
            var toDir = toDirRot * player.forward;
            var forwardCrossAxis = Vector3.Cross(playerUpAxis, toDir);
            var forwardMinTargetPos = Quaternion.AngleAxis(distanceDegrees, forwardCrossAxis) * originPos;
            placeholderPos = forwardMinTargetPos;
        }
        else if (guideDot >= viewingAngle || guideDot <= -viewingAngle)
        {
            if (guideDot < 0)
            {
                var oppositeCrossAxis = Vector3.Cross(playerUpAxis, -minTargetDirection);
                var oppositeMinTargetPos = Quaternion.AngleAxis(distanceDegrees, oppositeCrossAxis) * originPos;
                placeholderPos = oppositeMinTargetPos;
            }
            else if (computedDistanceDegrees <= 90f)
            {
                var ratio = Mathf.Min(distanceDegrees / computedDistanceDegrees, 1);
                placeholderPos = Vector3.Slerp(
                    originPos, 
                    (toTargetRot * originPos), ratio);
            }
            else if (computedDistanceDegrees >= 120f && targetDot > -polarRange && targetDot < polarRange)
            {
                Debug.Log("In the Polar Range!");
                var forwardCrossAxis = Vector3.Cross(playerUpAxis, (placeholderPos - player.position).normalized);
                // var forwardCrossAxis = Vector3.Cross(playerUpAxis, player.forward);
                var forwardMinTargetPos = Quaternion.AngleAxis(distanceDegrees, forwardCrossAxis) * originPos;
                placeholderPos = forwardMinTargetPos;
            }
            else
            {
                placeholderPos = minTargetPos;
            }
        }
    }
    
    public void UpdateTarget()
    {
        var forward = forwardAngle;
        forwardAngle = NormalizeDegrees(forward * 2);
        rainbowCounter += 1;
        audioManager.Play("Rainbow");
        rainbowCollected = true;

        if (rainbowCounter < maxRainbowCount || bonusRainbowMode)
        {
            isNearTarget = false;
            Destroy(targetObj.gameObject);
            NewTarget();
        }
        else
        {
            targetObj.gameObject.SetActive(false);
        }
    }

    public void EnterNearTarget()
    {
        isNearTarget = true;
    }

    public void ExitNearTarget()
    {
        isNearTarget = false;
    }

    public void DisableWalker()
    {
        planetWalkerEnabled = false;
    }

    public void EnableWalker()
    {
        planetWalkerEnabled = true;
    }

    public void ToggleWalker()
    {
        if (planetWalkerEnabled)
        {
            DisableWalker();
        }
        else
        {
            EnableWalker();
        }
    }

    private IEnumerator TriggerEndGame()
    {
        yield return new WaitForSeconds(1);
        GameEvents.current.ExerciseEnd();
    }
    
    private void TriggerMiniGameBonus()
    {
        reachedGoalRainbowCount = true;
        if (bonusStarShower)
        {
            moonGuidePinned = true;
        }

        StartCoroutine(TriggerEndGame());
    }
    
    private float NormalizeDegrees(float degrees)
    {
        if (degrees > 360) return degrees - 360;
        if (degrees < -360) return degrees + 360;
        if (Mathf.Abs(degrees) == 360) return 0;
        return degrees;
    }
}