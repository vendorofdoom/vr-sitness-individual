using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabRotate : MonoBehaviour
{
    private bool doRotate = false;
    private Transform interactor;
    private Vector3 prevPos;

    public void EnterGrab(SelectEnterEventArgs selectEnterEventArgs)
    {
        Debug.Log("Enter grab");
        doRotate = true;
        interactor = selectEnterEventArgs.interactorObject.transform;
        prevPos = interactor.position;

    }

    public void ExitGrab(SelectExitEventArgs selectEnterEventArgs)
    {
        Debug.Log("Exit grab:");
        doRotate = false;
    }

    private void Update()
    {
        if (doRotate)
        {
            transform.rotation = Quaternion.FromToRotation((prevPos - transform.position), (interactor.position - transform.position)) * transform.rotation;
            prevPos = interactor.position;
        }
    }
}