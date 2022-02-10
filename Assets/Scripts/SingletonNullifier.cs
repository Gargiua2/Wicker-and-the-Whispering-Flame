using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonNullifier
{
    public static void nullifySingletons() 
    {
        HUDManager.instance = null;
        Player.instance = null;
        PopupManager.instance = null;
        EventSystem.instance = null;
        NotificationPanel.instance = null;
        CombatController.instance = null;
        ProceduralManager.instance = null;
    }
}
