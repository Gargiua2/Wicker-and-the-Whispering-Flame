using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralSpawnMask : MonoBehaviour
{
    public List<SpawnMask> masks = new List<SpawnMask>();

    public bool pointFallsWithinMask(Vector3 point) 
    {
        Vector2 p = new Vector2(point.x, point.z);
        
        bool vsMask = false;

        foreach (SpawnMask sm in masks)
        {
            
            switch (sm.shape)
            {
                case MaskType.Square:
                    Debug.Log("CHECKING AGAINST SQUARE");
                    if ((p.x < sm.location.x + (sm.size.x / 2) && p.x > sm.location.x - (sm.size.x / 2)) && (p.y < sm.location.y + (sm.size.x / 2) && p.y > sm.location.y - (sm.size.x / 2)))
                        vsMask =  true;
                    break;
                case MaskType.Rectangle:
                    Debug.Log("CHECKING AGAINST RECT");
                    if ((p.x < sm.location.x + (sm.size.x/2) && p.x > sm.location.x - (sm.size.x / 2)) && (p.y < sm.location.y + (sm.size.y / 2) && p.y > sm.location.y - (sm.size.y / 2)))
                        vsMask =  true;
                    break;
                case MaskType.Circle:
                    Debug.Log("CHECKING AGAINST CIRCLE");
                    if (Vector2.Distance(p, sm.location) < sm.size.x)
                        vsMask = true;
                    break;
            }

        }

        return vsMask;
    }

    public void OnDrawGizmos()
    {
        foreach(SpawnMask sm in masks) 
        {
            switch (sm.shape)
            {
                case MaskType.Square:
                    Gizmos.DrawWireCube(new Vector3(sm.location.x, 0, sm.location.y), new Vector3(sm.size.x, 1, sm.size.x));
                    break;
                case MaskType.Rectangle:
                    Gizmos.DrawWireCube(new Vector3(sm.location.x, 0, sm.location.y), new Vector3(sm.size.x, 1, sm.size.y));
                    break;
                case MaskType.Circle:
                    Gizmos.DrawWireSphere(new Vector3(sm.location.x, 0, sm.location.y), sm.size.x);
                    break;
            }

        }
    }
}

[System.Serializable]
public struct SpawnMask 
{
    public Vector2 size;
    public Vector2 location;
    public MaskType shape;
}

public enum MaskType : int
{
    Square = 0,
    Rectangle = 1,
    Circle = 2
}