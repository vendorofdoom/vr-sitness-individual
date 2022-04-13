using System.Collections;
using UnityEngine;

public class RainbowCollision : MonoBehaviour
{
    private void SendUpdate(GameObject receiver)
    {
        receiver.SendMessage("UpdateTarget");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SendUpdate(other.gameObject);
        }
    }
}
