
using DefaultNamespace;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;

/// <summary>
/// Стартовая сцена, подымает зависимости
/// </summary>

public class SceneBootstrapper :  BaseScene
{
    private IGameManagers _gameManagers;
    private ISaveManager  _saveManager;
    private IGameManager  _gameManager;
    private IInventoryManager _inventoryManager;
    private IAnimaManager _animaManager;
    private ILoadManager  _loadManager;
    private IPublisher    _publisher;
    private IObjectLoader _objectLoader;
    private IObjectStorage _objectStorage;

    private IGameStageManager _gameStageManager;
    private IActivateCardManager _activateCardManager;
    private IBarsPlayerManager _barsPlayerManager;
    private IBarsEnemyManager _barsEnemyManager;
    private IEnemyManager _enemyManager;
    private IAlliesManager _alliesManager;
    private IPlayersManager _playersManager;
    private IDeckManager _deckManager;
    private ITargetManager _targetManager;
    private ICoroutiner _coroutiner;
    private IConfigurateManager _configurateManager;
    private IAudioManager _audioManager;
    private ITokenRewardManager _tokenRewardManager;

    private IBaseManagers  _baseManagers;
    private ILogicManager _logicManager;
    private IPopupManagers _popupManagers;
    private IPopupEvent _popupEvent;
    private IPopupPlayers _popupPlayers;
    private IPopupDescriptionCard _popupDescriptionCard;
    private IPopupPlaceInSlot _popupPlaceInSlot;
    private IPopupRewardEvent _popupRewardEvent;
    private IPopupGameMenu _popupGameMenu;
    private IPopupInventory _popupInventory;
    
    private void Awake()
    {
        var obj = GameObject.Find("CarrierObject");
        if (obj)
            Destroy(obj);
        var dependenciesObject = new GameObject("CarrierObject");
        dependenciesObject.AddComponent<Coroutiner>();
        _coroutiner = dependenciesObject.GetComponent<ICoroutiner>();
    }

    void Start()
   {
       _publisher = new Publisher();

       _saveManager = new SaveManager();
       _animaManager = new AnimaManager();
       _objectLoader = new ObjectLoader();
       _configurateManager = new ConfigurateManager();

       _objectStorage = new ObjectStorage(_objectLoader,_configurateManager);
       _tokenRewardManager = new TokenRewardManager(_publisher, _animaManager, _objectStorage,_configurateManager);
       _audioManager = new AudioManager(_objectStorage, _coroutiner);
       _barsPlayerManager = new BarsPlayerManager(_objectStorage);

       _inventoryManager = new InventoryManager(_objectStorage, _publisher, _coroutiner,_configurateManager);
       
       _gameStageManager = new GameStageManager(_publisher, _coroutiner);
       _gameManager = new GameManager(_publisher, _animaManager, _objectStorage, _coroutiner,_inventoryManager,_configurateManager);
       _enemyManager = new EnemyManager(_publisher, _coroutiner,_animaManager,_objectStorage, _configurateManager);
       _alliesManager = new AlliesManager(_coroutiner, _configurateManager);
       _playersManager = new PlayersManager(_publisher,_animaManager, _tokenRewardManager);
       _barsEnemyManager = new BarsEnemyManager(_enemyManager, _publisher);
       _activateCardManager = new ActivateCardManager(_publisher, _barsPlayerManager,_enemyManager);
       _targetManager = new TargetManager(_publisher, _animaManager, _activateCardManager, _enemyManager);
       _deckManager = new DeckManager(_objectStorage,_inventoryManager,_animaManager,_publisher,_coroutiner, _targetManager,_configurateManager);
       
       _popupEvent = new PopupEvent(_objectStorage,_publisher,_animaManager,_coroutiner, _configurateManager);
       _popupGameMenu = new PopupGameMenu(_objectStorage, _publisher, _animaManager,_coroutiner);
       _popupInventory = new PopupInventory(_inventoryManager, _animaManager);
       _popupPlaceInSlot = new PopupPlaceInSlot(_publisher);
       _popupPlayers = new PopupPlayers(_animaManager, _objectStorage, _publisher, _configurateManager);
       //_descriptionCardPopup = new PopupDescriptionCard(_popupDescriptionCard);
       _popupDescriptionCard = new PopupDescriptionCard(_coroutiner,_animaManager);
       _popupRewardEvent = new PopupRewardEvent(_publisher, _objectStorage, _configurateManager, _coroutiner);
       
       _publisher.AddSubscriber((ISubscriber) _gameManager);
       _publisher.AddSubscriber((ISubscriber) _inventoryManager);
       _publisher.AddSubscriber((ISubscriber) _gameStageManager);
       _publisher.AddSubscriber((ISubscriber) _activateCardManager);
       _publisher.AddSubscriber((ISubscriber) _barsPlayerManager);
       _publisher.AddSubscriber((ISubscriber) _barsEnemyManager);
       _publisher.AddSubscriber((ISubscriber) _enemyManager);
       _publisher.AddSubscriber((ISubscriber) _alliesManager);
       _publisher.AddSubscriber((ISubscriber) _playersManager);
       _publisher.AddSubscriber((ISubscriber) _deckManager);
       _publisher.AddSubscriber((ISubscriber) _targetManager);
       _publisher.AddSubscriber((ISubscriber) _audioManager);
       _publisher.AddSubscriber((ISubscriber) _tokenRewardManager);
       
       _publisher.AddSubscriber((ISubscriber) _popupGameMenu);
       _publisher.AddSubscriber((ISubscriber) _popupInventory);
       _publisher.AddSubscriber((ISubscriber) _popupEvent);
       _publisher.AddSubscriber((ISubscriber) _popupPlayers);
       _publisher.AddSubscriber((ISubscriber) _popupDescriptionCard);
       _publisher.AddSubscriber((ISubscriber) _popupRewardEvent);
       _publisher.AddSubscriber((ISubscriber) _popupPlaceInSlot);
       
       _baseManagers = new BaseManagers(_saveManager,_animaManager,_publisher,_objectStorage,_configurateManager,_coroutiner, _audioManager);
       _gameManagers = new GameManagers(_gameManager, _activateCardManager, _barsPlayerManager, _barsEnemyManager, 
           _enemyManager, _alliesManager,  _playersManager, _deckManager, _inventoryManager, _targetManager, _tokenRewardManager);
       _popupManagers = new PopupManagers(_popupGameMenu,_popupInventory, _popupEvent, _popupPlayers, _popupDescriptionCard,_popupPlaceInSlot, _popupRewardEvent);
       
       
       _logicManager = new LogicManager(_baseManagers,_gameManagers, _popupManagers);
       _loadManager = new LoadManager(_logicManager);
       //-------------------Делаем переход в сцену "Меню"
       _loadManager.Navigate(SceneTypeEnum.Bootstrapper, SceneTypeEnum.Menu, CustomObject.Empty);
   }

    public override void Activate()
    {
       // No implementation
    }
}