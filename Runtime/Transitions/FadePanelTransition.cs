using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasGroup))]
public class FadePanelTransition : MonoBehaviour,UITransitionListener
{
    private CanvasGroup canvasGroup;

    private float Alpha
    {
        get{
            return canvasGroup.alpha;
        }
        set{
            canvasGroup.alpha = value;
        }
    }

    public void HideTransition(Action OnComplete = null)
    {
        Alpha = 1;
        DOTween.To(()=>Alpha,a=>Alpha=a,0f,0.3f).OnComplete(
            ()=>{
                OnComplete?.Invoke();
                gameObject.SetActive(false);
            });
    }

    public void Init(UIRectEntity parent)
    {
        transform.localPosition = Vector3.zero;
        canvasGroup = GetComponent<CanvasGroup>();
        Alpha =0;
        gameObject.SetActive(false);
    }

    public void ShowTransition(Action OnComplete = null)
    {
        gameObject.SetActive(true);
        Alpha =0;
        DOTween.To(()=>Alpha,a=>Alpha=a,1f,0.3f).OnComplete(()=>
            OnComplete?.Invoke()
        );
    }

}
