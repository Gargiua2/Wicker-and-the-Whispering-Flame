using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    public LootTable table;
    public OverworldCollectable collectable;

    void Start()
    {

        collectable.collectableItem = ProceduralManager.instance.baseItemPool.DrawLoot();

        collectable.LoadLoot();
    }

}


[System.Serializable]
public struct Loot 
{
    public ScriptableObject loot;
    public int rarity;
}