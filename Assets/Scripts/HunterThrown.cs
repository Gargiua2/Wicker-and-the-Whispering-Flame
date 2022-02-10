using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Thrown Weapon", menuName = "Scriptables/Items/Weapons/Thrown Weapon")]
public class HunterThrown : Hunter
{
    public const int MAX_NUMBER_ACTIVE_PROJECTILES = 7;

    [Space, Header("Thrown Weapon Settings")]
    public GameObject projectile;
    public float ThrowDamage = 5;
    public float throwForce;
    public float throwGravity;
    public float throwCooldown;
    public override void PrimaryAttack(CombatController cc)
    {
        base.PrimaryAttack(cc);
    }

    public override void SecondaryAttack(CombatController cc)
    {
        GameObject proj = Instantiate(projectile, Camera.main.transform);
        proj.transform.localPosition = offsetFromCamera;
        projectile.transform.up = Camera.main.transform.forward;
        
        cc.weaponAnimator.GetComponent<SpriteRenderer>().enabled = false;
        cc.StartCoroutine(cc.GenericTimer(throwCooldown, ReenableWeapon));
        cc.StartCoroutine(cc.HunterAttackCooldown(throwCooldown));

        proj.GetComponent<ProjectileWeapon>().Throw(Camera.main.transform.forward, throwForce, throwGravity, this);
        
        EventSystem.OnUseMeleeEvent(this, 1);
    }

    public void ReenableWeapon()
    {
        CombatController.instance.weaponAnimator.GetComponent<SpriteRenderer>().enabled = true;
    }

    public override void OnAttackPerformed(CombatController cc, int atk)
    {
        if(atk == 0) 
        {
            base.OnAttackPerformed(cc, atk);
            return;
        }


    }
}
