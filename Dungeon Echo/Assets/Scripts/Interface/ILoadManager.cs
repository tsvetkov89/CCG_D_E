using DefaultNamespace;

namespace InterfaceNamespace
{
    public interface ILoadManager
    {
        void Navigate(SceneTypeEnum sceneTypeFrom, SceneTypeEnum sceneTypeTo, CustomObject customObject);
        ILogicManager LogicManager { get; }
    }
}