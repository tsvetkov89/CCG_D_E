namespace InterfaceNamespace
{
    public interface IBaseManagers
    {
        ISaveManager SaveManager { get; }
        IAnimaManager AnimaManager { get; }
        IObjectStorage ObjectStorage { get; }
        ICoroutiner Coroutiner { get; }
        IPublisher Publisher { get; }
        IConfigurateManager ConfigurateManager { get; }
        IAudioManager AudioManager { get; }
    }
}