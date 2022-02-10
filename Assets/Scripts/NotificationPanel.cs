using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using UnityEngine.UI;
using TMPro;
public class NotificationPanel : MonoBehaviour
{
    public Vector3 targetSlidePosition;
    public float animateInLength;
    public float displayLength;
    public float animateOutLength;
    Vector3 defaultPos;

    public TextMeshProUGUI TMPtitle;
    public TextMeshProUGUI TMPbody;
    public GameObject tabForMore;

    bool activeState = false;
    List<UIPanelContent> queuedContent;

    #region Singleton
    public static NotificationPanel instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    void Start()
    {
        defaultPos = ((RectTransform)transform).anchoredPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && queuedContent != null && activeState)
        {
            if(queuedContent.Count > 1) 
            {
                HUDManager.instance.SendUIPanel(queuedContent);
            } else 
            {
                HUDManager.instance.SendUIPanel(queuedContent[0]);
            }
            
        }    
    }

    public void SendNewNotification(string title, string body, UIPanelContent content = null)
    {
        List<UIPanelContent> listContent = new List<UIPanelContent>();
        listContent.Add(content);

        SendNewNotification(title, body, listContent);
    }

    public void SendNewNotification(string title, string body, List<UIPanelContent> content = null)
    {
        if (activeState == false)
            AnimatePanelIn();

        TMPtitle.text = title;
        TMPbody.text = body;

        queuedContent = null;

        if(content != null) 
        {
            if(content[0] != null) 
            {
                queuedContent = content;
                tabForMore.SetActive(true);
            } 
            else 
            {
                tabForMore.SetActive(false);
            }
        } else 
        {
            tabForMore.SetActive(false);
        }
    }

    Sequence animateIn;
    void AnimatePanelIn() 
    {
        activeState = true;

        animateIn.Kill();
        animateIn = DOTween.Sequence();

        animateIn.Append(((RectTransform)transform).DOAnchorPos3D(targetSlidePosition, animateInLength).SetUpdate(true));
        animateIn.AppendInterval(displayLength).SetUpdate(true);
        animateIn.Append(((RectTransform)transform).DOAnchorPos3D(defaultPos, animateOutLength).SetUpdate(true));
        animateIn.AppendCallback(EndActivePanel);
    }

    void EndActivePanel()
    {
        activeState = false;
    }

    


}
