using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public GameObject pillarA;
    public GameObject pillarB;
    public GameObject pillarC;

    public GameObject levelEnd;

    int destoryedSpawners = 0;
    public void Start()
    {
        EventSystem.OnSpawnerDestroyed += OnProgress;    
    }

    public void OnDestroy()
    {
        EventSystem.OnSpawnerDestroyed -= OnProgress;
    }

    public void OnDisable()
    {
        EventSystem.OnSpawnerDestroyed -= OnProgress;
    }

    public void OnProgress(int id) 
    {
        if (id == 0)
            pillarA.SetActive(true);

        if (id == 1)
            pillarB.SetActive(true);

        if (id == 2)
            pillarC.SetActive(true);

        destoryedSpawners++;

        if(destoryedSpawners >= 3) 
        {
            levelEnd.SetActive(true);
        }
    }
}
