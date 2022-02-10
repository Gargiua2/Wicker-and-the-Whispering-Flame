using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Panel Content", menuName = "UI/Panel Content")]
public class UIPanelContent : ScriptableObject
{
    public Sprite UIPanelHeaderImage;
    public string UIPanelTitle;
    public string UIPanelLabel;
    [TextArea(1, 6)] public string UIPanelBody;
}
