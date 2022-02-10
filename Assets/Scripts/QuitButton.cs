using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuitButton : MonoBehaviour
{
    void Start()
    {
        Button b = GetComponent<Button>();

        if (b != null)
        {
            b.onClick.AddListener(QuitGame);
        }
        else
        {
            Debug.LogWarning("QuitButton script attatched to the object " + gameObject.name + " does not have a Button component.");
        }
    }

    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }
}
