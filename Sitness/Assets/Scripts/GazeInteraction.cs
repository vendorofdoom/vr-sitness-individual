using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// From Nima's tutorial :) https://youtu.be/mv7Mn2zukEs

public class GazeInteraction : MonoBehaviour
{

    public LayerMask gazeInteractableLayer;
    public ParticleSystem ps;
    private ParticleSystem.EmissionModule em;

    private void Start()
    {
        em = ps.emission;
        em.enabled = false;
    }

    private void FixedUpdate()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, Mathf.Infinity, gazeInteractableLayer))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * raycastHit.distance, Color.red);
            GazeInteractable gi = raycastHit.transform.GetComponent<GazeInteractable>();
            if (gi != null)
            {
                gi.inFocus = true;
                ps.transform.position = raycastHit.point;
                em.enabled = true;
            }
        }
        else
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100f, Color.white);
            em.enabled = false;
        }
    }
}
