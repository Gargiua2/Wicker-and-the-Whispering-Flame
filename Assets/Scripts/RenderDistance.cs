using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderDistance : MonoBehaviour
{
    public float drawDist = 20;
    List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    void Start()
    {
        SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();
        if (sr != null) 
        {
            renderers.Add(sr);
        }

        foreach (SpriteRenderer r in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            renderers.Add(r);
        }
    }

    void Update()
    {
        float d = Vector3.Distance(Camera.main.gameObject.transform.position, transform.position);
        
        if(d > drawDist) 
        {
            foreach (SpriteRenderer r in renderers)
            {
                if (r != null)
                    r.enabled = false;
            }
        } else 
        {
            foreach (SpriteRenderer r in renderers)
            {
                if(r!=null)
                    r.enabled = true;
            }
        }

               
    }
}
