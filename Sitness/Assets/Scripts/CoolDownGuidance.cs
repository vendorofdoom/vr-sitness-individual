using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownGuidance : MonoBehaviour
{

    public Transform GuidesParent;
    public Transform High;
    public Transform Low;

    private float TimeInMovement = 0f;
    private bool MovingUp = true;
    public float MovingTime;

    private void Update()
    {
        if (MovingUp)
        {
            GuidesParent.rotation = Quaternion.Lerp(Low.rotation, High.rotation, TimeInMovement / MovingTime);
        }
        else
        {
            GuidesParent.rotation = Quaternion.Lerp(High.rotation, Low.rotation, TimeInMovement / MovingTime);
        }

        TimeInMovement += Time.deltaTime;
    }

    public void LowHands()
    {
        Debug.Log("Hands down low!");
        TimeInMovement = 0f;
        MovingUp = true;
    }

    public void HighHands()
    {
        Debug.Log("Hands up high!");
        TimeInMovement = 0f;
        MovingUp = false;
    }
}
