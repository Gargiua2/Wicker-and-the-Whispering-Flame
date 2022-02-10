using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int CurrentHP { get; private set; }
    public PlayerStats baseStats;
    public bool Dead = false;

    public AudioClip PlayerDamage;
    public AudioClip overflowDamage;
    public AudioClip PlayerDeath;

    int airJumpCount;
    public PlayerStats stats;
    public float overflowDamageMod = .3f;
    FirstPersonCharacterController controller;

    //Overflow Variables
    float overflow = 0;
    float maxOverflow = 100;

    float overflowDecreaseRate = 1;
    float overflowDecreaseTimer = 0;
    float overflowTimeBeforeDecrease = 2.5f;

    [HideInInspector]public AudioSource audioSource;

    

    #region Singleton
    public static Player instance;   
    void Awake()
    {
        if(instance == null)
            instance = this;
    }
    #endregion

    void Start()
    {
        controller = GetComponent<FirstPersonCharacterController>();
        audioSource = GetComponent<AudioSource>();
        SetStats(baseStats);
        CurrentHP = stats.maxHP;
        HUDManager.instance.UpdateHPDisplay(CurrentHP);
        HUDManager.instance.UpdateStatDisplay(stats, stats);
        EventSystem.OnFireSpell += OverflowDuringAttack;
    }

    private void OnDestroy()
    {
        EventSystem.OnFireSpell -= OverflowDuringAttack;
    }

    private void OnDisable()
    {
        EventSystem.OnFireSpell -= OverflowDuringAttack;
    }

    bool timeStop = false;
    public void Update()
    {
        if (overflowActive && overflow/maxOverflow <= .5f) 
        {
            overflowActive = false;
            overflowDecreaseRate = 1;
            HUDManager.instance.overflowOverlay.overflow = false;
        }

        

        overflowDecreaseTimer += Time.deltaTime;

        if(overflowDecreaseTimer > overflowTimeBeforeDecrease) 
        {
            SetOverflow(-overflowDecreaseRate);
        }

        if (Dead && Input.GetKeyDown(KeyCode.Escape)) 
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }

    bool overflowActive = false;
    public void SetOverflow(float v) 
    {
        

        if(v > 0) 
        {
            overflowDecreaseTimer = 0;

            if (overflowActive) 
            {
                return;
            }
        }

        overflow += v;

        overflow = Mathf.Clamp(overflow, 0, maxOverflow);

        HUDManager.instance.UpdateOverflowDisplay(overflow / maxOverflow);


        if(overflow/maxOverflow >= .95f && !overflowActive) 
        {
            overflowActive = true;
            EventSystem.OnPlayerOverflowEvent();
            overflowDecreaseRate = .2f;
            HUDManager.instance.overflowOverlay.overflow = true;
            audioSource.PlayOneShot(overflowDamage);
            TakeDamage(Random.Range(1, 8), transform, -1);
        }
    }

    public bool GetOverflowState() 
    {
        return overflowActive;
    }

    public void OverflowDuringAttack(Spell s, Focus f) 
    {
        if (overflowActive)
        {
            audioSource.PlayOneShot(overflowDamage);
            TakeDamage(Random.Range(1, 4), transform, -1);
        }
    }

    public float GetOverflowModifier() 
    {
        if (!overflowActive) 
        {
            return 1;
        } else 
        {
            
            return overflowDamageMod;
        }
    }

    public void TakeDamage(int damage, Transform damageSource, int magic) 
    {
        float resistanceMod;

        if (magic == 1) 
        {
            resistanceMod = Magic.GetResistanceMultiplier(stats.finesse);
        } else if(magic == 0)
        {
            resistanceMod = Magic.GetResistanceMultiplier(stats.physique);
        } else 
        {
            resistanceMod = 1;
        }



        CurrentHP -= (int)((float)damage * resistanceMod);

        DamageIndicatorManager.instance.CreateDamageIndicator(damageSource);

        if(!Dead)
            audioSource.PlayOneShot(PlayerDamage);

        if (CurrentHP <= 0 && !Dead)
        {
            HUDManager.instance.InitiateGameOver();
            Dead = true;
            audioSource.PlayOneShot(PlayerDeath);
        }

        EventSystem.OnTakeDamageEvent(damageSource, damage);
    }

    public void Heal(int amount) 
    {
        CurrentHP += amount;

        CurrentHP = Mathf.Clamp(CurrentHP, 0, stats.maxHP);

        EventSystem.OnTakeDamageEvent(null, 0);
    }

    public void SetStats(PlayerStats ps) 
    {
        stats.maxHP    = ps.maxHP;
        stats.favor    = ps.favor;
        stats.prowess  = ps.prowess;
        stats.craft    = ps.craft;
        stats.physique = ps.physique;
        stats.finesse  = ps.finesse;

        UpdateHiddenStats();

        EventSystem.OnUpdateStatsEvent(ps, ps);
    }

    public void AddStats(PlayerStats ps) 
    {
        PlayerStats prev = stats;

        stats.maxHP    += ps.maxHP;
        stats.favor    += ps.favor;
        stats.prowess  += ps.prowess;
        stats.craft    += ps.craft;
        stats.physique += ps.physique;
        stats.finesse  += ps.finesse;

        UpdateHiddenStats();

        EventSystem.OnUpdateStatsEvent(prev, stats);
    }

    void UpdateHiddenStats() 
    {
        airJumpCount = Mathf.FloorToInt((float)stats.finesse / 5f);
        controller.airJumps = airJumpCount;

    }
}

[System.Serializable]
public struct PlayerStats
{
    public int maxHP;
    public int favor;
    public int prowess;
    public int craft;
    public int physique;
    public int finesse;
}
