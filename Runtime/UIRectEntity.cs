using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public abstract class UIRectEntity : MonoBehaviour
{
    public abstract string NameSuffix {get;}
    public string ID;

    public RectTransform MainRect;

    public UITransitionListener transitionListener;
    public UIChangeListener[] changeListeners;
    public bool expandedView;

    public virtual void Init()
    {
        changeListeners = GetComponentsInChildren<UIChangeListener>();
        transitionListener = GetComponent<UITransitionListener>();

        if(transitionListener==null)
        {
            transitionListener = new BasicPanelTransition();
        }
        transitionListener.Init(this);
    }

    public void CallListeners(Action<UIChangeListener> onChangeListener)
    {
        if(changeListeners==null) return;
        foreach(var t in changeListeners)
        {
            onChangeListener(t);
        }
    }

    public void OnValidate()
    {
        #if UNITY_EDITOR
        if(Application.isPlaying) return;


        if(MainRect==null)
        {
            MainRect = gameObject.GetComponent<RectTransform>();
        }

        if(!string.IsNullOrEmpty(ID))
        {
            var uiManager = GetComponentInParent<UIManager>();
            if(uiManager!=null)
            {
                uiManager.Populate();
            }
        }

        #endif
    }
}

