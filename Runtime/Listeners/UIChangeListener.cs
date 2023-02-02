public interface UIChangeListener
{
    void OnShow(UIRectEntity entity,UIManager manager);
    void OnHide();

    void OnShowComplete();
    void OnHideComplete();
}