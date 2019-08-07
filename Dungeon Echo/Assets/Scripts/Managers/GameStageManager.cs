using System.Collections;
using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;

public class GameStageManager : IGameStageManager, ISubscriber
{
    private IPublisher _publisher;
    private List<GameEventName> _stageList;
    private ICoroutiner _coroutiner;

    public GameStageManager(IPublisher publisher, ICoroutiner coroutiner)
    {
        _publisher = publisher;
        _coroutiner = coroutiner;
    }

    //------------------- события
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoStageStartNewGame:
                _stageList = new List<GameEventName>
                {
                    GameEventName.GoStageAddCardSpell,
                    GameEventName.GoSelectCardSpell,
                    GameEventName.GoStageAddCardConsumables,
                    GameEventName.GoSelectCardConsumables,
                    //GameEventName.GoStageAddCardArea,
                    //GameEventName.GoSelectCardArea,
                    GameEventName.GoStageAddCardEvent,
                    GameEventName.GoSelectCardEvent
                };
                break;
            case GameEventName.GoSetNextStage:
            {
                var value = messageData.Value as ICard;
                if (value != null && value.GetDataCard().TypeCard == CardType.GameEvent)
                    AddEventInStageList(value);
                _publisher.Publish(null, new CustomEventArgs(_stageList[0]));
                _stageList.RemoveAt(0);
                break;
            }
            case GameEventName.GoFinishBattle:
            {
                _coroutiner.StartCoroutine(GoNewEvent());
                break;
            }
            case GameEventName.GoFinishStageEvent:
            {
                Debug.Log("finish event");
                _stageList.Add(GameEventName.GoStageAddCardEvent);
                _stageList.Add(GameEventName.GoSelectCardEvent);
                _publisher.Publish(null, new CustomEventArgs(GameEventName.GoSetNextStage));
                break;
            }
        }
    }

    private void AddEventInStageList(ICard card)
    {
        var eventName = card.GetDataCard().NameCard;
        switch (eventName)
        {
            case "random_enemy":
                _stageList.Add(GameEventName.GoStageAddCardEnemy);
                _stageList.Add(GameEventName.GoSelectCardEnemy);
                _stageList.Add(GameEventName.GoStageBattle);
                break;
            case  "random_event":
                _stageList.Add(GameEventName.GoStageRandomEvent);
                break;
        }

    }

    private IEnumerator GoNewEvent()
    {
        yield return new WaitForSeconds(0.4f);
        var list = new List<GameEventName>(){GameEventName.GoStageAddCardSpell,GameEventName.GoStageAddCardConsumables};
        var newevent = list[RandomExtensions.GetRandomElementDictionary(DropChance.ChanceReward)];       
        switch (newevent)
        {
            case GameEventName.GoStageAddCardSpell:
                _stageList.Add(GameEventName.GoStageAddCardSpell);
                _stageList.Add(GameEventName.GoSelectCardSpell);
                _stageList.Add(GameEventName.GoStageAddCardEvent);
                _stageList.Add(GameEventName.GoSelectCardEvent);
                _publisher.Publish(null, new CustomEventArgs(GameEventName.GoSetNextStage));
                Debug.Log("награда");
                break;
            case GameEventName.GoStageAddCardConsumables:
                _stageList.Add(GameEventName.GoStageAddCardConsumables);
                _stageList.Add(GameEventName.GoSelectCardConsumables);
                _stageList.Add(GameEventName.GoStageAddCardEvent);
                _stageList.Add(GameEventName.GoSelectCardEvent);
                _publisher.Publish(null, new CustomEventArgs(GameEventName.GoSetNextStage));
                Debug.Log("награда");
                break;
        }
    }
    
}


