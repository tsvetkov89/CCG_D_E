using System;
using System.Collections.Generic;
using System.Linq;
using InterfaceNamespace;
using UnityEngine;
using EnumNamespace;
using UnityEngine.UI;

public class InventoryManager : IInventoryManager, ISubscriber
{
    private IObjectStorage _objectStorage;
    private IPublisher _publisher;
    private ICoroutiner _coroutiner;
    
    private IDictionary<GameClass,List<string>> _inventoryPlayers;     //массив названий предметов в инвентаре
    private IDictionary<GameClass,List<string>> _equpmentCardsPlayers; //массив названий предметов в инвентаре
    
    private string[] _equipmentPlayer;                  //массив названий предметов в инвентаре
    private string[] _spellsPlayer;                     //массив названий предметов в инвентаре
    private GameClass _player;
           
    private List<GameObject> _arrayInventory;           //доступ   
    private List<GameObject> _arraySpells;              //      к слотам 
    private List<GameObject> _arrayPlayers;    
    
    //private GameObject _slotDecoration;               
    private GameObject _holderCards;            
    private GameObject _holderInventory;              

    private int _idSlot;
    private bool _autoequpment;
    private GameObject _parentObject;              //использую при снятии экипированной карты
    //-------------------Конструктор
    public InventoryManager(IObjectStorage objectStorage , IPublisher publisher, ICoroutiner coroutiner)
    {
        _objectStorage = objectStorage;
        _publisher = publisher;
        _coroutiner = coroutiner;
        _player = GameClass.Red;
        _autoequpment = true;
        _inventoryPlayers = new Dictionary<GameClass, List<string>>();
        _equpmentCardsPlayers = new Dictionary<GameClass, List<string>>();

        CreatePlayersInventory();
    }
    private void CreatePlayersInventory()
    {
        var enumerable = Enum.GetValues(typeof(GameClass)).Cast<GameClass>();
        /*foreach (var player in enumerable)
        {
            if(player == GameClass.Undefined) continue;
            _equpmentCardsPlayers[player] = new List<string>(){"","","","","",""};
        }*/
        foreach (var player in enumerable)
        {
            if(player == GameClass.Undefined) continue;
            _inventoryPlayers[player] = new List<string>(){"","","","",""};
        }
        _equpmentCardsPlayers[GameClass.Red] = new List<string>(){"BaseAttackRed","","","","",""};
        _equpmentCardsPlayers[GameClass.Green] = new List<string>(){"BaseAttackGreen","","","","",""};
        _equpmentCardsPlayers[GameClass.Blue] = new List<string>(){"BaseAttackBlue","","","","",""};
        _equpmentCardsPlayers[GameClass.Purple] = new List<string>(){"BaseAttackPurple","","","","",""};
    }
    //--------------------добавим карту в инвентарь
    private void AddItemInventory(string name, int index)
    {
        var list = _inventoryPlayers[_player];
        list[index] = name;
    }
    //-------------------добавим карту в список одетых карт
    private void AddItemEqupmentCard(string name, int index)
    {
        var list = _equpmentCardsPlayers[_player];
        list[index] = name;
    }

    private void AddItemInventory(string name)
    {
        var gameClass = _player;
        var addCard = _objectStorage.GetCardByName(name);
        var typeCard = addCard.GetDataCard().TypeCard;
        if (typeCard == CardType.Spell && gameClass != addCard.GetDataCard().GameClass)
            gameClass = addCard.GetDataCard().GameClass;
        var list = _inventoryPlayers[gameClass];
        var equpment = _equpmentCardsPlayers[gameClass];
        var indexInventory = list.IndexOf("");
        switch (indexInventory)
        {
            //-----------инвентарь переполнен
            case -1:
                Debug.Log("инвентарь переполнен!");
                return;
            //-----------в инвентаре есть еще место
            default:
                var indexEqupment = equpment.IndexOf("");
                if (_autoequpment && indexEqupment != -1)
                {
                    Debug.Log("аутоеквип!");
                    if (typeCard == CardType.Spell)
                    {
                        equpment[indexEqupment] = name;
                        if( gameClass == _player)
                            SpawnItemEqupmentCard(indexEqupment);
                    }
                    else
                    {
                        if (equpment[5] == "")
                        {
                            equpment[5] = name;
                            if( gameClass == _player)
                                SpawnItemEqupmentCard(5);
                        }
                        else
                        {
                            list[indexInventory] = name;
                            if( gameClass == _player)
                                SpawnItemInventory(indexInventory);
                        }
                    }
                }
                else
                {
                    list[indexInventory] = name;
                    if( gameClass == _player)
                        SpawnItemInventory(indexInventory);
                }
                break;
        }
    }
    //--------------------очистим слот инвентаря
    private void DeleteItemInventory(int index)                             
    {
        var list = _inventoryPlayers[_player];
        list[index] = "";
    }
    //-------------------очистим слот екипированных карт
    private void DeleteItemEqupmentCard(int index)                             
    {
        var list = _equpmentCardsPlayers[_player];
        list[index] = "";
    }
    //-----------устанавливаю ссылки на панели инвенторя
    public void SetHolders(GameObject holderCards, GameObject holderInventory, List<GameObject> gameObjects)
    {
        _holderCards = holderCards;
        _holderInventory= holderInventory;
        _arrayPlayers = gameObjects;
    }
    //--------------Метод, создает слоты для панелей инвентаря
    public void CreateSlotsInHolders()
    {
        ActionsWithSlotInventory component;
        //----------------------------
        float xmin = 0.04f, ymin = 0.5907f, xmax = 0.9580f, ymax = 0.9917f, ysize = -0.2f, xsize = 0.314f;
        _arrayInventory =_objectStorage.GetPollObjects(ObjectTypeEnum.SlotCardInInventory,5);
        foreach (var obj in _arrayInventory)
        {
            _objectStorage.Configurate(obj, _holderCards,true,xmin,xmax,ymin,ymax);
            ymin += ysize;
            ymax += ysize;
            component = obj.GetComponent<ActionsWithSlotInventory>();
            obj.transform.GetComponent<Image>().SetAlpha(0f);
            component.SetDependecies(_publisher);
            component.SetVarsSlot(true,false); 
        }
        //----------------------------
        _arraySpells  = _objectStorage.GetPollObjects(ObjectTypeEnum.SlotCardInInventory,6);
        xmin = 0.0644f; ymin = 0.5323f; xmax = 0.3094f; ymax = 0.9348f;
        var count = 0;
        foreach (var obj in _arraySpells)
        {
            _objectStorage.Configurate(obj, _holderInventory, true, xmin, xmax, ymin, ymax);
            component = obj.GetComponent<ActionsWithSlotInventory>();
            component.SetDependecies(_publisher);
            component.SetVarsSlot(true, false);
            xmin += xsize;
            xmax += xsize;
            if (count == 2)
                {xmin = 0.0644f; ymin = 0.0756f; xmax = 0.3094f; ymax = 0.475f;}
            count++;
        }
        CreateInventory(_player);
    }
    //---------------------Создание инвентаря, заполнение слотов
    private void CreateInventory(GameClass player)
    {
        ClearInventory();
        var list = _inventoryPlayers[player];
        //---------------заполняем инвентарь (сумку)
        for (var i = 0; i < list.Count; i++)
        {
            if (list[i] == "") continue;
            SpawnItemInventory(i);
        }
        list = _equpmentCardsPlayers[player];
        for (var i = 0; i < list.Count; i++)
        {
            if (list[i] == "") continue;
            SpawnItemEqupmentCard(i);
        }
    }
    //-------------- очищаем слоты инвентаря
    private void ClearInventory()
    {
        foreach (Transform child in _holderCards.transform)
        {
            if (child.transform.childCount <= 0) continue;
            _coroutiner.StartCoroutine(_objectStorage.ReturnInPool(ObjectTypeEnum.PrefabCardInInventory,
                child.transform.GetChild(0).gameObject, 0.1f));
            var component = child.GetComponent<ActionsWithSlotInventory>();
            component.SetVarsSlot(true,false);
        }
        foreach (Transform child in _holderInventory.transform)
        {
            if (child.transform.childCount <= 0) continue;
            _coroutiner.StartCoroutine(_objectStorage.ReturnInPool(ObjectTypeEnum.PrefabCardInInventory,
                child.transform.GetChild(0).gameObject, 0.1f));
            var component = child.GetComponent<ActionsWithSlotInventory>();
            component.SetVarsSlot(true,false);
        }
    }
    //--------------Обрабатываем события
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoEquipmentCard:
            {
                var cardData = messageData.Value as ICard;
                SearchEmptySlot(cardData, _arrayInventory[_idSlot]);
                break;
            }
            case GameEventName.GoUnequipmentCard:
            {
                var cardData = messageData.Value as ICard;
                ClearSlotEqupment(cardData, _parentObject);
                break;
            }
            case GameEventName.GoSetIdSlotInventory:
            {
                var index = messageData.Value;
                _idSlot = (int) index;
                break;
            }
            case GameEventName.GoUnPlaceInSlot:
                _parentObject = messageData.Value as GameObject;
                break;
            case GameEventName.GoAddCardInInventory:
            {
                var name = messageData.Value as string;
                AddItemInventory(name);
                break;
            }
            case GameEventName.GoSelectedIconPlayer:
            {
                var card = messageData.Value as ICard;
                if (card != null && card.GetDataCard().GameClass != _player)
                    SelectedIconPlayer(card);
                break;
            }
        }
    }
    //---------------- Смена иконки игрока, и его инвентаря
    private void SelectedIconPlayer(ICard card)
    {
        foreach (var player in _arrayPlayers)
        {
            var component =  player.GetComponent<ActionsWithCardGameClass>();
            if (!component.Selected) continue;
            component.СhangeCardType(component.CardGame);
            break;
        }
        foreach (var player in _arrayPlayers)
        {
            var component =  player.GetComponent<ActionsWithCardGameClass>();
            if (component.CardGame != card) continue;
            component.SetSelected();
            _player = card.GetDataCard().GameClass;
            CreateInventory(_player);
            break;
        }     
    }
    //----------------Поиск пустого слота экипировки/скила
    private void SearchEmptySlot(ICard equippedCard, GameObject slot)
    {
        var cardInInventory = slot.transform.GetChild(0).gameObject;
        var typeCard = equippedCard.GetDataCard().TypeCard;
        ActionsWithSlotInventory component;
        switch (typeCard)
        {
            case CardType.Consumables:
            {
                component = _arraySpells[5].GetComponent<ActionsWithSlotInventory>();
                if (!component.Equpped)
                    EquipmentInSLot(component, cardInInventory, _arraySpells[5], slot, 5, equippedCard);
                else
                    SwitchSLots(component, cardInInventory, _arraySpells[5], slot, equippedCard);
                break;
            }
            default:
            {
                var flagEqupment = false;
                for (var i = 0; i < _arraySpells.Count - 1; i++)
                {
                    component = _arraySpells[i].GetComponent<ActionsWithSlotInventory>();
                    if (!component.Empty) continue;
                    flagEqupment = true;
                    EquipmentInSLot(component, cardInInventory, _arraySpells[i], slot, i, equippedCard);
                    break;
                }
                if (flagEqupment) return;
                component = _arraySpells[0].GetComponent<ActionsWithSlotInventory>();
                SwitchSLots(component, cardInInventory, _arraySpells[0], slot, equippedCard);
                break;
            }
        }
    }
    //----------------Освобождаю слот экипировки/спеллов
    private void ClearSlotEqupment(ICard equippedCard, GameObject slot)
    {
        var list = _inventoryPlayers[_player]; 
        var cardInSlot = slot.transform.GetChild(0).gameObject;
        //------------ищу пустой слот в инвертаре
        var index = -1;
        ActionsWithSlotInventory component;
        for (var i = 0; i < list.Count; i++)
        {
            if (list[i] != "") continue;
            component = _arrayInventory[i].GetComponent<ActionsWithSlotInventory>();
            if (!component.Empty) continue;
            index = i;
            break;
        }
        if(index < 0) return;
        var cardType = equippedCard.GetDataCard().TypeCard;
        switch (cardType)
        {
            case CardType.Consumables:
                Debug.Log("id = " + _idSlot);
                component = _arraySpells[5].GetComponent<ActionsWithSlotInventory>();
                UnequipmentInSLot(component, cardInSlot, index, equippedCard);
                break;
            default:
                component = _arraySpells[_idSlot].GetComponent<ActionsWithSlotInventory>();
                UnequipmentInSLot(component, cardInSlot, index, equippedCard);
                break;
        }
    }
    private void UnequipmentInSLot(ActionsWithSlotInventory component, GameObject cardInSlot, int index, ICard equippedCard)
    {
        var vectorZero = new Vector3(0, 0f, 0f);
        component.SetVarsSlot(true,false, 0,  null);
        cardInSlot.transform.SetParent(_arrayInventory[index].transform);
        cardInSlot.transform.localPosition = vectorZero;
        component = _arrayInventory[index].GetComponent<ActionsWithSlotInventory>();
        component.SetVarsSlot(false,false, index,  equippedCard);
        AddItemInventory(equippedCard.GetDataCard().NameCard,index);
        DeleteItemEqupmentCard(_idSlot);
    }
    private void EquipmentInSLot(ActionsWithSlotInventory component, GameObject cardInInventory,
        GameObject parent, GameObject slot, int index, ICard equippedCard)
    {
        var vectorZero = new Vector3(0, 0f, 0f);
        component.SetVarsSlot(false,true, index,  equippedCard);
        cardInInventory.transform.SetParent(parent.transform);
        cardInInventory.transform.localPosition = vectorZero;
        component = slot.GetComponent<ActionsWithSlotInventory>();
        component.SetVarsSlot(true,false, 0,  null);
        //------------очищаем слот в инвентаре
        DeleteItemInventory(_idSlot);
        AddItemEqupmentCard(equippedCard.GetDataCard().NameCard, index);
        //_publisher.Publish(parent, new CustomEventArgs(GameEventName.GoAddCardInDeck,equippedCard));
        
    }
    private void SwitchSLots(ActionsWithSlotInventory component, GameObject cardInInventory,
        GameObject parent, GameObject slot, ICard equippedCard)
    {
        var vectorZero = new Vector3(0, 0f, 0f);
        var swithObject = parent.transform.GetChild(0).gameObject;
        var cardObject = component.CardGame;
        component.SetVarsSlot(false,true, 0,  equippedCard);
        cardInInventory.transform.SetParent(parent.transform);
        cardInInventory.transform.localPosition = vectorZero;
        swithObject.transform.SetParent(slot.transform);
        swithObject.transform.localPosition = vectorZero;
        component = slot.GetComponent<ActionsWithSlotInventory>();
        component.SetVarsSlot(false,false, _idSlot,  cardObject);
    }
    private void SpawnItemInventory(int index)
    {
        var list = _inventoryPlayers[_player];
        var component = _arrayInventory[index].GetComponent<ActionsWithSlotInventory>();
        var card = _objectStorage.GetPollObjects(ObjectTypeEnum.PrefabCardInInventory, 1)[0];
        _objectStorage.ConfigurateByParent(card,_arrayInventory[index],0,0,1,1);
        var cardDisplay = card.GetComponent<ActionsWithCards>();
        var name = list[index];
        var cardData = _objectStorage.GetCardByName(name);
        cardDisplay.СhangeCardType(cardData);
        cardDisplay.SetIdCard(index);
        cardDisplay.CardGame.DisplayCardInGame(card);
        component.SetVarsSlot(false, false, index, cardData);
        //card.transform.Find("ShirtCard").gameObject.SetActive(false);
        component.SetDependecies(_publisher);
    }
    private void SpawnItemEqupmentCard(int index)
    {
        var list = _equpmentCardsPlayers[_player];
        var component = _arraySpells[index].GetComponent<ActionsWithSlotInventory>();
        var card = _objectStorage.GetPollObjects(ObjectTypeEnum.PrefabCardInInventory, 1)[0];
        _objectStorage.ConfigurateByParent(card,_arraySpells[index],0,0,1,1);
        var cardDisplay = card.GetComponent<ActionsWithCards>();
        var name = list[index];
        var cardData = _objectStorage.GetCardByName(name);
        cardDisplay.СhangeCardType(cardData);
        cardDisplay.SetIdCard(index);
        cardDisplay.CardGame.DisplayCardInGame(card);
        component.SetVarsSlot(false, true, index, cardData);
        //card.transform.Find("ShirtCard").gameObject.SetActive(false);
        component.SetDependecies(_publisher);
    }
    public List<string> GetCardInDeck()
    {
        return (from equpmentCards in _equpmentCardsPlayers 
            from card in equpmentCards.Value where card != "" select card).ToList();
    }
}
