using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopupManager : MonoBehaviour
{
    public GameObject popup;

    #region Singleton
    public static PopupManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    public void CreatePopupText(string t, Vector3 pos, Color c, bool bold = false, float size = 60) 
    {
        GameObject text = Instantiate(popup);
        TextMeshProUGUI tmp = text.GetComponent<TextMeshProUGUI>();
        tmp.text = t;
        tmp.color = c;
        text.transform.position = Camera.main.WorldToScreenPoint(pos);
        tmp.fontSize = size;

        if (bold)
            tmp.fontStyle = FontStyles.Bold;
    }
}