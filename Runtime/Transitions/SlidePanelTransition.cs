using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class SlidePanelTransition : MonoBehaviour,UITransitionListener
{
    public Ease ease = Ease.InOutQuad;
    private float canvasWidth;
    public void HideTransition(Action OnComplete = null)
    {
         transform.DOLocalMoveX(-canvasWidth,0.5f).SetRelative(true).OnComplete(()=>
         {
            OnComplete?.Invoke();
            gameObject.SetActive(false);
         }
        ).SetEase(ease);
    }

    public void Init(UIRectEntity parent)
    {
        canvasWidth = GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.width;
        transform.localPosition = Vector3.left * canvasWidth;
        gameObject.SetActive(false);
    }

    public void ShowTransition(Action OnComplete = null)
    {
        gameObject.SetActive(true);
        transform.DOLocalMoveX(canvasWidth,0.5f).SetRelative(true).OnComplete(()=>
            OnComplete?.Invoke()
        ).SetEase(ease);
    }

}

