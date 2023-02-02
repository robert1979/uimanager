using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(RectTransform))]
[ExecuteAlways]
public class UIManager : MonoBehaviour
{
    private UIRectEntity previousPanel;
    private UIRectEntity activePanel;
    public string startPanelId;

    public UIPanel[] uiPanels;
    public UIPopUp[] uiPopUps;

    private Dictionary<string,UIRectEntity> panelDictionary;
    private Dictionary<string,UIRectEntity> popUpDictionary;

    public RectTransform canvasRect;

    private void Update()
    {
        #if UNITY_EDITOR
        if(!Application.isPlaying)
        {
            UpdateChildRects();
        }
        #endif
    }

    private void OnEnable()
    {
        #if UNITY_EDITOR
        if(!Application.isPlaying)
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }
        #endif
    }

    private void OnDisable()
    {
        #if UNITY_EDITOR
        if(!Application.isPlaying)
        {
            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
        }
        #endif
    }

    private void UpdateChildRects()
    {
        canvasRect = GetComponent<RectTransform>();

        if(canvasRect==null)return;

        var rects = GetComponentsInChildren<UIRectEntity>(true);
        foreach(var r in rects)
        {
            MatchOther(r.MainRect,canvasRect);
        }
    }

    public void MatchOther(RectTransform rt,  RectTransform other){
        Vector2 myPrevPivot = rt.pivot;
        myPrevPivot = other.pivot;
       //rt.position =  other.position;
        rt.localScale = Vector3.one;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,  other.rect.width);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,  other.rect.height);
        //rectTransf.ForceUpdateRectTransforms(); - needed before we adjust pivot a second time?
        rt.pivot = myPrevPivot;
    }

    private void Start()
    {
        if(!Application.isPlaying)  return;

        Populate();
        UpdateChildRects();
        foreach(var p in uiPanels){ p.Init();}
        foreach(var p in uiPopUps) {p.Init();}

        if(!string.IsNullOrEmpty(startPanelId))
        {
            ShowPanel(startPanelId);
        }
    }

    public void Populate()
    {
        uiPanels = GetComponentsInChildren<UIPanel>(true);
        uiPopUps = GetComponentsInChildren<UIPopUp>(true);
        panelDictionary = new Dictionary<string, UIRectEntity>();
        popUpDictionary = new Dictionary<string, UIRectEntity>();

        PopulateEntity(panelDictionary,uiPanels);
        PopulateEntity(popUpDictionary,uiPopUps);
    }

    private void PopulateEntity(Dictionary<string,UIRectEntity> dictionary, UIRectEntity[] panelEntities)
    {
        foreach(var p in panelEntities)
        {
            if(!string.IsNullOrEmpty(p.ID) && !dictionary.ContainsKey(p.ID))
            {
                dictionary.Add(p.ID,p);
            }
        }
    }

    private void ResetPosition(UIRectEntity[] entities)
    {
        foreach(var r in entities)
        {
            r.transform.localPosition = Vector3.zero;
        }
    }

    private void OnHierarchyChanged()
    {
        if(!Application.isPlaying)
        {
            Populate();
        }
    }

    public void ShowPanel(string panelId)
    {
        Debug.Assert(panelDictionary!=null,"panelDictionary is null");
        Debug.Assert(panelDictionary.ContainsKey(panelId),$"{panelId} is not a registered panel");

        if(activePanel!=null)
        {
            previousPanel = panelDictionary[activePanel.ID];
            previousPanel.CallListeners(a=>a.OnHide());
            previousPanel.transitionListener.HideTransition(()=>{
                previousPanel.CallListeners(a=>a.OnHideComplete());
            });
        }
        activePanel = (UIPanel)panelDictionary[panelId];
        activePanel.CallListeners(a=>a.OnShow(activePanel,this));
        activePanel.transitionListener.ShowTransition(()=>{
            activePanel.CallListeners(a=>a.OnShowComplete());
        });
    }
}
