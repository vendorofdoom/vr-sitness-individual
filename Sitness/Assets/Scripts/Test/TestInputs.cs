using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputs : MonoBehaviour
{
    PlanetWalkerPrototype planetWalker;

    // Start is called before the first frame update
    void Start()
    {
        planetWalker = FindObjectOfType<PlanetWalkerPrototype>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            planetWalker.rainbowCollected = true;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            planetWalker.rainbowCollected = false;
        }

    }
}
