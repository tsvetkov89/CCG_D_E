using UnityEngine;
using UnityEngine.EventSystems;
using InterfaceNamespace;
public class ClickHandlerMenu : MonoBehaviour, IPointerDownHandler
{
    private bool _flagClick;
    private IAnimaManager _animaManager;

    void Start()
    {
        _flagClick = false;
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {  
        if (!_flagClick)
        {
            _animaManager.SetStateAnimation(transform.parent.gameObject, "open", true);
            _flagClick = true;
        }
    }
    public void SetDependecies(IAnimaManager animaManager)
    {
        _animaManager = animaManager;
    }
}
