using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Spell", menuName = "Scriptables/Spells/Basic Spell")]
public class Spell : ScriptableObject, ICollectable
{
    [Header("General Settings")]
    public string spellName = "New Spell";
    public string spellDescription = "A Magic Spell!";
    [TextArea(1, 6)] public string lengthyDescription = "A Magic Spell!";
    public UIPanelContent descriptionPage;
    public Sprite sprite;

    [Space,Header("Gameplay Settings")]
    public FireType fireType;
    public MagicDamageType magicDamageType;
    public ProjectileBehaviours projectileBehavior;
    public GameObject projectilePrefab;
    public float Damage = 1;
    public float overflowAmount = 3;
    [Range(.3f, 1.5f)] public float shotSpeed = .7f;
    [SerializeField, Range(0,20)] float fireRate;
    [Range(0f, 7f)] public float accuracy;
    [Range(.01f, 4f)] public float range = 2f;
    [Range(1,7)] public int shotCount = 1;

    [Space, Header("Audio & Visual Settings")]
    [Range(.25f, 2.5f)] public float projectileSize = 1;
    public AudioClip spellCastAudio;
    public GameObject projectileParticleSystem;
    public ParticleProjectileAnimations projectileAnimationSettings;
    public GameObject impactParticleSystem;
    

    public void OnEquip() 
    {
        NotificationPanel.instance.SendNewNotification(spellName, spellDescription, GetDescriptions());
    }
    public void OnUnequip() 
    {
    
    }

    public virtual UIPanelContent GetDescriptions() 
    {
        UIPanelContent newContent = new UIPanelContent();
        newContent.UIPanelTitle = spellName;
        newContent.UIPanelBody = lengthyDescription;
        newContent.UIPanelHeaderImage = sprite;
        newContent.UIPanelLabel = "Spells";

        return newContent;

    }

    public Sprite GetDisplaySprite()
    {
        return sprite;
    }

    public Sprite GetIcon()
    {
        return sprite;
    }

    public string GetName()
    {
        return spellName;
    }

    public GameObject SpawnNewProjectile(out BaseProjectile castProjectile)
    {
        GameObject p = Instantiate(projectilePrefab);
        castProjectile = (BaseProjectile)p.AddComponent(GetProjectileType());
        castProjectile.damage = Damage;
        castProjectile.impactEffect = impactParticleSystem;
        castProjectile.animations = projectileAnimationSettings;
        castProjectile.source = this;

        return p;
    }

    public float GetFireRate() 
    {
        return Magic.CalculateFireRate(fireRate);
    }

   System.Type GetProjectileType() 
   {
        switch (projectileBehavior) 
        {
            case ProjectileBehaviours.BASE:
                return typeof(BaseProjectile);
                break;

            default:
                Debug.LogError("Warning, the spell " + spellName + " has attempted to use unimplemented projectile behaviour type " + projectileBehavior.ToString());
                return null;
        }
   }

    public enum ProjectileBehaviours 
    {
        BASE,
        HOMING
    }
}
