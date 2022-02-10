using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimatedImage : MonoBehaviour
{
    public List<Sprite> frames = new List<Sprite>();

    public float speed = 1;

    Image image;
    int activeFrame = 0;
    float timer = 0;

    void Start()
    {
        image = GetComponent<Image>();    
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > speed)
        {
            timer = 0;
            activeFrame++;

            if (activeFrame == frames.Count)
            {
                activeFrame = 0;
            }

            image.sprite = frames[activeFrame];
        }

    }
}
