using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Magic Melee Relic", menuName = "Scriptables/Items/Relics/Magic Melee Relic")]
public class ItemMagicMelee : BaseItem
{
    [Range(0, 1)] public float percentMagicTranser; 

    public override float GetDamageMultiplier(IDamageable target, Weapon attackingWeapon, Spell activeSpell = null)
    {
        if(attackingWeapon is Hunter) 
        {
            return Magic.GetFavorDamageMultiplier(Player.instance.stats.favor) * percentMagicTranser;
        }

        return 0;      
    }

}
