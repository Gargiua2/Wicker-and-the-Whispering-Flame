using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class SlidingUIPanel : MonoBehaviour
{
    

    public Vector3 targetSlidePosition;
    public float animateInLength;
    public float animateOutLength;
    Vector3 defaultPos;

    public Image headerImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;
    public TextMeshProUGUI labelText;
    public GameObject turnPageButton;

    bool panelOpen = false;
    List<UIPanelContent> panels = new List<UIPanelContent>();
    int panelIndex = 0;

    void Start()
    {
        defaultPos = ((RectTransform)transform).anchoredPosition;
    }

    public void ChangePanelContent(UIPanelContent content) 
    {
        if (panels.Count <= 1)
        {
            turnPageButton.SetActive(false);
        }

        headerImage.sprite = content.UIPanelHeaderImage;
        titleText.text = content.UIPanelTitle;
        bodyText.text = "          " + content.UIPanelBody;
        labelText.text = content.UIPanelLabel;
    }

    public void ChangePanelContent(List<UIPanelContent> contents) 
    {
        panels = contents;

        turnPageButton.SetActive(true);

        panelIndex = 0;

        ChangePanelContent(panels[0]);
    }

    public void AdvancePage()
    {
        panelIndex++;

        if(panelIndex >= panels.Count) 
        {
            panelIndex = 0;
        }

        ChangePanelContent(panels[panelIndex]);
    }

    public void SlidePanelIn() 
    {
        ((RectTransform)transform).DOAnchorPos3D(targetSlidePosition, animateInLength).SetUpdate(true);
    }

    public void SlidePanelOut() 
    {
        ((RectTransform)transform).DOAnchorPos3D(defaultPos, animateOutLength).SetUpdate(true);
    }
}
