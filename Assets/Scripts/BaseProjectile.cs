using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class BaseProjectile : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public Vector3 velocity;
    [HideInInspector] public GameObject impactEffect;
    [HideInInspector] public ParticleProjectileAnimations animations;

    [HideInInspector] public Spell source;
    float lifetimeTimer = 0;
    GameObject particleTail;
    Rigidbody rb;

    bool initialized = false;
    public void InitializeProjectile()
    {

        GameObject particles = Instantiate(source.projectileParticleSystem, transform);
        particles.transform.position = this.transform.position;
        particleTail = particles.GetComponent<ParticleProjectile>().particleTail;
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, source.range);

        GetComponent<BoxCollider>().size = source.projectileSize * Vector3.one;

        initialized = true;

        OnCast();
    }

    void Update()
    {
        if (initialized)
        {
            lifetimeTimer += Time.deltaTime;
            Move(velocity * Time.deltaTime * 60);
            Animate();
        }
    }

    public void Move(Vector3 vel)
    {
        RaycastHit hit;

        if (rb.SweepTest(vel.normalized, out hit, vel.magnitude, QueryTriggerInteraction.Ignore))
        {
            IDamageable enemyHit = hit.collider.GetComponent<IDamageable>();

            if (enemyHit != null)
            {
                OnHitDamageableEntity(null, hit, enemyHit);
            } else
            {
                OnHitEnvironment(null, hit);
            }
        }

        transform.position += vel * Time.timeScale;
    }

    public void Animate()
    {
        float targetScale;

        if (lifetimeTimer / source.range < animations.animateInEnd)
        {
            targetScale = Mathf.Lerp(animations.startScale, source.projectileSize, (lifetimeTimer / source.range) / animations.animateInEnd);
        } else if (lifetimeTimer / source.range > animations.animateOutStart)
        {
            targetScale = Mathf.Lerp(source.projectileSize, animations.endScale, Mathf.InverseLerp(animations.animateOutStart, 1, lifetimeTimer / source.range));
        } else
        {
            targetScale = source.projectileSize;
        }

        transform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }

    public virtual void OnCast()
    {

    }

    public virtual void OnHitEnvironment(Collision collision, RaycastHit hit)
    {
        Vector3 collisionPoint = collision != null ? collision.GetContact(0).point : hit.point;
        Vector3 collisionNormal = collision != null ? collision.GetContact(0).normal : hit.normal;
        if (particleTail != null)
        {
            particleTail.transform.SetParent(null);
            Destroy(particleTail, .5f);
        }

        GameObject impact = Instantiate(impactEffect, collisionPoint + hit.normal/4f, Quaternion.identity);
        impact.transform.forward = -velocity.normalized;
        Destroy(impact, 5f);

        Destroy(gameObject);
    }

    public virtual void OnHitDamageableEntity(Collision collision, RaycastHit hit, IDamageable damageableEntity)
    {
        EventSystem.OnProjectileLandEvent(source, damageableEntity);
        
        damageableEntity.RecieveDamage(CombatController.instance.GetSpellDamage(damageableEntity, source), source);

        Vector3 collisionPoint = collision != null ? collision.GetContact(0).point : hit.point;
        Vector3 collisionNormal = collision != null ? collision.GetContact(0).normal : hit.normal;
        if (particleTail != null)
        {
            particleTail.transform.SetParent(null);
            Destroy(particleTail, .5f);
        }

        GameObject impact = Instantiate(impactEffect, collisionPoint + hit.normal / 4f, Quaternion.identity);
        impact.transform.forward = -velocity.normalized;
        Destroy(impact, 5f);

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        var damageableEntity = collision.gameObject.GetComponent<IDamageable>();
        if (damageableEntity != null)
        {
            OnHitDamageableEntity(collision, new RaycastHit(), damageableEntity);
            return;
        }

        if (collision.gameObject.tag != "Projectile")
            OnHitEnvironment(collision, new RaycastHit());
    }
    
}

[System.Serializable]
public struct ParticleProjectileAnimations
{
    [Range(0, 1f)] public float animateInEnd;
    public float startScale;
    [Range(0, 1f)] public float animateOutStart;
    public float endScale;

    public ParticleProjectileAnimations(ParticleProjectileAnimations c) 
    {
        this.animateInEnd = c.animateInEnd;
        this.startScale = c.startScale;
        this.animateOutStart = c.animateOutStart;
        this.endScale = c.endScale;
    }
}