using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAreaSpawner : MonoBehaviour
{
    public GameObject raycastAligner;
    public int numItemsToSpawn;
    public GameObject spawnerHolder;

    public float itemXSpread = 10;
    public float itemYSpread = 0;
    public float itemZSpread = 10;

    public CoolDownSound coolDownSound;

    void Start()
    {
        SpreadItem();
        Debug.Log("spread!");
    }
    
    public void SpreadItem()
    {
        for (int i = 0; i < numItemsToSpawn; i++)
        {
            Vector3 randPosition = new Vector3(Random.Range(-itemXSpread, itemXSpread), Random.Range(-itemYSpread, itemYSpread), Random.Range(-itemZSpread, itemZSpread)) + transform.position;
            GameObject clone = Instantiate(raycastAligner, randPosition, Quaternion.identity);
            clone.transform.SetParent(spawnerHolder.transform, false);
        }
    }
}
