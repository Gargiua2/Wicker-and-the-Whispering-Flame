using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class WeaponAnimator : MonoBehaviour
{

    [SerializeField] ParticleSystem flash;
    [SerializeField]Weapon weapon;
    public GameObject weaponGO;

    WeaponAnimatorSettings settings;
    Animator weaponAnimator;

    const float defaultIdleAnimationFrequency = 60f;
    const float idleChance = 6;
    const float idleAnimationCooldown = .25f;

    bool idleAnimationLoopCoroutineActive = false;
    bool idleAnimationSmoothLoopCoroutineActive = false;
    bool idleAnimationRandomLoopCoroutineActive = false;

    float counter;
    bool activeBob = false;

    void Start()
    {
        weaponAnimator = gameObject.GetComponent<Animator>();

        //if(focus != null)
          //  InitializeFocus(focus);

    }

    void Update()
    {

        
        if (weapon != null && ActiveAnimationIs(FocusAnimations.WEAPON_NONE)) 
        {
            IdleAnimation();
        }

        if (weapon != null)
            IdleAnimationTweens();


    }

    void LateUpdate()
    {
        if (weapon.freeStanding)
        {
            
            float yOffset = Mathf.Lerp(2f, 0, Vector3.Angle(Camera.main.transform.forward, Vector3.up) / 95);
            GameObject.Find("Dummy").transform.position = Camera.main.transform.position + (Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.forward, new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z), Vector3.up), Vector3.up) * weapon.offsetFromCamera + (Vector3.up * yOffset)) ;
            GameObject.Find("Dummy").transform.forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        }
    }

    public void InitializeWeapon(Weapon w) 
    {
        

        weapon = w;

        settings = weapon.animationSettings;
        
        if(weaponAnimator == null)
            weaponAnimator = gameObject.GetComponent<Animator>();

        weaponAnimator.enabled = false;
        GetComponent<SpriteRenderer>().sprite = w.sprite;
        weaponAnimator.enabled = true;

        weaponAnimator.runtimeAnimatorController = weapon.overrideController;

        settings.validateSettings();
        
    }

    void IdleAnimation() 
    {
        switch (settings.idleAnimationType) 
        {
            case IdleAnimationType.IDLE_FIXED_RATE:
                IdleFixedLoop();
                break;
            case IdleAnimationType.IDLE_RANDOM:
                IdleRandom();
                break;
            case IdleAnimationType.IDLE_LOOP:
                LoopIdle();
                break;
        }
    }

    void IdleAnimationTweens() 
    {
        if (Input.GetKey(KeyCode.W) && !activeBob && !GameObject.Find("Main Camera").GetComponent<FirstPersonCharacterController>().GetGroundState()) 
        {
            StartBobCycle();
        }
    }

    Sequence weaponBobSequence;
    void StartBobCycle() 
    {
        if (!attackTween.IsActive() && !activeAutoAttack) 
        {
            activeBob = true;

            weaponBobSequence = DOTween.Sequence()
                .Append(transform.DOLocalMoveY(transform.localPosition.y + .025f, Random.Range(.2f, .2f)))
                .Append(transform.DOLocalMoveY(transform.localPosition.y, Random.Range(.2f, .2f)))
                .OnComplete(bobCallback);
        }
        
    }

    void bobCallback() 
    {
        transform.localPosition = weapon.offsetFromCamera;
        activeBob = false;
    }

    Sequence attackTween;
    bool activeAutoAttack = false;
    Vector3 localRotFrom;
    Vector3 localPosFrom;
    float autoHoldLength;
    public void attackAnimationTweening(float animationLength, bool auto, int specialTween = -1) 
    {
        AttackTweeningSettings tween;

        if(specialTween == -1) 
        {
            tween.rotatationTarget = settings.rotatationTarget;
            tween.moveTargetOffset = settings.moveTargetOffset;
            tween.startupAttackAnimationLength = settings.startupAttackAnimationLength;
            tween.holdAttackAnimationLength = settings.holdAttackAnimationLength;
            tween.endAttackAnimationLength = settings.endAttackAnimationLength;
        } else 
        {
            tween = settings.secondaryTweens[specialTween];
        }

        if (weaponBobSequence.IsActive()) 
        {
            weaponBobSequence.Kill();
            bobCallback();
        }

        if(auto && !activeAutoAttack) 
        {
            activeAutoAttack = true;

            float toAttack = animationLength * tween.startupAttackAnimationLength;
            float holdAttack = animationLength * tween.holdAttackAnimationLength;
            float fromAttack = animationLength * tween.endAttackAnimationLength;
            autoHoldLength = holdAttack + fromAttack;
            localRotFrom = transform.localRotation.eulerAngles;
            localPosFrom = transform.localPosition;
            attackTween = DOTween.Sequence();
            attackTween.Append(transform.DOLocalMove(transform.localPosition + tween.moveTargetOffset, toAttack));
            attackTween.Join(transform.DOLocalRotate(transform.localRotation.eulerAngles + tween.rotatationTarget, toAttack));
        } else if (!auto) 
        {
            float toAttack = animationLength * tween.startupAttackAnimationLength;
            float holdAttack = animationLength * tween.holdAttackAnimationLength;
            float fromAttack = animationLength * tween.endAttackAnimationLength;

            attackTween = DOTween.Sequence();
            attackTween.Append(transform.DOLocalMove(transform.localPosition + tween.moveTargetOffset, toAttack));
            attackTween.Join(transform.DOLocalRotate(transform.localRotation.eulerAngles + tween.rotatationTarget, toAttack));
            attackTween.AppendInterval(holdAttack);
            attackTween.Append(transform.DOLocalRotate(transform.localRotation.eulerAngles, fromAttack));
            attackTween.Join(transform.DOLocalMove(transform.localPosition, fromAttack));
        }
        
        
    }

    public void animateSpecial(float length) 
    {
        weaponAnimator.Play(GetAnimationName(FocusAnimations.WEAPON_SPECIAL), -1, length);
    }

    public void animateWeaponAttack(bool auto, float length = -1) 
    {
        
        ResetIdleCoroutines();

        //flash.Play();
        if (auto)
        {
            weaponAnimator.Play(GetAnimationName(FocusAnimations.WEAPON_FIRE_AUTO));
            weaponAnimator.SetBool("IsFiring", true);
        }
        else 
        {
            if(length != -1) 
            {
                weaponAnimator.Play(GetAnimationName(FocusAnimations.WEAPON_FIRE_SINGLE), -1, length);
            } else 
            {
                weaponAnimator.Play(GetAnimationName(FocusAnimations.WEAPON_FIRE_SINGLE));
            }
        }
    }
    
    public float GetDelayMultiplier()
    {
        return settings.startupAttackAnimationLength;
    }

    public void endFire() 
    {
        weaponAnimator.SetBool("IsFiring", false);

        activeAutoAttack = false;
        Sequence exitTween = DOTween.Sequence();
        exitTween.Append(transform.DOLocalRotate(localRotFrom, autoHoldLength));
        exitTween.Join(transform.DOLocalMove(localPosFrom, autoHoldLength));
    }

    #region Idle Animation Logic
    void IdleFixedLoop() 
    {
        if(!idleAnimationLoopCoroutineActive && ActiveAnimationIs(FocusAnimations.WEAPON_NONE)) 
        {
            StartCoroutine(RunIdleFixedLoop());
        }
    }
    IEnumerator RunIdleFixedLoop() 
    {
        idleAnimationLoopCoroutineActive = true;
        yield return new WaitForSeconds(settings.idleAnimationFrequency);
        weaponAnimator.Play(GetAnimationName(FocusAnimations.WEAPON_IDLE));
        idleAnimationLoopCoroutineActive = false;
    }

 //---------------------------------------------------------------------------------------------------------------------

    void IdleRandom() 
    {

        if(!idleAnimationRandomLoopCoroutineActive && ActiveAnimationIs(FocusAnimations.WEAPON_NONE)) 
        {
            weaponAnimator.Play(GetAnimationName(FocusAnimations.WEAPON_IDLE));
            StartCoroutine(RunRandomLoop());
        }
    }

    IEnumerator RunRandomLoop()
    {
        idleAnimationRandomLoopCoroutineActive = true;
        float roll = Mathf.Lerp(settings.idleAnimationChance.x, settings.idleAnimationChance.y, Random.value);
        float waitLength = GetActiveAnimationLength() + roll;
        yield return new WaitForSeconds(waitLength);
        idleAnimationRandomLoopCoroutineActive = false;
    }

    //---------------------------------------------------------------------------------------------------------------------

    void LoopIdle() 
    {
        if (!idleAnimationSmoothLoopCoroutineActive && ActiveAnimationIs(FocusAnimations.WEAPON_NONE))
        {
            weaponAnimator.Play(GetAnimationName(FocusAnimations.WEAPON_IDLE));
            StartCoroutine(RunIdleLoop());
        }
    }

    IEnumerator RunIdleLoop() 
    {
        idleAnimationSmoothLoopCoroutineActive = true;
        float waitLength = GetActiveAnimationLength();
        yield return new WaitForSeconds(waitLength + .02f);
        idleAnimationSmoothLoopCoroutineActive = false;
    }

    #endregion

    #region Helper Functions
    string GetAnimationName(FocusAnimations anim) 
    {
        return anim.ToString();
    }

    string GetActiveAnimationName() 
    {
        return weaponAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

    float GetActiveAnimationLength()
    {
        if(weaponAnimator.GetCurrentAnimatorClipInfo(0).Length > 0)
         return weaponAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        return 0;
    }

    bool ActiveAnimationIs(FocusAnimations toCompareAgainst) 
    {
        return (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName(toCompareAgainst.ToString()));
    }

    void ResetIdleCoroutines() 
    {
        StopAllCoroutines();

        idleAnimationLoopCoroutineActive = false;
        idleAnimationSmoothLoopCoroutineActive = false;
        idleAnimationRandomLoopCoroutineActive = false;
    }
    #endregion
}

#region Related Data Types
public enum FocusAnimations 
{
    WEAPON_IDLE,
    WEAPON_FIRE_SINGLE,
    WEAPON_FIRE_AUTO,
    WEAPON_SPECIAL,
    WEAPON_NONE
}

public enum IdleAnimationType 
{
    IDLE_LOOP,
    IDLE_FIXED_RATE,
    IDLE_RANDOM,
    IDLE_NONE
}

[System.Serializable]
public struct WeaponAnimatorSettings 
{
    [Header("Idle Animation Settings")]
    public IdleAnimationType idleAnimationType;
    [Range(.1f, 1f)] public float idleAnimationSpeed;
    [Range(.1f, 3f)] public float idleAnimationFrequency;
    public Vector2 idleAnimationChance;

    [Space, Header("Attack Tweening Settings")]
    public Vector3 rotatationTarget;
    public Vector3 moveTargetOffset;
    [Range(0, 1)] public float startupAttackAnimationLength;
    [Range(0, 1)] public float holdAttackAnimationLength;
    [Range(0, 1)] public float endAttackAnimationLength;
    public AttackTweeningSettings[] secondaryTweens;

    public void validateSettings() 
    {
        if(1 - (startupAttackAnimationLength + holdAttackAnimationLength + endAttackAnimationLength) > .005 ) 
        {
            
            float error = 1 - (startupAttackAnimationLength + holdAttackAnimationLength + endAttackAnimationLength);
            Debug.LogError("Weapon animator settings attack tweening values added up to less than 1. Correcting automatically. \n Error amount was " + error);
            Debug.LogError(startupAttackAnimationLength + ", " + holdAttackAnimationLength + ", " + endAttackAnimationLength);
            endAttackAnimationLength += error;
        } else if (1 - (startupAttackAnimationLength + holdAttackAnimationLength + endAttackAnimationLength) > .005) 
        {
            Debug.LogError("Weapon animator settings attack tweening values added up to more than 1. Using default values instead");
            startupAttackAnimationLength = .3f;
            holdAttackAnimationLength = .3f;
            endAttackAnimationLength = .4f;
        }
    } 

    /*Old settings, commenting out for now
    [Range(.1f, 2f)]public float swaySpeed;
    [Range(0f, 1f)] public float swayHeight;

    public float getSwayHeight() 
    {
        return Mathf.Lerp(.0005f, .002f, swayHeight);
    }*/
}

[System.Serializable]
public struct AttackTweeningSettings 
{
    public Vector3 rotatationTarget;
    public Vector3 moveTargetOffset;
    [Range(0, 1)] public float startupAttackAnimationLength;
    [Range(0, 1)] public float holdAttackAnimationLength;
    [Range(0, 1)] public float endAttackAnimationLength;
}
#endregion