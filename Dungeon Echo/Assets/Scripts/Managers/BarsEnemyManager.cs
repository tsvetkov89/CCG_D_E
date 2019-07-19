﻿using System.Collections;
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
            case GameEventName.GoDamageEnemy:
                var card = messageData.Value as ICard;
                DamageRandomEnemy(card);
                break;
        }
    }
    private void InitBarsEnemy(GameObject enemy)
    {
        _hpEnemysText[enemy].text = _curAndMaxHpEnemys[enemy][1].ToString();
        _damageEnemysText[enemy].text = _curAndMaxDamageEnemys[enemy][1].ToString();
    }
    private void DamageRandomEnemy(ICard card)
    {
       
        var attribute = card.GetDataCard().AttributeSpell;
        var damagespell = attribute[0].value;
        var randomEnemy = _enemyManager.GetRandomEnemy();
        if (!randomEnemy) return;
        _curAndMaxHpEnemys[randomEnemy][0] -= damagespell;
        _hpEnemysText[randomEnemy].text = _curAndMaxHpEnemys[randomEnemy][0].ToString();
        if (_curAndMaxHpEnemys[randomEnemy][0] < 1)
        {
            _publisher.Publish(null, new CustomEventArgs(GameEventName.GoDeadEnemy, randomEnemy));
            _hpEnemysText.Remove(randomEnemy);
            _damageEnemysText.Remove(randomEnemy);
            _curAndMaxHpEnemys.Remove(randomEnemy);
            _curAndMaxDamageEnemys.Remove(randomEnemy);
        }
    }
    
}

