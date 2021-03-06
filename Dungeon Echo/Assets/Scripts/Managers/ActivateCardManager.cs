using InterfaceNamespace;
using UnityEngine;
using EnumNamespace;
using TMPro;

public class ActivateCardManager: IActivateCardManager,ISubscriber
{
    private IPublisher _publisher;
    private IBarsPlayerManager _barsPlayerManager;
    private Membership _membership;
    private IEnemyManager _enemyManager;
    private TMP_Text _counterCardPlay;
    
    private int _numberCardPlayed;
    public ActivateCardManager(IPublisher publisher,IBarsPlayerManager barsPlayerManager, IEnemyManager enemyManager)
    {
        _publisher = publisher;
        _barsPlayerManager = barsPlayerManager;
        _enemyManager = enemyManager;
        _membership = Membership.Undefined;
        _numberCardPlayed = 2;
    }

    public void SetDependecies(GameObject obj)
    {
        var text = obj.transform.GetComponentsInChildren<Transform>().SearchChild("Text").gameObject;
        _counterCardPlay = text.GetComponent<TMP_Text>();
    }

    //--------------Обрабатываем события
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoNextTurn:
            {
                if (_membership == Membership.Undefined)
                {
                    _membership = Membership.Player;
                }
                else
                {
                    _membership = _membership == Membership.Player ? Membership.Enemy : Membership.Player;
                }
                return;
            }
            case GameEventName.GoActivateCard:
            {
                var card = messageData.Value as ICard;
                
                if (_membership == Membership.Player && _numberCardPlayed > 0)
                {
                    --_numberCardPlayed;
                    _counterCardPlay.text = _numberCardPlayed.ToString();
                    ActivateCardPlayer(card);
                    if (_numberCardPlayed == 0 && _enemyManager.EnemyIsLive())
                    {
                        _publisher.Publish(null,new CustomEventArgs(GameEventName.GoNextTurn));
                        _publisher.Publish(null,new CustomEventArgs(GameEventName.GoEndTurnPlayer));
                    }
                }
                break;
            }
            case GameEventName.GoEndTurnEnemy:
            {
                _numberCardPlayed = 2;
                _counterCardPlay.text = _numberCardPlayed.ToString();
                break;
            }

            case GameEventName.GoEndTurnPlayer:
            {
                _numberCardPlayed = 0;
                break;
            }
            case GameEventName.GoFinishBattle:
            {
                _numberCardPlayed = 2;
                _membership = Membership.Undefined;
                break;
            }
            case GameEventName.GoStageBattle:
            {
                _counterCardPlay.text = _numberCardPlayed.ToString();
                break;
            }
        }
    }
    //--------------проверяем можно ли разыграть карту
    public bool PossiblePlayPlayerCard(ICard card)
    {
        if (card.GetDataCard().TypeCard == CardType.Consumables)
            return true;
        var gameclass = card.GetDataCard().GameClass;
        return _barsPlayerManager.PlayerIsAlive(gameclass);
    }
    public bool IsManaPlayPlayerCard(ICard card)
    {
        if (card.GetDataCard().TypeCard == CardType.Consumables)
            return true;
        var gameclass = card.GetDataCard().GameClass;
        return _barsPlayerManager.PlayerIsMana(gameclass);
    }

    private void ActivateCardPlayer(ICard card)
    {
        var type = card.GetDataCard().TypeSubCard;
        switch (type)
        {
            case SubTypeCard.AttackingSpell:
                _publisher.Publish(null, new CustomEventArgs(GameEventName.GoDamageEnemy,card));
                break;
            case SubTypeCard.Consumables:
                if(card.GetDataCard().Application == Membership.Player)
                    _publisher.Publish(null, new CustomEventArgs(GameEventName.GoUseConsumablesOnPlayer,card));
                break;
            case SubTypeCard.SummonSpell:
                _publisher.Publish(null, new CustomEventArgs(GameEventName.GoSummonAllies,card));
                break;
        }
        _publisher.Publish(null, new CustomEventArgs(GameEventName.ActivateCardPlayer,card));
    }
}