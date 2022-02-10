using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class URLOpener : MonoBehaviour
{
    public string url;

    void Start()
    {
        Button b = GetComponent<Button>();
        
        if(b != null) 
        {
            b.onClick.AddListener(OpenURL);
        } else 
        {
            Debug.LogWarning("URLOpener script attatched to the object " + gameObject.name + " does not have a Button component.");
        }
    }

    public void OpenURL() 
    {
        Application.OpenURL(url);
    }
}
