using System.Collections;
using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using TMPro;
using UnityEngine;

public class BarsEnemyManager : IBarsEnemyManager, ISubscriber
{
    private IPublisher _publisher;
    private IEnemyManager _enemyManager;
    private IDictionary<GameObject, TextMeshProUGUI> _hpEnemysText;
    private IDictionary<GameObject, TextMeshProUGUI> _damageEnemysText;
    private IDictionary<GameObject, List<int>> _curAndMaxHpEnemys;
    private IDictionary<GameObject, List<int>> _curAndMaxDamageEnemys;
    private GameObject _targetEnemy;
    public BarsEnemyManager (IEnemyManager enemyManager, IPublisher publisher)
    {
        _enemyManager = enemyManager;
        _publisher =  publisher;
        _hpEnemysText = new Dictionary<GameObject, TextMeshProUGUI>();
        _damageEnemysText = new Dictionary<GameObject, TextMeshProUGUI>();
        _curAndMaxHpEnemys = new Dictionary<GameObject, List<int>>();
        _curAndMaxDamageEnemys = new Dictionary<GameObject, List<int>>();
        /*_objectStorage = objectStorage;
        _hpPlayersText = new Dictionary<GameClass, TextMeshProUGUI>();
        _mpPlayersText = new Dictionary<GameClass, TextMeshProUGUI>();
        _cardsPlayers = new Dictionary<GameClass, ICard>();
        _curAndMaxMpPlayers = new Dictionary<GameClass, List<int>>();*/
    }
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.SpawnEnemy:
            {
                var enemy = messageData.Value as GameObject;
                var componentEnemy =  enemy.GetComponent<ActionsWithCards>();
                var child = enemy.GetComponentsInChildren<Transform>().SearchChild("Hp");  
                var componentText = child.GetComponent<TextMeshProUGUI>();
                _hpEnemysText[enemy] = componentText;
                child = enemy.GetComponentsInChildren<Transform>().SearchChild("Damage");  
                componentText = child.GetComponent<TextMeshProUGUI>();
                _damageEnemysText[enemy] = componentText;
                var attribute = componentEnemy.CardGame.GetDataCard().AttributeUnit;
                var hp = attribute[0];
                var damage = attribute[1];
                _curAndMaxHpEnemys[enemy] = new List<int>() {hp.value, hp.value};
                _curAndMaxDamageEnemys[enemy] = new List<int>() {damage.value, damage.value};
                InitBarsEnemy(enemy);
                break;
            }
            case GameEventName.TargetEnemy:
                _targetEnemy = messageData.Value as GameObject;
                break;
            case GameEventName.GoDamageEnemy:
                var card = messageData.Value as ICard;
                DamageTargetEnemy(card);
                break;
        }
    }
    private void InitBarsEnemy(GameObject enemy)
    {
        _hpEnemysText[enemy].text = _curAndMaxHpEnemys[enemy][1].ToString();
        _damageEnemysText[enemy].text = _curAndMaxDamageEnemys[enemy][1].ToString();
    }
    private void DamageTargetEnemy(ICard card)
    {
        var attribute = card.GetDataCard().AttributeSpell;
        var damagespell = attribute[0].value;
        if (_curAndMaxHpEnemys[_targetEnemy][0] - damagespell < 1)
        {
            _publisher.Publish(null, new CustomEventArgs(GameEventName.GoDeadEnemy, _targetEnemy));
            _hpEnemysText.Remove(_targetEnemy);
            _damageEnemysText.Remove(_targetEnemy);
            _curAndMaxHpEnemys.Remove(_targetEnemy);
            _curAndMaxDamageEnemys.Remove(_targetEnemy);
        }
        else
        {
            _curAndMaxHpEnemys[_targetEnemy][0] -= damagespell;
            _hpEnemysText[_targetEnemy].text = _curAndMaxHpEnemys[_targetEnemy][0].ToString();
        }
    }
    
}


