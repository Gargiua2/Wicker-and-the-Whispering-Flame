using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTower : MonoBehaviour, IDamageable
{
    public GameObject spawnPoint;
    public float radiusAgainstPlayer;
    public float spawnWithinRadius;
    public float spawnOutsideRadius;
    public float spawnPercentageChance;
    public float trySpawnTimer;
    public int maxEntitiesSpawned = 5;
    public LayerMask groundMask;
    public AudioClip onSpawn;
    public AudioClip onDie;
    public AudioClip onDamage;
    public ParticleSystem spawnFlash;
    public ParticleSystem deathFlash;
    public int HP;

    public float hoverHeight;
    public float hoverSpeed;
    
    float counter = 0;
    SpriteRenderer r;
    AudioSource s;
    int maxHP;
    List<Entity> spawnedEntities = new List<Entity>();

    public int id;
    void Start()
    {
        s = GetComponent<AudioSource>();
        r = GetComponent<SpriteRenderer>();
        maxHP = HP;
        EventSystem.OnKillEnemy += TryDeregisterEnemy;
        InvokeRepeating("TrySpawn", 0, trySpawnTimer);        
    }

    void OnDestroy()
    {
        EventSystem.OnKillEnemy -= TryDeregisterEnemy;
    }
    void OnDisable() 
    {
        EventSystem.OnKillEnemy -= TryDeregisterEnemy;
    }


    void Update() 
    {
        if(Navigation.instance.GetDistanceToPlayer(new Vector3(transform.position.x, 0, transform.position.z)) < radiusAgainstPlayer && r!=null)
        {
            r.color = Color.Lerp(Color.red, Color.white, (float)HP / (float)maxHP);  
        } else if(r != null)
        {
            r.color = Color.grey;
        }
    }

    void TrySpawn() 
    {
        Debug.Log("Trying tower spawn");
        //Roll for chance and check player distance - if no good, return.
        if (Random.value > spawnPercentageChance || Navigation.instance.GetDistanceToPlayer(new Vector3(transform.position.x, 0, transform.position.z)) > radiusAgainstPlayer || spawnedEntities.Count >= maxEntitiesSpawned)
            return;

        //Get a point in a radius around yourself
        float angle = Random.value * (Mathf.PI * 2);
        Vector3 point = transform.position + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * Random.Range(spawnOutsideRadius,spawnWithinRadius);

        Ray r = new Ray(point, Vector3.down);
        RaycastHit h;

        if(Physics.Raycast(r, out h, groundMask)) 
        {

            GameObject spawn = Instantiate(spawnPoint, h.point + Vector3.up * 1.5f, Quaternion.identity);
            s.PlayOneShot(onSpawn);
            spawnFlash.Play();
            spawn.GetComponent<EnemySpawnerSpawnPoint>().tower = this;
        }
    }

    public void TryDeregisterEnemy(Entity e) 
    {
        if (spawnedEntities.Contains(e)) 
        {
            spawnedEntities.Remove(e);
        }
    }

    public void RegisterEnemyToTower(GameObject e) 
    {
        Entity newEntity = e.GetComponent<Entity>();

        if (newEntity != null)
        {
            spawnedEntities.Add(newEntity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0, transform.position.z), radiusAgainstPlayer);
    }

    public void RecieveDamage(float amount, Spell spell)
    {
        if (Navigation.instance.GetDistanceToPlayer(new Vector3(transform.position.x, 0, transform.position.z)) > radiusAgainstPlayer)
            return;

        int flatAmt = (int)amount/2;
        PopupManager.instance.CreatePopupText("-" + flatAmt.ToString(),transform.position, Color.red);

        HP -= flatAmt;

        r.color = Color.Lerp(Color.red, Color.white, (float)HP / (float)maxHP);
        GetComponent<Animator>().Play("OnDamage");
        if(HP <= 0) 
        {
            s.PlayOneShot(onDie);
            deathFlash.Play();
            Destroy(r);
            
            foreach(Entity e in spawnedEntities) 
            {
                e.RecieveDamage(1000, null);
            }

            EventSystem.OnSpawnerDestroyedEvent(id);

            Destroy(this);
        } else 
        {
            s.PlayOneShot(onDamage,.6f);
        }
    }
}
