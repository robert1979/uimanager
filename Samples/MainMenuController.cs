using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour, UIChangeListener
{
    private UIManager uiManager;
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

    public void OnSettingsPressed()
    {
        uiManager.ShowPanel("Settings");
    }
}
