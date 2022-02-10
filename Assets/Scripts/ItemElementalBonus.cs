using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Elemental Bonus Relic", menuName = "Scriptables/Items/Relics/Elemental Bonus Relic")]
public class ItemElementalBonus : BaseItem
{
    public MagicDamageType typeWithBonus;
    public float bonusAmount;

    public override float GetDamageBonus(IDamageable target, Weapon attackingWeapon, Spell activeSpell = null)
    {
        if(activeSpell != null) 
        {
            if(activeSpell.magicDamageType == typeWithBonus) 
            {
                Debug.Log("ATTACK IS POWERED UP BY ELEMENTAL BONUS");
                return bonusAmount;
            }
        }

        return 0;
    }

}
