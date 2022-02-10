using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCursor : MonoBehaviour
{
    void LateUpdate()
    {
        if(Cursor.lockState == CursorLockMode.None) 
        {
            ((RectTransform)transform).position = (Input.mousePosition) + new Vector3(70,-70);
            Cursor.visible = false;
        } else 
        {
            ((RectTransform)transform).anchoredPosition = Vector3.one * 5000;
            
        }
        
    }
}
