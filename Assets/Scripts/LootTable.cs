using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Loot Table", menuName = "Loot Table")]
public class LootTable : ScriptableObject
{
    public List<Loot> loot;

    public ScriptableObject DrawLoot()
    {
        int totalRarity = 0;

        foreach (Loot a in loot)
        {
            totalRarity += a.rarity;
        }

        int r = Random.Range(0, totalRarity);

        int runningRarity = 0;

        foreach (Loot a in loot)
        {
            runningRarity += a.rarity;

            if (r < runningRarity)
            {
                return a.loot;
            }
        }

        return (null);
    }
}
