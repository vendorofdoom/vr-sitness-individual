using UnityEngine;

public class RainbowParticles : MonoBehaviour
{
    public GameObject rainbow;
    private ParticleSystem particles;
    
    void Start()
    {
        particles = rainbow.GetComponent<ParticleSystem>();
        particles.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("EnterNearTarget");
            // particles.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("ExitNearTarget");
        }
    }
}
