using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceWindowed : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = false;
        Screen.SetResolution(1920, 1080,FullScreenMode.MaximizedWindow);
    }
}
