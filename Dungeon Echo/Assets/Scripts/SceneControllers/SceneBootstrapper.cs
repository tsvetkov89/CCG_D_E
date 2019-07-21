
using DefaultNamespace;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;

/// <summary>
/// Стартовая сцена, подымает зависимости
/// </summary>

public class SceneBootstrapper :  BaseScene
{
    private IBaseManagers  _baseManagers;
    private IGameManagers _gameManagers;
    private ISaveManager  _saveManager;
    private IGameManager  _gameManager;
    private IInventoryManager _inventoryManager;
    private IAnimaManager _animaManager;
    private ILoadManager  _loadManager;
    private IPublisher    _publisher;
    private IObjectLoader _objectLoader;
    private IObjectStorage _objectStorage;
    private ILogicManager _logicManager;
    private IGameStageManager _gameStageManager;
    private IActivateCardManager _activateCardManager;
    private IBarsPlayerManager _barsPlayerManager;
    private IBarsEnemyManager _barsEnemyManager;
    private IEnemyManager _enemyManager;
    private IPlayersManager _playersManager;
    private IDeckManager _deckManager;
    private ITargetManager _targetManager;
    private ICoroutiner _coroutiner;

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
       _targetManager = new TargetManager();
      
       _objectStorage = new ObjectStorage(_objectLoader);
       _barsPlayerManager = new BarsPlayerManager(_objectStorage);

       _inventoryManager = new InventoryManager(_objectStorage, _publisher, _coroutiner);
       
       _gameStageManager = new GameStageManager(_publisher);
       _gameManager = new GameManager(_publisher, _animaManager, _objectStorage, _coroutiner,_inventoryManager);
       _deckManager = new DeckManager(_objectStorage,_inventoryManager,_animaManager,_publisher,_coroutiner);
       _enemyManager = new EnemyManager(_publisher, _coroutiner,_animaManager);
       _playersManager = new PlayersManager(_publisher);
       _barsEnemyManager = new BarsEnemyManager(_enemyManager, _publisher);
       _activateCardManager = new ActivateCardManager(_publisher, _barsPlayerManager,_enemyManager);
       
       _publisher.AddSubscriber((ISubscriber) _gameManager);
       _publisher.AddSubscriber((ISubscriber) _inventoryManager);
       _publisher.AddSubscriber((ISubscriber) _gameStageManager);
       _publisher.AddSubscriber((ISubscriber) _activateCardManager);
       _publisher.AddSubscriber((ISubscriber) _barsPlayerManager);
       _publisher.AddSubscriber((ISubscriber) _barsEnemyManager);
       _publisher.AddSubscriber((ISubscriber) _enemyManager);
       _publisher.AddSubscriber((ISubscriber) _playersManager);
       _publisher.AddSubscriber((ISubscriber) _deckManager);
       
       _baseManagers = new BaseManagers(_saveManager,_animaManager,_publisher,_objectStorage,_coroutiner);
       _gameManagers = new GameManagers(_gameManager, _activateCardManager, _barsPlayerManager, _barsEnemyManager, 
           _enemyManager, _playersManager, _deckManager, _inventoryManager, _targetManager);
       _logicManager = new LogicManager(_baseManagers,_gameManagers);
       _loadManager = new LoadManager(_logicManager);
       //-------------------Делаем переход в сцену "Меню"
       _loadManager.Navigate(SceneTypeEnum.Bootstrapper, SceneTypeEnum.Menu, CustomObject.Empty);
   }

    public override void Activate()
    {
       // No implementation
    }
}