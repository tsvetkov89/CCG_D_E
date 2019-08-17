using System.Collections;
using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupGameMenu : IPopupGameMenu, ISubscriber
{
    private IObjectStorage _objectStorage;
    private IPublisher _publisher;
    private IAnimaManager _animaManager;
    private ICoroutiner _coroutiner;
    
    private GameObject _popupMenu;
    private GameObject _btnCloseGameMenu;
    
    public PopupGameMenu( IObjectStorage objectStorage, IPublisher publisher, IAnimaManager animaManager,
        ICoroutiner coroutiner)
    {
        _objectStorage = objectStorage;
        _publisher = publisher;
        _animaManager = animaManager;
        _coroutiner = coroutiner;
    }
    public void SetDependecies(GameObject popupMenu)
    {
        _popupMenu = popupMenu;
        _btnCloseGameMenu= _popupMenu.GetComponentsInChildren<Transform>().SearchChild("btnClose").gameObject;
        var uiButtonsPopup = _btnCloseGameMenu.GetComponent<UiButtonsPopups>();
        uiButtonsPopup.SetDependecies(_publisher);   
    }
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoOpenGameMenu:
                _popupMenu.transform.SetAsLastSibling();
                _popupMenu.SetActive(true);
                break;
            case GameEventName.GoCloseGameMenu:
                _popupMenu.SetActive(false);
                break;
        }  
    }
}