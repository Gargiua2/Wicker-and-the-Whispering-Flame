using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OozeEntity : Entity
{
    [Space, Header("Ooze Settings")]
    public int size = 2;
    public Vector2Int largeSplitRange;
    public Vector2Int mediusmSplitRange;
    public GameObject oozeMedium;
    public GameObject oozeSmall;
    public float splitSpawnRadius;
    public float oozeDeathRate;
    public AudioClip onAttack;
    public GameObject meleeAttack;
    public float delayBeforeAttack = .15f;
    public float damage = 5f;
    public float attackRange = 3.5f;

    [Header("Radii")]
    public float chaseRadius;
    public float attackRadius;
    public float leashRadius;
    [HideInInspector] public Vector3 home;
    [HideInInspector] public bool curInDamageState = false;
    public override void Awake()
    {
        base.Awake();
        home = transform.position;

    }

    public override void Die()
    {
        if (alive)
        {
            EventSystem.OnKillEnemyEvent(this);

            Destroy(gameObject, oozeDeathRate);

            alive = false;

            EnemyManager.instance.DeregisterEnemy(this);
            StartCoroutine(splitTimer());
        }
    }

    IEnumerator splitTimer()
    {
        yield return new WaitForSeconds(oozeDeathRate/2);

        Split();
    }

    public override void Attack()
    {
        if (!Player.instance.Dead)
        {
            GameObject attack = Instantiate(meleeAttack, (transform.position + Player.instance.transform.position) / 2, Quaternion.identity);
            attack.transform.LookAt(Player.instance.transform);
            attack.GetComponent<DestroyAfterPlay>().source = transform;
            audioSource.PlayOneShot(onAttack);
            StartCoroutine(DealDamage());
        }
    }

    public IEnumerator DealDamage() 
    {
        yield return new WaitForSeconds(delayBeforeAttack);

        if(Navigation.instance.GetDistanceToPlayer(transform.position) < attackRange)
            Player.instance.TakeDamage((int)damage, transform, 0);
    }

    public void Split() 
    {
        
        if (size - 1 == 1) 
        {

            int splitAmount = Random.Range(largeSplitRange.x, largeSplitRange.y + 1);

            for (int i = 0; i <= splitAmount; i++) 
            {
                float spawnAngle = Random.value * Mathf.PI * 2;
                Vector3 spawnPoint = transform.position + new Vector3(Mathf.Sin(spawnAngle), 0, Mathf.Cos(spawnAngle)) * splitSpawnRadius;

                Instantiate(oozeMedium, spawnPoint, Quaternion.identity);
            }
            
        } else if(size - 1 == 0) 
        {
            int splitAmount = Random.Range(mediusmSplitRange.x, mediusmSplitRange.y + 1);

            for (int i = 0; i <= splitAmount; i++)
            {
                float spawnAngle = Random.value * Mathf.PI * 2;
                Vector3 spawnPoint = transform.position + new Vector3(Mathf.Sin(spawnAngle), 0, Mathf.Cos(spawnAngle)) * splitSpawnRadius;

                Instantiate(oozeSmall, spawnPoint, Quaternion.identity);
            }

        }
    }
}
