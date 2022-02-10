using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMessageManager : MonoBehaviour
{
    void Start()
    {
        EventSystem.OnPlayerOverflow += OverflowPopup;
    }

    public List<UIPanelContent> OverflowPopupContent;
    bool overflowPopupSent = false;
    public void OverflowPopup() 
    {
        if (!overflowPopupSent) 
        {
            overflowPopupSent = true;
            HUDManager.instance.SendUIPanel(OverflowPopupContent);
        }
    }

}
