using System.Collections;
using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandlerTokenReward : MonoBehaviour, IPointerDownHandler
{
    private IPublisher _publisher;
    private IToken _token;
    private GameObject _closePanel; 
    private bool _flagClick;
    void Start()
    {
        _flagClick = false;
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {  
        if (!_flagClick)
        {
            Debug.Log("Кликнул по обьекту");
            _publisher.Publish(null, new CustomEventArgs(GameEventName.GoSelectTokenReward, _token));
            _flagClick = true;
        }
        else
        {
            Debug.Log("Отменил клик");
            _publisher.Publish(null, new CustomEventArgs(GameEventName.GoDeActivateTargetsPlayer, _token));
            _flagClick = false;
        }
    }

    public void SetDependecies(IPublisher publisher, IToken token)
    {
        _publisher = publisher;
    }
}
