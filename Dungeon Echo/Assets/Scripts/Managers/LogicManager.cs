using UnityEngine;
using InterfaceNamespace;
using JetBrains.Annotations;

public class LogicManager : ILogicManager 
{
    public LogicManager(IBaseManagers baseManagers,  IGameManagers gameManagers)
    {
        BaseManagers = baseManagers;
        GameManagers = gameManagers;
    }
    public IBaseManagers BaseManagers { get; private set; }
    public IGameManagers GameManagers { get; private set; }
}