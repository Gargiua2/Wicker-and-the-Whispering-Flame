using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAudioIn : MonoBehaviour
{
    public AnimationCurve audioCurve;
    public float fadeLength = .5f;
    
    float targetVolume;
    AudioSource source;
    float timer = 0;

    void Start()
    {
        source = GetComponent<AudioSource>();
        targetVolume = source.volume;

        source.volume = 0;
    }

    void Update()
    {
        source.volume = Mathf.Lerp(0, targetVolume, audioCurve.Evaluate(Mathf.Lerp(0,1, timer/fadeLength)));

        timer += Time.deltaTime;

        if(source.volume == targetVolume) 
        {
            Destroy(this);
        }
    }
}
