using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Overflow AOE Relic", menuName = "Scriptables/Items/Relics/Overflow AOE Relic")]
public class ItemOverflowAOE : BaseItem
{
    public GameObject particleEffect;
    public float range = 10;
    public float damage = 5;
    public override void OnEquip()
    {
        base.OnEquip();

        EventSystem.OnPlayerOverflow += DoAOEAttacK;
    }

    private void OnDisable()
    {
        EventSystem.OnPlayerOverflow -= DoAOEAttacK;
    }

    private void OnDestroy()
    {
        EventSystem.OnPlayerOverflow -= DoAOEAttacK;
    }

    public void DoAOEAttacK() 
    {
        GameObject p =Instantiate(particleEffect, Player.instance.transform.position - Vector3.down * .75f, Quaternion.identity);
        p.transform.forward = Vector3.up;

        List<Entity> nearby = EnemyManager.instance.GetNearestEnemies(range);

        foreach(Entity e in nearby) 
        {
            e.RecieveDamage(damage, null);
        }
    }

}
