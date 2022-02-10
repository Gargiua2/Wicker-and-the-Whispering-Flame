using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    

    public LayerMask lm;
    Transform tip;

    HunterThrown weapon;
    Vector3 velocity;
    float gravity;
    Vector3 facing;

    public void Throw(Vector3 _facing, float force, float _gravity, HunterThrown w)
    {
        CombatController.activeProjectiles.Enqueue(gameObject);

        tip = transform.Find("Tip");
        weapon = w;
        transform.up = facing;
        velocity = force * _facing;
        gravity = _gravity;
        transform.SetParent(null);
        Update();
    }
    void LateUpdate()
    {
        if (CombatController.activeProjectiles.Count > HunterThrown.MAX_NUMBER_ACTIVE_PROJECTILES)
        {
            Destroy(CombatController.activeProjectiles.Dequeue());   
        }
    }

    public void Despawn() 
    {
        Destroy(gameObject);
    }

    void Update()
    {
        Vector3 pTip = tip.position;

        velocity = velocity - new Vector3(0, gravity, 0);
        transform.position += velocity;

        transform.up = velocity.normalized;
        
        Ray r = new Ray(pTip, velocity);
        RaycastHit hit;
        
        if(Physics.Raycast(r, out hit, velocity.magnitude, lm)) 
        {
            IDamageable e = hit.transform.gameObject.GetComponent<IDamageable>();
            if (e != null) 
            {
                e.RecieveDamage(weapon.ThrowDamage, null);
                velocity = (-velocity)/3 + Vector3.up/8;
                transform.forward = velocity.normalized;
                transform.position += velocity * Time.deltaTime * 60;

                EventSystem.OnMeleeLandEvent(weapon, 1, e);
            } else 
            {
                //transform.position = hit.point + hit.normal;
                GetComponent<BoxCollider>().enabled = true;
                Destroy(this);
            }
        }
    } 

}
