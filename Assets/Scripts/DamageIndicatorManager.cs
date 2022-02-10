using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicatorManager : MonoBehaviour
{
    [SerializeField] GameObject indicator;
    [SerializeField] float indicatorLifetime = .35f;
    [SerializeField] float indicatorRadius = 50;
    
    #region SINGLETON
    public static DamageIndicatorManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    public void CreateDamageIndicator(Transform source) 
    {

        GameObject i = Instantiate(indicator, transform);

        i.GetComponent<DamageIndicator>().init(indicatorLifetime, indicatorRadius, source);
        
    }

    
}
