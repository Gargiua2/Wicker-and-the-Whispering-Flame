using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToFloor : MonoBehaviour
{

    public LayerMask treeMask;
    // Start is called before the first frame update
    void Start()
    {
        Ray r = new Ray(transform.position, Vector3.down);
        RaycastHit h;

        if(Physics.Raycast(r, out h, treeMask)) 
        {
            transform.position = new Vector3(transform.position.x, h.point.y + .015f, h.point.z);
            transform.forward = h.normal;
        }
    }

}
