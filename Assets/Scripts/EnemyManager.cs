using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    List<Entity> enemies = new List<Entity>();
    int idCount = 0;

    #region Singleton
    public static EnemyManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    public int RegisterEnemy(Entity e) 
    {
        enemies.Add(e);
        idCount ++;
        return idCount;
    }

    public void DeregisterEnemy(Entity e) 
    {
        enemies.Remove(e);
    }

    public float GetDistanceToNearestEnemyInViewCone() 
    {
        if (enemies.Count == 0)
            return 10;

        float nearestDistance = Mathf.Infinity;
        
        foreach(Entity e in enemies) 
        {
            float d = Vector3.Distance(e.transform.position, Camera.main.transform.position);

            if (d < nearestDistance && Vector3.Dot(Player.instance.transform.forward, e.transform.position) > 0)
                nearestDistance = d;
        }

        if(nearestDistance > 10000000) 
        {
            return 25;
        }

        return nearestDistance;
    }

    public List<Entity> GetNearestEnemies(float r) 
    {
        List<Entity> near = new List<Entity>();

        foreach (Entity e in enemies)
        {
            float d = Vector3.Distance(e.transform.position, Camera.main.transform.position);
            
            if(d < r) 
            {
                near.Add(e);
            }
        }

        return near;
    }
}
