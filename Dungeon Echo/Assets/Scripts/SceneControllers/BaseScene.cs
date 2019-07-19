
using DefaultNamespace;
using InterfaceNamespace;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour, IScene
{
   protected ILoadManager LoadManager;
   public void SetDependencies(SceneTypeEnum sceneType, ILoadManager loadManager)
   {
      LoadManager = loadManager;
      SceneType = sceneType;
   }
   private SceneTypeEnum SceneType { get; set; }
   public abstract void Activate();
}
