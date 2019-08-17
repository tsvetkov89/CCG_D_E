using UnityEngine;
using InterfaceNamespace;
using JetBrains.Annotations;

public class LogicManager : ILogicManager 
{
    public LogicManager(IBaseManagers baseManagers,  IGameManagers gameManagers, IPopupManagers popupManagers)
    {
        BaseManagers = baseManagers;
        GameManagers = gameManagers;
        PopupManagers = popupManagers;
    }
    public IBaseManagers BaseManagers { get; private set; }
    public IGameManagers GameManagers { get; private set; }
    public IPopupManagers PopupManagers { get; private set; }
}