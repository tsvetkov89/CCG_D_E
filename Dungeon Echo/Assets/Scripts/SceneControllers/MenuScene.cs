using UnityEngine;
using UnityEngine.UI;
using DefaultNamespace;
using EnumNamespace;
using InterfaceNamespace;

/// <summary>
/// Сцена, меню игры
/// </summary>
public class MenuScene : BaseScene, ISubscriber
{
    private ISaveManager      _saveManager;
    private IAnimaManager     _animaManager;
    private IPublisher        _publisher;
    private GameObject _buttGameObject;
    private Button _btnContinue;

    void Start ()
    {
        //Find object in scene
        _buttGameObject = GameObject.Find("HUD/btnStart");
        var objButton= GameObject.Find("HUD/btnContinue");
        var component = objButton.GetComponentsInChildren<Transform>().SearchChild("buttonContinue");
        _btnContinue = component.GetComponent<Button>();
    }

    void OnDestroy()
    {
        LoadManager.LogicManager.BaseManagers.Publisher.RemoveSubscriber(this);
    }
    public override void Activate()
    {
        // Set dependencies
        SetDependecies(LoadManager.LogicManager.BaseManagers.SaveManager, LoadManager.LogicManager.BaseManagers.AnimaManager,
            LoadManager.LogicManager.BaseManagers.Publisher);
        var clickHandler = _buttGameObject.GetComponent<ClickHandlerMenu>();
        if (clickHandler == null)
        {
            throw new UnityException("There is no MouseClickHandler script on UI object");
        }
        var  uiButtonsMenu = _buttGameObject.GetComponent<UiButtonsMenu>();
        if (uiButtonsMenu == null)
        {
            throw new UnityException("There is no MouseClickHandler script on UI object");
        }
        clickHandler.SetDependecies(_animaManager);
        uiButtonsMenu.SetDependecies(_saveManager,_animaManager, _publisher);
        if(_saveManager.CheckLoad())
        {
            _btnContinue.interactable = true;
        }
    }
    public void SetDependecies(ISaveManager saveManager, IAnimaManager animaManager, IPublisher publisher)
    {
        _saveManager = saveManager;
        _animaManager = animaManager;
        _publisher = publisher;
        _publisher.AddSubscriber(this);
    }
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        if ( message == GameEventName.GoStageStartNewGame)
        {
            //--------------------Делаем переход в сцену "Игра"
            LoadManager.Navigate(SceneTypeEnum.Menu, SceneTypeEnum.Game, CustomObject.Empty);
        }
    }
}