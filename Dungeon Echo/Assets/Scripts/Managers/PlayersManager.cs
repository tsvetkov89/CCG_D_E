using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using System.Collections;

public class PlayersManager : IPlayersManager, ISubscriber
{
    private readonly IPublisher _publisher;
    
    private List<GameObject> _listPlayers;
    public PlayersManager (IPublisher publisher)
    {
        _publisher = publisher;
        _listPlayers = new List<GameObject>();
    }
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.SpawnPlayer:
            {
                var player = messageData.Value as GameObject;
                _listPlayers.Add(player);
                break;
            }
            case GameEventName.GoEnemyAttack:
            {
                var enemy = messageData.Value as GameObject;
                var player = GetRandomEnemy();
                var list = new List<GameObject> {enemy, player};
                _publisher.Publish(null, new CustomEventArgs(GameEventName.GoPlayerAttack,list));
                break;  
            }
            
                
        }
    }

    private GameObject GetRandomEnemy()
    {
        return RandomExtensions.Random(_listPlayers);
    }

}
