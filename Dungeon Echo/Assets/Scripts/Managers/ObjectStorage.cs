using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using Object = UnityEngine.Object;
/// <summary>
/// Менеджер посталяет обьекты
/// </summary>
public class ObjectStorage : IObjectStorage
{
    private IObjectLoader _objectLoader;
    private IConfigurateManager _configurateManager;
    private readonly IDictionary<ObjectTypeEnum, object> _poolPrefabs;
    private readonly IDictionary<ObjectTypeEnum, object[]> _poolsDataObjects;
    private readonly IDictionary<ObjectTypeEnum, List<GameObject>> _poolsObjects;
    
    private readonly IDictionary<SubTypeCard, IDictionary<string, ICard>> _dictCards;
    private readonly IDictionary<string, AudioClip> _audioClips;
    private  List<Adventure> _adventure;
    private IObjectStorage _objectStorageImplementation;

    public ObjectStorage (IObjectLoader objectLoader, IConfigurateManager configurateManager)
    {
        _objectLoader = objectLoader;
        _configurateManager = configurateManager;
        _poolPrefabs = new Dictionary<ObjectTypeEnum, object>();
        _poolsDataObjects = new Dictionary<ObjectTypeEnum, object[]>();
        _poolsObjects = new Dictionary<ObjectTypeEnum, List<GameObject>>();
        _dictCards = new Dictionary<SubTypeCard, IDictionary<string, ICard>>();
        _audioClips = new Dictionary<string, AudioClip>();
        _adventure = new List<Adventure>();
        CreateDictionaryAudio();
    }
    public void CreateAllPools()
    {
        CreatePoolsObject();
        CreateDictionaryCards();
    }
    public T GetObjectInStorage<T>(ObjectTypeEnum typeEnum)where T : Object
    {
        object getObject;
        var isExists = _poolPrefabs.TryGetValue(typeEnum, out getObject);
        if (isExists != false) return (T)getObject;
        var stringPath = Strings.GetResoursePath(typeEnum);
        getObject = _objectLoader.Load<T>(stringPath);
        _poolPrefabs.Add(typeEnum,getObject);
        return (T) getObject;
    }
    public T[] GetPoolObjectsInStorage<T>(ObjectTypeEnum typeEnum) where T : Object
    {
        object[] getObject;
        var isExists = _poolsDataObjects.TryGetValue(typeEnum, out getObject);
        if (isExists != false)
        {
            return (T[]) getObject;
        }
        var stringPath = Strings.GetResoursePath(typeEnum);
        getObject = _objectLoader.LoadAll<T>(stringPath);
        _poolsDataObjects.Add(typeEnum,getObject);
        return (T[]) getObject;
    }
    public List<GameObject> GetPollObjects(ObjectTypeEnum typeEnum, int size)
    {
        var pool = new List<GameObject>();
        var count = 0;
        foreach (var obj in _poolsObjects[typeEnum])
        {
            if (obj.activeSelf) continue;
            pool.Add(obj);
            count++;
            if (count == size) break;
        }
        foreach (var objDel in pool)
        {
            if (_poolsObjects[typeEnum].Contains(objDel))
            {
                _poolsObjects[typeEnum].Remove(objDel);
            }
        }
        return pool;
    }
    private void CreatePoolsObject()
    {
        CreatePoolByType(ObjectTypeEnum.PrefabIconPlayer, 4);
        CreatePoolByType(ObjectTypeEnum.PrefabCardArea, 4);
        CreatePoolByType(ObjectTypeEnum.PrefabCardEnemy, 4);
        CreatePoolByType(ObjectTypeEnum.PrefabCardBattleEnemy, 5);
        CreatePoolByType(ObjectTypeEnum.PrefabCardEvent, 4);
        CreatePoolByType(ObjectTypeEnum.PrefabCard, 26);
        CreatePoolByType(ObjectTypeEnum.PrefabCardInInventory, 12);
        CreatePoolByType(ObjectTypeEnum.SlotCardInInventory, 18);
        CreatePoolByType(ObjectTypeEnum.SlotInventory2, 9);
        CreatePoolByType(ObjectTypeEnum.PrefabBtnEvent, 4);
    }
    private void  CreatePoolByType(ObjectTypeEnum typeEnum,int count)
    {
        List<GameObject> getObject;
        var isExists = _poolsObjects.TryGetValue(typeEnum, out getObject);
        if (isExists) return;
        _poolsObjects[typeEnum] = new List<GameObject>();
        for (var i = 0; i < count; i++)
        {
            var prefab = GetObjectInStorage<GameObject>(typeEnum);
            var clone = _objectLoader.Instantiate(prefab, _configurateManager.GetPoolParent());
            clone.SetActive(false);
            _poolsObjects[typeEnum].Add(clone);
        }
    }
    public IDictionary<string, ICard> GetDictionaryCards (SubTypeCard subTypeCard)
    {
        IDictionary<string, ICard>  dictionary;
        var isExists = _dictCards.TryGetValue(subTypeCard, out dictionary);
        if (isExists == false)
        {
            throw new UnityException("It is unknown dictionary (ObjectStorage)");
        }
        return dictionary;
    }

    public List<Adventure> GetAdventure()
    {
        return _adventure;
    }

    public AudioClip GetAudioByName(string name)
    {
        return _audioClips[name];
    }

    //--------------------Берем рандомную карту по типу и редкости
    public ICard GetRandomCardByType(SubTypeCard type, CardRarity rarity)
    {
        var flag = false;
        ICard value = null;
        var dictCards = GetDictionaryCards(type);
        while (!flag)
        {
            var index = RandomExtensions.Random(dictCards.Count);
            var card =  dictCards.Values.ElementAt(index);
            if (card.GetDataCard().Rarity != rarity) continue;
            value = card;
            flag = true;
        }
        return value;
    }
    public ICard GetCardByName(string name)
    {
        foreach (var dict in _dictCards)
        {
            var values = dict.Value;
            ICard value = null;
            var isExists = values.TryGetValue(name, out value);
            if (isExists == false) continue;
            return value;
        }
        return null;
    }
    private void CreateDictionaryCards()
    {
        var enumerable = Enum.GetValues(typeof(SubTypeCard)).Cast<SubTypeCard>();
        foreach (var subType in enumerable)
        {
            if(subType == SubTypeCard.Undefined) continue;
            _dictCards[subType] = new Dictionary<string, ICard>();
        }
        //--------загружаем данные о картах экипировки
        var allCardsEquipment = GetPoolObjectsInStorage<CardEquipment>(ObjectTypeEnum.CardsEquipment);
        foreach (var equipment in allCardsEquipment)
        {
            var name = equipment.cardName;
            var typeCard = equipment.subType;
            _dictCards[typeCard].Add(name,equipment);
        }
        //--------загружаем данные о картах заклинаний
        var allCardsSpell = GetPoolObjectsInStorage<CardSpell>(ObjectTypeEnum.CardsSpells);
        foreach (var spell in allCardsSpell)
        {
            var name = spell.cardName;
            var typeCard = spell.subType;
            _dictCards[typeCard].Add(name,spell);
        }
        //--------загружаем данные о картах расходниках
        var allCardsConsumables = GetPoolObjectsInStorage<CardConsumables>(ObjectTypeEnum.CardsConsumables);
        foreach (var consumables in allCardsConsumables)
        {
            var name = consumables.cardName;
            var typeCard = consumables.subType;
            _dictCards[typeCard].Add(name,consumables);
        }
        var allCardsGameClass = GetPoolObjectsInStorage<CardsGameClass>(ObjectTypeEnum.CardsGameClass);
        foreach (var cardsGame in allCardsGameClass)
        {
            var name = cardsGame.cardName.ToString();
            var typeCard = cardsGame.subType;
            _dictCards[typeCard].Add(name,cardsGame);
        }
        
        var allCardsGameArea = GetPoolObjectsInStorage<CardsGameArea>(ObjectTypeEnum.CardsArea);
        foreach (var cardsArea in allCardsGameArea)
        {
            var name = cardsArea.cardName;
            var typeCard = cardsArea.subType;
            _dictCards[typeCard].Add(name,cardsArea);
        }
        var allCardsEnemy= GetPoolObjectsInStorage<CardEnemy>(ObjectTypeEnum.CardsEnemy);
        foreach (var cardsEnemy in allCardsEnemy)
        {
            var name = cardsEnemy.cardName;
            var typeCard = cardsEnemy.subType;
            _dictCards[typeCard].Add(name,cardsEnemy);
        }
        var allCardsEvents= GetPoolObjectsInStorage<CardGameEvent>(ObjectTypeEnum.CardsEvents);
        foreach (var cardsEvent in allCardsEvents)
        {
            var name = cardsEvent.cardName;
            var typeCard = cardsEvent.subType;
            _dictCards[typeCard].Add(name,cardsEvent);
        }
        var allCardsUnits= GetPoolObjectsInStorage<CardUnit>(ObjectTypeEnum.CardsUnits);
        foreach (var cardUnit in allCardsUnits)
        {
            var name = cardUnit.cardName;
            var typeCard = cardUnit.subType;
            _dictCards[typeCard].Add(name,cardUnit);
        }
        var allAdventure= GetPoolObjectsInStorage<Adventure>(ObjectTypeEnum.Adventure);
        foreach (var adventure in allAdventure)
            _adventure.Add(adventure);
    }

    private void CreateDictionaryAudio()
    {
        var allAudio = GetPoolObjectsInStorage<AudioClip>(ObjectTypeEnum.Music);
        foreach (var audioClip in allAudio)
        {
            _audioClips.Add(audioClip.name, audioClip);
        }
    }
    public IEnumerator ReturnInPool(ObjectTypeEnum typeEnum, GameObject obj, float t)
    {
        yield return new WaitForSeconds(t);
        _poolsObjects[typeEnum].Add(obj);
        _configurateManager.ConfigurateZero(obj);
    }
    /*public void ConfigurateByParent( GameObject child,GameObject parent)
    {
        child.transform.SetParent(parent.transform);
        child.GetComponent<RectTransform>().SetRect(0, 0, 1, 1);
        child.GetComponent<RectTransform>().SetOffset(0,0,0,0);
        child.SetActive(true);
    }*/
   
}
