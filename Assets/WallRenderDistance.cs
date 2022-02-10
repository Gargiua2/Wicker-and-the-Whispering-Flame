using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRenderDistance : MonoBehaviour
{
    public bool vertical = false;
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

        float d;
        if (!vertical) 
        {
            d = (new Vector3(transform.position.x, 0, 0) - new Vector3(Player.instance.transform.position.x, 0, 0)).magnitude;
        } else 
        {
            d = (new Vector3(0, 0, transform.position.z) - new Vector3(0, 0, Player.instance.transform.position.z)).magnitude;
        }
        

        if (d > drawDist)
        {
            foreach (SpriteRenderer r in renderers)
            {
                if (r != null)
                    r.enabled = false;
            }
        }
        else
        {
            foreach (SpriteRenderer r in renderers)
            {
                if (r != null) 
                {
                    r.enabled = true;
                    r.color = Color.Lerp(Color.white, new Color(0, 0, 0, 0), d / drawDist);
                }
                    

                
            }
        }


    }
}
