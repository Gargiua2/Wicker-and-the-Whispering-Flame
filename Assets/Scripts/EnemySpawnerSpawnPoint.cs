using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemySpawnerSpawnPoint : MonoBehaviour
{
    public ParticleSystem spawnFlash;
    public ParticleSystem prespawn;
    public float lengthToSpawn;
    public float spawnFlashDelay;
    public AudioClip onSpawn;
    public GameObject enemyToSpawn;

    AudioSource source;
    [HideInInspector] public SpawnTower tower;
    void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(toSpawn());           
    }

    IEnumerator toSpawn() 
    {
        yield return new WaitForSeconds(lengthToSpawn);
        prespawn.Stop();
        spawnFlash.Play();
        source.PlayOneShot(onSpawn);
        StartCoroutine(spawnEnemy());
    }

    IEnumerator spawnEnemy() 
    {
        yield return new WaitForSeconds(spawnFlashDelay);

        GameObject e = Instantiate(enemyToSpawn, spawnFlash.transform.position + Vector3.up/3, Quaternion.identity);

        if(tower != null) 
        {
            tower.RegisterEnemyToTower(e);
        }
    }
}
