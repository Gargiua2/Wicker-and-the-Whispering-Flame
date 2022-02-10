using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Focus", menuName = "Scriptables/Items/Weapons/Focus")]
public class Focus : Weapon
{
    [Space, Header("Focus Settings")]
    public float fireRateModifier = 1;
    public float damageModifier = 1;
    public float rangeModifier = 1;
    public float shotSpeedModifier = 1;
    public float accuracyModifier = 1;
    public List<DamageTypeModifier> damageTypeModifiers;
    public List<MagicDamageType> additionalDamageType;

    [Space, Header("Additional Animation & Visual Config")]
    public Vector3 projectileLaunchOffset;

    public override void OnEquip() 
    {
        base.OnEquip();
    }

    public override void OnUpdate() 
    {
        
    }

    public override void OnUnequip()
    {
    
    }

    public override Sprite GetDisplaySprite()
    {
        return sprite;
    }

    public override string GetName()
    {
        return weaponName;
    }
}
