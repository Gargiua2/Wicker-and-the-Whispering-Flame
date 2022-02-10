using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ProceduralManager : MonoBehaviour
{
    public List<GameObject> procedurals = new List<GameObject>();
    public LootTable baseItemPool;
    [HideInInspector]public ProceduralSpawnMask spawnMasks;


    public static ProceduralManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;

        spawnMasks = this.GetComponent<ProceduralSpawnMask>();
    }

    void Start()
    {
        foreach (GameObject pg in procedurals)
        {
            pg.GetComponent<IProcedural>().Generate();
        }
    }

    public LootTable GetItemPool() 
    {
        return baseItemPool;
    }

}
