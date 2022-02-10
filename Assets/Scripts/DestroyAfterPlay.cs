using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterPlay : MonoBehaviour
{
    public float delay = .2f;
    public Transform source;
    public Vector3 pos;
    public float dist = 1;

    public bool enemyAttack = true;
    private void Start()
    {

        if(enemyAttack)
        Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length + delay);
        
    }

    public void Run(float animLength, float delay, Vector3 _pos) 
    {
        pos = _pos;
        GetComponent<Animator>().Play("LongSlash", -1, animLength);
        Destroy(gameObject, animLength + delay);
    }

    private void Update()
    {
        if (enemyAttack) 
        {
            transform.position = (source.position + Navigation.instance.DirectionBetweenPoints(source.position, Player.instance.transform.position) * dist);
        } else 
        {
            transform.forward = Camera.main.transform.forward;
            transform.position = pos;
        }
        
    }

}
