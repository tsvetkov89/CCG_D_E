using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using System.Collections;
using System.Linq;

public class DeckManager : IDeckManager, ISubscriber
{
    private IInventoryManager _inventoryManager;
    private IObjectStorage _objectStorage;
    private IAnimaManager _animaManager;
    private IPublisher _publisher;
    private ICoroutiner _coroutiner;

    private int _maxCardsInHand;
    private int _currentCardsInHand;
    private int _currentCardsInDiscard;
    private List<string> _currentDeck;
    private List<string> _discardCards;
    private GameObject _hud;  //игровой HUD
    private GameObject _hand; //область руки игрока
    private GameObject _pointStopDrag;
    private GameObject _pointReturnCard;
    private List<GameObject> _poolCardsPlayer; //колода карт 
    public DeckManager(IObjectStorage objectStorage, IInventoryManager inventoryManager, IAnimaManager animaManager, 
        IPublisher publisher, ICoroutiner coroutiner)
    {
        _objectStorage = objectStorage;
        _inventoryManager = inventoryManager;
        _animaManager = animaManager;
        _publisher = publisher;
        _coroutiner = coroutiner;
        _maxCardsInHand = 6;
        _currentCardsInHand = 0;
        _currentCardsInDiscard = 0;
        _discardCards = new List<string>();
    }
    //----------------- events
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoSelectCardEnemy:
            {
                _coroutiner.StartCoroutine(GetCardsInHand());
                break;
            }
            case GameEventName.GoEndTurnPlayer:
                foreach (var card in _poolCardsPlayer)
                {
                    var component = card.GetComponent<DraggableCard>();
                    component.enabled = false;
                    if (_currentDeck.Count != 0) continue;
                    _currentDeck =  _discardCards.GetRange(0, _discardCards.Count);
                    _discardCards.Clear();
                }
                break;
            case GameEventName.GoEndTurnEnemy:
                _coroutiner.StartCoroutine(ActivateDraggableCard(1.2f));
                Debug.Log("card in hand = "+ _currentCardsInHand);
                if(_currentCardsInHand <=4)
                    _coroutiner.StartCoroutine(GetCardsInHand(2,0.5f));
                else if ((6 - _currentCardsInHand)> 0)
                    _coroutiner.StartCoroutine(GetCardsInHand(1,0.5f));
                break;
            case GameEventName.GoPlayerTurn:
                _coroutiner.StartCoroutine(ActivateDraggableCard(0.6f));
                break;  
            case GameEventName.ActivateCardPlayer:
                var activateCard = messageData.Value as ICard;
                _discardCards.Add(activateCard.GetDataCard().NameCard);
                _currentCardsInHand--;
                break;
            case GameEventName.GoFinishBattle:
                FinishBattle();
                break;
        }
    }

    private void FinishBattle()
    {
        foreach (var cardPlayer in _poolCardsPlayer)
        {
            var draggable = cardPlayer.GetComponent<DraggableCard>();
            draggable.enabled = false;
            if (!cardPlayer.activeSelf) continue;
            var component = cardPlayer.GetComponent<ActionsWithCards>();
            component.Animator.enabled = true;
            _objectStorage.ConfigurateByParent(cardPlayer, _hud, false);
        }
        _currentCardsInHand = 0;
        _discardCards.Clear();
    }
    public void SetUiComponents(GameObject hud, GameObject pointStopDrag, GameObject pointReturnCard)
    {
        _hud = hud;
        var transform = _hud.GetComponentsInChildren<Transform>().SearchChild("PanelHandPlayer");
        _hand = transform.gameObject;
        _pointStopDrag = pointStopDrag;
        _pointReturnCard = pointReturnCard; 
    }
    public void PlaceObjects()
    {
        _poolCardsPlayer = _objectStorage.GetPollObjects(ObjectTypeEnum.PrefabCard, 9);
        foreach (var card in _poolCardsPlayer)
        {
            _objectStorage.Configurate(card, _hud, false,0.034f,0.06f,0.194f,0.46f);
            card.gameObject.AddComponent<DraggableCard>();
            var component = card.GetComponent<DraggableCard>();
            component.enabled = false;
        }
    }

    private IEnumerator ActivateDraggableCard(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var card in _poolCardsPlayer)
        {
            var component = card.GetComponent<DraggableCard>();
            component.enabled = true;
        }
    }

    private IEnumerator GetCardsInHand()
    {
        _currentDeck = _inventoryManager.GetCardInDeck();
        _currentDeck = RandomExtensions.Shuffle(_currentDeck);
        yield return new WaitForSeconds(2f);
        _currentCardsInHand = 0;
        var list =  _currentDeck.GetRange(0, _currentDeck.Count);
        foreach (var card in list)
        {
            foreach (var cardPlayer in _poolCardsPlayer)
            {
                if (cardPlayer.activeSelf) continue;
                foreach (Transform child in cardPlayer.transform)
                    child.gameObject.SetActive( true);
                var cardDisplay = cardPlayer.GetComponent<ActionsWithCards>();
                var cardByName =  _objectStorage.GetCardByName(card);
                cardDisplay.СhangeCardType(cardByName);
                cardDisplay.SetIdCard(_currentCardsInHand);
                cardDisplay.CardGame.DisplayCardInGame(cardPlayer);
                cardDisplay.SetDependecies(_publisher, _animaManager);
                cardDisplay.enabled = false;
                var component = cardPlayer.GetComponent<DraggableCard>();
                component.SetDependecies( _pointStopDrag, _pointReturnCard);
                cardPlayer.SetActive(true);
                _animaManager.SetStateAnimation(cardPlayer, "go_hand",true);
                _currentCardsInHand++;
                _coroutiner.StartCoroutine(SwithParentCard(cardPlayer));
                _currentDeck.Remove(card);
                yield return new WaitForSeconds(0.2f);
                break;
            }
            if (_currentCardsInHand == _maxCardsInHand) break;
        }
    }

    private IEnumerator GetCardsInHand(int count, float time)
    {
        yield return new WaitForSeconds(time);
        var list = _currentDeck.GetRange(0, _currentDeck.Count);
        var counter = 0;
        Debug.Log("list = " + list.Count);
        foreach (var card in list)
        {
            foreach (var cardPlayer in _poolCardsPlayer)
            {
                if (cardPlayer.activeSelf) continue;
                foreach (Transform child in cardPlayer.transform)
                    child.gameObject.SetActive(true);
                var cardDisplay = cardPlayer.GetComponent<ActionsWithCards>();
                var cardByName = _objectStorage.GetCardByName(card);
                cardDisplay.СhangeCardType(cardByName);
                cardDisplay.SetIdCard(_currentCardsInHand);
                cardDisplay.CardGame.DisplayCardInGame(cardPlayer);
                cardDisplay.SetDependecies(_publisher, _animaManager);
                cardDisplay.enabled = false;
                cardPlayer.SetActive(true);
                _animaManager.SetStateAnimation(cardPlayer, "go_hand", true);
                _currentCardsInHand++;
                counter++;
                _coroutiner.StartCoroutine(SwithParentCard(cardPlayer));
                _currentDeck.Remove(card);
                yield return new WaitForSeconds(0.2f);
                break;
            }

            if (counter == count) break;
        }
        if (counter == count) yield break;
        Debug.Log("Недодал");
        _currentDeck =  _discardCards.GetRange(0, _discardCards.Count);
        _discardCards.Clear();
        _coroutiner.StartCoroutine(GetCardsInHand(count - counter,0.0f));
    }
    private IEnumerator SwithParentCard(GameObject card)
    {
        yield return new WaitForSeconds(0.6f);
        _objectStorage.ConfigurateByParent(card,_hand,0.5f, 0.5f, 0.5f, 0.5f);
        card.GetComponent<CanvasGroup>().blocksRaycasts = true;     
    }
}