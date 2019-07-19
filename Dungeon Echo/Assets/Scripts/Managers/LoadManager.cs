using UnityEngine;
using System;
using System.Collections;
using DefaultNamespace;
using InterfaceNamespace;
using UnityEngine.SceneManagement;
using Object = System.Object;

public class LoadManager : ILoadManager
{
    public ILogicManager LogicManager { get; private set; }
    private AsyncOperation _loadOperation; 
    public LoadManager(ILogicManager logicManager)
    {
        LogicManager = logicManager;
    }

    public void Navigate(SceneTypeEnum sceneTypeFrom, SceneTypeEnum sceneTypeTo, CustomObject customObject)
    {
        var sceneName = Strings.GetScenePath(sceneTypeTo);
        // Load Scene
        _loadOperation = SceneManager.LoadSceneAsync(sceneName);
        _loadOperation.allowSceneActivation = true;
        LogicManager.BaseManagers.Coroutiner.StartCoroutine(WaitSceneLoading(sceneTypeTo, customObject,
            () => OnChanged(EventTypeEnum.NavigationEnd, sceneTypeTo)));
    }
    private IEnumerator WaitSceneLoading(SceneTypeEnum sceneTypeTo, CustomObject customObject, Action action)
    {
        yield return new WaitForEndOfFrame();
        var isLoaded = _loadOperation.isDone;
        while (isLoaded == false)
        {
            yield return new WaitForEndOfFrame();
            isLoaded = _loadOperation.isDone;
        }

        var sceneAttachedScripts = UnityEngine.Object.FindObjectOfType<BaseScene>();
        IScene sceneTo = sceneAttachedScripts;
        sceneTo.SetDependencies(sceneTypeTo, this);

        sceneTo.Activate();
        action.Invoke();                                       
    }
   public event EventHandler<EventArgsGeneric<Object>> Changed;
   private void OnChanged(EventTypeEnum eventType, object someObject)
   {
        var handler = Changed;
        if(handler != null)
        {
            var args = new EventArgsGeneric<Object>(eventType, someObject);
            handler(this, args);
        }
    }
}

