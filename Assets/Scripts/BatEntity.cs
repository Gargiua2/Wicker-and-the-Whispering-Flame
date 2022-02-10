using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Child class inhereting from entity, used by bat enemies.

public class BatEntity : Entity
{
    //Paramters specific to bats.
    [Space, Header("Bat Settings")]
    public Vector3 home; //The point this entity should return to if it loses track of its target.
    public GameObject meleeAttack; //The prefab of the visual effect we use to represent bat attacks.
    public int damage = 3;
    public float meleeRange;
    public float attackDelay;
    public AudioClip slash; 
    [HideInInspector] public float pHP; //The HP this bat had last frame.
    

    //Initialization
    public override void Awake()
    {
        base.Awake();
        home = transform.position;
        pHP = HP;
    }

    //Specfic implementation of how bats attack.
    public override void Attack()
    {
        if (!Player.instance.Dead) 
        {
            GameObject attack = Instantiate(meleeAttack, (transform.position + Player.instance.transform.position) / 2, Quaternion.identity);
            attack.transform.LookAt(Player.instance.transform);
            attack.GetComponent<DestroyAfterPlay>().source = transform;
            audioSource.PlayOneShot(slash);
            StartCoroutine(DealDamage());
        }
        
    }

    //Coroutine used to create a small delay between when a bat starts to attack, and when the attack should land.
    //Makes the effect look better visually, also gives the player a short frame to dodge.
    IEnumerator DealDamage() 
    {
        yield return new WaitForSeconds(attackDelay);

        if(Vector3.Distance(transform.position, Player.instance.transform.position) <= meleeRange) 
        {
            Player.instance.TakeDamage(damage, transform, 0);
        }
    }
}
