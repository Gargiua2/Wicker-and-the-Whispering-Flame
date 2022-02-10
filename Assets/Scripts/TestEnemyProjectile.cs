using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyProjectile : MonoBehaviour
{
    public Vector3 direction;
    public float speed = .75f;
    public Transform source;
    void Start()
    {
        Destroy(gameObject, 5f);    
    }
    void Update()
    {
        if(Time.timeScale != 0)
            transform.position += direction * speed * Time.deltaTime * 60;    
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player") 
        {
            Player.instance.TakeDamage(3, source, 1);
            Destroy(gameObject) ;
        }
    }
}
