using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
public class WraithEntity : Entity
{
    [Space, Header("Wraith Settings")]
    public GameObject projectile;
    public int burstCount;
    public GameObject hand;
    public Vector3 handTween;
    public float toAttack;
    public float holdAttack;
    public float fromAttack;
    [Range(0, 50)] public float leashRadius;
    [Range(0, 50)] public float attackRadius;
    [Range(0, 50)] public float targetRadius;
    [Range(0, 50)] public float safetyRadius;
    [Range(0, 50)] public float dangerRadius;
    public float attackCooldownLength;
    public bool attackCooldown = false;

    Vector3 handPos;
    [HideInInspector] public Vector3 homePosition;

    public override void Awake()
    {
        base.Awake();
        handPos = hand.transform.position;
        homePosition = transform.position;
    }

    Sequence handMotion;
    float t;
    public override void Attack()
    {
        attackCooldown = true;
        anim.Play("ATTACK");
        hand.SetActive(true);
        t = Time.time;

        handMotion = DOTween.Sequence();

        handMotion.Append(hand.transform.DOLocalMove(handTween, toAttack));

            handMotion.AppendInterval(holdAttack /2);
            handMotion.AppendCallback(FireProjectile);
        

        handMotion.AppendInterval(holdAttack / 2);

        handMotion.Append(hand.transform.DOLocalMove(hand.transform.localPosition, fromAttack));
        handMotion.AppendCallback(EndAttack);

        StartCoroutine(attackCooldownTimer());

    }

    IEnumerator attackCooldownTimer() 
    {
        
        yield return new WaitForSeconds(attackCooldownLength);
        attackCooldown = false;
    }

    public void FireProjectile() 
    {
        GameObject p = Instantiate(projectile, hand.transform.position + transform.forward/5, Quaternion.identity);
        p.GetComponent<TestEnemyProjectile>().direction = Navigation.instance.DirectionBetweenPoints(p.transform.position, Player.instance.transform.position);
        p.GetComponent<TestEnemyProjectile>().source = transform;
    }

    public void EndAttack() 
    {
        anim.Play("IDLE");
        hand.SetActive(false);
    }


    public void CancelAttack() 
    {
        handMotion.Kill();

        hand.transform.position = handPos;
        hand.SetActive(false);

        anim.Play("IDLE");
    }
    void OnDrawGizmos()
    {
    #if UNITY_EDITOR
        if (Selection.Contains(gameObject)) 
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, leashRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, targetRadius);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, safetyRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, dangerRadius);
        }
    #endif
    }
}
