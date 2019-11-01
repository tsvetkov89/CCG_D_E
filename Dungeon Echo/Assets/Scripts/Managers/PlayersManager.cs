using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using System.Collections;

public class PlayersManager : IPlayersManager, ISubscriber
{
    private readonly IPublisher _publisher;
    private readonly ITokenRewardManager _tokenRewardManager;
    private readonly IAnimaManager _animaManager;
    
    private List<GameObject> _listPlayers;
    public PlayersManager (IPublisher publisher, IAnimaManager animaManager, ITokenRewardManager tokenRewardManager)
    {
        _publisher = publisher;
        _animaManager = animaManager;
        _tokenRewardManager = tokenRewardManager;
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
                var a = player.GetComponent<ActionsWithCardGameClass>();
                _tokenRewardManager.CreateIconTokenByGameClass(a.CardGame.GetDataCard().GameClass);
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
            case GameEventName.GoActivateTargetPlayer:
            {
                var target = (GameClass) messageData.Value;
                foreach (var player in _listPlayers)
                {
                    var a = player.GetComponent<ActionsWithCardGameClass>();
                    a.SetEvent(GameEventName.GoSelectTokenReward);
                    if(a.CardGame.GetDataCard().GameClass == target)
                        _animaManager.SetStateAnimation(player,"target",true);
                }
                Debug.Log("Активируй класс:"+target);
                break;
            }
            case GameEventName.GoDeActivateTargetsPlayer:
            {
                foreach (var player in _listPlayers)
                {
                    var a = player.GetComponent<ActionsWithCardGameClass>();
                    _animaManager.SetStateAnimation(player,"target",false);
                }
                break;
            }
            case GameEventName.GoEndGiveTokensReward:
            {
                foreach (var player in _listPlayers)
                {
                    var a = player.GetComponent<ActionsWithCardGameClass>();
                    a.SetEvent(GameEventName.Undefined);
                }
                break;
            }

        }
    }
    private GameObject GetRandomEnemy()
    {
        return RandomExtensions.Random(_listPlayers);
    }
}
