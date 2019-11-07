using InterfaceNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumNamespace;
/// <summary>
/// Менеджер игровой логики
/// </summary>
public class GameManager : IGameManager, ISubscriber
{
    private readonly ICoroutiner _coroutiner;
    private readonly IPublisher _publisher;
    private readonly IAnimaManager _animaManager;
    private readonly IObjectStorage _objectStorage;
    private readonly IInventoryManager _inventoryManager;

    private readonly IConfigurateManager _configurateManager;
    //private List<GameObject> _poolGameClass; 
    private List<GameObject> _poolGameCard; 
    private List<GameObject> _poolCardArea;
    private List<GameObject> _poolCardEnemy;
    private List<GameObject> _poolCardBattleUnit;
    private List<GameObject> _poolCardEvent;

    //private IDictionary<ObjectTypeEnum,List<GameObject>> _poolGameObjects;
    private GameObject _hud;  //игровой HUD
    private GameObject _shirtDeckInGame; //рубашка колоды
    private ICard _selectedCardUnit;
    public GameManager(IPublisher publisher, IAnimaManager animaManager, IObjectStorage objectStorage,
        ICoroutiner coroutiner, IInventoryManager inventoryManager, IConfigurateManager configurateManager)
    {
        _publisher = publisher;
        _animaManager = animaManager;
        _objectStorage = objectStorage;
        _coroutiner = coroutiner;
        _inventoryManager = inventoryManager;
        _configurateManager = configurateManager;
    }
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoStageAddCardSpell:
                _coroutiner.StartCoroutine(SpawnCardsByType(CardType.Spell, 
                    DropChance.ChanceSpell, DropChance.StartСhanceRarity, 3));
                break;
            case GameEventName.GoStageAddCardConsumables:
                _coroutiner.StartCoroutine(SpawnCardsByType(CardType.Consumables, 
                    DropChance.ChanceConsumables, DropChance.StartСhanceRarity, 3));
                break;
            case GameEventName.GoStageAddCardArea:
                _coroutiner.StartCoroutine(SpawnCardArea());
                break;
            case GameEventName.GoStageAddCardEvent:
                _coroutiner.StartCoroutine(SpawnCardEvent());
                break;
            case GameEventName.GoStageAddCardEnemy:
                _coroutiner.StartCoroutine(SpawnCardEnemy());
                break;
            case GameEventName.GoStageRandomEvent:
                _coroutiner.StartCoroutine(StartRandomEvent());
                break;
            case GameEventName.GoSelectCardEnemy:
            {
                foreach (var card in _poolCardEnemy)
                {
                    if (!card.activeSelf) continue;
                    var cardDisplay = card.GetComponent<ActionsWithCards>();
                    _selectedCardUnit = cardDisplay.CardGame;
                    break;
                }
                _coroutiner.StartCoroutine(PreparationStageBattle());
                //_coroutiner.StartCoroutine(SpawnCardsInHand());
                break;
            }
            case GameEventName.GoSummonAllies:
                _selectedCardUnit = messageData.Value as ICard;
                SpawnAllies();
                break;
            case GameEventName.GoStageBattle:
                _publisher.Publish(null, new CustomEventArgs(GameEventName.GoNextTurn));
                break;
            case GameEventName.GoFinishBattle:
                FinishBattle();
                break;
            case GameEventName.GoRewardEvent:
                _coroutiner.StartCoroutine(EndRandomEvent(0.7f));
                break;
            case GameEventName.GoOpenPopupWithDescriptionCard:
            {
                ReturnNoSelectedCard(_poolGameCard);
                var cardGame  = messageData.Value;
                _publisher.Publish(_hud, new CustomEventArgs(GameEventName.GoSetNextStage, cardGame));
                break;
            }
            case GameEventName.GoOpenPopupNoDescription:
            {
                ReturnNoSelectedCard(_poolCardArea);
                ReturnNoSelectedCard(_poolCardEvent);
                ReturnNoSelectedCard(_poolCardEnemy);
                var cardGame  = messageData.Value;
                _publisher.Publish(_hud, new CustomEventArgs(GameEventName.GoSetNextStage, cardGame));
                break;
            }
            case GameEventName.GoDeadEnemy:
                var objCard  = messageData.Value as GameObject;
                _configurateManager.ConfigurateByParent(objCard, _hud, false);
                break;
            /*case GameEventName.GoEndTurnPlayer:
                foreach (var card in _poolCardsPlayer)
                {
                    var component = card.GetComponent<DraggableCard>();
                    component.enabled = false;
                }
                break;
            case GameEventName.GoEndTurnEnemy:
                foreach (var card in _poolCardsPlayer)
                {
                    var component = card.GetComponent<DraggableCard>();
                    component.enabled = true;
                }
                break;*/
        }
    }
    //---------------- раздача карт по типу
    private IEnumerator SpawnCardsByType(CardType type, DropChance typeDropChance, DropChance rarityDropChance, int count)
    {
        SelectCardInDeck(_poolGameCard);
        yield return new WaitForSeconds(0.3f);
        SubTypeCard[] arrayType;
        var arrayRarity = new[] {CardRarity.Common, CardRarity.Rare, CardRarity.Legendary};
        switch (type)
        {
            case CardType.Equipment:
                arrayType = new[] {SubTypeCard.Decoration};
                break;
            case CardType.Spell:
                arrayType = new[] {SubTypeCard.AttackingSpell, SubTypeCard.ImprovingSpell, SubTypeCard.CurseSpell};
                break;
            case CardType.Consumables:
                arrayType = new[] {SubTypeCard.Consumables};
                break;
            default:
                throw new UnityException("It is unknown CardType in SpawnCardsByType");
        }
        var typeCard = arrayType[RandomExtensions.GetRandomElementDictionary(typeDropChance)];
        var rarity = arrayRarity[RandomExtensions.GetRandomElementDictionary(rarityDropChance)];
        var number = 1;
        foreach (var card in _poolGameCard)
        {
            if (number > count) break;
            if (card.activeSelf) continue;
            var cardDisplay = card.GetComponent<ActionsWithCards>();
            var randomCard = _objectStorage.GetRandomCardByType(typeCard, rarity);
            cardDisplay.СhangeCardType(randomCard);
            cardDisplay.SetIdCard(number);
            cardDisplay.CardGame.DisplayCardInGame(card);
            cardDisplay.SetDependecies(_publisher, _animaManager);
            card.SetActive(true);
            _animaManager.SetStateAnimation(card, "dist_num", number);
            yield return new WaitForSeconds(0.2f);
            ++number;
        }
    }
    private IEnumerator SpawnCardArea()
    {
        SelectCardInDeck(_poolGameCard);
        yield return new WaitForSeconds(0.3f);
        var count = 1;
        foreach (var card in _poolCardArea)
        {
            if (count > 3) break;
            if (card.activeSelf) continue;
            var cardDisplay = card.GetComponent<ActionsWithCards>();
            var randomCard = _objectStorage.GetRandomCardByType(SubTypeCard.GameArea, CardRarity.Common);
            cardDisplay.СhangeCardType(randomCard);
            cardDisplay.SetIdCard(count);
            cardDisplay.CardGame.DisplayCardInGame(card);
            cardDisplay.SetDependecies(_publisher, _animaManager);
            card.SetActive(true);
            _animaManager.SetStateAnimation(card, "dist_num", count);
            yield return new WaitForSeconds(0.2f);
            ++count;
        }
    }
    private IEnumerator SpawnCardEvent()
    {
        SelectCardInDeck(_poolGameCard);
        SelectCardInDeck(_poolCardArea);
        yield return new WaitForSeconds(0.3f); 
        var count = 1;
        var arrayRarity = new[] {CardRarity.Common, CardRarity.Rare, CardRarity.Legendary};
        var rarity = arrayRarity[RandomExtensions.GetRandomElementDictionary(DropChance.ChanceEvent)];
        foreach (var card in _poolCardEvent)
        {
            if (count > 3) break;
            if (card.activeSelf) continue;
            var cardDisplay = card.GetComponent<ActionsWithCards>();
            var randomCard = _objectStorage.GetRandomCardByType(SubTypeCard.GameEvent, rarity);
            //var randomCard = _objectStorage.GetCardByName("random_event");
            cardDisplay.СhangeCardType(randomCard);
            cardDisplay.SetIdCard(count);
            cardDisplay.CardGame.DisplayCardInGame(card);
            cardDisplay.SetDependecies(_publisher, _animaManager);
            card.SetActive(true);
            _animaManager.SetStateAnimation(card, "dist_num", count);
            yield return new WaitForSeconds(0.2f);
            ++count;
        }
    }
    private IEnumerator SpawnCardEnemy()
    {
        SelectCardInDeck(_poolGameCard);
        SelectCardInDeck(_poolCardArea);
        SelectCardInDeck(_poolCardEvent);
        yield return new WaitForSeconds(0.3f);
        var count = 1;
        foreach (var card in _poolCardEnemy)
        {
            if (count > 3) break;
            if (card.activeSelf) continue;
            foreach (Transform child in card.transform)
                child.gameObject.SetActive( true);
            var cardDisplay = card.GetComponent<ActionsWithCards>();
            var randomCard = _objectStorage.GetRandomCardByType(SubTypeCard.LightEnemy, CardRarity.Common);
            cardDisplay.СhangeCardType(randomCard);
            cardDisplay.SetIdCard(count);
            cardDisplay.CardGame.DisplayCardInGame(card);
            cardDisplay.SetDependecies(_publisher, _animaManager);
            card.SetActive(true);
            _animaManager.SetStateAnimation(card, "dist_num", count);
            yield return new WaitForSeconds(0.2f);
            ++count;
        }
    }

    private IEnumerator StartRandomEvent()
    {
        SelectCardInDeck(_poolCardEvent);
        yield return new WaitForSeconds(1f);
        var list = _objectStorage.GetAdventure();
        var adventure = RandomExtensions.Random(list);
        _publisher.Publish(null, new CustomEventArgs(GameEventName.GoStartEvent, adventure));
    }

    /*private IEnumerator SpawnCardsInHand()
    {
        var cards = _inventoryManager.GetCardInDeck();
        yield return new WaitForSeconds(2f);
        var count = 1;
        foreach (var card in cards)
        {
            foreach (var cardPlayer in _poolCardsPlayer)
            {
                if (cardPlayer.activeSelf) continue;
                foreach (Transform child in cardPlayer.transform)
                    child.gameObject.SetActive( true);
                var cardDisplay = cardPlayer.GetComponent<ActionsWithCards>();
                var cardByName =  _objectStorage.GetCardByName(card);
                cardDisplay.СhangeCardType(cardByName);
                cardDisplay.SetIdCard(count);
                cardDisplay.CardGame.DisplayCardInGame(cardPlayer);
                cardDisplay.SetDependecies(_publisher, _animaManager);
                cardDisplay.enabled = false;
                cardPlayer.SetActive(true);
                _animaManager.SetStateAnimation(cardPlayer, "go_hand",true);
                count++;
                _coroutiner.StartCoroutine(SwithParentCard(cardPlayer));
                yield return new WaitForSeconds(0.2f);
                break;
            }
        }
    }*/
    /*private IEnumerator SwithParentCard(GameObject card)
    {
        yield return new WaitForSeconds(0.6f);
        _objectStorage.ConfigurateByParent(card,_hand);
        card.GetComponent<CanvasGroup>().blocksRaycasts = true;     
    }*/
    public void PlaceObjects()
    {
        _poolCardBattleUnit = _objectStorage.GetPollObjects(ObjectTypeEnum.PrefabCardBattleUnit, 8);
        foreach (var objCard in _poolCardBattleUnit)
            _configurateManager.ConfigurateByParent(objCard, _hud, false);
        _poolCardArea = _objectStorage.GetPollObjects(ObjectTypeEnum.PrefabCardArea, 3);
        foreach (var objGameArea in _poolCardArea)
            _configurateManager.ConfigurateByParent(objGameArea, _hud, false);
        _poolCardEvent = _objectStorage.GetPollObjects(ObjectTypeEnum.PrefabCardEvent, 3);
        foreach (var objGameEvent in _poolCardEvent)
            _configurateManager.ConfigurateByParent(objGameEvent, _hud, false);
        _poolCardEnemy = _objectStorage.GetPollObjects(ObjectTypeEnum.PrefabCardEnemy, 3);
        foreach (var objGameEnemy in _poolCardEnemy)
            _configurateManager.ConfigurateByParent(objGameEnemy, _hud, false);
        _poolGameCard = _objectStorage.GetPollObjects(ObjectTypeEnum.PrefabCard, 5);
        foreach (var objCard in _poolGameCard)
            _configurateManager.ConfigurateByParent(objCard, _hud, false);
        /*_poolCardsPlayer = _objectStorage.GetPollObjects(ObjectTypeEnum.PrefabCard, 9);
        foreach (var card in _poolCardsPlayer)
        {
            _objectStorage.Configurate(card, _hud, false,0.034f,0.06f,0.194f,0.46f);
            card.gameObject.AddComponent<DraggableCard>();
            var component = card.GetComponent<DraggableCard>();
            component.enabled = false;
        }*/
        _shirtDeckInGame.transform.SetAsLastSibling();
        _publisher.Publish(null, new CustomEventArgs(GameEventName.GoSetNextStage));
    }
    private IEnumerator PreparationStageBattle()
    {
        ReturnCardEvent();
        yield return new WaitForSeconds(0.4f);
        foreach (var element in _poolCardArea)
        {
            if (!element.activeSelf) continue;
            _animaManager.SetStateAnimation(element,"stage_battle",true);
        }
        _publisher.Publish(null, new CustomEventArgs(GameEventName.GoSetNextStage));
        yield return new WaitForSeconds(0.4f);
        var spawnUnits = _selectedCardUnit.GetDataCard().SpawnUnits;
        var count = 1;
        foreach (var nameSpawnUnit in spawnUnits)
        {
            foreach (var card in _poolCardBattleUnit)
            {
                if (card.activeSelf) continue;   
                var cardDisplay = card.GetComponent<ActionsWithCards>();
                var cardByName =  _objectStorage.GetCardByName(nameSpawnUnit);
                cardDisplay.СhangeCardType(cardByName);
                cardDisplay.SetIdCard(count);
                cardDisplay.CardGame.DisplayCardInGame(card);
                cardDisplay.SetDependecies(_publisher, _animaManager);
                cardDisplay.enabled = false;
                card.SetActive(true);
                _publisher.Publish(null, new CustomEventArgs(GameEventName.SpawnEnemy, card));
                _animaManager.SetStateAnimation(card, "stage_battle", true);
                break;
            }
            count++;
        }
    }

    private void SpawnAllies()
    {
        var spawnUnits = _selectedCardUnit.GetDataCard().SpawnUnits;
        var count = 1;
        foreach (var nameSpawnUnit in spawnUnits)
        {
            foreach (var card in _poolCardBattleUnit)
            {
                if (card.activeSelf) continue;   
                var cardDisplay = card.GetComponent<ActionsWithCards>();
                var cardByName =  _objectStorage.GetCardByName(nameSpawnUnit);
                Debug.Log(cardByName);
                cardDisplay.СhangeCardType(cardByName);
                cardDisplay.SetIdCard(count);
                cardDisplay.CardGame.DisplayCardInGame(card);
                cardDisplay.SetDependecies(_publisher, _animaManager);
                cardDisplay.enabled = false;
                card.SetActive(true);
                _publisher.Publish(null, new CustomEventArgs(GameEventName.SpawnAllies, card));
                _animaManager.SetStateAnimation(card, "ally", true);
                break;
            }
            count++;
        }
    }

    private void FinishBattle()
    {
        foreach (var element in _poolCardArea)
        {
            if (!element.activeSelf) continue;
            _animaManager.SetStateAnimation(element,"stage_battle",false);
        }
    }

    private void ReturnCardEvent()
    {
        foreach (var element in _poolCardEvent)
        {
            if (!element.activeSelf) continue;
            _animaManager.SetStateAnimation(element,"return",true);
        }
    }
    public void SetUiComponents(GameObject hud, GameObject deckInGame)
    {
        _hud = hud;
        _shirtDeckInGame = deckInGame;
    }
    private void ReturnNoSelectedCard(IEnumerable<GameObject> pool)
    {
        foreach (var element in pool)
        {
            if (!element.activeSelf) continue;
            var card = element.GetComponent<ActionsWithCards>();
            _animaManager.SetStateAnimation(element, !card.Selected ? "return_num" : "select_num",
                card.IdCard);
        }
    }
    private void SelectCardInDeck(IEnumerable<GameObject> pool)
    {
        foreach (var element in pool)
        {
            if (!element.activeSelf) continue;
            var card = element.GetComponent<ActionsWithCards>();
            if (card.Selected) _animaManager.SetStateAnimation(element,"dist_deck",true);
        }
    }

    private IEnumerator EndRandomEvent(float pause)
    {
        yield return new WaitForSeconds(pause);
        ReturnCardEvent();
    }
    /*yield return new WaitForSeconds(pause);
    _publisher.Publish(null, new CustomEventArgs(GameEventName.GoFinishStageEvent));*/
}
