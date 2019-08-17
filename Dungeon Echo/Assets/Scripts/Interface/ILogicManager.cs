namespace InterfaceNamespace
{
    public interface ILogicManager
    {
        IBaseManagers BaseManagers { get; }
        IGameManagers  GameManagers { get; }
        IPopupManagers  PopupManagers { get; }
    }
}