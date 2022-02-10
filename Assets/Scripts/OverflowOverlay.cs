using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverflowOverlay : MonoBehaviour
{
    public List<Sprite> frames = new List<Sprite>();

    public float speed = 1;

    public bool overflow;
    public Image overflowIcon;
    public Image HPIcon;

    Image overlay;
    int activeFrame = 0;
    float timer = 0;
    void Awake()
    {
        overlay = GetComponent<Image>();
    }
    public void SetOverflowAmount(float percent) 
    {
        percent = percent.Remap(.5f, 1, 0, 1);

        if(!overflow || percent < .6f)
        overlay.color = new Color(1, 1, 1, percent);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > speed) 
        {
            timer = 0;
            activeFrame++;

            if(activeFrame == frames.Count) 
            {
                activeFrame = 0;
            }

            overlay.sprite = frames[activeFrame];
        }

        if (overflow)
        {
            overflowIcon.enabled = true;
            HPIcon.enabled = false;
        } else 
        {
            overflowIcon.enabled = false;
            HPIcon.enabled = true;
        }
        
    }
}
