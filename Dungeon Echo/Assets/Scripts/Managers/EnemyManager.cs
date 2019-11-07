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
    private readonly IObjectStorage _objectStorage;
    private readonly IConfigurateManager _configurateManager;
    
    private List<GameObject> _listEnemys;
    private GameObject _panelEnemy;
    public EnemyManager (IPublisher publisher, ICoroutiner coroutiner, IAnimaManager animaManager, IObjectStorage objectStorage, IConfigurateManager configurateManager)
    {
        _publisher = publisher;
        _coroutiner = coroutiner;
        _animaManager = animaManager;
        _objectStorage = objectStorage;
        _configurateManager = configurateManager;
        _listEnemys = new List<GameObject>();
        
    }

    public void SetDependecies(GameObject panel)
    {
        _panelEnemy = panel;
    }
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.SpawnEnemy:
            {
                var enemy = messageData.Value as GameObject;
                _listEnemys.Add(enemy);
                _coroutiner.StartCoroutine(SwitchParent(enemy,0.6f));
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
    private IEnumerator SwitchParent(GameObject obj, float pause)
    {
        yield return new WaitForSeconds(pause);
        _configurateManager.ConfigurateByParent(obj,_panelEnemy,1,1,1,1);   
    }
}
