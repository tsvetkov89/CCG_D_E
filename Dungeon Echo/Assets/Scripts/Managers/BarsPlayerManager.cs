using System.Collections;
using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarsPlayerManager : IBarsPlayerManager, ISubscriber
{
    private IObjectStorage _objectStorage;
    private IDictionary<GameClass, Image> _hpPlayersImg;
    private IDictionary<GameClass, Image> _mpPlayersImg;
    private IDictionary<GameClass, TextMeshProUGUI> _hpPlayersText;
    private IDictionary<GameClass, TextMeshProUGUI> _mpPlayersText;
    private IDictionary<GameClass, ICard> _cardsPlayers;
    private IDictionary<GameClass, List<int>> _curAndMaxHpPlayers;
    private IDictionary<GameClass, List<int>> _curAndMaxMpPlayers;
    public BarsPlayerManager (IObjectStorage objectStorage)
    {
        _objectStorage = objectStorage;
        _hpPlayersImg = new Dictionary<GameClass, Image>();
        _mpPlayersImg = new Dictionary<GameClass, Image>();
        _hpPlayersText = new Dictionary<GameClass, TextMeshProUGUI>();
        _mpPlayersText = new Dictionary<GameClass, TextMeshProUGUI>();
        _cardsPlayers = new Dictionary<GameClass, ICard>();
        _curAndMaxHpPlayers = new Dictionary<GameClass, List<int>>();
        _curAndMaxMpPlayers = new Dictionary<GameClass, List<int>>();
    }

    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoCreateBarsPlayers:
            {
                var arrayPlayers = messageData.Value as List<GameObject>;
                foreach (var player in arrayPlayers)
                {
                    var componentPlayer = player.GetComponent<ActionsWithCardGameClass>();
                    var gameclass = componentPlayer.CardGame.GetDataCard().GameClass;

                    var obj = player.GetComponentsInChildren<Transform>().SearchChild("Health");
                    var child = obj.GetComponentsInChildren<Transform>().SearchChild("Bar");
                    var componentChild = child.GetComponent<Image>();
                    _hpPlayersImg[gameclass] = componentChild;
                    child = obj.GetComponentsInChildren<Transform>().SearchChild("NumberValue");
                    var componentText = child.GetComponent<TextMeshProUGUI>();
                    _hpPlayersText[gameclass] = componentText;

                    obj = player.GetComponentsInChildren<Transform>().SearchChild("Mana");
                    child = obj.GetComponentsInChildren<Transform>().SearchChild("Bar");
                    componentChild = child.GetComponent<Image>();
                    _mpPlayersImg[gameclass] = componentChild;
                    child = obj.GetComponentsInChildren<Transform>().SearchChild("NumberValue");
                    componentText = child.GetComponent<TextMeshProUGUI>();
                    _mpPlayersText[gameclass] = componentText;

                }

                InitDictPlayers();
                break;
            }
            case GameEventName.GoActivateCard:
            {
                var card = messageData.Value as ICard;
                var gameclass = card.GetDataCard().GameClass;
                var attribute = card.GetDataCard().CostAttribute;
                var mp = attribute.value;
                if (_curAndMaxMpPlayers[gameclass][0] - mp < 0)
                    _curAndMaxMpPlayers[gameclass][0] = 0;
                else
                    _curAndMaxMpPlayers[gameclass][0] =_curAndMaxMpPlayers[gameclass][0] - mp;
                var f = _curAndMaxMpPlayers[gameclass][0] / (float)_curAndMaxMpPlayers[gameclass][1];
                _mpPlayersImg[gameclass].fillAmount = f;
                _mpPlayersText[gameclass].text = _curAndMaxMpPlayers[gameclass][0]+ "/" + _curAndMaxMpPlayers[gameclass][1];
                break;
            }
            case GameEventName.GoPlayerAttack:
            {
                var list = messageData.Value as List<GameObject>;
                var enemy = list[0];
                var player = list[1];
                var component = enemy.GetComponent<ActionsWithCards>();
                var attribute = component.CardGame.GetDataCard().AttributeUnit;
                var damage = attribute[1];
                var componentGameClass = player.GetComponent<ActionsWithCardGameClass>();
                var gameclass = componentGameClass.CardGame.GetDataCard().GameClass;
                _curAndMaxHpPlayers[gameclass][0] -= damage.value;
                _hpPlayersText[gameclass].text = _curAndMaxHpPlayers[gameclass][0] + "/" + _curAndMaxHpPlayers[gameclass][1];
                var f = _curAndMaxHpPlayers[gameclass][0] / (float)_curAndMaxHpPlayers[gameclass][1];
                _hpPlayersImg[gameclass].fillAmount = f;
                f = _curAndMaxMpPlayers[gameclass][0] / (float)_curAndMaxMpPlayers[gameclass][1];
                break;
            }
        }
    }
    private void InitDictPlayers()
    {
        var cardsPlayers = _objectStorage.GetDictionaryCards(SubTypeCard.GameClass);
        foreach (var card in cardsPlayers)
        {
            _cardsPlayers.Add(card.Value.GetDataCard().GameClass, card.Value);
        }

        foreach (var player in _cardsPlayers)
        {
            var gameclass = player.Value.GetDataCard().GameClass;
            var attribute = player.Value.GetDataCard().Attribute;
            var hp = attribute[0];
            var mp = attribute[1];
            _curAndMaxHpPlayers[gameclass] = new List<int>() {hp.value, hp.value};
            _curAndMaxMpPlayers[gameclass] = new List<int>() {mp.value, mp.value};
        }
        InitBarsPlayers();
    }

    private void InitBarsPlayers()
    {
        foreach (var player in _cardsPlayers)
        {
            var gameclass = player.Value.GetDataCard().GameClass;
            var f = _curAndMaxHpPlayers[gameclass][0] / (float)_curAndMaxHpPlayers[gameclass][1];
            _hpPlayersImg[gameclass].fillAmount = f;
            f = _curAndMaxMpPlayers[gameclass][0] / (float)_curAndMaxMpPlayers[gameclass][1];
            _mpPlayersImg[gameclass].fillAmount = f;
            _hpPlayersText[gameclass].text = _curAndMaxHpPlayers[gameclass][0] + "/" + _curAndMaxHpPlayers[gameclass][1];
            _mpPlayersText[gameclass].text = _curAndMaxMpPlayers[gameclass][0]+ "/" + _curAndMaxMpPlayers[gameclass][1];
        }
    }
    public bool PlayerIsAlive(GameClass gameClass)
    {
        return _curAndMaxHpPlayers[gameClass][0] > 0;
    }
    public bool PlayerIsMana(GameClass gameClass)
    {
        return _curAndMaxMpPlayers[gameClass][0] > 0;
    }
    
}
