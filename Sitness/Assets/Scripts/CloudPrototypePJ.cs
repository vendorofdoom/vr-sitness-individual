using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPrototypePJ : MonoBehaviour
{
    // Start is called before the first frame update

    public LineRenderer lineRenderer;
    public Transform leftHandle;
    public Transform rightHandle;

    public Transform cube;
    public Transform cubeSpawnPoint;

    public GameObject cloudPrefab;

    public float distanceBetweenHands;

    void Start()
    {
        lineRenderer.SetPosition(0, leftHandle.position);
        lineRenderer.SetPosition(1, rightHandle.position);
    }


    void Update()
    {
        lineRenderer.SetPosition(0, leftHandle.position);
        lineRenderer.SetPosition(1, rightHandle.position);
        cube.localScale = Vector3.one * (Vector3.Distance(leftHandle.position, rightHandle.position) / 3);
        cubeSpawnPoint.position = Vector3.Lerp(leftHandle.position, rightHandle.position, 0.5f);

        // PJ's arm stretch maxes at around 1.655 and hands together is 0.109  
        distanceBetweenHands = Vector3.Distance(leftHandle.position, rightHandle.position);

    }

    public void ProduceCloud()
    {
            GameObject bebeCloud = Instantiate(cloudPrefab);
            bebeCloud.transform.position = cube.position;
            bebeCloud.transform.localScale = cube.localScale;
            bebeCloud.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 5);
    }
}