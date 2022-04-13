using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeInteractable : MonoBehaviour
{
    public float lifetime = 4f;
    public bool inFocus = false;
    public BeachObjectSpawner creator;

    private void Update()
    {
        if (inFocus)
        {
            lifetime -= Time.deltaTime;
            inFocus = false;
        }

        if (lifetime <= 0f)
        {
            Debug.Log("Goodbye cruel world!");
            creator.SpawnRandObject();
            Destroy(gameObject);
        }
    }

}
