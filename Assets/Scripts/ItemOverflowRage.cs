using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Overflow Rage Relic", menuName = "Scriptables/Items/Relics/Overflow Rage Relic")]
public class ItemOverflowRage : BaseItem
{

    public float damageMultiplier;

    public override float GetDamageMultiplier(IDamageable target, Weapon attackingWeapon, Spell activeSpell = null)
    {

        if(Player.instance.GetOverflowState() && attackingWeapon is Hunter) 
        {

            return damageMultiplier;
        }

        return 0;
    }
}
