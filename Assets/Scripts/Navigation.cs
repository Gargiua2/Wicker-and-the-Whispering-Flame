using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Navigation : MonoBehaviour
{
    public LayerMask LOSMask;

    #region Singleton
    public static Navigation instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    public Vector3 FindPointOnRadius(Vector3 targetPos, Vector3 from, float radius, bool ForceDistance = false) 
    {
        Vector3 flatPos = new Vector3(from.x, targetPos.y, from.z);

        if ((Vector3.Distance(flatPos, targetPos) > radius) || ForceDistance) 
        {
            Vector3 dirToTarget = (flatPos - targetPos).normalized;
            Vector3 point = (dirToTarget * radius) + targetPos;
            return point;
        }

        return flatPos;

    }

    public bool CheckLineOfSightAgainstPlayer(Vector3 from)
    {
        GameObject player = Player.instance.gameObject;
        
        Ray r = new Ray(from, DirectionBetweenPoints(from, player.transform.position));
        RaycastHit hit;
        
        if(Physics.Raycast(r, out hit, 1000, LOSMask)) 
        {
            if(hit.transform.gameObject.tag == "Player") 
            {
                return true;
            }
        }

        return false;
    }

    public Vector3 FindNearestPointOnRadiusWithPlayerLOS(float radius, Vector3 from, int numIncrements = 16) 
    {
        Vector3 playerPosition = Player.instance.gameObject.transform.position;
        List<Vector3> pointsWithLOS = new List<Vector3>();

        for(int i = 0; i < numIncrements; i++) 
        {
            float a = Mathf.Lerp(0f, 2f * Mathf.PI, i / (float)numIncrements);
            Vector3 check = playerPosition + (new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a)) * radius);

            if (Physics.OverlapSphere(check, .5f, LOSMask).Length <= 0) 
            {
                if (CheckLineOfSightAgainstPlayer(check))
                {
                    pointsWithLOS.Add(check);
                }
            }
            
        }

        if (pointsWithLOS.Count <= 0) 
        {
            return Vector3.zero;
        } 
        else 
        {
            float nearestDistance = Mathf.Infinity;
            Vector3 currentNearest = Vector3.zero;

            foreach (Vector3 p in pointsWithLOS) 
            {
                float d = Vector3.Distance(from, p);

                if(d < nearestDistance) 
                {
                    nearestDistance = d;
                    currentNearest = p;
                }
            }

            return currentNearest;
        }

        
    }

    public float GetDistanceToPlayer(Vector3 p) 
    {
        return (Vector3.Distance(p, Player.instance.transform.position));
    }

    public Vector3 wander(Transform agent, float radius, float forwardOffset, float wanderStrength, ref float angle) 
    {
        angle = angle + Random.Range(-wanderStrength, wanderStrength);
        Vector3 t = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius + (agent.forward * forwardOffset);
        return t;
    }

    public Vector3 DirectionBetweenPoints(Vector3 from, Vector3 to) 
    {
        return ((to - from).normalized);
    }

}
