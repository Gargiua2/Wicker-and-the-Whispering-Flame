using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedIcon : MonoBehaviour
{
    public float rotSpeed;
    
    // Update is called once per frame
    void Update()
    {
        transform.RotateAroundLocal(Vector3.forward, rotSpeed);
    }
}
