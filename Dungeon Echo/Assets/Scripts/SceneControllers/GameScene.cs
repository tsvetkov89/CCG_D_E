using System.Collections;
using EnumNamespace;
using UnityEngine;
using InterfaceNamespace;
using TMPro;

/// <summary>
/// Игровая сцена
/// </summary>
public class GameScene : BaseScene, ISubscriber
{
    private ISaveManager  _saveManager;
    private IAnimaManager _animaManager;
    private IPublisher    _publisher;
    private IObjectStorage _objectStorage;
    private IGameManager  _gameManager;
    private ICoroutiner _coroutiner;
    private IDeckManager _deckManager;
    private ITargetManager _targetManager;
    private IEnemyManager _enemyManager;
    private IConfigurateManager _configurateManager;
    private IAudioManager _audioManager;
    private IPopupManagers _popupManagers;
    
    private GameObject _hud;
    private GameObject _poolParent;
    private GameObject _popupInventory;
    private GameObject _popupPlaceInSlot;            //вспылывающее окно, просмотр карты в инвентаре
    private GameObject _popupDescriptionCard;        //вспылывающее окно, просмотр карты при выборе
    private GameObject _popupPlayers;
    private GameObject _popupEvent;
    private GameObject _popupFade;
    private GameObject _popupGameMenu;
    private GameObject _btnEscapeBattle;
    private GameObject _btnEndTurn;
    private GameObject _btnMenu;
    private GameObject _panelEnemy;
    private GameObject _pointStopDrag;
    private GameObject _pointReturnCard;
    private GameObject _targetPointer;
    private GameObject _defaultCard;
    private GameObject _targetPlayer;
    private GameObject _soundGameObject;
    private GameObject _popupRewardEvent;
    private GameObject _deckInGame;
    
    private PopupPlaceInSlot _placeInSlotPopup;
    
    private GameObject _btnGameObject;
    private GameObject _btnContinuePopupReward;
    private GameObject _popupText;
    private TextMeshProUGUI _messageText;
    private Membership _membership;
    private void Awake()
    {
        _soundGameObject = GameObject.Find("SoundGameObject");
    }

    private void Start()
    {
        _hud  = GameObject.Find("HUD");
        _btnGameObject = GameObject.Find("HUD/PopupInventory/btnClosePanelInventory");
        _btnContinuePopupReward = GameObject.Find("HUD/PopupRewardEvent/btnContinue");
        _deckInGame  = GameObject.Find("HUD/DeckInGame");
        _popupInventory = GameObject.Find("HUD/PopupInventory");
        _popupPlaceInSlot = GameObject.Find("HUD/PopupPlaceInSlot");
        _popupDescriptionCard = GameObject.Find("HUD/PopupDescriptionCard");
        _popupPlayers = GameObject.Find("HUD/PopupPlayers");
        _popupEvent = GameObject.Find("HUD/PopupEvent");
        _popupFade = GameObject.Find("HUD/PopupFade");
        _popupGameMenu  = GameObject.Find("HUD/PopupGameMenu");
        _btnEscapeBattle = GameObject.Find("HUD/btnEscapeBattle");
        _btnEndTurn = GameObject.Find("HUD/btnEndTurn");
        _btnMenu = GameObject.Find("HUD/btnMenu");
        _poolParent= GameObject.Find("PoolObjects");
        _panelEnemy = GameObject.Find("HUD/PanelEnemy");
        _popupText = GameObject.Find("HUD/PopupText");
        _pointStopDrag = GameObject.Find("HUD/PointStopDragCard");
        _pointReturnCard = GameObject.Find("HUD/PointReturnCard");
        _targetPointer = GameObject.Find("HUD/TargetPointer");
        _defaultCard  = GameObject.Find("HUD/prefabCard");
        _targetPlayer = GameObject.Find("HUD/TargetingPlayer");
        _popupRewardEvent = GameObject.Find("HUD/PopupRewardEvent");
            
        var child = _popupText.GetComponentsInChildren<Transform>().SearchChild("Message"); 
        _messageText = child.GetComponent<TextMeshProUGUI>();

        _popupPlaceInSlot.GetComponent<RectTransform>().SetOffset(0,0,0,0);
        _popupInventory.GetComponent<RectTransform>().SetOffset(0,0,0,0);
        _popupDescriptionCard.GetComponent<RectTransform>().SetOffset(0,0,0,0);
        _popupPlayers.GetComponent<RectTransform>().SetOffset(0,0,0,0);
        _popupEvent.GetComponent<RectTransform>().SetOffset(0,0,0,0);
        _popupFade.GetComponent<RectTransform>().SetOffset(0,0,0,0);
        _popupGameMenu.GetComponent<RectTransform>().SetOffset(0,0,0,0);
        _popupRewardEvent.GetComponent<RectTransform>().SetOffset(0,0,0,0);
        
        _popupDescriptionCard.SetActive(false);
        _popupInventory.SetActive(false); 
        _popupPlaceInSlot.SetActive(false);
        _popupEvent.SetActive(false);
        _popupFade.SetActive(false);
        _popupGameMenu.SetActive(false);
        _btnEscapeBattle.SetActive(false);
        _btnEndTurn.SetActive(false);
        _popupRewardEvent.SetActive(false);
            
        _targetPointer.SetActive(false);
        _targetPlayer.SetActive(false);
        
        _membership = Membership.Undefined;
    }
    public override void Activate()
    {
        SetDependecies(LoadManager.LogicManager);
        if (_saveManager.CheckLoad())
        {
            //TODO Continue game
            Debug.Log("Continue");
        }
        else
        {   //-------------стадия начало игры
            CreateDependecies();
            // TODO пофиксить
            var gameclass = _objectStorage.GetCardByName("Purple") as CardsGameClass;
            _publisher.Publish(this, new CustomEventArgs(GameEventName.GoSaveGameClass,gameclass));
        }
    }
    private void CreateDependecies()
    {
        var uiButtonsPopup = _btnGameObject.GetComponent<UiButtonsPopups>();
        uiButtonsPopup.SetDependecies(_publisher);
        _audioManager.SetDependecies(_soundGameObject);
        _publisher.Publish(this,new CustomEventArgs(GameEventName.GoStageStartGame));
        _configurateManager.SetDependecies(_poolParent,_defaultCard);
        _objectStorage.CreateAllPools();
        
        _gameManager.SetUiComponents(_hud, _deckInGame);
        _gameManager.PlaceObjects();
        _deckManager.SetUiComponents(_hud, _pointStopDrag, _pointReturnCard);
        _deckManager.PlaceObjects();
        _enemyManager.SetDependecies(_panelEnemy);
              
        
        uiButtonsPopup = _btnEndTurn.GetComponent<UiButtonsPopups>();
        uiButtonsPopup.SetDependecies(_publisher);
        _publisher.AddSubscriber(uiButtonsPopup);
        uiButtonsPopup = _btnMenu.GetComponent<UiButtonsPopups>();
        uiButtonsPopup.SetDependecies(_publisher);
        _publisher.AddSubscriber(uiButtonsPopup);
        
        var cardPlaceInSlot = _objectStorage.GetPollObjects(ObjectTypeEnum.PrefabCard,1)[0];
        cardPlaceInSlot.transform.SetParent(_popupPlaceInSlot.transform);
        cardPlaceInSlot.SetActive(true);
        cardPlaceInSlot.GetComponent<RectTransform>().SetRect(0.0614f,0.0575f,0.412f,0.9422f);
        cardPlaceInSlot.GetComponent<RectTransform>().SetOffset(0,0,0,0);

        _popupDescriptionCard.AddComponent<ClickHandlerPopupCard>();
        var component = _popupDescriptionCard.GetComponent<ClickHandlerPopupCard>();
        component.SetDependecies(_publisher);
      
        _targetManager.SetDependecies(_targetPointer,_targetPlayer, _hud);
        _popupManagers.PopupGameMenu.SetDependecies(_popupGameMenu);
        _popupManagers.PopupInventory.SetDependecies(_popupInventory); 
        _popupManagers.PopupEvent.SetDependecies(_popupEvent);
        _popupManagers.PopupPlayers.SetDependecies(_popupPlayers);
        _popupManagers.PopupInventory.SearchAndSetHolders(_popupManagers.PopupPlayers.GetArrayPlayers());
        _publisher.Publish(this,
            new CustomEventArgs(GameEventName.GoCreateBarsPlayers, _popupManagers.PopupPlayers.GetArrayPlayers()));

        _popupManagers.PopupDescriptionCard.SetDependecies(_popupDescriptionCard);
        _popupManagers.PopupPlaceInSlot.SetDependecies(_popupPlaceInSlot, cardPlaceInSlot);
        _popupManagers.PopupRewardEvent.SetDependecies(_popupRewardEvent,_btnContinuePopupReward);

    }
    private void SetDependecies(ILogicManager logicManager)
    {
        _popupManagers = logicManager.PopupManagers;

        _saveManager = logicManager.BaseManagers.SaveManager;
        _animaManager = logicManager.BaseManagers.AnimaManager;
        _objectStorage = logicManager.BaseManagers.ObjectStorage;
        _publisher = logicManager.BaseManagers.Publisher;
        _gameManager = logicManager.GameManagers.GameManager;
        _coroutiner = logicManager.BaseManagers.Coroutiner;
        _deckManager = logicManager.GameManagers.DeckManager;
        _targetManager = logicManager.GameManagers.TargetManager;
        _enemyManager = logicManager.GameManagers.EnemyManager;
        _configurateManager = logicManager.BaseManagers.ConfigurateManager;
        _audioManager = logicManager.BaseManagers.AudioManager;
        _publisher.AddSubscriber(this);   
    }

    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoStageBattle:
                _coroutiner.StartCoroutine(GoStageBattle());
                break;
            case GameEventName.GoFinishBattle:
                _membership = Membership.Undefined;
                _animaManager.SetStateAnimation(_hud, "battle", false);
                _btnEscapeBattle.SetActive(false);
                _btnEndTurn.SetActive(false);
                break;
            case GameEventName.GoSaveGameClass:
                _saveManager.SaveClass(messageData.Value as CardsGameClass);
                break;
            case GameEventName.GoNextTurn:
            {
                if (_membership == Membership.Undefined)
                {
                    _membership = Membership.Player;
                }
                else
                {
                    _membership = _membership == Membership.Player ? Membership.Enemy : Membership.Player;
                    _messageText.text = _membership == Membership.Player ? "Your turn" : "Enemy turn";
                    _popupText.transform.SetAsLastSibling();
                    _animaManager.SetStateAnimation(_popupText, "startturn", true);
                    _coroutiner.StartCoroutine(GoClosePopupText());
                } 
                return;
            }
            case GameEventName.GoStageRandomEvent:
                Debug.Log("GoStageRandomEvent");
                _popupFade.transform.SetAsLastSibling();
                _popupFade.SetActive(true);
                _animaManager.SetStateAnimation(_hud, "faiding", true);
                break;
            case GameEventName.GoRewardEvent:
                _popupFade.transform.SetAsLastSibling();
                _popupFade.SetActive(true);
                _animaManager.SetStateAnimation(_hud, "faiding", false);
                _coroutiner.StartCoroutine(GoClosePopupFade(1f));
                break;
        }
    }
    public void OnApplicationQuit()                //Сохраняем при выходе из игры на PC
    {
        Debug.Log("Quit");
        _saveManager.SaveGame();
    }
    public void OnApplicationPause(bool pause)     //Сохраняем при выходе из игры на Android
    {
        if (pause)  _saveManager.SaveGame();
    }
    private IEnumerator GoStageBattle()
    {
        yield return new WaitForSeconds(0.3f);
        _animaManager.SetStateAnimation(_hud, "battle", true);
        _btnEscapeBattle.SetActive(true);
        _btnEndTurn.SetActive(true);
        _popupText.transform.SetAsLastSibling();
        yield return new WaitForSeconds(1.7f);
        _messageText.text = _membership == Membership.Player ? "Your turm" : "Enemy turm";
        _animaManager.SetStateAnimation(_popupText, "startturn", true);
        yield return new WaitForSeconds(1.1f);
        _publisher.Publish(this, new CustomEventArgs(GameEventName.GoPlayerTurn));
        _animaManager.SetStateAnimation(_popupText, "startturn", false);
    }
    private IEnumerator GoClosePopupText()
    {
        yield return new WaitForSeconds(0.6f);
        _animaManager.SetStateAnimation(_popupText, "startturn", false);
    }
    private IEnumerator GoClosePopupFade(float pause)
    {
        yield return new WaitForSeconds(pause);
       
        _popupFade.SetActive(false);
    }
}
