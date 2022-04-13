using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCreationEvents : MonoBehaviour
{
    Timer timer;
    CloudPrototype cloudPrototype;
    public GameObject cloudBubble;
    private bool begun;
    
    // Start is called before the first frame update
    void Start()
    {
        begun = false;
        cloudPrototype = FindObjectOfType<CloudPrototype>(includeInactive: true);
        timer = FindObjectOfType<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!begun && timer.exerciseBegun && !timer.exerciseComplete)
        {
            cloudPrototype.gameObject.SetActive(true);
            begun = true;
        }
        
        if (timer.exerciseComplete)
        {
            // StartCoroutine(DisableClouds());
            cloudPrototype.gameObject.SetActive(false);
            begun = false;
        }
    }

    IEnumerator DisableClouds()
    {
        yield return new WaitForSeconds(3);
        cloudPrototype.gameObject.SetActive(false);
        begun = false;
    }
}
