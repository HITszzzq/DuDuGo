using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform playerTransform;
    public float followSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if(playerTransform != null)
        {
            transform.position = Vector2.Lerp(transform.position, playerTransform.position, followSpeed * Time.deltaTime);
        }
    }
}
