using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    [SerializeField] float moonRoationSpeed;
    [SerializeField] float startingRotation;
    [SerializeField] Light directionalLight;
    [SerializeField] Color dim;
    [SerializeField] Color bright;

    Transform player;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        transform.RotateAround(Vector3.zero, Vector3.right, startingRotation);
    }

    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.right, moonRoationSpeed);
        transform.LookAt(player);

        directionalLight.intensity = Mathf.Lerp(.15f, .3f, transform.position.y / 750);
        directionalLight.color = Color.Lerp(dim, bright, transform.position.y / 750);
    }
}
