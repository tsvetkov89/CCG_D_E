using UnityEngine;
using InterfaceNamespace;
using JetBrains.Annotations;

public class BaseManagers : IBaseManagers
{
    public BaseManagers(ISaveManager saveManager, IAnimaManager animaManager,
        IPublisher publisher, IObjectStorage objectStorage, IConfigurateManager configurateManager,
        ICoroutiner coroutiner, IAudioManager audioManager)
    {
        SaveManager = saveManager;
        AnimaManager = animaManager;
        ObjectStorage = objectStorage;
        Publisher = publisher;
        Coroutiner = coroutiner;
        ConfigurateManager = configurateManager;
        AudioManager = audioManager;
    }

    public ISaveManager SaveManager { get; private set; }
    public IAnimaManager AnimaManager { get; private set; }
    public IObjectStorage ObjectStorage { get; private set; }
    public ICoroutiner Coroutiner { get; private set; }
    public IPublisher Publisher { get; private set; }
    public IConfigurateManager ConfigurateManager { get; private set; }
    public IAudioManager AudioManager { get; private set; }
}