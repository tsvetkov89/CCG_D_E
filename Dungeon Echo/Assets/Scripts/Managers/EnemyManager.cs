using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using System.Collections;

public class EnemyManager : IEnemyManager, ISubscriber
{
    private readonly ICoroutiner _coroutiner;
    private readonly IPublisher _publisher;
    private readonly IAnimaManager _animaManager;
    private List<GameObject> _listEnemys;
    public EnemyManager (IPublisher publisher, ICoroutiner coroutiner, IAnimaManager animaManager)
    {
        _publisher = publisher;
        _coroutiner = coroutiner;
        _animaManager = animaManager;
        _listEnemys = new List<GameObject>();
    }
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.SpawnEnemy:
            {
                var enemy = messageData.Value as GameObject;
                Debug.Log("add enemy in manager" + enemy.name);
                _listEnemys.Add(enemy);
                break;
            }
            case GameEventName.GoEndTurnPlayer:
            {
                _coroutiner.StartCoroutine(EnemyTurn());
                break;
            }
            case GameEventName.GoDeadEnemy:
            {
                var gameObject = messageData.Value as GameObject;
                _listEnemys.Remove(gameObject);
                _animaManager.SetStateAnimation(gameObject,"dead",true);
                if(_listEnemys.Count == 0)
                    _publisher.Publish(null,new CustomEventArgs(GameEventName.GoFinishBattle));
                break;
            }
        }
   }

    public GameObject GetRandomEnemy()
    {
        return _listEnemys.Count > 0 ? RandomExtensions.Random(_listEnemys) : null;
    }
    public bool EnemyIsLive()
    {
        return _listEnemys.Count > 0;
    }
    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var enemy in _listEnemys)
        {
            _publisher.Publish(null,new CustomEventArgs(GameEventName.GoEnemyAttack,enemy));
        }
        yield return new WaitForSeconds(1.5f);
        _publisher.Publish(null,new CustomEventArgs(GameEventName.GoNextTurn));
        _publisher.Publish(null,new CustomEventArgs(GameEventName.GoEndTurnEnemy));
    }
}
