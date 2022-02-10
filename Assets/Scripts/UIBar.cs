using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIBar : MonoBehaviour
{
    float value;
    public Image FilledElement;
    public void SetValue(float v) 
    {
        value = v;
        FilledElement.fillAmount = v;
    }
}
