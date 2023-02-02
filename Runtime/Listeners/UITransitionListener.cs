using System;
public interface UITransitionListener
{
    void Init(UIRectEntity parent);
    void ShowTransition(Action OnComplete=null);
    void HideTransition(Action OnComplete=null);
}