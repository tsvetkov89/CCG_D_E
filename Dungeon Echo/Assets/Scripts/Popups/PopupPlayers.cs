using System.Collections;
using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using UnityEngine.UI;

public class PopupPlayers: IPopupPlayers, ISubscriber
{
    private IAnimaManager _animaManager;
    private IObjectStorage _objectStorage;
    private IConfigurateManager _configurateManager;
    private IPublisher _publisher;
    
    private GameObject _popupPlayers;                   
    private List<GameObject> _arrayPlayers;
    private IDictionary<GameClass, List<float>> _dictionaryPoints;
    private bool _flagOpenInventory;
    
    public PopupPlayers(IAnimaManager animaManager, IObjectStorage objectStorage, IPublisher publisher, IConfigurateManager configurateManager)
    {
        _animaManager = animaManager;
        _objectStorage = objectStorage;
        _publisher = publisher;
        _configurateManager = configurateManager;

        _dictionaryPoints = new Dictionary<GameClass, List<float>>();
        //points = xmin, ymax, ymin, ymax
        _dictionaryPoints[GameClass.Red] = new List<float>(){0.124f, 0.888f,0.761f,0.985f};
        _dictionaryPoints[GameClass.Green] = new List<float>(){0.124f, 0.888f,0.511f,0.744f};
        _dictionaryPoints[GameClass.Blue] = new List<float>(){0.124f, 0.888f,0.259f,0.492f};
        _dictionaryPoints[GameClass.Purple] = new List<float>(){0.124f, 0.888f,0.0154f,0.2405f};

    }

    public void SetDependecies(GameObject popupPlayers)
    {
        _popupPlayers = popupPlayers;
        _flagOpenInventory = false;
        CreatePanelPlayers();
    }
    
    //---------------действия на ивенты
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoOpenInventory:
                foreach (var obj in _arrayPlayers)
                {
                    var componentObj = obj.GetComponent<ActionsWithCardGameClass>();
                    componentObj.enabled = true;
                }
                break;
            case GameEventName.GoCloseInventory:
                break;
            case GameEventName.GoOpenPanelPlayers:
                foreach (var obj in _arrayPlayers)
                {
                    var componentObj = obj.GetComponent<ActionsWithCardGameClass>();
                    componentObj.enabled = false;
                }
                break;
        }
    }
    private void CreatePanelPlayers()
    {   
       
        var holder = _popupPlayers.GetComponentsInChildren<Transform>().SearchChild("PopupPlayers");   
        var holderPlayers = holder.gameObject; //найдем панель под иконки игроков
        //----------------------------
        _arrayPlayers =_objectStorage.GetPollObjects(ObjectTypeEnum.PrefabIconPlayer,4);
        var players = _objectStorage.GetDictionaryCards(SubTypeCard.GameClass);
        var playersCards = new List<ICard>(players.Values);
        var count = 0;
        foreach (var obj in _arrayPlayers)
        {
            var gameclass = playersCards[count].GetDataCard().GameClass;
            var points = _dictionaryPoints[gameclass];
            _configurateManager.Configurate(obj, holderPlayers,true,
                points[0],points[1],points[2],points[3]);
            var componentObj = obj.GetComponent<ActionsWithCardGameClass>();
            componentObj.СhangeCardType(playersCards[count]);
            var icon = obj.GetComponentsInChildren<Transform>().SearchChild("Icon");
            icon.GetComponent<Image>().sprite = componentObj.CardGame.GetDataCard().Art;
            componentObj.SetDependecies(_publisher);
            if (gameclass == GameClass.Red)
                componentObj.SetSelected();
            
            var rect = obj.GetComponent<RectTransform>();
            obj.GetComponent<BoxCollider2D>().SetSizeBox2D(rect,10f,10f);

            _publisher.AddSubscriber(componentObj); 
            _publisher.Publish(null,new CustomEventArgs(GameEventName.SpawnPlayer, obj));
            count++;
        }
    }
    public List<GameObject> GetArrayPlayers()
    {
        return _arrayPlayers;
    }

}