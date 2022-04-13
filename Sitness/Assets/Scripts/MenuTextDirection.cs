using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTextDirection : MonoBehaviour
{
    public Transform text;

    // Start is called before the first frame update
    void Start()
    {
        LookAtCamera();
    }

    // Update is called once per frame
    void Update()
    {
        LookAtCamera();
    }

    private void LookAtCamera()
    {
        text.LookAt(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -Camera.main.transform.position.z), Vector3.up);
        //text.localRotation = Quaternion.Euler(text.localRotation.eulerAngles.x, text.localRotation.eulerAngles.y, 0);
    }
}
