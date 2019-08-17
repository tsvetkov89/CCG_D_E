using UnityEngine;
using InterfaceNamespace;
using JetBrains.Annotations;

public class GameManagers : IGameManagers
{
    public GameManagers(IGameManager gameManager, IActivateCardManager activateCardManager, IBarsPlayerManager barsPlayerManager,
        IBarsEnemyManager barsEnemyManager, IEnemyManager enemyManager, IPlayersManager playersManager, IDeckManager deckManager, 
        IInventoryManager inventoryManager, ITargetManager targetManager, ITokenRewardManager tokenRewardManager)
    {
        GameManager = gameManager;
        ActivateCardManager = activateCardManager;
        BarsPlayerManager = barsPlayerManager;
        BarsEnemyManager = barsEnemyManager;
        EnemyManager = enemyManager;
        PlayersManager = playersManager;
        DeckManager = deckManager;
        InventoryManager = inventoryManager;
        TargetManager = targetManager;
        TokenRewardManager = tokenRewardManager;
    }
    public IGameManager GameManager { get; private set; }
    public IActivateCardManager ActivateCardManager { get; private set; }
    public IBarsPlayerManager BarsPlayerManager { get; private set; }
    public IBarsEnemyManager BarsEnemyManager { get; private set; }
    public IEnemyManager EnemyManager { get; private set; }
    public IPlayersManager PlayersManager { get; private set; }
    public IDeckManager DeckManager { get; private set; }
    public IInventoryManager InventoryManager { get; private set; }
    public ITargetManager TargetManager { get; private set; }
    public ITokenRewardManager TokenRewardManager { get; private set; }
}