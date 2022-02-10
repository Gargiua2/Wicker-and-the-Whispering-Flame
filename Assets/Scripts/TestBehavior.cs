using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehavior : MonoBehaviour
{
    public float flapSpeed = 2.5f;
    public float flapHeight = .2f;

    float counter = 0;
    
    void Update()
    {

        if(Time.timeScale != 0) 
        {
            Vector3 yOffset = new Vector3(0, Mathf.Sin(counter) * flapHeight * Time.deltaTime, 0);
            transform.position += yOffset;
            counter += flapSpeed * Time.deltaTime;
        }
    }
}
