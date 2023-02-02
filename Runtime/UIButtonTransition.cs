using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class UIButtonTransition : MonoBehaviour,UIChangeListener
{
    private UIManager uiManager;
    public string targetPanelId;
    public UnityEvent onClick;
    
    private void Awake()
    {
        var button = GetComponent<Button>();
        if(onClick.GetPersistentEventCount()==0)
        {
            button.onClick.AddListener(()=>{
                uiManager.ShowPanel(targetPanelId);
            });
        }
        else
        {
             button.onClick.AddListener(()=>{
                onClick.Invoke();
            });
        }
    }

    public void OnHide()
    {
    }

    public void OnHideComplete()
    {
    }

    public void OnShow(UIRectEntity entity, UIManager manager)
    {
        this.uiManager = manager;
    }

    public void OnShowComplete()
    {
    }

}
