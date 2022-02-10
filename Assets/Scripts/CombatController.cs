using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CombatController : MonoBehaviour
{
    public Weapon eWeapon;
    public Spell eSpell;
    public Spell secondarySpell = null;
    public Focus focus;
    public Hunter hunter;
    
    bool slot = true;

    public WeaponAnimator weaponAnimator;
    public AudioSource weaponAudio;
    public GameObject player;
    public GameObject weaponGO;
    public GameObject attackOrigin;
    public FirstPersonCharacterController fpController;

    bool fireOnCooldown = false;
    bool mousePressedWhileOnCooldown = false;
    bool activeAutoFire = false;
    bool attackOnCooldown = false;
    bool swapping = false;

    public static Queue<GameObject> activeProjectiles = new Queue<GameObject>();

    #region Singleton
    public static CombatController instance;
    void Awake()
    {

        if (instance == null)
            instance = this;
    }
    #endregion

    void Start()
    {
        EquipWeapon(focus);
        HUDManager.instance.UpdateSpellDisplay(eSpell, secondarySpell);
        HUDManager.instance.UpdateWeaponDisplay(focus, hunter);
    }

    void Update()
    {
        if (Player.instance.Dead || Time.timeScale == 0)
            return;

        if (Input.GetKeyDown(KeyCode.E) && !fireOnCooldown && !attackOnCooldown)
        {
            HandleSwap();         
        }

        if(Input.GetKeyDown(KeyCode.Q)  && !fireOnCooldown) 
        {
            SwapSpells();
        }

        if(eWeapon as Focus != null)
            HandleFiring();

        if (eWeapon as Hunter != null)
            HandleAttacking();
    }

    //HANDLE WEAPON SWAPPING///

    void HandleSwap()
    {
        Destroy(weaponAnimator);

        Sequence swap;
        swap = DOTween.Sequence()
            .Append(weaponGO.transform.DOLocalMoveY(weaponGO.transform.localPosition.y - 1, .25f))
            .OnComplete(FinishSwap);
    }

    void FinishSwap() 
    {
        slot = !slot;

        if (slot)
        {
            EquipWeapon(focus);
            EventSystem.OnWeaponSwappedEvent(hunter, focus);
            HUDManager.instance.UpdateWeaponDisplay(focus, hunter);
        }
        else
        {
            EquipWeapon(hunter);
            EventSystem.OnWeaponSwappedEvent(focus, hunter);
            HUDManager.instance.UpdateWeaponDisplay(hunter, focus);
        }

        
    }



    ///HUNTER COMBAT STUFF///
    void HandleAttacking() 
    {
        Hunter eHunter = eWeapon as Hunter;

        if (!attackOnCooldown) 
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                eHunter.PrimaryAttack(this);

            } else if (Input.GetMouseButtonDown(1)) 
            {
                eHunter.SecondaryAttack(this);
            }
        }
    }

    public IEnumerator DelayAttack(int atk)
    {
        Hunter eHunter = eWeapon as Hunter;

        yield return new WaitForSeconds(eHunter.attacks[atk].attackDelay);

        eHunter.OnAttackPerformed(this, atk);
    }

    public IEnumerator HunterAttackCooldown(int atk) 
    {
        Hunter eHunter = eWeapon as Hunter;

        attackOnCooldown = true;
        yield return new WaitForSeconds(eHunter.attacks[atk].attackCooldown);
        attackOnCooldown = false;
    
    }

    public IEnumerator HunterAttackCooldown(float t) 
    {
        attackOnCooldown = true;
        yield return new WaitForSeconds(t);
        attackOnCooldown = false;
    }

    public delegate void callback();
    public IEnumerator GenericTimer(float t, callback c)
    {
        yield return new WaitForSeconds(t);
        c();
    }

    ///FOCUS COMBAT STUFF///
    void HandleFiring() 
    {
        switch (eSpell.fireType) 
        {
            case FireType.SINGLE:
                if (fireOnCooldown && Input.GetMouseButtonDown(0))
                {
                    mousePressedWhileOnCooldown = true;
                }

                if ((Input.GetMouseButtonDown(0) || mousePressedWhileOnCooldown) && !fireOnCooldown) 
                {
                    FireSpell(false);
                    weaponAnimator.animateWeaponAttack(false);
                    mousePressedWhileOnCooldown = false;
                }
                break;

            case FireType.AUTO:
                if (Input.GetMouseButtonDown(0)) 
                {
                    FireSpell(true);
                    activeAutoFire = true;
                    weaponAnimator.animateWeaponAttack(true);
                } 
                else if (Input.GetMouseButton(0)) 
                {
                    FireSpell(true);
                } 
                else if (Input.GetMouseButtonUp(0)) 
                {
                    weaponAnimator.endFire();
                }
                break;

            case FireType.BURST:
                Debug.LogError("A spell with the fire type BURST was fired. This fire type is not implimented");
                break;

            case FireType.CHARGE:
                Debug.LogError("A spell with the fire type CHARGE was fired. This fire type is not implimented");
                break;
        }
    }

    void FireSpell(bool auto) 
    {
        Focus eFocus = eWeapon as Focus;
        if (!fireOnCooldown) 
        {
            weaponAnimator.attackAnimationTweening(eSpell.GetFireRate() * eFocus.fireRateModifier, auto);
            StartCoroutine(DelayFire());
            StartCoroutine(FireCooldown());
            Player.instance.SetOverflow(eSpell.overflowAmount);
        }
    }

    void CreateSpellProjectile() 
    {
        Focus eFocus = eWeapon as Focus;

        weaponAudio.PlayOneShot(eSpell.spellCastAudio);

        float aimDistance = 25f;

        for (int i = 0; i < eSpell.shotCount; i++)
        {
            //Get a new projectile from our spell, as well as it's projectile script
            BaseProjectile castProjectile;
            GameObject s = eSpell.SpawnNewProjectile(out castProjectile);

            //Move our new projectile to the spawn position of our focus, and rotate it properly.
            s.transform.SetParent(weaponGO.transform);
            s.transform.localPosition = eFocus.projectileLaunchOffset;
            s.transform.LookAt(transform);
            s.transform.SetParent(null);
            s.transform.localScale = new Vector3(eSpell.projectileAnimationSettings.startScale, eSpell.projectileAnimationSettings.startScale, eSpell.projectileAnimationSettings.startScale);

            //Project a point into the distance to aim at, offset that point by a random amount based on our accuracy.
            //Use this to get the direction our projectile should be fired in.
            Vector3 aimPoint = transform.position + transform.forward * aimDistance;
            aimPoint += Random.insideUnitSphere * eSpell.accuracy;
            Vector3 aimDirection = (aimPoint - s.transform.position).normalized;

            //Set the velocity of our projectile and initialize it
            castProjectile.velocity = aimDirection * eSpell.shotSpeed;
            castProjectile.InitializeProjectile();
        }
        
    }

    public float GetSpellDamage(IDamageable target, Spell source) 
    {
        PlayerStats ps = Player.instance.stats;

        float mod = 1;

        foreach(DamageTypeModifier dtm in focus.damageTypeModifiers) 
        {

            if(dtm.damageType == source.magicDamageType) 
            {
                mod = dtm.typeModfier;
            }
        }

        return ((source.Damage + Inventory.instance.GetItemDamageBonuses(target, focus, source)) * mod * Magic.GetFavorDamageMultiplier(ps.favor) * Player.instance.GetOverflowModifier() * Inventory.instance.GetItemDamageMultipliers(target, focus, source)) ;  
    }

    IEnumerator FireCooldown() 
    {
        Focus eFocus = eWeapon as Focus;

        fireOnCooldown = true;
        yield return new WaitForSeconds(eSpell.GetFireRate() * eFocus.fireRateModifier);
        fireOnCooldown = false;
    }

    IEnumerator DelayFire() 
    {
        Focus eFocus = eWeapon as Focus;

        yield return new WaitForSeconds(weaponAnimator.GetDelayMultiplier() * eSpell.GetFireRate() * eFocus.fireRateModifier);
        EventSystem.OnFireSpellEvent(eSpell, eFocus);
        CreateSpellProjectile();

    }

    public GameObject weaponPrefab;
    GameObject dummy;

    public void CollectWeapon(Weapon w) 
    {
        if (w as Focus != null)
        {
            focus = w as Focus;
            if (slot)
            {
                EquipWeapon(w);
                HUDManager.instance.UpdateWeaponDisplay(eWeapon, hunter);
            }
        }
        else if (w as Hunter != null)
        {
            hunter = w as Hunter;
            if (!slot) 
            {
                EquipWeapon(w);
                HUDManager.instance.UpdateWeaponDisplay(eWeapon, focus);
            }
        }

        
    }

    public void EquipWeapon(Weapon w)
    {
        

        eWeapon = w;

        Destroy(weaponGO);
        weaponGO = Instantiate(weaponPrefab);
        weaponAnimator = weaponGO.GetComponent<WeaponAnimator>();
        weaponAudio = weaponGO.GetComponent<AudioSource>();

        if (dummy != null)
            Destroy(dummy);

        if (!w.freeStanding)
        {
            weaponGO.transform.parent = Camera.main.transform;
            weaponGO.transform.localScale = w.scale;
            weaponGO.transform.localPosition = w.offsetFromCamera;
            weaponGO.transform.localEulerAngles = w.rotationOffset;
        }
        else
        {
            new GameObject("Dummy");
            GameObject d = GameObject.Find("Dummy");
            dummy = d;
            weaponGO.transform.parent = d.transform;
            weaponGO.transform.localScale = w.scale;
            weaponGO.transform.position = w.offsetFromCamera;
            weaponGO.transform.eulerAngles = w.rotationOffset;
        }
        
        weaponAnimator.InitializeWeapon(w);
    }

    public void SwapSpells() 
    {
        if (secondarySpell != null)
        {
            Spell t = eSpell;
            eSpell = secondarySpell;
            secondarySpell = t;

            HUDManager.instance.UpdateSpellDisplay(eSpell, secondarySpell);
        }
        
    }

    public void CollectSpell(Spell c) 
    {
        if(secondarySpell == null) 
        {
            secondarySpell = c;
        } else
        {
            EquipSpell(c);
        }

        c.OnEquip();

        HUDManager.instance.UpdateSpellDisplay(eSpell,secondarySpell);
    }

    public void EquipSpell(Spell s) 
    {
        eSpell = s;
        HUDManager.instance.UpdateSpellDisplay(s, secondarySpell);
        
    }

    private void OnDrawGizmos()
    {

        
    }
}
