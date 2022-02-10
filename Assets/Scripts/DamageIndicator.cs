using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DamageIndicator : MonoBehaviour
{
    float LT;
    Transform s;
    float r;

    float timer = 0;

    Image i;
    void Awake()
    {
        i = GetComponent<Image>();
    }

    void Update()
    {
        PositionIndicator();

        timer += Time.deltaTime;

        i.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), timer / LT);

        if(timer > LT) 
        {
            Destroy(gameObject);
        }
    }

    public void init(float lifetime, float radius, Transform source)
    {
        LT = lifetime;
        s = source;
        r = radius;

        PositionIndicator();
    }

    void PositionIndicator()
    {

        RectTransform p = transform as RectTransform;
        p.anchoredPosition = new Vector2(0, 0);

        Vector2 direction = (new Vector2(s.position.x, s.position.z) - new Vector2(Player.instance.transform.position.x, Player.instance.transform.position.z)).normalized;
        float a = Vector2.SignedAngle(direction, new Vector2(Player.instance.transform.forward.x, Player.instance.transform.forward.z).normalized);


        p.anchoredPosition = new Vector3(Mathf.Sin(Mathf.Deg2Rad * a) * r, Mathf.Cos(Mathf.Deg2Rad * a) * r, 0);
        p.eulerAngles = new Vector3(0, 0, -a);
    }

    //FROM https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
