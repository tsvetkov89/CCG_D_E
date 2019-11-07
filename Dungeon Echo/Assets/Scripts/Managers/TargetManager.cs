using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetManager : ITargetManager, ISubscriber
{
    private IAnimaManager _animaManager;
    private IActivateCardManager _activateCardManager;
    private IPublisher _publisher;
    private IEnemyManager _enemyManager;
    private ICard _selectedCard;
    
    private DraggableCard _draggableCard;
    private GameObject _targetPointer;
    private DraggeblePointer _draggeblePointer;
    private GameObject _hud;
    private GameObject _targeting;
    private GameObject _targetPlayer;
    private Vector2 _defaultMaxTargetPlayer;
    private Vector2 _defaultMinTargetPlayer;
    private GameObject _targetIconPlayer;
    public TargetManager(IPublisher publisher, IAnimaManager animaManager, IActivateCardManager activateCardManager, IEnemyManager enemyManager)
    {
        _publisher = publisher;
        _animaManager = animaManager;
        _activateCardManager = activateCardManager;
        _enemyManager = enemyManager;
        _targeting = null;
    }
    public void SetDependecies(GameObject targetPointer, GameObject target, GameObject hud)
    {
        _targetPointer = targetPointer;
        _targetPlayer = target;
        
        var rect = _targetPlayer.GetComponent<RectTransform>();
        _defaultMaxTargetPlayer = rect.anchorMax;
        _defaultMinTargetPlayer = rect.anchorMin;
        
        _targetIconPlayer = _targetPlayer.GetComponentsInChildren<Transform>().SearchChild("Target").gameObject;
        _hud = hud;
        _draggeblePointer = _targetPointer.GetComponent<DraggeblePointer>();
        _draggeblePointer.SetDependecies(_publisher);
    }

    public bool PointerActive()
    {
        return _targetPointer.activeSelf;
    }
    public void PointerInit(DraggableCard draggableCard)
    {
        _draggableCard = draggableCard;
        var component = _draggableCard.GetComponent<ActionsWithCards>();
        _selectedCard = component.CardGame;
        _draggeblePointer.Init(_draggableCard, _selectedCard.GetDataCard().Application);
    }

    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.TargetСapture: 
                ActivateCard();
                break;
            case GameEventName.NonTargetСapture:
                _draggableCard.OnEndDrag();
                break;
            case GameEventName.TargetingСapture:
                _targeting = messageData.Value as GameObject;
                _animaManager.SetStateAnimation(_targeting, "target", true);
                break;
            case GameEventName.NonTargetingСapture:
                if (_targeting != null)
                {
                    _animaManager.SetStateAnimation(_targeting, "target", false);
                    _targeting = null;
                }
                break;
            case GameEventName.TargetingPlayer:
                _targeting = messageData.Value as GameObject;
                _targetPlayer.transform.position = _targeting.transform.position;
                _targetPlayer.gameObject.GetComponent<RectTransform>().SetRectByAnchor(_defaultMaxTargetPlayer, _defaultMinTargetPlayer);
                _targetPlayer.SetActive(true);
                //_animaManager.SetStateAnimation(_targetIconPlayer, "target", true);
                break;
            case GameEventName.NonTargetingPlayer:
                //_animaManager.SetStateAnimation(_targetIconPlayer, "target", false);
                _targetPlayer.SetActive(false);
                break;
            case GameEventName.TargetingArea:
                _targeting = messageData.Value as GameObject;
                _targetPlayer.transform.position = _targeting.transform.position;
                _targetPlayer.GetComponent<RectTransform>().SetRectByParent(_targeting.gameObject.GetComponent<RectTransform>());
                _targetPlayer.SetActive(true);
                break;
            case GameEventName.NonTargetingArea:
                _targetPlayer.SetActive(false);
                break;
        }
    }
    private void ActivateCard()
    {
        if (_activateCardManager.PossiblePlayPlayerCard(_selectedCard) &&
            _activateCardManager.IsManaPlayPlayerCard(_selectedCard))
        {
            _draggableCard.ParentToReturnTo = _hud.transform;
            _draggableCard.gameObject.SetActive(false);
            _draggableCard.gameObject.SetActive(true);
            var component = _draggableCard.gameObject.GetComponent<ActionsWithCards>();
            _publisher.Publish(null, new CustomEventArgs(GameEventName.GoActivateCard, _selectedCard));
            _publisher.Publish(null, new CustomEventArgs(GameEventName.GoGetPositionActivateCard, _draggableCard.gameObject));
            _draggableCard.enabled = false;
            component.Animator.enabled = true;

            //_animaManager.SetStateAnimation(_draggableCard.gameObject,"go_hand",false);
            //var ancohors = _draggableCard.gameObject.GetComponent<RectTransform>().SetAnchors();
            //_draggableCard.gameObject.GetComponent<RectTransform>().SetRect(ancohors[0],ancohors[1],ancohors[2],ancohors[3]);
            _animaManager.SetStateAnimation(_draggableCard.gameObject, "dissolve", true);
        }
        _draggableCard.OnEndDrag();
    }
}
