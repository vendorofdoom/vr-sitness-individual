using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CoolDownPrototype : MonoBehaviour
{

    // SHADER TUTORIAL: https://youtu.be/gRq-IdShxpU (I only got to 3m23s before deciding it's bedtime so I'm sure lots more to be learned!)

    public Transform water;

    public float minWaterLevel;
    public float maxWaterLevel;

    public Transform leftHand;
    public Transform rightHand;

    public XRControllerWithRumble leftRumble;
    public XRControllerWithRumble rightRumble;


    public float prevY;

    // Start is called before the first frame update
    void Start()
    {
        prevY = (leftHand.position.y + rightHand.position.y) / 2;
    }

    // Update is called once per frame
    void Update()
    {

        float currY = (leftHand.position.y + rightHand.position.y) / 2;
        float deltaY = (currY - prevY) * 0.5f;

        float waterY = Mathf.Clamp(water.position.y + deltaY, minWaterLevel, maxWaterLevel);

        water.position = new Vector3(water.position.x, waterY, water.position.z);

        prevY = currY;
    }
}
