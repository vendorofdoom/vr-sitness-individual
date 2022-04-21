using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenGuidance : MonoBehaviour
{
    [SerializeField]
    private MovementState movementState;

    public float PullApartTime;
    public float SwooshTime;

    public Transform GuideParentLeft;
    public Transform GuideParentRight;
    public Transform MinRotation;
    public Transform MaxRotationLeft;
    public Transform MaxRotationRight;

    private float TimeInMovement = 0f;

    private enum MovementState
    {
        Pause,
        PullApart,
        Release,
        Swoosh
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (movementState)
        {
            case MovementState.Pause:
                break;
            case MovementState.PullApart:
                GuideParentLeft.rotation = Quaternion.Lerp(MinRotation.rotation, MaxRotationLeft.rotation, TimeInMovement / PullApartTime);
                GuideParentRight.rotation = Quaternion.Lerp(MinRotation.rotation, MaxRotationRight.rotation, TimeInMovement / PullApartTime);
                break;
            case MovementState.Release:
                break;
            case MovementState.Swoosh:
                GuideParentLeft.rotation = Quaternion.Lerp(MaxRotationLeft.rotation, MinRotation.rotation, TimeInMovement / SwooshTime);
                GuideParentRight.rotation = Quaternion.Lerp(MaxRotationRight.rotation, MinRotation.rotation, TimeInMovement / SwooshTime);
                break;
        }

        TimeInMovement += Time.deltaTime;
    }

    public void Pause()
    {
        Debug.Log("Pause");
        movementState = MovementState.Pause;
        TimeInMovement = 0f;
    }

    public void PullApart()
    {
        Debug.Log("Pull apart");
        movementState = MovementState.PullApart;
        TimeInMovement = 0f;
    }

    public void Release()
    {
        Debug.Log("Release!");
        movementState = MovementState.Release;
        TimeInMovement = 0f;
    }

    public void Swoosh()
    {
        Debug.Log("Swoosh!");
        movementState = MovementState.Swoosh;
        TimeInMovement = 0f;
    }
    
}
