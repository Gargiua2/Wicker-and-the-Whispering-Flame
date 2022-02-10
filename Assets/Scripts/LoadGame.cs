using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadGame : MonoBehaviour
{
    public string toScene = "NavMeshTest";

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Button b = GetComponent<Button>();

        if (b != null)
        {
            b.onClick.AddListener(Load);
        }
        else
        {
            Debug.LogWarning("LoadGame script attatched to the object " + gameObject.name + " does not have a Button component.");
        }
    }

    public void Load()
    {
        SingletonNullifier.nullifySingletons();
        SceneManager.LoadScene(toScene);
    }
}
