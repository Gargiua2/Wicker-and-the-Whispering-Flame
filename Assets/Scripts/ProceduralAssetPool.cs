using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Procedural Pool", menuName = "Procedurals/Pool")]
public class ProceduralAssetPool : ScriptableObject
{
    public List<ProceduralPoolItem> pool = new List<ProceduralPoolItem>();

    public GameObject DrawAsset() 
    {
        int totalRarity = 0;

        foreach (ProceduralPoolItem a in pool) 
        {
            totalRarity += a.rarity;
        }

        int r = Random.Range(0, totalRarity);

        int runningRarity = 0;

        foreach (ProceduralPoolItem a in pool) 
        {
            runningRarity += a.rarity;

            if(r < runningRarity) 
            {
                return a.asset;
            }
        }

        return (null);
    }
}

[System.Serializable]
public struct ProceduralPoolItem 
{
    public GameObject asset;
    public int rarity;
}
