using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachObjectSpawner : MonoBehaviour
{
    public GameObject[] itemsToPickFrom;
    public int spawnedObjectCounter;

    public float raycastDistance = 100f;
    public Vector3[] rayOrigins;

    public LayerMask layerMask;

    // TODO: have water vs. land locations?

    // Start is called before the first frame update
    void Start()
    {
        spawnedObjectCounter = 0;
        SpawnRandObject();
    }

    public void SpawnRandObject()
    {
        Vector3 rayOrigin = rayOrigins[Random.Range(0, rayOrigins.Length)];

        Debug.Log("spawn: " + rayOrigin.ToString());

        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, raycastDistance, layerMask))
        {
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            GameObject randObj = GameObject.Instantiate(itemsToPickFrom[Random.Range(0, itemsToPickFrom.Length)], transform);
            randObj.transform.position = transform.InverseTransformPoint(hit.point);
            randObj.transform.rotation = spawnRotation * randObj.transform.rotation;
            randObj.transform.localRotation *= Quaternion.Euler(0, Random.Range(0, 360), 0);

            GazeInteractable gazeableObj = randObj.GetComponent<GazeInteractable>();
            if (gazeableObj != null)
            {
                gazeableObj.creator = this;
            }
            
            spawnedObjectCounter++;
        }
        else
        {
            Debug.Log("Unable to spawn random beach object");
        }
    }

}
