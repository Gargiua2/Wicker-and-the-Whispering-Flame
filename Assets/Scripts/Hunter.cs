using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hunter Weapon", menuName = "Scriptables/Items/Weapons/Hunter Weapon")]
public class Hunter : Weapon
{

    [Space, Header("Attack Settings")]
    public HunterAttack[] attacks;
    public List<TweenSegment> movement;
    
    public virtual void PrimaryAttack(CombatController cc) 
    {
        cc.weaponAnimator.attackAnimationTweening(attacks[0].attackLength, false);

        cc.weaponAudio.PlayOneShot(attacks[0].attackAudio);

        GameObject FXObject = Instantiate(attacks[0].attackEffect);
        DestroyAfterPlay FX = FXObject.GetComponent<DestroyAfterPlay>();
        FX.Run(attacks[0].attackLength + .1f, .05f, Camera.main.transform.position + Camera.main.transform.forward * (attacks[0].hitBoxDistance + attacks[0].hitboxHalfExtents.z / 2));

        cc.StartCoroutine(cc.DelayAttack(0));
        cc.StartCoroutine(cc.HunterAttackCooldown(0));

        EventSystem.OnUseMeleeEvent(this, 0);
    }

    public virtual void SecondaryAttack(CombatController cc) 
    {
        cc.weaponAnimator.attackAnimationTweening(attacks[1].attackLength, false, 0);
        cc.weaponAnimator.animateSpecial(attacks[1].attackLength);
        cc.weaponAudio.PlayOneShot(attacks[1].attackAudio);

        GameObject FXObject = Instantiate(attacks[1].attackEffect);
        DestroyAfterPlay FX = FXObject.GetComponent<DestroyAfterPlay>();
        FX.Run(attacks[1].attackLength + .1f, .05f, Camera.main.transform.position + Camera.main.transform.forward * (attacks[1].hitBoxDistance + attacks[1].hitboxHalfExtents.z / 2));

        cc.StartCoroutine(cc.DelayAttack(1));
        cc.StartCoroutine(cc.HunterAttackCooldown(1));

        EventSystem.OnUseMeleeEvent(this, 1);
    }

    public virtual void OnAttackPerformed(CombatController cc, int atk) 
    {
        cc.attackOrigin.transform.localPosition = new Vector3(cc.attackOrigin.transform.localPosition.x, cc.attackOrigin.transform.localPosition.y, attacks[atk].hitBoxDistance);
        Collider[] hits = Physics.OverlapBox(cc.attackOrigin.transform.position, attacks[atk].hitboxHalfExtents, cc.attackOrigin.transform.rotation);

        if (atk == 1)
            cc.fpController.MovementSequence(movement);

        foreach (Collider c in hits)
        {
            IDamageable e = c.gameObject.GetComponent<IDamageable>();

            if (e != null)
            {
                EventSystem.OnMeleeLandEvent(this, atk, e);
                e.RecieveDamage(GetHunterAttackDamage(e, atk), null);
            }
        }
    }

    public virtual float GetHunterAttackDamage(IDamageable target, int atk) 
    {
        PlayerStats ps = Player.instance.stats;

        return ((attacks[atk].baseDamage + Inventory.instance.GetItemDamageBonuses(target, this)) * Magic.GetProwessDamageMultiplier(ps.prowess) * Inventory.instance.GetItemDamageMultipliers(target, this));
    }

    public override Sprite GetDisplaySprite()
    {
        return sprite;
    }

    public override string GetName()
    {
        return weaponName;
    }

    public override void OnEquip()
    {
        base.OnEquip();
    }

    public override void OnUnequip()
    {

    }
}

[System.Serializable]
public struct HunterAttack 
{
    public float baseDamage;
    public float attackLength;
    public float attackDelay;
    public float attackCooldown;

    public Vector3 hitboxHalfExtents;
    public float hitBoxDistance;

    public AudioClip attackAudio;
    public GameObject attackEffect;

    public bool lockMovement;
}
