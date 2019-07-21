namespace InterfaceNamespace
{
    public interface IGameManagers
    {
        IGameManager  GameManager { get; }
        IActivateCardManager ActivateCardManager { get; }
        IBarsPlayerManager BarsPlayerManager { get; }
        IBarsEnemyManager BarsEnemyManager { get; }
        IEnemyManager EnemyManager { get; }
        IInventoryManager InventoryManager { get; }
        IDeckManager DeckManager { get; }
        ITargetManager TargetManager { get; }
    }
}