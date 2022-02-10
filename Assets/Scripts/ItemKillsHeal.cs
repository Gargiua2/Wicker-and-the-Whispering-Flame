using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Health Steal Relic", menuName = "Scriptables/Items/Relics/Health Steal Relic")]
public class ItemKillsHeal : BaseItem
{
    [Range(0,1)] public float percentChance;
    [Range(0, 1)] public float percentHeal;

    public override void OnEquip()
    {
        base.OnEquip();

        EventSystem.OnKillEnemy += HPSteal;

    }

    private void OnDestroy()
    {
        EventSystem.OnKillEnemy -= HPSteal;
    }

    private void OnDisable()
    {
        EventSystem.OnKillEnemy -= HPSteal;
    }

    public void HPSteal(Entity e) 
    {
        if(Random.value <= percentChance)
            Player.instance.Heal((int)(e.maxHP * percentHeal));
    }
}
