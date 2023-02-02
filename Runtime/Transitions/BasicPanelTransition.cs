using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class BasicPanelTransition : UITransitionListener
{

    public GameObject panelObj;


    public void HideTransition(Action OnComplete)
    {
        panelObj.SetActive(false);
        OnComplete.Invoke();
    }

    public void Init(UIRectEntity parent)
    {
        panelObj = parent.gameObject;
        parent.transform.localPosition = Vector3.zero;
        parent.gameObject.SetActive(false);
    }

    public void ShowTransition(Action OnComplete)
    {
        panelObj.SetActive(true);
        OnComplete?.Invoke();
    }
}