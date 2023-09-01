using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float delayTime = 0.0f;
    public float repeatInterval = 1.0f;
    public int numToSpawn = 1;
    private int numCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObject", delayTime, repeatInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject SpawnObject()
    {
        if(prefabToSpawn)
        {
            GameObject prefabObject = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            numCount++;
            if(numCount >= numToSpawn)
            {
                CancelInvoke("SpawnObject");
            }
            return prefabObject;
        }
        return null;
    }
}
