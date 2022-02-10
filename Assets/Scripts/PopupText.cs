using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    Vector2 direction;
    float speed;
    void Start()
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
        direction = ((Random.insideUnitCircle.normalized * .3f) + (Vector2.up * .7f)).normalized;
        speed = 12;
        Destroy(gameObject, .7f);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime * 60);
    }
}
